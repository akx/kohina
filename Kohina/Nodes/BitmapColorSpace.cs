
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kohina.Nodes
{
	public enum ColorSpaceConvertMode {
		RGBtoHSV = 0,
		HSVtoRGB = 1
	};
	public class BitmapColorSpace: Node
	{
		[PinAttribs("Input", PinDirection.Input, DataType.Bitmap)]
		Pin inputPin = null;
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		
		ColorSpaceConvertMode mode = ColorSpaceConvertMode.RGBtoHSV;
		
		public ColorSpaceConvertMode Mode {
			get { return mode; }
			set { mode = value; }
		}
		
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				Bitmap bmp = inputPin.Read<Bitmap>(request);
				if(bmp == null) return null;
				bmp = bmp.ResizeIfNeeded(
					(request is BitmapPinRequest) ? (request as BitmapPinRequest).DesiredSize : bmp.Size
				);
				Rectangle r = new Rectangle(0, 0, bmp.Width, bmp.Height);
				BitmapData bData = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				CBlend.ConvertColorSpace(bData.Scan0, (UInt32)(bData.Stride * bData.Height), (int)mode);
				bmp.UnlockBits(bData);
				return bmp;
			}
			return null;
		}
		
	}
}
