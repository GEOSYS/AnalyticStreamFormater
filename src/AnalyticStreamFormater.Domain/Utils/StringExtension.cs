using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class StringExtension
    {
        /// <summary>
        /// Compute MD5 hash
        /// </summary>
        /// <returns>MD5</returns>
        public static string GetMd5Hash( this string s )
        {
            using( var md5Hash = MD5.Create() )
            {
                StringBuilder lStringBuilder = new StringBuilder();

                byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(s));
                for( int i = 0; i < bytes.Length; i++ )
                {
                    lStringBuilder.Append(bytes[i].ToString("X2"));
                }

                return lStringBuilder.ToString();
            }
        }
    }
}
