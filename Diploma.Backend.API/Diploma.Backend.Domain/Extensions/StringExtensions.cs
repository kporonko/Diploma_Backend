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
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            return hashedPassword;
        }
    }
}
