using System.Text;

namespace LinqToYourDoom {
	public static class UtfNoBomEncoding {
		public static readonly UTF8Encoding Utf8NoBom = new(/* byteOrderMark */ false);
		public static readonly UnicodeEncoding Utf16NoBom = new(bigEndian: false, byteOrderMark: false);
		public static readonly UTF32Encoding Utf32NoBom = new(bigEndian: false, byteOrderMark: false);
	}
}
