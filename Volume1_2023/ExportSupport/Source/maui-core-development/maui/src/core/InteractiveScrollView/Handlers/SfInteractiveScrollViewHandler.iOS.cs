using CoreGraphics;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System;

namespace Syncfusion.Maui.Core.Internals
{
    internal partial class SfInteractiveScrollViewHandler : ViewHandler<SfInteractiveScrollView, PlatformScrollViewer>
    {
        CGPoint m_offsetOnOrientationChange;
        View? m_mauiView;

        internal bool CanBecomeFirstResponder { get; set; }

        #region Overrided implementation
        protected override PlatformScrollViewer CreatePlatformView()
        {
            PlatformScrollViewer platformView = new PlatformScrollViewer();
            platformView.LayoutChanged += OnLayoutChanged;
            return platformView;
        }

        protected override void ConnectHandler(PlatformScrollViewer platformView)
        {
            base.ConnectHandler(platformView);
            platformView.KeyPressesBegan += OnKeyPressesBegan;
            platformView.KeyPressesEnded += OnKeyPressesEnded;
            platformView.Scrolled += OnScrolled;
            platformView.ScrollAnimationEnded += OnScrollAnimationEnded;
        }

        protected override void DisconnectHandler(PlatformScrollViewer platformView)
        {
            platformView.ScrollAnimationEnded -= OnScrollAnimationEnded;
            platformView.Scrolled -= OnScrolled;
            platformView.KeyPressesBegan -= OnKeyPressesBegan;
            platformView.KeyPressesEnded -= OnKeyPressesEnded;
            platformView.LayoutChanged -= OnLayoutChanged;
            platformView.ClearSubviews();
            base.DisconnectHandler(platformView);
        }
        #endregion

        #region Mapping implementation
        public static void MapCanBecomeFirstResponder(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null)
                return;

            handler.PlatformView.SetCanBecomeFirstResponder(scrollView.CanBecomeFirstResponder);
        }

        public static void MapContent(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null || handler.MauiContext == null)
                return;

            var platformScrollView = handler.PlatformView;
            if (scrollView.PresentedContent != null)
            {
                handler.m_mauiView = scrollView.PresentedContent;
                var nativeContent = scrollView.PresentedContent.ToPlatform(handler.MauiContext);
                platformScrollView.ClearSubviews();
                platformScrollView.AddSubview(nativeContent);
            }
        }

        public static void MapContentSize(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null)
                return;

            handler.PlatformView.Scrolled -= handler.OnScrolled;
            handler.PlatformView.UpdateContentSize(scrollView.ContentSize);
            handler.PlatformView.Scrolled += handler.OnScrolled;
        }

        public static void MapIsEnabled(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null)
                return;

            if (scrollView.IsEnabled)
                handler.PlatformView.ScrollEnabled = scrollView.Orientation != ScrollOrientation.Neither;
            else
                handler.PlatformView.ScrollEnabled = scrollView.IsEnabled;
        }

        public static void MapHorizontalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            handler.PlatformView?.UpdateHorizontalScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
        }

        public static void MapVerticalScrollBarVisibility(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            handler.PlatformView?.UpdateVerticalScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
        }

        public static void MapScrollOrientation(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView)
        {
            if (handler.PlatformView == null)
                return;

            handler.m_offsetOnOrientationChange = handler.PlatformView.ContentOffset;
            handler.PlatformView.ScrollEnabled = scrollView.Orientation != ScrollOrientation.Neither;
            handler.UpdateScrollBarVisibility();
        }

        void UpdateScrollBarVisibility()
        {
            switch (VirtualView.Orientation)
            {
                case ScrollOrientation.Neither:
                    PlatformView?.UpdateVerticalScrollBarVisibility(ScrollBarVisibility.Never);
                    PlatformView?.UpdateHorizontalScrollBarVisibility(ScrollBarVisibility.Never);
                    break;
                case ScrollOrientation.Vertical:
                    PlatformView?.UpdateVerticalScrollBarVisibility(VirtualView.VerticalScrollBarVisibility);
                    PlatformView?.UpdateHorizontalScrollBarVisibility(ScrollBarVisibility.Never);
                    break;
                case ScrollOrientation.Horizontal:
                    PlatformView?.UpdateVerticalScrollBarVisibility(ScrollBarVisibility.Never);
                    PlatformView?.UpdateHorizontalScrollBarVisibility(VirtualView.HorizontalScrollBarVisibility);
                    break;
                case ScrollOrientation.Both:
                    PlatformView?.UpdateVerticalScrollBarVisibility(VirtualView.VerticalScrollBarVisibility);
                    PlatformView?.UpdateHorizontalScrollBarVisibility(VirtualView.HorizontalScrollBarVisibility);
                    break;
            }
        }


        public static void MapScrollTo(SfInteractiveScrollViewHandler handler, SfInteractiveScrollView scrollView, object? args)
        {
            if (handler.PlatformView == null)
                return;

            if (args is ScrollToParameters parameters)
            {
                handler.PlatformView.ScrollTo(parameters.ScrollX, parameters.ScrollY, parameters.Animated);

                //Sometimes, the scrolled event does not occurrs on followed by the above "ScrollTo" method, when on size reduction So the following fallback condition is used.
                if (scrollView.ScrollX != handler.PlatformView.ContentOffset.X || scrollView.ScrollY != handler.PlatformView.ContentOffset.Y)
                {
                    ScrollChangedEventArgs scrolledEventArgs = new ScrollChangedEventArgs(handler.PlatformView.ContentOffset.X, handler.PlatformView.ContentOffset.Y, scrollView.ScrollX, scrollView.ScrollY);
                    scrollView.OnScrollChanged(scrolledEventArgs);
                }
                if (parameters.Animated == false)
                    handler.VirtualView.SendScrollFinished();
            }
        }
        #endregion

        #region Helper implementation

        private void OnScrollAnimationEnded(object? sender, EventArgs e)
        {
            VirtualView?.SendScrollFinished();
        }

        private void OnLayoutChanged(object? sender, EventArgs e)
        {
            if (PlatformView == null || VirtualView == null)
                return;

            VirtualView.ViewportWidth = PlatformView.Frame.Width;
            VirtualView.ViewportHeight = PlatformView.Frame.Height;
        }

        private void OnKeyPressesEnded(object? sender, UIKeyEventArgs e)
        {
            if (m_mauiView != null)
                e.Handled = m_mauiView.HandleKeyRelease(e.Presses, e.PressesEvent);
            if (e.Handled == false)
                VirtualView?.HandleKeyRelease(e.Presses, e.PressesEvent);
        }

        private void OnKeyPressesBegan(object? sender, UIKeyEventArgs e)
        {
            if (m_mauiView != null)
                e.Handled = m_mauiView.HandleKeyPress(e.Presses, e.PressesEvent);
            if (e.Handled == false)
                VirtualView?.HandleKeyPress(e.Presses, e.PressesEvent);
        }

        void OnScrolled(object? sender, EventArgs e)
        {
            if (PlatformView == null || VirtualView == null)
                return;

            if (VirtualView.Orientation == ScrollOrientation.Horizontal)
            {
                PlatformView.ContentOffset = new CGPoint(PlatformView.ContentOffset.X, m_offsetOnOrientationChange.Y);
            }
            else if (VirtualView.Orientation == ScrollOrientation.Vertical)
            {
                PlatformView.ContentOffset = new CGPoint(m_offsetOnOrientationChange.X, PlatformView.ContentOffset.Y);
            }

            if (PlatformView.ContentOffset.X < 0)
            {
                PlatformView.ContentOffset = new CGPoint(0, PlatformView.ContentOffset.Y);
            }
            if (PlatformView.ContentOffset.Y < 0)
            {
                PlatformView.ContentOffset = new CGPoint(PlatformView.ContentOffset.X, 0);
            }

            ScrollChangedEventArgs scrolledEventArgs = new ScrollChangedEventArgs(PlatformView.ContentOffset.X, PlatformView.ContentOffset.Y, VirtualView.ScrollX, VirtualView.ScrollY);
            VirtualView.OnScrollChanged(scrolledEventArgs);
        }
        #endregion
    }
}