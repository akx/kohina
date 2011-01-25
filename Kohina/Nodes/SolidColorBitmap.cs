using System.ComponentModel;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kohina.Nodes
{
	[NodeCategoryAttribute(DataType.Bitmap, NodeCategoryKind.Generator)]
	public class SolidColorBitmap: Node
	{
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		
		[PinAttribs("Color", PinDirection.Input, DataType.Color)]
		Pin colorPin = null;
		
		Size size = new Size(640, 480);
		public Size Size {
			get { return size; }
			set { size = value; }
		}
		
		protected override object GetPinDefaultConstantValue(Pin p)
		{
			if(p == colorPin) return (object)Color.Orange;
			return base.GetPinDefaultConstantValue(p);
		}
		
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				Size sz = (request is BitmapPinRequest) ? ((BitmapPinRequest)request).DesiredSize : size;
				Color c = colorPin.Read<Color>();
				Bitmap bmp = new Bitmap(sz.Width, sz.Height, PixelFormat.Format32bppArgb);
				Rectangle r = bmp.GetEntireRect();
				BitmapData bd = bmp.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				CBlend.Clear(bd.Scan0, (UInt32)(bd.Stride * bd.Height), (uint)c.ToArgb(), 0xFFFFFFFF);
				bmp.UnlockBits(bd);
				return bmp;
			}
			return null;
		}
		
		
		
	}
}
