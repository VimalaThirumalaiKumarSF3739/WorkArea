using Microsoft.Maui;

namespace Syncfusion.Maui.Core.Internals
{
    internal partial class SfInteractiveScrollViewHandler
    {
        public SfInteractiveScrollViewHandler() : base(Mapper, CommandMapper)
        {

        }

        internal static IPropertyMapper<SfInteractiveScrollView, SfInteractiveScrollViewHandler> Mapper =
            new PropertyMapper<SfInteractiveScrollView, SfInteractiveScrollViewHandler>(ViewMapper)
            {
                [nameof(SfInteractiveScrollView.PresentedContent)] = MapContent,
                [nameof(SfInteractiveScrollView.HorizontalScrollBarVisibility)] = MapHorizontalScrollBarVisibility,
                [nameof(SfInteractiveScrollView.VerticalScrollBarVisibility)] = MapVerticalScrollBarVisibility,
                [nameof(SfInteractiveScrollView.Orientation)] = MapScrollOrientation,
#if IOS || MACCATALYST
                [nameof(SfInteractiveScrollView.ContentSize)] = MapContentSize,
                [nameof(SfInteractiveScrollView.IsEnabled)] = MapIsEnabled,
                [nameof(SfInteractiveScrollView.CanBecomeFirstResponder)] = MapCanBecomeFirstResponder,
#endif
#if WINDOWS
                [nameof(SfInteractiveScrollView.ContentSize)] = MapContentSize
#endif
            };

        internal static CommandMapper<SfInteractiveScrollView, SfInteractiveScrollViewHandler> CommandMapper =
            new(ViewCommandMapper)
            {
                [nameof(SfInteractiveScrollView.ScrollTo)] = MapScrollTo
            };
    }
}