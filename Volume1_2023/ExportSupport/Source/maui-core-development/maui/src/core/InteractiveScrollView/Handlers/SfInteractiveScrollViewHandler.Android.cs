using Android.Content;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System;

namespace Syncfusion.Maui.Core.Internals
{
    internal partial class SfInteractiveScrollViewHandler : ViewHandler<SfInteractiveScrollView, PlatformScrollViewer>
    {
        ScrollToParameters? m_scrollOffsetRequest;
        Android.Views.View? m_content;

        #region Overrided implementation
        protected override PlatformScrollViewer CreatePlatformView()
        {
            PlatformScrollViewer? vScroller = new PlatformScrollViewer(Context);
            return vScroller;
        }

        protected override void ConnectHandler(PlatformScrollViewer platformView)
        {
            base.ConnectHandler(platformView);
            platformView.ScrollChanged += OnScrollChanged;
            platformView.LayoutChange += OnLayoutChange;
        }

        protected override void DisconnectHandler(PlatformScrollViewer platformView)
        {
            platformView.ScrollChanged -= OnScrollChanged;
            platformView.LayoutChange -= OnLayoutChange;
            if (m_content != null)
                m_content.LayoutChange -= OnContentLayoutChange;
            platformView.DisconnectViews();
            base.DisconnectHandler(platformView);
        }
        #endregion

        #region Mapping implementation
        public static void MapScrollOrientation(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            switch (scrollView.Orientation)
            {
                case ScrollOrientation.Neither:
                    handler.SetScrollingEnabled(false, false);
                    break;
                case ScrollOrientation.Vertical:
                    handler.SetScrollingEnabled(false, true);
                    handler.SetVScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
                    break;
                case ScrollOrientation.Horizontal:
                    handler.SetScrollingEnabled(true, false);
                    handler.SetHScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
                    break;
                case ScrollOrientation.Both:
                    handler.SetScrollingEnabled(true, true);
                    handler.SetHScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
                    handler.SetVScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
                    break;
            }
        }

        public static void MapHorizontalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            handler.SetHScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
        }

        public static void MapVerticalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            handler.SetVScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
        }

        public static void MapScrollTo(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView, object? args)
        {
            ScrollToParameters? parameters = args as ScrollToParameters;
            if (parameters != null)
            {
                handler.ScrollTo(parameters);
            }
        }

        public static void MapContent(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null || handler.MauiContext == null)
                return;

            Microsoft.Maui.Controls.View? content = scrollView.PresentedContent;
            if (content != null)
            {
                var nativeElement = content.ToPlatform(handler.MauiContext);
                nativeElement.LayoutChange += handler.OnContentLayoutChange;
                handler.m_content = nativeElement;

                if (handler.PlatformView is PlatformScrollViewer vScroller)
                    vScroller.SetContent(nativeElement);
            }
        }
        #endregion

        #region Helper implementation
        void SetScrollingEnabled(bool enableHorizontalScrolling, bool enableVerticalScrolling)
        {
            PlatformView.SetHorizontalScrollingEnabled(enableHorizontalScrolling);
            PlatformView.SetVerticalScrollingEnabled(enableVerticalScrolling);
        }

        private void OnContentLayoutChange(object? sender, Android.Views.View.LayoutChangeEventArgs e)
        {
            if (VirtualView.IsContentLayoutRequested)
                VirtualView.IsContentLayoutRequested = false;

            if (m_scrollOffsetRequest != null)
            {
                ScrollTo(m_scrollOffsetRequest);
                m_scrollOffsetRequest = null;
            }
        }

        private void OnLayoutChange(object? sender, Android.Views.View.LayoutChangeEventArgs e)
        {
            if (e.OldLeft != e.Left || e.OldTop != e.Top || e.OldRight != e.Right || e.OldBottom != e.Bottom)
            {
                VirtualView.ViewportWidth = Context.FromPixels(PlatformView.ViewPortWidth);
                VirtualView.ViewportHeight = Context.FromPixels(PlatformView.ViewPortHeight);
            }
        }

        private void OnScrollChanged(object? sender, Android.Views.View.ScrollChangeEventArgs e)
        {
            double oldScrollX = Context.FromPixels(e.OldScrollX);
            double oldScrollY = Context.FromPixels(e.OldScrollY);
            double scrollX = Context.FromPixels(e.ScrollX);
            double scrollY = Context.FromPixels(e.ScrollY);

            ScrollChangedEventArgs scrolledEventArgs = new ScrollChangedEventArgs(scrollX, scrollY, oldScrollX, oldScrollY);
            VirtualView.OnScrollChanged(scrolledEventArgs);
        }

        void SetVScrollBarVisibility(ScrollBarVisibility scrollBarVisibility)
        {
            switch (scrollBarVisibility)
            {
                case ScrollBarVisibility.Default:
                    PlatformView.SetVerticalScrollBarEnabled(true);
                    PlatformView.SetVerticalScrollBarFadingEnabled(true);
                    break;
                case ScrollBarVisibility.Always:
                    PlatformView.SetVerticalScrollBarEnabled(true);
                    PlatformView.SetVerticalScrollBarFadingEnabled(false);
                    break;
                case ScrollBarVisibility.Never:
                    PlatformView.SetVerticalScrollBarEnabled(false);
                    break;
            }
        }

        void SetHScrollBarVisibility(ScrollBarVisibility scrollBarVisibility)
        {
            switch (scrollBarVisibility)
            {
                case ScrollBarVisibility.Default:
                    PlatformView.SetHorizontalScrollBarEnabled(true);
                    PlatformView.SetHorizontalScrollBarFadingEnabled(true);
                    break;
                case ScrollBarVisibility.Always:
                    PlatformView.SetHorizontalScrollBarEnabled(true);
                    PlatformView.SetHorizontalScrollBarFadingEnabled(false);
                    break;
                case ScrollBarVisibility.Never:
                    PlatformView.SetHorizontalScrollBarEnabled(false);
                    break;
            }
        }

        void ScrollTo(ScrollToParameters parameters)
        {
            if (VirtualView.IsContentLayoutRequested == false)
            {
                int hOffset = (int)PlatformView.Context.ToPixels(parameters.ScrollX);
                int vOffset = (int)PlatformView.Context.ToPixels(parameters.ScrollY);
                PlatformView.ScrollToOffset(hOffset, vOffset, parameters.Animated, VirtualView.Orientation, () => VirtualView.SendScrollFinished());
            }
            else
                m_scrollOffsetRequest = parameters;
        }
        #endregion
    }
}