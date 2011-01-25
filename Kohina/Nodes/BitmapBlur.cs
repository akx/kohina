
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Kohina.Nodes
{
	
	[NodeCategoryAttribute(DataType.Bitmap, NodeCategoryKind.Modifier)]
	public class BitmapBlur: Node
	{
		[PinAttribs("Input", PinDirection.Input, DataType.Bitmap)]
		Pin inputPin = null;
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		[PinAttribs("Radius", PinDirection.Input, DataType.Number)]
		Pin radiusPin = null;
		
		bool boxBlur = true;
		
		public bool BoxBlur {
			get { return boxBlur; }
			set { boxBlur = value; }
		}
		
		
		protected override object GetPinDefaultConstantValue(Pin p)
		{
			if(p == radiusPin) return (object)3.0;
			return null;
		}
		
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				Bitmap bmp = inputPin.Read<Bitmap>(request);
				if(bmp == null) return null;
				bmp = bmp.ResizeIfNeeded(
					(request is BitmapPinRequest) ? (request as BitmapPinRequest).DesiredSize : bmp.Size
				);
				double radius = radiusPin.Read<double>(request);
				if(radius < 0) return bmp;
				Bitmap blurred = BitmapFilters.Blur(bmp, (int)Math.Round(radius), boxBlur);
				bmp.Dispose();
				return blurred;
			}
			return null;
		}
	}
}
