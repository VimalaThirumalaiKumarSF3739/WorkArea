#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIScrollView;
#elif __ANDROID__
using PlatformView = Syncfusion.Maui.Core.Hosting.ExtMauiScrollView;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.Controls.ScrollViewer;
#elif NETSTANDARD || ((NET6_0 || NET7_0) && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif
using Microsoft.Maui;

namespace Syncfusion.Maui.Core.Hosting
{
    /// <summary>
    /// ToDo
    /// </summary>
    public partial interface IScrollViewHandler : IViewHandler
    {
        /// <summary>
        /// ToDo
        /// </summary>
        new IScrollView VirtualView { get; }

        /// <summary>
        /// ToDo
        /// </summary>
        new PlatformView PlatformView { get; }
    }
}