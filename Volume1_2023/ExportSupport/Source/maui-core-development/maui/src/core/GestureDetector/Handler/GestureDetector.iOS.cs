using Microsoft.Maui.Graphics;
using MauiView = Microsoft.Maui.Controls.View;
using GestureStatus = Microsoft.Maui.GestureStatus;
using UIKit;
using System;
using CoreGraphics;
using Microsoft.Maui;
using Foundation;

namespace Syncfusion.Maui.Core.Internals
{
    public partial class GestureDetector
    {
        private UIPinchGestureExt? pinchGesture;
        private UIPanGestureExt? panGesture;
        private UITapGestureExt? tapGesture;
        private UILongPressGestureExt? longPressGesture;
        internal UITapGestureExt? DoubleTapGesture { get; set; }

        internal void SubscribeNativeGestureEvents(MauiView? mauiView)
        {
            if (mauiView != null)
            {
                var handler = mauiView.Handler;
                UIView? nativeView = handler?.PlatformView as UIView;
                if (nativeView != null)
                {
                    if (pinchGestureListeners?.Count > 0)
                    {
                        pinchGesture = new UIPinchGestureExt(this);
                        nativeView.AddGestureRecognizer(pinchGesture);
                    }

                    if (panGestureListeners?.Count > 0)
                    {
                        panGesture = new UIPanGestureExt(this);
                        nativeView.AddGestureRecognizer(panGesture);
                    }

                    if (doubleTapGestureListeners?.Count > 0)
                    {
                        DoubleTapGesture = new UITapGestureExt(this, 2);
                        nativeView.AddGestureRecognizer(DoubleTapGesture);
                    }

                    if (tapGestureListeners?.Count > 0)
                    {
                        tapGesture = new UITapGestureExt(this, 1);
                        nativeView.AddGestureRecognizer(tapGesture);
                    }

                    if (longPressGestureListeners?.Count > 0)
                    {
                        longPressGesture = new UILongPressGestureExt(this);
                        nativeView.AddGestureRecognizer(longPressGesture);
                    }
                }
            }
        }
        internal void CreateNativeListener()
        {
            if (MauiView != null)
            {
                var handler = MauiView.Handler;
                UIView? nativeView = handler?.PlatformView as UIView;
                if (nativeView != null)
                {
                    if (pinchGesture == null && pinchGestureListeners?.Count > 0)
                    {
                        pinchGesture = new UIPinchGestureExt(this);
                        nativeView.AddGestureRecognizer(pinchGesture);
                    }

                    if (panGesture == null && panGestureListeners?.Count > 0)
                    {
                        panGesture = new UIPanGestureExt(this);
                        nativeView.AddGestureRecognizer(panGesture);
                    }

                    if (DoubleTapGesture == null && doubleTapGestureListeners?.Count > 0)
                    {
                        DoubleTapGesture = new UITapGestureExt(this, 2);
                        nativeView.AddGestureRecognizer(DoubleTapGesture);
                    }

                    if (tapGesture == null && tapGestureListeners?.Count > 0)
                    {
                        tapGesture = new UITapGestureExt(this, 1);
                        nativeView.AddGestureRecognizer(tapGesture);
                    }

                    if (longPressGesture == null && longPressGestureListeners?.Count > 0)
                    {
                        longPressGesture = new UILongPressGestureExt(this);
                        nativeView.AddGestureRecognizer(longPressGesture);
                    }
                }
            }
        }

        internal void UnsubscribeNativeGestureEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                UIView? nativeView = handler.PlatformView as UIView;

                if (nativeView != null)
                {
                    var gestures = nativeView.GestureRecognizers;

                    if (gestures != null)
                    {
                        if (pinchGesture != null)
                        {
                            nativeView.RemoveGestureRecognizer(pinchGesture);
                        }
                        if (panGesture != null)
                        {
                            nativeView.RemoveGestureRecognizer(panGesture);
                        }
                        if (tapGesture != null)
                        {
                            nativeView.RemoveGestureRecognizer(tapGesture);
                        }
                        if (DoubleTapGesture != null)
                        {
                            nativeView.RemoveGestureRecognizer(DoubleTapGesture);
                        }
                        if (longPressGesture != null)
                        {
                            nativeView.RemoveGestureRecognizer(longPressGesture);
                        }
                    }
                }
            }
        }

        internal class UIPanGestureExt : UIPanGestureRecognizer
        {
            IGestureListener? gestureListener;
            public UIPanGestureExt(GestureDetector gestureDetector)
            {
                if (gestureDetector.MauiView is IGestureListener _gestureListener)
                    gestureListener = _gestureListener;

                this.AddTarget(() => OnScroll(gestureDetector));

                ShouldRecognizeSimultaneously += GestureRecognizer;
            }

            bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
            {
                if (otherGestureRecognizer is UITouchRecognizerExt || otherGestureRecognizer is UIScrollRecognizerExt || gestureListener == null)
                {
                    return true;
                }

                return !gestureListener.IsTouchHandled;
            }

            private void OnScroll(GestureDetector gestureDetector)
            {
                if (!gestureDetector.MauiView.IsEnabled || gestureDetector.MauiView.InputTransparent)
                {
                    return;
                }

                var locationInView = LocationInView(View);
                var translateLocation = TranslationInView(View);
                var state = GestureStatus.Completed;

                switch (State)
                {
                    case UIGestureRecognizerState.Began:
                        state = GestureStatus.Started;
                        break;
                    case UIGestureRecognizerState.Changed:
                        state = GestureStatus.Running;
                        break;
                    case UIGestureRecognizerState.Cancelled:
                    case UIGestureRecognizerState.Failed:
                        state = GestureStatus.Canceled;
                        break;
                    case UIGestureRecognizerState.Ended:
                        state = GestureStatus.Completed;
                        break;
                }

                Point velocity = Point.Zero;
                if (state == GestureStatus.Completed || state == GestureStatus.Canceled)
                {
                    var nativeVelocity = VelocityInView(View);
                    velocity = new Point(nativeVelocity.X, nativeVelocity.Y);
                }

                gestureDetector.OnScroll(state, new Point(locationInView.X, locationInView.Y), new Point(translateLocation.X, translateLocation.Y), velocity);
                SetTranslation(CGPoint.Empty, View);
            }
        }

        private class UIPinchGestureExt : UIPinchGestureRecognizer
        {
            IGestureListener? gestureListener;
            public UIPinchGestureExt(GestureDetector gestureDetector)
            {
                if (gestureDetector.MauiView is IGestureListener _gestureListener)
                    gestureListener = _gestureListener;

                this.AddTarget(() => OnPinch(gestureDetector));

                ShouldRecognizeSimultaneously = (g, o) => gestureListener == null || !gestureListener.IsTouchHandled;
            }

            private void OnPinch(GestureDetector gestureDetector)
            {
                if (!gestureDetector.MauiView.IsEnabled || gestureDetector.MauiView.InputTransparent)
                {
                    return;
                }

                var locationInView = LocationInView(View);
                var state = GestureStatus.Completed;
                double angle = double.NaN;
                if (NumberOfTouches == 2)
                {
                    CGPoint touchStart = LocationOfTouch(0, View);
                    CGPoint touchEnd = LocationOfTouch(1, View);
                    angle = MathUtils.GetAngle((float)touchStart.X, (float)touchEnd.X, (float)touchStart.Y, (float)touchEnd.Y);
                }

                switch (State)
                {
                    case UIGestureRecognizerState.Began:
                        state = GestureStatus.Started;
                        break;
                    case UIGestureRecognizerState.Changed:
                        state = GestureStatus.Running;
                        break;
                    case UIGestureRecognizerState.Cancelled:
                    case UIGestureRecognizerState.Failed:
                        state = GestureStatus.Canceled;
                        break;
                    case UIGestureRecognizerState.Ended:
                        state = GestureStatus.Completed;
                        break;
                }

                gestureDetector.OnPinch(state, new Point(locationInView.X, locationInView.Y), angle, (float)Scale);
                Scale = 1; // Resetting the previous scale value.
            }
        }

        internal class UITapGestureExt : UITapGestureRecognizer
        {
            IGestureListener? gestureListener;
            public UITapGestureExt(GestureDetector gestureDetector, nuint tapsCount)
            {
                NumberOfTapsRequired = tapsCount;

                if (gestureDetector.MauiView is IGestureListener _gestureListener)
                    gestureListener = _gestureListener;

                if (tapsCount == 1 && gestureDetector.DoubleTapGesture != null)
                    RequireGestureRecognizerToFail(gestureDetector.DoubleTapGesture);

                this.AddTarget(() => OnTap(gestureDetector));

                ShouldRecognizeSimultaneously = (g, o) => gestureListener == null || !gestureListener.IsTouchHandled;
            }

            private void OnTap(GestureDetector gestureDetector)
            {
                if (!gestureDetector.MauiView.IsEnabled || gestureDetector.MauiView.InputTransparent)
                {
                    return;
                }

               var locationInView = LocationInView(View);
               gestureDetector.OnTapped(new Point(locationInView.X, locationInView.Y), (int)NumberOfTapsRequired);
            }
        }

        private class UILongPressGestureExt : UILongPressGestureRecognizer
        {
            IGestureListener? gestureListener;
            public UILongPressGestureExt(GestureDetector gestureDetector)
            {
                if (gestureDetector.MauiView is IGestureListener _gestureListener)
                    gestureListener = _gestureListener;

                this.AddTarget(() => OnLongPress(gestureDetector));

                ShouldRecognizeSimultaneously = (g, o) => gestureListener == null || !gestureListener.IsTouchHandled;
            }

            private void OnLongPress(GestureDetector gestureDetector)
            {
                if (!gestureDetector.MauiView.IsEnabled || gestureDetector.MauiView.InputTransparent)
                {
                    return;
                }

                var locationInView = LocationInView(View);
                gestureDetector.OnLongPress(new Point(locationInView.X, locationInView.Y));
            }
        }
    }
}

