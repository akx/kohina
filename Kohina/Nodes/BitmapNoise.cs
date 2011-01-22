
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Kohina.Nodes
{
	/// <summary>
	/// Description of BitmapNoise.
	/// </summary>
	public class BitmapNoise: Node
	{
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		
		UInt32 oMask = 0xFFFFFFFF, aMask = 0xFFFFFFFF;
		
		public uint OMask {
			get { return oMask; }
			set { oMask = value; }
		}
		
		public uint AMask {
			get { return aMask; }
			set { aMask = value; }
		}
		
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				Size sz = (request is BitmapPinRequest) ? (request as BitmapPinRequest).DesiredSize : new Size(256, 256);
				Bitmap bmp = new Bitmap(sz.Width, sz.Height, PixelFormat.Format32bppArgb);
				BitmapData bd = bmp.LockBits(new Rectangle(0, 0, sz.Width, sz.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				CBlend.Noise(bd.Scan0, (UInt32)(bd.Stride * bd.Height), oMask, aMask);
				bmp.UnlockBits(bd);
				return bmp;
			}
			return null;
		}
	}
}
