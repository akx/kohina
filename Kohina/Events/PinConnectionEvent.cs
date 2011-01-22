
using System;

namespace Kohina.Events
{
	public enum PinConnectionEventType {
		Connected,
		Disconnected
	};
	public class PinConnectionEventArgs : EventArgs {
		PinConnectionEventType eventType;
		Pin pin;
		public PinConnectionEventType EventType { get; set; }
		public Pin Pin { get; set; }
		public PinConnectionEventArgs(PinConnectionEventType eventType, Pin pin)
		{
			this.eventType = eventType;
			this.pin = pin;
		}
	}
	public delegate void PinConnectionHandler(object sender, PinConnectionEventArgs e);
}
