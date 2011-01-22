
using System;
using System.Drawing;

namespace Kohina.Nodes
{
	/// <summary>
	/// Description of ColorBlender.
	/// </summary>
	public class ColorBlender: Node
	{
		[PinAttribs("Color1", PinDirection.Input, DataType.Color)]
		Pin color1Pin = null;
		[PinAttribs("Color2", PinDirection.Input, DataType.Color)]
		Pin color2Pin = null;
		[PinAttribs("Mix", PinDirection.Input, DataType.Number)]
		Pin mixPin = null;
		[PinAttribs("Output", PinDirection.Output, DataType.Color)]
		Pin output = null;
		
		protected override object GetPinDefaultConstantValue(Pin p)
		{
			if(p == color1Pin) return (object)Color.Black;
			if(p == color2Pin) return (object)Color.White;
			if(p == mixPin) return (object)0.5;
			return base.GetPinDefaultConstantValue(p);
		}
		
		
		
		public override object GetPinValue(Pin pin, PinRequest request) {
			if(pin == output) {
				Color color1 = color1Pin.Read<Color>(request);
				Color color2 = color2Pin.Read<Color>(request);
				double mix = mixPin.Read<double>(request);
				mix = Math.Abs(mix) % 1;
				double iMix = 1.0 - mix;
				return Color.FromArgb(
					(int) (color1.A * iMix + color2.A * mix),
					(int) (color1.R * iMix + color2.R * mix),
					(int) (color1.G * iMix + color2.G * mix),
					(int) (color1.B * iMix + color2.B * mix)
				);
			}
			return null;
		}
	}
}
