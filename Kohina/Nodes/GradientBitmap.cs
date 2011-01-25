
using System;
using System.Drawing.Drawing2D;

namespace Kohina.Nodes
{
using System.ComponentModel;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kohina.Nodes
{
	public enum GradientType {
		Linear,
		Radial,
		RadialInvert
	};
	
	[NodeCategoryAttribute(DataType.Bitmap, NodeCategoryKind.Generator)]
	public class GradientBitmap: Node
	{
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		
		[PinAttribs("Color 1", PinDirection.Input, DataType.Color)]
		Pin color1Pin = null;
		
		[PinAttribs("Color 2", PinDirection.Input, DataType.Color)]
		Pin color2Pin = null;
		
		[PinAttribs("Direction", PinDirection.Input, DataType.Number)]
		Pin directionPin = null;

		GradientType gType = GradientType.Linear;
		
		public GradientType GradientType {
			get { return gType; }
			set { gType = value; }
		}
		
		
		protected override object GetPinDefaultConstantValue(Pin p)
		{
			if(p == color1Pin) return (object)Color.Blue;
			if(p == color2Pin) return (object)Color.Black;
			if(p == directionPin) return (object)90.0;
			return base.GetPinDefaultConstantValue(p);
		}
		
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				Size sz = (request is BitmapPinRequest) ? ((BitmapPinRequest)request).DesiredSize : new Size(256, 256);
				Color c1 = color1Pin.Read<Color>();
				Color c2 = color2Pin.Read<Color>();
				float angle = (float)(directionPin.Read<double>());
				
				Bitmap bmp = new Bitmap(sz.Width, sz.Height, PixelFormat.Format32bppArgb);
				using(Graphics g = Graphics.FromImage(bmp)) {
					Rectangle r = bmp.GetEntireRect();
					Brush b;
					if(this.gType == GradientType.Linear) {
						b = new LinearGradientBrush(r, c1, c2, angle);
					} else {
						g.Clear(c1);
						GraphicsPath gp = new GraphicsPath();
						gp.AddEllipse(r);
						if(this.gType == GradientType.RadialInvert) {
							Color x = c1;
							c1 = c2;
							c2 = x;
							
						}
						PathGradientBrush pgb = new PathGradientBrush(gp);
						pgb.WrapMode = WrapMode.Tile;
						pgb.CenterColor = c1;
						pgb.SurroundColors = new Color[]{c2};
						b = pgb;
					}
					g.FillRectangle(b, r);
					b.Dispose();
				}
				return bmp;
			}
			return null;
		}
		
		
		
	}
}

}
