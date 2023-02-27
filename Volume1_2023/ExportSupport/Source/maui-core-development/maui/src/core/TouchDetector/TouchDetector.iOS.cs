using MauiView = Microsoft.Maui.Controls.View;
using UIKit;
using Foundation;
using CoreGraphics;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using Microsoft.Maui.Devices;
using System;

namespace Syncfusion.Maui.Core.Internals
{
    public partial class TouchDetector
    {
        internal void SubscribeNativeTouchEvents(MauiView? mauiView)
        {
            if (mauiView != null && mauiView.Handler != null)
            {
                var handler = mauiView.Handler;
                UIView? nativeView = handler?.PlatformView as UIView;

                if (nativeView != null)
                {
                    UITouchRecognizerExt touchRecognizer = new UITouchRecognizerExt(this);
                    nativeView.AddGestureRecognizer(touchRecognizer);

                    UIHoverRecognizerExt hoverGesture = new UIHoverRecognizerExt(this);
                    nativeView.AddGestureRecognizer(hoverGesture);

                    UIScrollRecognizerExt scrollRecognizer = new UIScrollRecognizerExt(this);
                    nativeView.AddGestureRecognizer(scrollRecognizer);
                }
            }
        }

        internal void UnsubscribeNativeTouchEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                UIView? nativeView = handler.PlatformView as UIView;

                if (nativeView != null)
                {
                    var gestures = nativeView.GestureRecognizers;

                    if (gestures != null)
                    {
                        foreach (var item in gestures)
                        {
                            if (item is UITouchRecognizerExt || item is UIHoverRecognizerExt || item is UIScrollRecognizerExt)
                            {
                                nativeView.RemoveGestureRecognizer(item);
                            }
                        }
                    }
                }
            }
        }
    }

    internal class UIHoverRecognizerExt : UIHoverGestureRecognizer
    {
        TouchDetector touchDetector;
        ITouchListener? touchListener;

        public UIHoverRecognizerExt(TouchDetector listener) : base(Hovering)
        {
            touchDetector = listener;
            if (touchDetector.MauiView is ITouchListener _touchListener)
                touchListener = _touchListener;
            ShouldRecognizeSimultaneously += GestureRecognizer;

            this.AddTarget(() => OnHover(touchDetector));
        }

        /// <summary>
        /// Having static member for base action hence <see cref="UIHoverGestureRecognizer"/> does not have default consturctor.
        /// </summary>
        private static void Hovering()
        {

        }

        bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (otherGestureRecognizer is UITouchRecognizerExt || touchListener == null)
            {
                return true;
            }

            return !touchListener.IsTouchHandled;
        }

        private void OnHover(TouchDetector gestureDetecture)
        {
            if (!touchDetector.MauiView.IsEnabled || touchDetector.MauiView.InputTransparent)
            {
                return;
            }

            var state = State == UIGestureRecognizerState.Began ? PointerActions.Entered :
                State == UIGestureRecognizerState.Changed ? PointerActions.Moved :
                State == UIGestureRecognizerState.Ended ? PointerActions.Exited : PointerActions.Cancelled;

            long pointerId = Handle.Handle.ToInt64();
            CGPoint point = LocationInView(View);

            gestureDetecture.OnTouchAction(pointerId, state, new Point(point.X, point.Y));
        }
    }

    internal class UITouchRecognizerExt : UIPanGestureRecognizer
    {
        TouchDetector touchDetector;
        ITouchListener? touchListener;

        internal UITouchRecognizerExt(TouchDetector listener)
        {
            touchDetector = listener;
            if (touchDetector.MauiView is ITouchListener _touchListener)
                touchListener = _touchListener;
            
            ShouldRecognizeSimultaneously += GestureRecognizer;
        }

        bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (otherGestureRecognizer is GestureDetector.UIPanGestureExt || otherGestureRecognizer is UIScrollRecognizerExt || touchListener == null)
            {
                return true;
            }

            return !touchListener.IsTouchHandled;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            if (!touchDetector.MauiView.IsEnabled || touchDetector.MauiView.InputTransparent)
            {
                return;
            }

            UITouch? touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                long pointerId = touch.Handle.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);

                touchDetector.OnTouchAction(
                    new PointerEventArgs(pointerId, PointerActions.Pressed, new Point(point.X, point.Y))
                    {
                        IsLeftButtonPressed = touch.TapCount == 1
                    });
            }
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
            if (!touchDetector.MauiView.IsEnabled|| touchDetector.MauiView.InputTransparent)
            {
                return;
            }

            UITouch? touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                long pointerId = touch.Handle.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                touchDetector.OnTouchAction(
                   new PointerEventArgs(pointerId, PointerActions.Moved, new Point(point.X, point.Y))
                   {
                       IsLeftButtonPressed = touch.TapCount == 1
                   });
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            if (!touchDetector.MauiView.IsEnabled || touchDetector.MauiView.InputTransparent)
            {
                return;
            }

            UITouch? touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                long pointerId = touch.Handle.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                touchDetector.OnTouchAction(pointerId, PointerActions.Released, new Point(point.X, point.Y));
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
            if (!touchDetector.MauiView.IsEnabled || touchDetector.MauiView.InputTransparent)
            {
                return;
            }

            UITouch? touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                long pointerId = touch.Handle.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                touchDetector.OnTouchAction(pointerId, PointerActions.Cancelled, new Point(point.X, point.Y));
            }
        }
    }

    internal class UIScrollRecognizerExt : UIPanGestureRecognizer
    {
        TouchDetector touchDetector;
        ITouchListener? touchListener;

        internal UIScrollRecognizerExt(TouchDetector listener)
        {
            touchDetector = listener;
            if (touchDetector.MauiView is ITouchListener _touchListener)
                touchListener = _touchListener;

            this.AddTarget(() => OnScroll(this));

            if (UpdateAllowedScrollTypesMask())
            {
                AllowedScrollTypesMask = UIScrollTypeMask.All;
            }

            ShouldRecognizeSimultaneously += GestureRecognizer;
            ShouldReceiveTouch += GesturerTouchRecognizer;
        }

        bool UpdateAllowedScrollTypesMask()
        {
            // Check if the current platform is iOS and if it is 13.4 or above
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                var iosVersion = new Version(UIDevice.CurrentDevice.SystemVersion);
                if (iosVersion >= new Version(13, 4)) { return true; }
            }
            else if (DeviceInfo.Platform == DevicePlatform.MacCatalyst)
            {

#if MACCATALYST13_4_OR_GREATER

                return true;
#endif
            }

            return false;
        }

        bool GesturerTouchRecognizer(UIGestureRecognizer recognizer, UITouch touch)
        {
            return false;
        }

        private void OnScroll(UIScrollRecognizerExt touchRecognizerExt)
        {
            if (!touchDetector.MauiView.IsEnabled || touchDetector.MauiView.InputTransparent)
            {
                return;
            }

            long pointerId = touchRecognizerExt.Handle.Handle.ToInt64();
            CGPoint delta = touchRecognizerExt.TranslationInView(View);
            CGPoint point = touchRecognizerExt.LocationInView(View);

            touchDetector.OnScrollAction(pointerId, new Point(point.X, point.Y), delta.Y != 0 ? delta.Y : delta.X);
        }

        bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (otherGestureRecognizer is GestureDetector.UIPanGestureExt || otherGestureRecognizer is UITouchRecognizerExt || touchListener == null)
            {
                return true;
            }

            return !touchListener.IsTouchHandled;
        }
    }
}
