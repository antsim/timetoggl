using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TimeToggl.Extensions
{
    public static class SecureStringExtensions
    {
        public static String ToNonSecureString(this SecureString value)
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
    }
}
