using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Syncfusion.Maui.Core;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core.Hosting
{
    /// <summary>
    /// Represents application host extension, that used to configure handlers defined in Syncfusion maui core.
    /// </summary>
    public static class AppHostBuilderExtensions
    {
        /// <summary>
        /// Configures the implemented handlers in Syncfusion.Maui.Core.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static MauiAppBuilder ConfigureSyncfusionCore(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(IDrawableView), typeof(SfDrawableViewHandler));
                handlers.AddHandler(typeof(IDrawableLayout), typeof(SfViewHandler));
                handlers.AddHandler(typeof(SfDropdownView), typeof(SfDropdownViewHandler));
                handlers.AddHandler(typeof(SfInteractiveScrollView), typeof(SfInteractiveScrollViewHandler));
                handlers.AddHandler(typeof(ListViewScrollViewExt), typeof(ListViewScrollViewHandler));
                handlers.AddHandler(typeof(ISignaturePad), typeof(SignaturePadHandler));
                handlers.AddHandler(typeof(WindowOverlayContainer), typeof(OverlayContainerHandler));
#if __IOS__ || __MACCATALYST__ || __ANDROID__
                handlers.AddHandler(typeof(CustomScrollLayout), typeof(CustomScrollLayoutHandler));
                handlers.AddHandler(typeof(SfInputView), typeof(SfInputViewHandler));
#endif
#if __ANDROID__
                handlers.AddHandler(typeof(SnapLayout), typeof(SnapLayoutHandler));
                handlers.AddHandler(typeof(DataGridScrollViewExt), typeof(DataGridScrollViewHandler));
#endif
            });

            builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("Maui Material Assets.ttf", "Maui Material Assets");
            });

            return builder;
        }
    }
}
