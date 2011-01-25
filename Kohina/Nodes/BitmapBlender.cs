
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Kohina.Nodes
{
	[NodeCategoryAttribute(DataType.Bitmap, NodeCategoryKind.Mixer)]
	public class BitmapBlender: Node
	{
		[PinAttribs("Input 1", PinDirection.Input, DataType.Bitmap)]
		Pin input1Pin = null;
		[PinAttribs("Input 2", PinDirection.Input, DataType.Bitmap)]
		Pin input2Pin = null;
		[PinAttribs("Mix", PinDirection.Input, DataType.Number)]
		Pin mixPin = null;
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		
		ColorMatrix cm;
		ImageAttributes ia;
		double cmMix = -1;
		
		protected override object GetPinDefaultConstantValue(Pin p)
		{
			if(p == mixPin) return 0.5;
			return base.GetPinDefaultConstantValue(p);
		}
		
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				Bitmap bmp1 = input1Pin.Read<Bitmap>(request);
				Bitmap bmp2 = input2Pin.Read<Bitmap>(request);
				if(bmp1 == null && bmp2 == null) return null;
				if(bmp1 != null && bmp2 == null) return bmp1;
				if(bmp1 == null && bmp2 != null) return bmp2;
				Size sz = (request is BitmapPinRequest) ? (request as BitmapPinRequest).DesiredSize : new Size(
					Math.Min(bmp1.Size.Width, bmp2.Size.Width),
					Math.Min(bmp1.Size.Height, bmp2.Size.Height)
				);
				Bitmap bmp = new Bitmap(sz.Width, sz.Height, PixelFormat.Format32bppPArgb);
				double mix = mixPin.Read<double>(request);
				if(cmMix != mix) {
					if(ia == null) ia = new ImageAttributes();
					if(cm == null) cm = new ColorMatrix(
						new float[][] {
   							new float[] {1,  0,  0,  0, 0},
							new float[] {0,  1,  0,  0, 0},
							new float[] {0,  0,  1,  0, 0},
							new float[] {0,  0,  0,  1, 0},
							new float[] {0f, 0f, 0f, 0, 1}
						}
					);
					cm.Matrix33 = (float)mix;
					cmMix = mix;
					ia.SetColorMatrix(cm);
				}
				   
				
				using(Graphics g = Graphics.FromImage(bmp)) {
					g.DrawImageUnscaled(bmp1, 0, 0);
					g.CompositingMode = CompositingMode.SourceOver;
					g.DrawImage(bmp2, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia);
				}
				bmp1.Dispose();
				bmp2.Dispose();
				return bmp;
			}
			return null;
		}
		
	}
}
