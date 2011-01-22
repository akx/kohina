using System.ComponentModel;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kohina.Nodes
{
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
				Bitmap bmp = new Bitmap(sz.Width, sz.Height, PixelFormat.Format32bppPArgb);
				using(Graphics g = Graphics.FromImage(bmp)) {
					g.Clear(colorPin.Read<Color>());
				}
				return bmp;
			}
			return null;
		}
		
		
		
	}
}
