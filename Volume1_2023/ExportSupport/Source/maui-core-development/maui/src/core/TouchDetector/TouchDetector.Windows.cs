using System;
using MauiView = Microsoft.Maui.Controls.View;
using Microsoft.UI.Xaml;
using Microsoft.Maui;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Core.Internals
{
    public partial class TouchDetector
    {
        internal void SubscribeNativeTouchEvents(MauiView? mauiView)
        {
            if (mauiView != null)
            {
                var handler = mauiView.Handler;
                UIElement? nativeView = handler?.PlatformView as UIElement;
                if (nativeView != null)
                {
                    nativeView.PointerPressed += PlatformView_PointerPressed;
                    nativeView.PointerMoved += PlatformView_PointerMoved;
                    nativeView.PointerReleased += PlatformView_PointerReleased;
                    nativeView.PointerCanceled += PlatformView_PointerCanceled;
                    nativeView.PointerWheelChanged += PlatformView_PointerWheelChanged;
                    nativeView.PointerEntered += PlatformView_PointerEntered;
                    nativeView.PointerExited += PlatformView_PointerExited;
                }
            }
        }

        private void PlatformView_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                var pointerPoint = e.GetCurrentPoint(nativeView);
                var property = pointerPoint.Properties;
                PointerEventArgs eventArgs = new PointerEventArgs(pointerPoint.PointerId, PointerActions.Exited, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y))
                {
                    IsLeftButtonPressed = property.IsLeftButtonPressed,
                    IsRightButtonPressed = property.IsRightButtonPressed,
                };

                OnTouchAction(eventArgs);
            }
        }

        private void PlatformView_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                var pointerPoint = e.GetCurrentPoint(nativeView);
                var property = pointerPoint.Properties;
                PointerEventArgs eventArgs = new PointerEventArgs(pointerPoint.PointerId, PointerActions.Entered, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y))
                {
                    IsLeftButtonPressed = property.IsLeftButtonPressed,
                    IsRightButtonPressed = property.IsRightButtonPressed,
                };

                OnTouchAction(eventArgs);
            }
        }

        private void PlatformView_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                var pointerPoint = e.GetCurrentPoint(nativeView);
                e.Handled = OnScrollAction(pointerPoint.PointerId, new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y), pointerPoint.Properties.MouseWheelDelta, e.Handled);
            }
        }

        private void PlatformView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                nativeView.CapturePointer(e.Pointer);
                var pointerPoint = e.GetCurrentPoint(nativeView);
                var property = pointerPoint.Properties;

                PointerEventArgs eventArgs = new PointerEventArgs(pointerPoint.PointerId, PointerActions.Pressed, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y))
                {
                    IsLeftButtonPressed = property.IsLeftButtonPressed,
                    IsRightButtonPressed = property.IsRightButtonPressed,
                };

                OnTouchAction(eventArgs);

                if (touchListeners[0].IsTouchHandled)
                    nativeView.ManipulationMode = ManipulationModes.None;
            }
        }

        private void PlatformView_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                var pointerPoint = e.GetCurrentPoint(nativeView);
                OnTouchAction(pointerPoint.PointerId, PointerActions.Moved, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));
            }
        }

        private void PlatformView_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                nativeView.ReleasePointerCapture(e.Pointer);
                var pointerPoint = e.GetCurrentPoint(nativeView);
                OnTouchAction(pointerPoint.PointerId, PointerActions.Cancelled, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));

                if (nativeView.ManipulationMode == ManipulationModes.None)
                    nativeView.ManipulationMode = ManipulationModes.System;
            }
        }

        private void PlatformView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                nativeView.ReleasePointerCapture(e.Pointer);
                var pointerPoint = e.GetCurrentPoint(nativeView);
                OnTouchAction(pointerPoint.PointerId, PointerActions.Released, GetDeviceType(pointerPoint.PointerDeviceType),  new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));

                if (nativeView.ManipulationMode == ManipulationModes.None)
                    nativeView.ManipulationMode = ManipulationModes.System;
            }
        }

        private static PointerDeviceType GetDeviceType(Microsoft.UI.Input.PointerDeviceType deviceType)
        {
            return deviceType == Microsoft.UI.Input.PointerDeviceType.Mouse ? PointerDeviceType.Mouse : PointerDeviceType.Touch;
        }

        internal void UnsubscribeNativeTouchEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                UIElement? nativeView = handler.PlatformView as UIElement;
                if (nativeView != null)
                {
                    nativeView.PointerPressed -= PlatformView_PointerPressed;
                    nativeView.PointerMoved -= PlatformView_PointerMoved;
                    nativeView.PointerReleased -= PlatformView_PointerReleased;
                    nativeView.PointerCanceled -= PlatformView_PointerCanceled;
                    nativeView.PointerWheelChanged -= PlatformView_PointerWheelChanged;
                    nativeView.PointerEntered -= PlatformView_PointerEntered;
                    nativeView.PointerExited -= PlatformView_PointerExited;
                }
            }
        }
    }
}
