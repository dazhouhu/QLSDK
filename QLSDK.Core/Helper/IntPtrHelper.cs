using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QLSDK.Core
{
    /// <summary>
    /// 指针处理类
    /// </summary>
    internal class IntPtrHelper
    {
        public static byte[] IntPtrToBytes(IntPtr ptr, int length)
        {
            var bytes = new byte[length];
            Marshal.Copy(ptr, bytes, 0, length);
            return bytes;
        }

        public static string IntPtrTostring(IntPtr ptr)
        {
            return Marshal.PtrToStringAnsi(ptr);
        }
        public static string IntPtrToUTF8string(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

            List<byte> bytes = new List<byte>();
            for (int offset = 0; ; offset++)
            {
                byte b = Marshal.ReadByte(ptr, offset);
                if (b == 0)
                    break;
                else
                    bytes.Add(b);
            }
            return Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count);
        }

        public static IntPtr IntPtrFromBytes(byte[] bytes)
        {
            int size = bytes.Length;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return buffer;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static IntPtr IntPtrFromObject(object obj)
        {
            var hwnd = GCHandle.Alloc(obj);
            var ptr = GCHandle.ToIntPtr(hwnd);
            if (hwnd.IsAllocated)
                hwnd.Free();
            return ptr;
        }
    }
}
