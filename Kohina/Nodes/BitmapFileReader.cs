
using System;
using System.Diagnostics;
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
		
		Bitmap cachedBmp, cachedSizedBmp;
		string cachedBmpName;
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
					
				if(!string.IsNullOrEmpty(fileName) && File.Exists(fileName) && (cachedBmp == null || cachedBmpName != fileName)) {
					try {
						cachedBmp = new Bitmap(fileName);
						cachedBmpName = fileName;
					} catch {
						if(cachedSizedBmp != null) cachedSizedBmp.Dispose();
						if(cachedBmp != null) cachedBmp.Dispose();
						cachedSizedBmp = null;
						cachedBmp = null;
						cachedBmpName = null;
					}
				}
				if(cachedBmp != null) {
					BitmapPinRequest bp = request as BitmapPinRequest;
					if(bp != null) {
						if(cachedSizedBmp != null) {
							lock(cachedSizedBmp) {
								if(cachedSizedBmp.Size == bp.DesiredSize) {
									return new Bitmap(cachedSizedBmp);
								} else {
									cachedSizedBmp.Dispose();
									cachedSizedBmp = null;
								}
							}
						}
						Debug.Print("Recaching bitmap {0}", bp.DesiredSize);
						cachedSizedBmp = new Bitmap(cachedBmp, bp.DesiredSize);
						
						return new Bitmap(cachedSizedBmp);
					}
					return new Bitmap(cachedBmp);
				}
			}
			return null;
		}
		
		
		
	}
}
