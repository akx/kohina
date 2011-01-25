
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kohina.Nodes
{
	[NodeCategoryAttribute(DataType.Bitmap, NodeCategoryKind.Mixer)]
	public class BitmapXBlender: Node
	{
		[PinAttribs("Input 1", PinDirection.Input, DataType.Bitmap)]
		Pin input1Pin = null;
		[PinAttribs("Input 2", PinDirection.Input, DataType.Bitmap)]
		Pin input2Pin = null;
		[PinAttribs("Mix", PinDirection.Input, DataType.Number)]
		Pin mixPin = null;
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		
		BlendMode blendMode;
		public BlendMode BlendMode {
			get { return blendMode; }
			set { blendMode = value; }
		}
		
		bool useSrcAlpha;
		public bool UseSrcAlpha {
			get { return useSrcAlpha; }
			set { useSrcAlpha = value; }
		}
		
		
		
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
				if(bmp2 == null) return bmp1;
				if(bmp1 == null) return bmp2;
				Size sz = (request is BitmapPinRequest) ? (request as BitmapPinRequest).DesiredSize : new Size(
					Math.Min(bmp1.Size.Width, bmp2.Size.Width),
					Math.Min(bmp1.Size.Height, bmp2.Size.Height)
				);
				bmp1 = bmp1.ResizeIfNeeded(sz);
				bmp2 = bmp2.ResizeIfNeeded(sz);
				Rectangle r = new Rectangle(0, 0, sz.Width, sz.Height);
				double mix = mixPin.Read<double>(request);
				BitmapData b1Data = bmp1.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				BitmapData b2Data = bmp2.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				
				CBlend.Blend(blendMode, b1Data.Scan0, b2Data.Scan0, (UInt32)(b1Data.Stride * b1Data.Height), (float)mix, useSrcAlpha);
				bmp1.UnlockBits(b1Data);
				bmp2.UnlockBits(b2Data);
				bmp2.Dispose();
				return bmp1;
			}
			return null;
		}
		
	}
}
