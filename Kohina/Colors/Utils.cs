
using System;
using System.Drawing;

namespace Kohina.Colors
{
	public static class Utils
	{
		static ColorToStringConverterDelegate[] cConverters = new ColorToStringConverterDelegate[]{
			Converters.SixHex,
			Converters.SixHexPS,
			Converters.CSSRGB,
			Converters.CSSHSV,
			Converters.DecimalRGB,
			Converters.HSL,
			Converters.CMYK,
		};
		static StringToColorConverterDelegate[] cRecognizers = new StringToColorConverterDelegate[]{
			Recognizers.FromHex,
			Recognizers.FromHSV,
			Recognizers.FromHSL,
			Recognizers.FromRGB
		};
		
		public static Color Recognize(string s) {
			foreach(StringToColorConverterDelegate converter in cRecognizers) {
				Color? c = converter(s);
				if(c.HasValue) {
					return c.Value;
				}
			}
			return Color.Black;
		}
	}
}
