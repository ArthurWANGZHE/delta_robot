using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using imcpkg;

namespace Example
{
    class Global
    {        
        public const int MAX_NAXIS = 16;
        public static IntPtr g_handle;
        public static int g_naxis;
        public static uint g_imcType;

        private Global()
        {
            g_handle = IntPtr.Zero;
            g_naxis = 0;
        }

        public static bool isOpen()
        {
            return g_handle != IntPtr.Zero;
        }
       public static bool Is4xxxIMC(uint imctype)
       {
           if ((imctype & 0xF000) == 0x4000)
               return true;
           else
               return false;        
       }
       public static string GetFunErrStr()
       {
           string err;
           IntPtr errptr;
        // err = IMC_Pkg.PKG_IMC_GetFunErrStrA();
           errptr = IMC_Pkg.PKG_IMC_GetFunErrStrW();
           err = System.Runtime.InteropServices.Marshal.PtrToStringUni(errptr);
           return err;
       }
       public static string GetRegErrStr(UInt16 errVal)
       {
           string err;
           IntPtr errptr;
           errptr = IMC_Pkg.PKG_IMC_GetRegErrorStrW(errVal);
           err = System.Runtime.InteropServices.Marshal.PtrToStringUni(errptr);
           return err;
       }
    }
}
