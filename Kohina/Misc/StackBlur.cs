﻿
using System;
using System.Drawing.Imaging;

namespace Kohina.Misc
{
	/*
	
	Based on:
	
	
	StackBlur - a fast almost Gaussian Blur For Canvas v0.2
	
	Version: 	0.2
	Author:		Mario Klingemann
	Contact: 	mario@quasimondo.com
	Website:	http://www.quasimondo.com/StackBlurForCanvas
	Twitter:	@quasimondo
	
	In case you find this class useful - especially in commercial projects -
	I am not totally unhappy for a small donation to my PayPal account
	mario@quasimondo.de
	
	Copyright (c) 2010 Mario Klingemann
	
	
	
	
	Permission is hereby granted, free of charge, to any person
	obtaining a copy of this software and associated documentation
	files (the "Software"), to deal in the Software without
	restriction, including without limitation the rights to use,
	copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the
	Software is furnished to do so, subject to the following
	conditions:
	
	The above copyright notice and this permission notice shall be
	included in all copies or substantial portions of the Software.
	
	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
	EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
	OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
	NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
	HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
	WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
	FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
	OTHER DEALINGS IN THE SOFTWARE.
	
	*/
	
	public class StackBlur
	{
		static int[] mul_table = new int[]{
	        512,512,456,512,328,456,335,512,405,328,271,456,388,335,292,512,
	        454,405,364,328,298,271,496,456,420,388,360,335,312,292,273,512,
	        482,454,428,405,383,364,345,328,312,298,284,271,259,496,475,456,
	        437,420,404,388,374,360,347,335,323,312,302,292,282,273,265,512,
	        497,482,468,454,441,428,417,405,394,383,373,364,354,345,337,328,
	        320,312,305,298,291,284,278,271,265,259,507,496,485,475,465,456,
	        446,437,428,420,412,404,396,388,381,374,367,360,354,347,341,335,
	        329,323,318,312,307,302,297,292,287,282,278,273,269,265,261,512,
	        505,497,489,482,475,468,461,454,447,441,435,428,422,417,411,405,
	        399,394,389,383,378,373,368,364,359,354,350,345,341,337,332,328,
	        324,320,316,312,309,305,301,298,294,291,287,284,281,278,274,271,
	        268,265,262,259,257,507,501,496,491,485,480,475,470,465,460,456,
	        451,446,442,437,433,428,424,420,416,412,408,404,400,396,392,388,
	        385,381,377,374,370,367,363,360,357,354,350,347,344,341,338,335,
	        332,329,326,323,320,318,315,312,310,307,304,302,299,297,294,292,
	        289,287,285,282,280,278,275,273,271,269,267,265,263,261,259
		};
        
   
		static int[] shr_table = new int[]{
		     9, 11, 12, 13, 13, 14, 14, 15, 15, 15, 15, 16, 16, 16, 16, 17, 
			17, 17, 17, 17, 17, 17, 18, 18, 18, 18, 18, 18, 18, 18, 18, 19, 
			19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 19, 20, 20, 20,
			20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 21,
			21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 21,
			21, 21, 21, 21, 21, 21, 21, 21, 21, 21, 22, 22, 22, 22, 22, 22, 
			22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22,
			22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 23, 
			23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23,
			23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23,
			23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 23, 
			23, 23, 23, 23, 23, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 
			24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
			24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
			24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24,
			24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24
		};
		
		class BlurStack {
			public int a = 0, r = 0, g = 0, b = 0;
			public BlurStack next = null;
		}
		
		class BlurStackState {
			public BlurStack stackStart = new BlurStack();
			public BlurStack stack = null;
			public BlurStack stackEnd = null;
			public BlurStack stackIn = null;
			public BlurStack stackOut = null;
			public int radiusPlus1;
			public BlurStackState(int div, int radiusPlus1) {
				this.radiusPlus1 = radiusPlus1;
				stack = stackStart;
				for ( int i = 1; i < div; i++ )
				{
					stack = stack.next = new BlurStack();
					if ( i == radiusPlus1 ) stackEnd = stack;
				}
				stack.next = stackStart;
			}
			public void Clear(int pa, int pr, int pg, int pb) {
				stack = stackStart;
				
				for( int i = 0; i < radiusPlus1; i++ )
				{
					stack.a = pa;
					stack.r = pr;
					stack.g = pg;
					stack.b = pb;
					stack = stack.next;
				}
			}
		}
		
		private static void InitializeRadiusBasedVars(int inRadius, out int radius, out int div, out int radiusPlus1, out int sumFactor, out int mul_sum, out int shr_sum) {
			radius = inRadius;
			div = radius + radius + 1;
			radiusPlus1  = radius + 1;
			sumFactor = radiusPlus1 * ( radiusPlus1 + 1 ) / 2;
			mul_sum = mul_table[radius];
			shr_sum = shr_table[radius];
		}
		
		unsafe public static void Apply(int radiusX, int radiusY, int width, int height, BitmapData data)
		{
			int i, x, y, p, yp, yi, yw, r_sum, g_sum, b_sum, a_sum,r_out_sum, g_out_sum, b_out_sum, a_out_sum,
			r_in_sum, g_in_sum, b_in_sum, a_in_sum, pr, pg, pb, pa, rbs;

			var w4 = width << 2;
			var widthMinus1  = width - 1;
			var heightMinus1 = height - 1;
			
			byte* pixels = (byte*)(data.Scan0.ToPointer());
			
			int div, radiusPlus1, sumFactor, mul_sum, shr_sum, radius;
			BlurStackState bss;
			
			if(radiusX > 0) {
				InitializeRadiusBasedVars(radiusX, out radius, out div, out radiusPlus1, out sumFactor, out mul_sum, out shr_sum);
				bss = new StackBlur.BlurStackState(div, radiusPlus1);
				
				yw = yi = 0;
				
				for ( y = 0; y < height; y++ )
				{
					r_in_sum = g_in_sum = b_in_sum = a_in_sum = r_sum = g_sum = b_sum = a_sum = 0;
					
					a_out_sum = radiusPlus1 * ( pa = pixels[yi] );
					r_out_sum = radiusPlus1 * ( pr = pixels[yi+1] );
					g_out_sum = radiusPlus1 * ( pg = pixels[yi+2] );
					b_out_sum = radiusPlus1 * ( pb = pixels[yi+3] );
					
					a_sum += sumFactor * pa;
					r_sum += sumFactor * pr;
					g_sum += sumFactor * pg;
					b_sum += sumFactor * pb;
					
					bss.Clear(pa, pr, pg, pb);
					
					
					for( i = 1; i < radiusPlus1; i++ )
					{
						p = yi + (( widthMinus1 < i ? widthMinus1 : i ) << 2 );
						a_sum += ( bss.stack.a = ( pa = pixels[p])) * ( rbs = radiusPlus1 - i );
						r_sum += ( bss.stack.r = ( pr = pixels[p+1])) * rbs;
						g_sum += ( bss.stack.g = ( pg = pixels[p+2])) * rbs;
						b_sum += ( bss.stack.b = ( pb = pixels[p+3])) * rbs;
						
						a_in_sum += pa;
						r_in_sum += pr;
						g_in_sum += pg;
						b_in_sum += pb;
						
						bss.stack = bss.stack.next;
					}
					
					
					bss.stackIn = bss.stackStart;
					bss.stackOut = bss.stackEnd;
					for ( x = 0; x < width; x++ )
					{
						pixels[yi]   = (byte)((a_sum * mul_sum) >> shr_sum);
						pixels[yi+1] = (byte)((r_sum * mul_sum) >> shr_sum);
						pixels[yi+2] = (byte)((g_sum * mul_sum) >> shr_sum);
						pixels[yi+3] = (byte)((b_sum * mul_sum) >> shr_sum);
						
						a_sum -= a_out_sum;
						r_sum -= r_out_sum;
						g_sum -= g_out_sum;
						b_sum -= b_out_sum;
						
						a_out_sum -= bss.stackIn.a;
						r_out_sum -= bss.stackIn.r;
						g_out_sum -= bss.stackIn.g;
						b_out_sum -= bss.stackIn.b;
						
						p =  ( yw + ( ( p = x + radius + 1 ) < widthMinus1 ? p : widthMinus1 ) ) << 2;
						
						a_in_sum += ( bss.stackIn.a = pixels[p]);
						r_in_sum += ( bss.stackIn.r = pixels[p+1]);
						g_in_sum += ( bss.stackIn.g = pixels[p+2]);
						b_in_sum += ( bss.stackIn.b = pixels[p+3]);
						
						a_sum += a_in_sum;
						r_sum += r_in_sum;
						g_sum += g_in_sum;
						b_sum += b_in_sum;
						
						bss.stackIn = bss.stackIn.next;
						
						a_out_sum += ( pa = bss.stackOut.a );
						r_out_sum += ( pr = bss.stackOut.r );
						g_out_sum += ( pg = bss.stackOut.g );
						b_out_sum += ( pb = bss.stackOut.b );
						
						a_in_sum -= pa;
						r_in_sum -= pr;
						g_in_sum -= pg;
						b_in_sum -= pb;
						
						bss.stackOut = bss.stackOut.next;
			
						yi += 4;
					}
					yw += width;
				}
			}
			
			if(radiusY > 0) {
			
				InitializeRadiusBasedVars(radiusY, out radius, out div, out radiusPlus1, out sumFactor, out mul_sum, out shr_sum);
				bss = new StackBlur.BlurStackState(div, radiusPlus1);
	
				
				for ( x = 0; x < width; x++ )
				{
					r_in_sum = g_in_sum = b_in_sum = a_in_sum = r_sum = g_sum = b_sum = a_sum = 0;
					
					yi = x << 2;
					a_out_sum = radiusPlus1 * ( pa = pixels[yi]);
					r_out_sum = radiusPlus1 * ( pr = pixels[yi+1]);
					g_out_sum = radiusPlus1 * ( pg = pixels[yi+2]);
					b_out_sum = radiusPlus1 * ( pb = pixels[yi+3]);
					
					a_sum += sumFactor * pa;
					r_sum += sumFactor * pr;
					g_sum += sumFactor * pg;
					b_sum += sumFactor * pb;
					
					bss.Clear(pa, pr, pg, pb);
					
					yp = width;
					
					for( i = 1; i <= radius; i++ )
					{
						yi = ( yp + x ) << 2;
						
						a_sum += ( bss.stack.a = ( pa = pixels[yi])) * ( rbs = radiusPlus1 - i );
						r_sum += ( bss.stack.r = ( pr = pixels[yi+1])) * rbs;
						g_sum += ( bss.stack.g = ( pg = pixels[yi+2])) * rbs;
						b_sum += ( bss.stack.b = ( pb = pixels[yi+3])) * rbs;
					   
						a_in_sum += pa;
						r_in_sum += pr;
						g_in_sum += pg;
						b_in_sum += pb;
						
						bss.stack = bss.stack.next;
					
						if( i < heightMinus1 )
						{
							yp += width;
						}
					}
					
					yi = x;
					bss.stackIn = bss.stackStart;
					bss.stackOut = bss.stackEnd;
					for ( y = 0; y < height; y++ )
					{
						p = yi << 2;
						pixels[p]   = (byte)((a_sum * mul_sum) >> shr_sum);
						pixels[p+1] = (byte)((r_sum * mul_sum) >> shr_sum);
						pixels[p+2] = (byte)((g_sum * mul_sum) >> shr_sum);
						pixels[p+3] = (byte)((b_sum * mul_sum) >> shr_sum);
						
						a_sum -= a_out_sum;
						r_sum -= r_out_sum;
						g_sum -= g_out_sum;
						b_sum -= b_out_sum;
					   
						a_out_sum -= bss.stackIn.a;
						r_out_sum -= bss.stackIn.r;
						g_out_sum -= bss.stackIn.g;
						b_out_sum -= bss.stackIn.b;
						
						p = ( x + (( ( p = y + radiusPlus1) < heightMinus1 ? p : heightMinus1 ) * width )) << 2;
						
						a_sum += ( a_in_sum += ( bss.stackIn.a = pixels[p]));
						r_sum += ( r_in_sum += ( bss.stackIn.r = pixels[p+1]));
						g_sum += ( g_in_sum += ( bss.stackIn.g = pixels[p+2]));
						b_sum += ( b_in_sum += ( bss.stackIn.b = pixels[p+3]));
					   
						bss.stackIn = bss.stackIn.next;
						
						a_out_sum += ( pa = bss.stackOut.a );
						r_out_sum += ( pr = bss.stackOut.r );
						g_out_sum += ( pg = bss.stackOut.g );
						b_out_sum += ( pb = bss.stackOut.b );
						
						a_in_sum -= pa;
						r_in_sum -= pr;
						g_in_sum -= pg;
						b_in_sum -= pb;
						
						bss.stackOut = bss.stackOut.next;
						
						yi += width;
					}
				}
			}
		}
	}
}
