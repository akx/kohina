
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Kohina.Nodes
{
	
	[NodeCategoryAttribute(DataType.Bitmap, NodeCategoryKind.Modifier)]
	public class BitmapJPEGTransmuter: Node
	{
		[PinAttribs("Input", PinDirection.Input, DataType.Bitmap)]
		Pin inputPin = null;
		[PinAttribs("Output", PinDirection.Output, DataType.Bitmap)]
		Pin outputPin = null;
		
		int quality = 90;
		int hack = 0;
		
		public int Quality {
			get { return quality; }
			set { quality = value; }
		}
		
		public int Hack {
			get { return hack; }
			set { hack = value; }
		}
		
		private static ImageCodecInfo jpegCodec = null;
		private EncoderParameters epar;
		private int eparQ = 0;
		
		public override object GetPinValue(Pin pin, PinRequest request)
		{
			if(pin == outputPin) {
				Bitmap bmp = inputPin.Read<Bitmap>(request);
				if(bmp == null) return null;
				bmp = bmp.ResizeIfNeeded(
					(request is BitmapPinRequest) ? (request as BitmapPinRequest).DesiredSize : bmp.Size
				);
				if(jpegCodec == null) {
					jpegCodec = ImageCodecInfo.GetImageEncoders().First((enc)=>enc.MimeType.Contains("jpeg"));
					epar = new EncoderParameters(1);
				}
				if(eparQ != quality) {
					epar.Param[0] = new EncoderParameter(Encoder.Quality, quality);
					eparQ = quality;
				}
				
				MemoryStream ms = new MemoryStream();
				bmp.Save(ms, jpegCodec, epar);
				bmp.Dispose();
				ms.Position = 0;
				if(hack != 0) {
					Random r = new Random();
					byte[] content = new byte[ms.Length];
					ms.Read(content, 0, (int)ms.Length);
					for(int i = 0; i < content.Length; i++) {
						if(r.Next(10000) < hack) content[i] = (byte)r.Next(255);
					}
					ms.Dispose();
					ms = new MemoryStream(content);
				}
				Bitmap nbmp = bmp;
				try {
					nbmp = new Bitmap(ms);
					Guid[] gs = ((Image)nbmp).FrameDimensionsList;
					foreach (var g in gs) {
						Debug.Print("{0} {1}", gs.Length, g);
					}
				} catch {
					nbmp = bmp;
				}
				if(nbmp != bmp) bmp.Dispose();
				return nbmp;
			}
			return null;
		}
	}
}
