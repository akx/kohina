
using System;
using System.Drawing;

namespace Kohina.Nodes
{
	[NodeCategoryAttribute(DataType.Bitmap, NodeCategoryKind.Utility)]
	public class BitmapCacher: Node
	{
		[PinAttribs("Input", PinDirection.Input, DataType.Bitmap)]
		Pin inputPin = null;
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		
		Bitmap cached = null;
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				if(cached == null) {
					cached = inputPin.Read<Bitmap>(request);
				}
				if(cached != null) {
					return new Bitmap(cached);
				}
			}
			return null;
		}
		
		public override void Interact()
		{
			cached = null;
		}
		
	}
}
