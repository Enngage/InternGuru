using System;

namespace Core.Helpers
{
    public static class ConvertHelper
    {
        /// <summary>
        /// Formats bytes to MB/KB/GB so that size is easy to read
        /// </summary>
        /// <param name="Bytes">Bytes to convert</param>
        /// <returns>Formatted string with size</returns>
        public static string FormatBytes(int Bytes)
        {
            if (Bytes >= 1073741824)
            {
                decimal size = decimal.Divide(Bytes, 1073741824);
                return String.Format("{0:##.##} GB", size);
            }
            else if (Bytes >= 1048576)
            {
                decimal size = decimal.Divide(Bytes, 1048576);
                return String.Format("{0:##.##} MB", size);
            }
            else if (Bytes >= 1024)
            {
                decimal size = decimal.Divide(Bytes, 1024);
                return String.Format("{0:##.##} KB", size);

            }
            else if (Bytes > 0 & Bytes < 1024)
            {
                decimal size = Bytes;
                return String.Format("{0:##.##} Bytes", size);
            }
            else
            {
                return String.Format("0 Bytes");
            }
        }
    }
}
