using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kohina.Nodes
{
	public class BitmapChannelRemap: Node
	{
		[PinAttribs("Input", PinDirection.Input, DataType.Bitmap)]
		Pin inputPin = null;
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		
		UInt32 mask = 0x18100800;
		
		public uint Mask {
			get { return mask; }
			set { mask = value; }
		}
		
		
		
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				Bitmap bmp = inputPin.Read<Bitmap>(request);
				if(bmp == null) return null;
				Size sz = (request is BitmapPinRequest) ? (request as BitmapPinRequest).DesiredSize : bmp.Size;
				Bitmap bmpC = new Bitmap(bmp, sz);
				bmp.Dispose();
				Rectangle r = new Rectangle(0, 0, sz.Width, sz.Height);
				BitmapData bData = bmpC.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				CBlend.CRemap(bData.Scan0, (UInt32)(bData.Stride * bData.Height), mask);
				bmpC.UnlockBits(bData);
				return bmpC;
			}
			return null;
		}
		
	}
}
