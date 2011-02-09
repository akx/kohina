
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
		[PinAttribs("Radius X", PinDirection.Input, DataType.Number)]
		Pin radiusXPin = null;
		[PinAttribs("Radius Y", PinDirection.Input, DataType.Number)]
		Pin radiusYPin = null;
		
		
		public enum BlurAlgorithm {
			BoxBlur,
			SlowBlur,
			StackBlur
		}
		
		BlurAlgorithm algorithm = BlurAlgorithm.StackBlur;
		
		public BlurAlgorithm Algorithm {
			get { return algorithm; }
			set { algorithm = value; }
		}
		
		
		
		protected override object GetPinDefaultConstantValue(Pin p)
		{
			if(p == radiusXPin || p == radiusYPin) return (object)3.0;
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
				int radiusX = (int)Math.Round(radiusXPin.Read<double>(request));
				int radiusY = (int)Math.Round(radiusYPin.Read<double>(request));
				if(algorithm == BlurAlgorithm.StackBlur) {
					BitmapFilters.StackBlur(bmp, radiusX, radiusY);
					return bmp;
				} else {
					Bitmap blurred = BitmapFilters.Blur(bmp, radiusX, radiusY, algorithm == BlurAlgorithm.BoxBlur);
					bmp.Dispose();
					return blurred;
				}
				
			}
			return null;
		}
	}
}
