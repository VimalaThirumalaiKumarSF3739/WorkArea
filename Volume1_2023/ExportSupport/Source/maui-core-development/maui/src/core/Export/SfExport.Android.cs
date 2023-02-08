using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.IO;
using System.Threading.Tasks;
using NativeBitmap = Android.Graphics.Bitmap;
using Android.Graphics;
using Stream = System.IO.Stream;
using Microsoft.Maui;

namespace Syncfusion.Maui.Core
{
    public static partial class SfExport
    {
        /// <summary>
        /// To render the view as a stream in the desired file format.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static async Task<Stream> GetStreamAsync(this View view, ImageFileFormat format)
        {
            if (view != null && view.Handler is IViewHandler viewHandler)
            {
                if (viewHandler.PlatformView is Android.Views.View nativeView)
                {
                    NativeBitmap bitmap = GetBitmapRender(nativeView, new Size(nativeView.Width, nativeView.Height));

                    if (bitmap != null)
                    {
                        Stream stream = new MemoryStream();
                        ConvertBitmapToStream(bitmap, format, stream);
                        //To return a Task<Stream> method, an async method with a delay of 1 millisecond is used
                        await Task.Delay(1);
                        stream.Position = 0;
                        return stream;
                    }
                }
            }
            return Stream.Null;
        }

        /// <summary>
        /// To render the native bitmap of the passed view.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        static NativeBitmap GetBitmapRender(Android.Views.View view, Size size)
        {
            if (NativeBitmap.Config.Argb8888 != null)
            {
                var width = size.Width;
                var height = size.Height;
                var bitmap = NativeBitmap.CreateBitmap((int)width, (int)height, NativeBitmap.Config.Argb8888);
                
                if(bitmap != null)
                {
                    var canvas = new Canvas(bitmap);
                    view.Draw(canvas);
                    return bitmap;
                }
            }

            NativeBitmap native =  GetBitmapRender(view, Size.Zero);
            return native;
        }

        /// <summary>
        /// To render the bitmap as a stream in the desired file format.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="format"></param>
        /// <param name="stream"></param>
        static void ConvertBitmapToStream(NativeBitmap bitmap, ImageFileFormat format, Stream stream)
        {
            switch (format)
            {
                case ImageFileFormat.Png:
                    bitmap.Compress(NativeBitmap.CompressFormat.Png, 100, stream);
                    break;
                case ImageFileFormat.Jpeg:
                default:
                    bitmap.Compress(NativeBitmap.CompressFormat.Jpeg, 100, stream);
                    break;
            }
        }
    }
}

