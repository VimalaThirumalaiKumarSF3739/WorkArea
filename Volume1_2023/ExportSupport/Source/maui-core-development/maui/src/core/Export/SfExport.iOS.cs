using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.IO;
using System.Threading.Tasks;
using UIKit;

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
                if (viewHandler.PlatformView is UIView uiView)
                {
                    UIGraphics.BeginImageContextWithOptions(uiView.Bounds.Size, false, 0);
                    var context = UIGraphics.GetCurrentContext();
                    uiView.DrawViewHierarchy(uiView.Bounds, afterScreenUpdates: true);
                    var image = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();
                    //To return a Task<Stream> method, an async method with a delay of 1 millisecond is used
                    await Task.Delay(1);

                    if (image != null)
                    {
                        Stream stream = ConvertImageToStream(image, format);
                        return stream;
                    }
                }
            }
            return Stream.Null;
        }

        /// <summary>
        /// To render the image as a stream in desired file format.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        static Stream ConvertImageToStream(UIImage image, ImageFileFormat format)
        {
            if (format == ImageFileFormat.Png)
            {
                return image.AsPNG().AsStream();
            }
            else
            {
                return image.AsJPEG().AsStream();
            }
        }
    }
}
