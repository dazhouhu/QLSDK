using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSDK.Core
{
    internal class ImageHelper
    {
        #region Image
        public static Bitmap CaptureScreen(object screenHandle,ref Rectangle rect)
        {
            var screen = Screen.AllScreens.Where(s => s.DeviceName == screenHandle.ToString()).FirstOrDefault();
            if (null == screen) throw new Exception("显示器不存在");

            
            int x = screen.WorkingArea.X;
            int y = screen.WorkingArea.Y;
            int width = screen.WorkingArea.Width;
            int height = screen.WorkingArea.Height;
            //按照屏幕宽高创建位图
            var image = new Bitmap(width, height);
            //从一个继承自Image类的对象中创建Graphics对象
            using (Graphics gc = Graphics.FromImage(image))
            {
                //抓屏并拷贝到myimage里
                gc.CopyFromScreen(new Point(x, y), new Point(0, 0), new Size(width, height));
            }
            rect.Location = new Point(x, y);
            rect.Size = new Size(width, height);

            return image;
        }

        public static Bitmap CaptureApplication(object appHandle, ref Rectangle rect)
        {
            var handle = (IntPtr)appHandle;
            IntPtr foreWindow = User32.GetForegroundWindow();
           
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);

            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);  //应用程序窗口大小

            int x = windowRect.Left;
            int y=windowRect.Top;
            int width = windowRect.Right - windowRect.Left;
            int height = windowRect.Bottom - windowRect.Top;
 
            if (handle != foreWindow)
            {
                //是否当前窗口，不是则显示黑色
                rect = new Rectangle(0, 0, 0, 0);
                return new Bitmap(width, height);
            }

            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);


            rect.Location = new Point(x, y);
            rect.Size = new Size(width, height);

            /*
            if (!User32.IsWindowVisible(handle))
            {
                rect.Size = new Size(0, 0);
            }
            else
            {
                var ltPoint = new Point(x+10, y+10);
                var rtPoint = new Point(x + width-10, y+10);
                var lbPoint = new Point(x+10, y + height- 100);
                var rbPoint = new Point(x + width-10, y + height-100);

                var ltHandle = User32.WindowFromPoint(ltPoint);
                var rtHandle = User32.WindowFromPoint(rtPoint);
                var lbHandle = User32.WindowFromPoint(lbPoint);
                var rbHandle = User32.WindowFromPoint(rbPoint);
                if (ltHandle!= handle || rtHandle != handle || lbHandle != handle || rbHandle != handle)
                {
                    rect.Size = new Size(0, 0);
                }
                else 
                {
                    rect.Size = new Size(width, height);
                }
                
            }
            */
            return (Bitmap)img;
        }

        public static Bitmap ScaleImage(Bitmap image,Size newSize)
        {
            if(newSize.Width==0 || newSize.Height==0)
            {
                return image;
            }
            var width = image.Width;
            var height = image.Height;
            if(image.Width==newSize.Width && image.Height==newSize.Height)
            {//大小一致不处理
                return image;
            }
            int newWidth = newSize.Width;
            int newHeight = newSize.Height;
            // 计算宽高缩放率
            float scaleWidth = ((float)newWidth) / width;
            float scaleHeight = ((float)newHeight) / height;
            var scale = Math.Min(scaleWidth, scaleHeight);
            int newW = (int)(width * scale);
            var newH = (int)(height * scale);
            Bitmap bitmap = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var x = (newWidth - newW) / 2;
                var y = (newHeight - newH) / 2;
                g.DrawImage(image, new Rectangle(x, 0, newW, newH), new Rectangle(0, 0, width, height), GraphicsUnit.Pixel);
            }
            return bitmap;
        }
        #endregion

        #region GetBytes

        public static byte[] GetRGBA(Bitmap image)
        {
            int w = image.Width;
            int h = image.Height;
            var buffer = new byte[w * h * 4];
            /*
            var idx = 0;
            for (var row = 0; row < h; row++)
            {
                for (var col = 0; col < w; col++)
                {
                    var color = image.GetPixel(col, row);
                    //转换为RGBA模式
                    buffer[idx] = color.R;
                    idx++;
                    buffer[idx] = color.G;
                    idx++;
                    buffer[idx] = color.B;
                    idx++;
                    buffer[idx] = color.A;
                    idx++;
                }
            }
            */
            var idx = 0;
            for (var row = h-1; row >=0; row--)
            {
                for (var col = 0; col < w; col++)
                {
                    var color = image.GetPixel(col, row);
                    //转换为RGBA模式
                    buffer[idx] = color.R;
                    idx++;
                    buffer[idx] = color.G;
                    idx++;
                    buffer[idx] = color.B;
                    idx++;
                    buffer[idx] = color.A;
                    idx++;
                }
            }
            return buffer;
        }

        public static byte[] GetYUV(Bitmap image)
        {
            var width = image.Width;
            var height = image.Height;
            var pixels = new Color[width * height];
            /*
            for(var i=0;i<height;i++)
            {
                for(var j=0;j<width;j++)
                {
                    pixels[i * width + j] = image.GetPixel(j, i);
                }
            }
            */
            for (var i = height-1; i >=0; i--)
            {
                for (var j = 0; j < width; j++)
                {
                    pixels[i * width + j] = image.GetPixel(j, i);
                }
            }

            byte[] yuv = new byte[width * height * 3 / 2];
            Color2YUV(yuv, pixels, width, height);
            
            return yuv;
        }
        private static void Color2YUV(byte[] yuv420sp, Color[] pixel, int width, int height)
        {
            int frameSize = width * height;

            int yIndex = 0;
            int uvIndex = frameSize;

            int a, R, G, B, Y, U, V;
            int index = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {

                    a = pixel[index].A;
                    R = pixel[index].R;
                    G = pixel[index].G;
                    B = pixel[index].B;

                    // well known RGB to YUV algorithm
                    Y = ((66 * R + 129 * G + 25 * B + 128) >> 8) + 16;
                    U = ((-38 * R - 74 * G + 112 * B + 128) >> 8) + 128;
                    V = ((112 * R - 94 * G - 18 * B + 128) >> 8) + 128;

                    // NV21 has a plane of Y and interleaved planes of VU each sampled by a factor of 2
                    //    meaning for every 4 Y pixels there are 1 V and 1 U.  Note the sampling is every other
                    //    pixel AND every other scanline.
                    yuv420sp[yIndex++] = (byte)((Y < 0) ? 0 : ((Y > 255) ? 255 : Y));
                    if (j % 2 == 0 && index % 2 == 0)
                    {
                        yuv420sp[uvIndex++] = (byte)((V < 0) ? 0 : ((V > 255) ? 255 : V));
                        yuv420sp[uvIndex++] = (byte)((U < 0) ? 0 : ((U > 255) ? 255 : U));
                    }

                    index++;
                }
            }
        }
        #endregion

    }

    /// <summary>
    /// Helper class containing Gdi32 API functions
    /// </summary>
    internal class GDI32
    {

        public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
            int nWidth, int nHeight, IntPtr hObjectSource,
            int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
            int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    }

    /// <summary>
    /// Helper class containing User32 API functions
    /// </summary>
    internal class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("gdi32.dll")]
        public static extern int GetClipBox(IntPtr hDC, ref RECT rect);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point point);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
    }
}
