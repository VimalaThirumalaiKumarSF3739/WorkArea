using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using WScrollBarVisibility = Microsoft.UI.Xaml.Controls.ScrollBarVisibility;
using ScrollBarVisibility = Microsoft.Maui.ScrollBarVisibility;
using PlatformScrollViewer = Microsoft.UI.Xaml.Controls.ScrollViewer;
using System;

namespace Syncfusion.Maui.Core.Internals
{
    internal partial class SfInteractiveScrollViewHandler : ViewHandler<SfInteractiveScrollView, PlatformScrollViewer>
    {
        #region Private variables
        ScrollToParameters? m_scrollOffsetRequest;
        FrameworkElement? m_content;
        double m_dpi = 96;
        #endregion

        #region Overrided implementation
        protected override PlatformScrollViewer CreatePlatformView()
        {
            PlatformScrollViewer? scrollViewer = new PlatformScrollViewer();
            scrollViewer.Loaded += OnLoaded;
            return scrollViewer;
        }

        protected override void ConnectHandler(PlatformScrollViewer platformView)
        {
            base.ConnectHandler(platformView);
            platformView.ViewChanged += OnViewChanged;
            platformView.KeyDown += OnKeyDown;
            platformView.KeyUp += OnKeyUp;
            platformView.ManipulationInertiaStarting += OnManipulationInertiaStarting;
            platformView.EffectiveViewportChanged += OnEffectiveViewportChanged;
        }

        protected override void DisconnectHandler(PlatformScrollViewer platformView)
        {
            if (m_content != null)
                m_content.SizeChanged -= OnContentSizeChanged;
            platformView.Loaded -= OnLoaded;
            platformView.KeyDown -= OnKeyDown;
            platformView.KeyUp -= OnKeyUp;
            platformView.ViewChanged -= OnViewChanged;
            platformView.EffectiveViewportChanged -= OnEffectiveViewportChanged;
            platformView.ManipulationInertiaStarting -= OnManipulationInertiaStarting;
            platformView.Content = null;
            base.DisconnectHandler(platformView);
        }
        #endregion

        #region Mapping implementation
        public static void MapScrollOrientation(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            handler.UpdateScrollOrientation();
        }

        public static void MapHorizontalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (scrollView.Orientation == ScrollOrientation.Neither ||
                scrollView.Orientation == ScrollOrientation.Vertical)
                return;
            handler.PlatformView.HorizontalScrollBarVisibility = handler.GetWScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
        }

        public static void MapVerticalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (scrollView.Orientation == ScrollOrientation.Neither ||
                scrollView.Orientation == ScrollOrientation.Horizontal)
                return;
            handler.PlatformView.VerticalScrollBarVisibility = handler.GetWScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
        }

        public static void MapScrollTo(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView, object? args)
        {
            ScrollToParameters? parameters = args as ScrollToParameters;
            if (parameters != null)
            {
                handler.ScrollTo(parameters);
            }
        }
        public static void MapContentSize(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.m_content == null)
                return;

            if (scrollView.ContentSize.IsZero || (scrollView.ContentSize.Width == handler.m_content.ActualWidth && 
                scrollView.ContentSize.Height == handler.m_content.ActualHeight))
            {
                scrollView.IsContentLayoutRequested = false;
            }
            else
            {
                scrollView.IsContentLayoutRequested = true;
            }
        }
        public static void MapContent(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null || handler.MauiContext == null)
                return;

            Microsoft.Maui.Controls.View? content = scrollView.PresentedContent;
            if (content != null)
            {
                var platformElement = content.ToPlatform(handler.MauiContext);
                handler.m_content = platformElement;
                //Enable keyboard shortcuts, that allows the default scroll behavior based on key inputs.
                platformElement.IsTabStop = true;
                platformElement.SizeChanged += handler.OnContentSizeChanged;
                handler.PlatformView.Content = platformElement;
            }
        }
        #endregion

        #region Helper implementation
        void UpdateScrollOrientation()
        {
            switch (VirtualView.Orientation)
            {
                case ScrollOrientation.Neither:
                    SetScrollBarVisibility(WScrollBarVisibility.Hidden, WScrollBarVisibility.Hidden);
                    SetScrollMode(ScrollMode.Disabled, ScrollMode.Disabled);
                    break;
                case ScrollOrientation.Vertical:
                    SetScrollBarVisibility(WScrollBarVisibility.Hidden, GetWScrollBarVisibility(VirtualView.VerticalScrollBarVisibility));
                    SetScrollMode(ScrollMode.Disabled, ScrollMode.Enabled);
                    break;
                case ScrollOrientation.Horizontal:
                    SetScrollBarVisibility(GetWScrollBarVisibility(VirtualView.HorizontalScrollBarVisibility), WScrollBarVisibility.Hidden);
                    SetScrollMode(ScrollMode.Enabled, ScrollMode.Disabled);
                    break;
                case ScrollOrientation.Both:
                    SetScrollBarVisibility(GetWScrollBarVisibility(VirtualView.HorizontalScrollBarVisibility),
                        GetWScrollBarVisibility(VirtualView.VerticalScrollBarVisibility));
                    SetScrollMode(ScrollMode.Enabled, ScrollMode.Enabled);
                    break;
            }
        }

        void SetScrollBarVisibility(WScrollBarVisibility hScrollBarVisibility, WScrollBarVisibility vScrollBarVisibility)
        {
            PlatformView.HorizontalScrollBarVisibility = hScrollBarVisibility;
            PlatformView.VerticalScrollBarVisibility = vScrollBarVisibility;
        }

        void SetScrollMode(ScrollMode horizontalScrollMode, ScrollMode verticalScrollMode)
        {
            PlatformView.HorizontalScrollMode = horizontalScrollMode;
            PlatformView.VerticalScrollMode = verticalScrollMode;
        }

        WScrollBarVisibility GetWScrollBarVisibility(ScrollBarVisibility scrollBarVisibility)
        {
            switch (scrollBarVisibility)
            {
                case ScrollBarVisibility.Always:
                    return WScrollBarVisibility.Visible;
                case ScrollBarVisibility.Never:
                    return WScrollBarVisibility.Hidden;
                default:
                    return WScrollBarVisibility.Auto;
            }
        }

        void ScrollTo(ScrollToParameters parameters)
        {
            if (VirtualView.IsContentLayoutRequested == false)
            {
                if (parameters.ScrollX != PlatformView.HorizontalOffset || parameters.ScrollY != PlatformView.VerticalOffset)
                {
                    PlatformView.ChangeView(parameters.ScrollX, parameters.ScrollY, null, !parameters.Animated);
                }
                else
                {
                    ScrollChangedEventArgs scrolledEventArgs = new ScrollChangedEventArgs(PlatformView.HorizontalOffset, PlatformView.VerticalOffset, VirtualView.ScrollX, VirtualView.ScrollY);
                    VirtualView.OnScrollChanged(scrolledEventArgs);
                }
                m_scrollOffsetRequest = null;
            }
            else
                m_scrollOffsetRequest = parameters;
        }

        private void OnContentSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (VirtualView.IsContentLayoutRequested)
                VirtualView.IsContentLayoutRequested = false;

            if (m_scrollOffsetRequest != null)
            {
                ScrollTo(m_scrollOffsetRequest);
                m_scrollOffsetRequest = null;
            }
        }

        private void OnEffectiveViewportChanged(FrameworkElement sender, EffectiveViewportChangedEventArgs args)
        {
            VirtualView.ViewportWidth = PlatformView.ViewportWidth;
            VirtualView.ViewportHeight = PlatformView.ViewportHeight;
        }

        private void OnManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            if (VirtualView == null || VirtualView.Orientation == ScrollOrientation.Neither)
                return;

            // NOTE: This calculation is made practicaly with user experience, since there is no reference
            // for the correct caluclation is obtained for Windows. This is tested with different resolutions.
            // The friction factor is a manual constant, tested with multiple resolutions.
            double frictionFactor = 8;
            double minFlingVelocity = 0.25;
            double flingDistanceX = PlatformView.HorizontalOffset;
            double flingDistanceY = PlatformView.VerticalOffset;

            // Squaring the value, since the fling grows exponentially.
            double velocityX = Math.Pow(e.Velocities.Linear.X, 2);
            double velocityY = Math.Pow(e.Velocities.Linear.Y, 2);

            double signX = e.Velocities.Linear.X / Math.Abs(e.Velocities.Linear.X);
            double signY = e.Velocities.Linear.Y / Math.Abs(e.Velocities.Linear.Y);

            if (velocityX < minFlingVelocity && velocityY < minFlingVelocity)
                return;

            if (VirtualView.Orientation != ScrollOrientation.Vertical)
            {
                if (velocityX >= minFlingVelocity)
                {
                    flingDistanceX += (-signX * velocityX * m_dpi * frictionFactor);
                }
            }

            if (VirtualView.Orientation != ScrollOrientation.Horizontal)
            {
                if (velocityY >= minFlingVelocity)
                {
                    flingDistanceY += (-signY * velocityY * m_dpi * frictionFactor);
                }
            }
            PlatformView.ChangeView(flingDistanceX, flingDistanceY, null);
            VirtualView.IsScrolling = true;
            e.Handled = true;
        }

        private void OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Shift)
            {
                // Restore the original scroll mode which was before the shift key was pressed.
                UpdateScrollOrientation();
            }
        }

        private void OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Shift)
            {
                PlatformView.VerticalScrollMode = ScrollMode.Disabled;
                PlatformView.VerticalScrollBarVisibility = WScrollBarVisibility.Hidden;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is PlatformScrollViewer platformScrollViewer)
            {
                m_dpi = platformScrollViewer.XamlRoot.RasterizationScale * 96;
            }
        }

        private void OnViewChanged(object? sender, ScrollViewerViewChangedEventArgs e)
        {
            if (VirtualView.IsContentLayoutRequested == true)
                return;

            ScrollChangedEventArgs scrolledEventArgs = new ScrollChangedEventArgs(PlatformView.HorizontalOffset, PlatformView.VerticalOffset, VirtualView.ScrollX, VirtualView.ScrollY);
            VirtualView.OnScrollChanged(scrolledEventArgs);

            if (!e.IsIntermediate)
            {
                VirtualView.SendScrollFinished();
                VirtualView.IsScrolling = false;
            }
            else
                VirtualView.IsScrolling = true;
        }
        #endregion
    }
}