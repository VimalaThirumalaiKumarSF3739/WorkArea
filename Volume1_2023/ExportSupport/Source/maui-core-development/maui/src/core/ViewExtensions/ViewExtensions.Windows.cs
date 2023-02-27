﻿using Microsoft.Maui.Controls;
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

namespace Syncfusion.Maui.Core.Internals
{
    public static partial class ViewExtensions
    {
        const double imageResolution = 96.0;

        /// <summary>
        /// <para> To convert a view to a stream in a specific file format, the <b> GetStreamAsync </b> method is used. Currently, the supported file formats are <b> JPEG or PNG </b>. </para>
        /// <para> To get the stream for the view in <b> PNG </b> file format, use <b> await view.GetStreamAsync(ImageFileFormat.Png); </b> </para>
        /// <para> To get the stream for the view in <b> JPEG </b> file format, use <b> await view.GetStreamAsync(ImageFileFormat.Jpeg); </b> </para>
        /// <remarks> The view's stream can only be rendered when the view is added to the visual tree. </remarks>
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
        /// <para> To save a view as an image in the desired file format, the <b> SaveAsImage </b> is used.Currently, the supported image formats are <b> JPEG or PNG </b>. </para>
        /// <para> By default, the image format is <b> PNG </b>.For example, <b> view.SaveAsImage("Test"); </b> </para>
        /// <para> To save a view in the <b> PNG </b> image format, the filename should be passed with the <b> ".png" extension </b> 
        /// while to save the image in the <b> JPEG </b> image format, the filename should be passed with the <b> ".jpeg" extension </b>, 
        /// for example, <b> "view.SaveAsImage("Test.png")" and "view.SaveAsImage("Test.jpeg")" </b> respectively. </para>
        /// <para> <b> Saved location: </b>
        /// For <b> Windows, Android, and Mac </b>, the image will be saved in the <b> Pictures folder </b>, and for <b> iOS </b>, the image will be saved in the <b> Photos Album folder </b>. </para>
        /// <remarks> <para> In <b> Windows and Mac </b>, when you save an image with an already existing filename, the existing file is replaced with a new file, but the filename remains the same. </para>
        /// <para> In <b> Android </b>, when you save the same view with an existing filename, the new image will be saved with a filename with a number appended to it, 
        /// for example, Test(1).jpeg and the existing filename Test.jpeg will be removed.When you save a different view with an already existing filename, 
        /// the new image will be saved with a filename with a number will be appended to it, for example, Test(1).jpeg, and the existing filename Test.jpeg will remain in the folder. </para>
        /// <para> In <b> iOS </b>, due to its platform limitation, the image will be saved with the default filename, for example, IMG_001.jpeg, IMG_002.png and more. </para> </remarks>
        /// <remarks> The view can be saved as an image only when the view is added to the visual tree. </remarks>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="fileName"></param>
        public static async void SaveAsImage(this View view, string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string imageExtension = string.IsNullOrEmpty(extension) ? "png" : extension.Trim('.').ToLower();
            var imageFormat = imageExtension == "jpg" || imageExtension == "jpeg" ? ImageFileFormat.Jpeg : ImageFileFormat.Png;
            fileName = Path.GetFileNameWithoutExtension(fileName) + "." + imageFormat.ToString().ToLower();
            string targetFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), fileName);
            using (Stream stream = await view.GetStreamAsync(imageFormat))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                await File.WriteAllBytesAsync(targetFile, memoryStream.ToArray());
            }
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
