using System.Security.Cryptography;
using System.Text;

namespace Intro.Utility.Extensions
{
    // tODO move these outside of dataaccess project (Intro.Core?)
    public static class StringExtensions
    {
        public static byte[] Encrypt(this string password)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            using (var hashAlgorithm = SHA256.Create())
            {
                return hashAlgorithm.ComputeHash(passwordBytes);
            }
        }
    }
}