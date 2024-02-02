using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts password string to hash.
        /// </summary>
        /// <param name="password">Entered password.</param>
        /// <returns>Hash of password.</returns>
        public static string ConvertPasswordToHash(this string password)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
