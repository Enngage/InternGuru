using Common.Project;
using System.Security.Cryptography;
using System.Text;

namespace Common.Helpers
{
    public static class HashHelper
    {
        /// <summary>
        /// Store version hash in static property so that it doesn't need to re-calculated all over again
        /// </summary>
        private static string versionHash = null;

        /// <summary>
        ///  Gets hash using current version information
        /// </summary>
        public static string GetVersionHash()
        {
            if (string.IsNullOrEmpty(versionHash))
            {
                // initialize hash
                var version = VersionInfo.Version;

                versionHash = HashHelper.GetHashString(version.Major.ToString() + version.Minor.ToString());
            }

            return versionHash;
        }

        /// <summary>
        /// Calculates hash from given input string
        /// </summary>
        /// <param name="inputString">Text to hash</param>
        /// <returns>Hashed bytes</returns>
        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA1.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        /// <summary>
        /// Gets hashed string from given input string
        /// </summary>
        /// <param name="inputString">Text to hash</param>
        /// <returns>Hashed string</returns>
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
