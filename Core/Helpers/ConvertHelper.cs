namespace Core.Helpers
{
    public static class ConvertHelper
    {
        /// <summary>
        /// Formats bytes to MB/KB/GB so that size is easy to read
        /// </summary>
        /// <param name="bytes">Bytes to convert</param>
        /// <returns>Formatted string with size</returns>
        public static string FormatBytes(int bytes)
        {
            if (bytes >= 1073741824)
            {
                var size = decimal.Divide(bytes, 1073741824);
                return $"{size:##.##} GB";
            }
            if (bytes >= 1048576)
            {
                var size = decimal.Divide(bytes, 1048576);
                return $"{size:##.##} MB";
            }
            if (bytes >= 1024)
            {
                var size = decimal.Divide(bytes, 1024);
                return $"{size:##.##} KB";

            }
            if (bytes > 0 & bytes < 1024)
            {
                decimal size = bytes;
                return $"{size:##.##} Bytes";
            }
            return "0 Bytes";
        }
    }
}
