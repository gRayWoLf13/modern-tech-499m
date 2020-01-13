using System.Text;
using System.Security.Cryptography;

namespace modern_tech_499m.Security
{
    /// <summary>
    /// Helpers for the <see cref="string"/> class
    /// </summary>
    public static class StringHelpers
    {
        /// <summary>
        /// Hashes a string using <see cref="SHA256"/> algorithm
        /// </summary>
        /// <param name="inputString">A string to hash</param>
        /// <returns>A hash result as byte array</returns>
        public static byte[] GetStringHash(this string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return null;
            var algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.Unicode.GetBytes(inputString));
        }
    }
}
