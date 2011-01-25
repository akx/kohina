
using System;

namespace Kohina.Nodes
{
	[NodeCategoryAttribute(DataType.Number, NodeCategoryKind.Generator)]
	public class TimeToLFO: Node
	{
		[PinAttribs("Frequency", PinDirection.Input, DataType.Number)]
		Pin frequencyPin = null;
		[PinAttribs("Phase", PinDirection.Input, DataType.Number)]
		Pin phasePin = null;
		[PinAttribs("Amplitude", PinDirection.Input, DataType.Number)]
		Pin ampPin = null;
		[PinAttribs("Bias", PinDirection.Input, DataType.Number)]
		Pin biasPin = null;
		
		[PinAttribs("Output", PinDirection.Output, DataType.Number)]
		Pin outputPin = null;
		
		protected override object GetPinDefaultConstantValue(Pin p)
		{
			if(p == frequencyPin) return 1.0;
			if(p == phasePin) return 0.0;
			if(p == ampPin) return 1.0;
			if(p == biasPin) return 0.0;
			return base.GetPinDefaultConstantValue(p);
		}
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				double time = (request != null ? request.Time : DateTime.Now.TimeOfDay.TotalSeconds);
				
				double freq = frequencyPin.Read<double>(request);
				double phase = phasePin.Read<double>(request);
				double amp = ampPin.Read<double>(request);
				double bias = biasPin.Read<double>(request);
				
				return Math.Sin((freq * time + phase) * Math.PI * 2) * amp + bias;
			}
			return base.GetPinValue(pin, request);
		}
		
		
		public TimeToLFO()
		{
		}
	}
}
