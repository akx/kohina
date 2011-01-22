
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Kohina
{
	public class PinPropertyDescriptor: PropertyDescriptor {
		private Pin pin;
		
		public PinPropertyDescriptor(Pin pin, Attribute[] attrs): base(pin.Name, attrs)
		{
			this.pin = pin;
		}
		
		
		public override string ToString()
		{
			return string.Format("[PinPropertyDescriptor Pin={0}]", this.pin);
		}
		
		
		public override Type ComponentType {
			get { return typeof(Pin); }
		}
		
		public override bool IsReadOnly {
			get { return pin.Connected; }
		}
		
		public override Type PropertyType {
			get {
				switch(pin.DataType) {
					case DataType.Number:
						return typeof(double);
					case DataType.Color:
						return typeof(Color);
					case DataType.String:
						return typeof(String);
					case DataType.Bytes:
						return typeof(byte[]);
					case DataType.Bitmap:
						return null;
					case DataType.Vector2:
						return typeof(PointF);
				}
				return typeof(object);
			}
		}
		
		public override bool CanResetValue(object component) { return false; }
		public override void ResetValue(object component) {	}
		
		public override object GetValue(object component) { return pin.ConstantValue; }
		
		
		
		public override void SetValue(object component, object value)
		{
			Debug.Print("{0} cv -> {1}", pin, value);
			pin.ConstantValue = value;
			OnValueChanged(component, EventArgs.Empty);
		}
		
		public override bool ShouldSerializeValue(object component)
		{
			return true;
		}
		
	}
	
	public class NodePropertyProxy: CustomTypeDescriptor
	{
		private Node node;
		
		public NodePropertyProxy(Node node): base()
		{
			this.node = node;
		}
		
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			List<PropertyDescriptor> pdes = new List<PropertyDescriptor>();
			
			foreach(Pin p in node.InputPins) {
				if(p.DataType == DataType.Bitmap) continue;
				Debug.Print("Creating PD for {0} of {1}", p, node);
				Attribute[] ac = new Attribute[]{
					new DisplayNameAttribute(p.Name),
					new DescriptionAttribute(p.Description),
					new CategoryAttribute("Input Pins")
				};
				pdes.Add(new PinPropertyDescriptor(p, ac));
			}
			
			return new PropertyDescriptorCollection(pdes.ToArray());
		}
		
		public override object GetPropertyOwner(PropertyDescriptor pd)
		{
			return node;
		}
		
		
	}
}
