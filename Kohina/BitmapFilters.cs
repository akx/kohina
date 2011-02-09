
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kohina
{
	public static class BitmapFilters
	{
		public static Bitmap Blur(Bitmap image, int blurSizeX, int blurSizeY, bool boxBlur)
		{
			Rectangle rectangle = new Rectangle(0, 0, image.Width, image.Height);
		    Bitmap blurred = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
		    
		    BitmapData srcData = image.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
		    BitmapData dstData = blurred.LockBits(rectangle, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
		    CBlend.Clear(dstData.Scan0, (uint)(dstData.Stride * dstData.Height), 0xFFFFFFFF, 0xFFFFFFFF);
		    for (Int32 xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
		    {
		        for (Int32 yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
		        {
		            float avgR = 0, avgG = 0, avgB = 0;
		            float blurPixelCount = 0;
		            int x0 = Math.Max(0, xx - blurSizeX);
		            int x1 = Math.Min(xx + blurSizeX, image.Width);
		            int y0 = Math.Max(0, yy - blurSizeY);
		            int y1 = Math.Min(yy + blurSizeY, image.Height);
		            for (Int32 x = x0; x < x1; x++)
		            {
		            	for (Int32 y = y0; y < y1; y++)
		                {
		            		float dist = 1;
		            		if(!boxBlur) {
		            			dist = 1.0f - (float)(Math.Sqrt((x - xx) * (x - xx) + (y - yy) * (y - yy)) / Math.Max(blurSizeX, blurSizeY));
			            		if(dist <= 0) continue;		            			
		            		}
		            		
		            		unsafe {
			            		byte * px = (((byte*)srcData.Scan0.ToPointer()) + y * srcData.Stride + x * 4);
			            		
			            		avgR += px[0] * dist;
			            		avgG += px[1] * dist;
			            		avgB += px[2] * dist;
		            		}
		
		                    blurPixelCount += dist;
		                }
		            }
		
		            avgR = avgR / blurPixelCount;
		            avgG = avgG / blurPixelCount;
		            avgB = avgB / blurPixelCount;
		
		            unsafe {
			            byte * px = (((byte*)dstData.Scan0.ToPointer()) + yy * dstData.Stride + xx * 4);
			            px[0] = (byte)avgR;
			            px[1] = (byte)avgG;
			            px[2] = (byte)avgB;
		            }
		        }
		    }
		    image.UnlockBits(srcData);
		    blurred.UnlockBits(dstData);
		
		    return blurred;
		}
		
		
		public static void StackBlur(Bitmap image, int radiusX, int radiusY)
		{
			if(radiusX > 255) radiusX = 255;
			if(radiusY > 255) radiusY = 255;
			
			Rectangle rectangle = new Rectangle(0, 0, image.Width, image.Height);
		    BitmapData data = image.LockBits(rectangle, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
		    Misc.StackBlur.Apply(radiusX, radiusY, data.Width, data.Height, data);
		    image.UnlockBits(data);
		}
	}
}
