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
        /// <summary>
        /// 转换IntPtr到byte数据
        /// </summary>
        public static byte[] IntPtrToBytes(IntPtr ptr, int length)
        {
            var bytes = new byte[length];
            Marshal.Copy(ptr, bytes, 0, length);
            return bytes;
        }
        /// <summary>
        /// 转换IntPtr到字符串数据
        /// </summary>
        public static string IntPtrTostring(IntPtr ptr)
        {
            return Marshal.PtrToStringAnsi(ptr);
        }
        /// <summary>
        /// 转换IntPtr到UTF8数据
        /// </summary>
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
        /// <summary>
        /// 转换byte到IntPtr数据
        /// </summary>
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
        /// <summary>
        /// 转换object到IntPtr数据
        /// </summary>
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
