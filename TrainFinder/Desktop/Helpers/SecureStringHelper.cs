using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Desktop
{
    public static class SecureStringHelper
    {
        public static string UnsSecure(this SecureString password)
        {
            if (password == null)
                return string.Empty;
            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(password);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
        public static bool ComparePassword(this SecureString securePassword, SecureString secureConfirmPassword,
            out string password)
        {
            password = null;
            if (securePassword.UnsSecure().Equals(password=secureConfirmPassword.UnsSecure()))
            {
                return true;
            }
            return false;
        }
    }
}