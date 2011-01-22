
using System;
using System.Drawing;
using System.IO;

namespace Kohina.Nodes
{
	public class BitmapFileReader: Node
	{	
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		
		string fileName = null;
		
		public string FileName {
			get { return fileName; }
			set { fileName = value; }
		}
		
		Bitmap cachedBmp;
		string cachedBmpName;
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				if(!string.IsNullOrEmpty(fileName) && File.Exists(fileName) && (cachedBmp == null || cachedBmpName != fileName)) {
					try {
						cachedBmp = new Bitmap(fileName);
						cachedBmpName = fileName;
					} catch {
						cachedBmp = null;
						cachedBmpName = null;
					}
				}
				if(cachedBmp != null) {
					BitmapPinRequest bp = request as BitmapPinRequest;
					if(bp != null) return new Bitmap(cachedBmp, bp.DesiredSize);
					else return new Bitmap(cachedBmp);
				}
			}
			return null;
		}
		
		
		
	}
}
