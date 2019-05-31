using System.Text;

namespace ossServer.Utils
{
    public static class EncodingUtils
    {
        /// <summary>
        /// This method is used to encode the specified string as Quoted-Printable text
        /// </summary>
        /// <param name="encode">The string to encode.</param>
        /// <param name="foldWidth">If greater than zero, line folds with soft line breaks are inserted at the
        /// specified interval.</param>
        /// <returns>The Quoted-Printable encoded string</returns>
        /// <remarks>Character values 9, 32-60, and 62-126 go through as-is.  All others are encoded as "=XX"
        /// where XX is the 2 digit hex value of the character (i.e. =0D=0A for a carriage return and line feed).</remarks>
        public static string ToQuotedPrintable(this string encode, int foldWidth)
        {
            // Don't bother if there's nothing to encode
            if (encode == null || encode.Length == 0)
                return encode;

            StringBuilder sb = new StringBuilder(encode.Length + 100);

            foldWidth--;    // Account for soft line break character

            for (int idx = 0, len = 0; idx < encode.Length; idx++)
            {
                // Characters 9, 32 - 60, and 62 - 126 go through as-is as do any Unicode characters
                if (encode[idx] == '\t' || (encode[idx] > '\x1F' && encode[idx] < '=') || (encode[idx] > '=' &&
                  encode[idx] < '\x7F') || (int)encode[idx] > 255)
                {
                    if (foldWidth > 0 && len + 1 > foldWidth)
                    {
                        sb.Append("=\r\n");     // Soft line break
                        len = 0;
                    }

                    sb.Append(encode[idx]);
                    len++;
                }
                else
                {
                    // All others encode as =XX where XX is the 2 digit hex value of the character
                    if (foldWidth > 0 && len + 3 > foldWidth)
                    {
                        sb.Append("=\r\n");     // Soft line break
                        len = 0;
                    }

                    sb.AppendFormat("={0:X2}", (int)encode[idx]);
                    len += 3;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// This method is used to decode the specified Quoted-Printable encoded string
        /// </summary>
        /// <param name="decode">The string to decode</param>
        /// <returns>The decoded data as a string</returns>
        public static string FromQuotedPrintable(this string decode)
        {
            // Don't bother if there's nothing to decode
            if (decode == null || decode.Length == 0 || decode.IndexOf('=') == -1)
                return decode;

            StringBuilder sb = new StringBuilder(decode.Length);

            string hexDigits = "0123456789ABCDEF";
            int pos1, pos2;

            for (int idx = 0; idx < decode.Length; idx++)
            {
                // Is it an encoded character?
                if (decode[idx] == '=' && idx + 2 <= decode.Length)
                {
                    // Ignore a soft line break
                    if (decode[idx + 1] == '\r' && decode[idx + 2] == '\n')
                    {
                        idx += 2;
                        continue;
                    }

                    pos1 = hexDigits.IndexOf(decode[idx + 1]);
                    pos2 = hexDigits.IndexOf(decode[idx + 2]);

                    if (pos1 != -1 && pos2 != -1)
                    {
                        idx += 2;
                        sb.Append((char)(pos1 * 16 + pos2));
                    }
                }
                else
                    sb.Append(decode[idx]);
            }

            return sb.ToString();
        }
    }
}
