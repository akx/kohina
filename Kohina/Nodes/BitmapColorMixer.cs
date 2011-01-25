
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Kohina.Nodes
{
	[NodeCategoryAttribute(DataType.Bitmap, NodeCategoryKind.Modifier)]
	public class BitmapColorMixer: Node
	{
		[PinAttribs("Input", PinDirection.Input, DataType.Bitmap)]
		Pin inputPin = null;
		[PinAttribs("R Mult", PinDirection.Input, DataType.Number)]
		Pin rMulPin = null;
		[PinAttribs("G Mult", PinDirection.Input, DataType.Number)]
		Pin gMulPin = null;
		[PinAttribs("B Mult", PinDirection.Input, DataType.Number)]
		Pin bMulPin = null;
		[PinAttribs("R Bias", PinDirection.Input, DataType.Number)]
		Pin rBiasPin = null;
		[PinAttribs("G Bias", PinDirection.Input, DataType.Number)]
		Pin gBiasPin = null;
		[PinAttribs("B Bias", PinDirection.Input, DataType.Number)]
		Pin bBiasPin = null;
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		
		ColorMatrix cm;
		ImageAttributes ia;
		
		protected override object GetPinDefaultConstantValue(Pin p)
		{
			if(p.DataType == DataType.Number) return 0.0;
			return base.GetPinDefaultConstantValue(p);
		}
		
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				Bitmap source = inputPin.Read<Bitmap>(request);
				if(source == null) return null;
				Size sz = ((request is BitmapPinRequest) ? (request as BitmapPinRequest).DesiredSize : source.Size);
				Bitmap bmp = new Bitmap(sz.Width, sz.Height, PixelFormat.Format32bppPArgb);
				double rM = rMulPin.Read<double>(request);
				double gM = gMulPin.Read<double>(request);
				double bM = bMulPin.Read<double>(request);
				double rB = rBiasPin.Read<double>(request);
				double gB = gBiasPin.Read<double>(request);
				double bB = bBiasPin.Read<double>(request);
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
				cm.Matrix00 = (float)rM;
				cm.Matrix11 = (float)gM;
				cm.Matrix22 = (float)bM;
				cm.Matrix40 = (float)rB;
				cm.Matrix41 = (float)gB;
				cm.Matrix42 = (float)bB;
				ia.SetColorMatrix(cm);
				using(Graphics g = Graphics.FromImage(bmp)) {
					g.Clear(Color.Black);
					g.CompositingMode = CompositingMode.SourceOver;
					g.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia);
				}
				source.Dispose();
				return bmp;
			}
			return null;
		}

	}
}
