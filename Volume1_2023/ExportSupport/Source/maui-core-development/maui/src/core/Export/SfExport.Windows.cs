using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Maui;

namespace Syncfusion.Maui.Core
{
    public static partial class SfExport
    {
        const double imageResolution = 96.0;

        /// <summary>
        /// To render the view as a stream in the desired file format.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public async static Task<Stream> GetStreamAsync(this View view, ImageFileFormat format)
        {
            if (view != null && view.Handler is IViewHandler viewHandler)
            {
                if (viewHandler.PlatformView is UIElement uIElement)
                {
                    var renderTargetBitmap = new RenderTargetBitmap();
                    await renderTargetBitmap.RenderAsync(uIElement);
                    var pixel = await renderTargetBitmap.GetPixelsAsync();
                    var randomStream = new InMemoryRandomAccessStream();
                    var imageFormat = ConvertToBitmapEncoder(format);
                    var encoder = await BitmapEncoder.CreateAsync(imageFormat, randomStream);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)renderTargetBitmap.PixelWidth, (uint)renderTargetBitmap.PixelHeight, imageResolution, imageResolution, pixel.ToArray());
                    await encoder.FlushAsync();

                    return randomStream.AsStream();
                }
            }
            
            return Stream.Null;
        }

        /// <summary>
        /// To render the desired bitmap encoder format. 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        static Guid ConvertToBitmapEncoder(ImageFileFormat format) =>
            format switch
            {
                ImageFileFormat.Jpeg => BitmapEncoder.JpegEncoderId,
                ImageFileFormat.Png => BitmapEncoder.PngEncoderId,
                _ => BitmapEncoder.JpegEncoderId
            };
    }
}
