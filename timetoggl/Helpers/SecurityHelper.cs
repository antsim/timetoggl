using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TimeToggl.Helpers
{
    public static class SecurityHelper
    {
        public static String SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string GenerateAuthHeader(string user, SecureString pass)
        {
            string combined = Base64Encode($"{user}:{SecureStringToString(pass)}");
            return $"Basic {combined}";
        }

        public static string GenerateAuthHeader(string user, string pass)
        {
            string combined = Base64Encode($"{user}:{pass}");
            return $"Basic {combined}";
        }
    }
}
