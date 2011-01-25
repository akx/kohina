
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kohina
{
	public static class BitmapFilters
	{
		public static Bitmap Blur(Bitmap image, Int32 blurSize, bool boxBlur)
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
		            for (Int32 x = Math.Max(0, xx - blurSize); x < Math.Min(xx + blurSize, image.Width); x++)
		            {
		            	for (Int32 y = Math.Max(0, yy - blurSize); y < Math.Min(yy + blurSize, image.Height); y++)
		                {
		            		float dist = 1;
		            		if(!boxBlur) {
			            		dist = 1.0f - (float)(Math.Sqrt((x - xx) * (x - xx) + (y - yy) * (y - yy)) / blurSize);
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
	}
}
