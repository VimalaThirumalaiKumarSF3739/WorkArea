using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Core.Internals
{
    public partial class GestureDetector
    {
        bool wasPinchStarted;
        bool wasPanStarted;
        bool isPinching;
        bool isPanning;
        Windows.Foundation.Point touchMovePoint;
        readonly Dictionary<uint, Windows.Foundation.Point> touchPointers = new Dictionary<uint, Windows.Foundation.Point>();
        ManipulationModes defaultManipulationMode = ManipulationModes.System;
        Point panVelocity;

        internal void SubscribeNativeGestureEvents(View? mauiView)
        {
            if (mauiView != null)
            {
                var handler = mauiView.Handler;
                UIElement? nativeView = handler?.PlatformView as UIElement;
                if (nativeView != null)
                {
                    defaultManipulationMode = nativeView.ManipulationMode;
                    if (tapGestureListeners != null && tapGestureListeners.Count > 0)
                    {
                        nativeView.Tapped += PlatformView_Tapped;
                    }

                    if (doubleTapGestureListeners != null && doubleTapGestureListeners.Count > 0)
                    {
                        nativeView.DoubleTapped += PlatformView_DoubleTapped;
                    }

                    if (longPressGestureListeners != null && longPressGestureListeners.Count > 0)
                    {
                        nativeView.Holding += PlatformView_Holding;
                    }

                    if ((panGestureListeners != null && panGestureListeners.Count > 0) || (pinchGestureListeners != null && pinchGestureListeners.Count > 0))
                    {
                        nativeView.PointerPressed += PlatformView_PointerPressed;
                        nativeView.PointerMoved += PlatformView_PointerMoved;
                        nativeView.PointerReleased += PlatformView_PointerReleased;
                        nativeView.PointerCanceled += PlatformView_PointerCanceled;
                        nativeView.PointerExited += PlatformView_PointerExited;
                        nativeView.PointerCaptureLost += PlatformView_PointerCaptureLost;
                    }

                    if (panGestureListeners != null && panGestureListeners.Count > 0)
                    {
                        //// Manipulation inertia event is wired to get the pan velocity value.
                        nativeView.ManipulationInertiaStarting += OnNativeViewManipulationInertiaStarting;
                    }
                }
            }
        }

        internal void CreateNativeListener()
        {
            SubscribeNativeGestureEvents(MauiView);
        }

        void AddPointer(Pointer pointer, Windows.Foundation.Point position)
        {
            if (!touchPointers.ContainsKey(pointer.PointerId))
            {
                touchPointers.Add(pointer.PointerId, position);
            }
        }

        void RemovePointer(Pointer pointer)
        {
            if (touchPointers.ContainsKey(pointer.PointerId))
            {
                touchPointers.Remove(pointer.PointerId);
            }
        }

        private void PlatformView_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            OnPointerEnd(sender, e, false);
        }

        void OnPointerEnd(object sender, PointerRoutedEventArgs e, bool isReleasedProperly)
        {
            RemovePointer(e.Pointer);
            PinchCompleted(isReleasedProperly);
            PanCompleted(isReleasedProperly);
            if (sender is UIElement platformView)
                platformView.ManipulationMode = defaultManipulationMode;
        }

        private void PlatformView_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            //TODO : need to check and refix the issue with e.Position value of ManipulationRoutedDelatEventArgs instead using below position.
            touchMovePoint = e.GetCurrentPoint(sender as UIElement).Position;
            OnPinch(e.Pointer.PointerId, touchMovePoint);
            OnPan(e.Pointer.PointerId, touchMovePoint);
        }

        private void PlatformView_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }
            OnPointerEnd(sender, e, true);
        }

        private void PlatformView_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }
            OnPointerEnd(sender, e, false);
        }

        private void PlatformView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }
            OnPointerEnd(sender, e, true);
        }

        private void PlatformView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            AddPointer(e.Pointer, e.GetCurrentPoint(sender as UIElement).Position);

            //TODO: Need to revisit this fix for the issue scrolling is not working while panning on the screen in the control. 
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                if (touchPointers.Count == 1 && panGestureListeners != null && panGestureListeners.Count > 0
                    /*&& panGestureListeners[0].IsTouchHandled*/)
                    nativeView.ManipulationMode = ManipulationModes.TranslateX
                                  | ManipulationModes.TranslateY
                                  | ManipulationModes.TranslateInertia;

                if (touchPointers.Count == 2 && pinchGestureListeners != null && pinchGestureListeners.Count > 0 /*&& pinchGestureListeners[0].IsTouchHandled*/)
                    nativeView.ManipulationMode = ManipulationModes.Scale;

                if (touchPointers.Count > 2)
                    nativeView.ManipulationMode = defaultManipulationMode;
            }
        }

        /// <summary>
        /// Invoks on inertia started while pan action. This is used to set the pan velocity in PanEventArgs after the pointer up action.
        /// </summary>
        /// <param name="sender">The native view.</param>
        /// <param name="e">Manipulation event args.</param>
        private void OnNativeViewManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            if (panGestureListeners == null || panGestureListeners.Count == 0)
                return;

            //// On panning need to provide velocity of fling action. So scroll velocity is stored and will be used with pan completed event.
            //// 1000 offset value is used to normalize the velocity values in all platforms. So that the velocity will be in the range of 0,1000,2000,... in both negative and positive values across all platforms. 
            this.panVelocity = new Point(e.Velocities.Linear.X * 1000, e.Velocities.Linear.Y * 1000);
        }

        void OnPan(uint pointerID, Windows.Foundation.Point position)
        {
            if (panGestureListeners == null || panGestureListeners.Count == 0 || touchPointers.Count != 1 || !touchPointers.ContainsKey(pointerID))
                return;

            var oldTouchPoint = new Point(touchPointers[pointerID].X, touchPointers[pointerID].Y);
            touchPointers[pointerID] = position;
            var touchPoint = new Point(touchMovePoint.X, touchMovePoint.Y);
            var translationPoint = new Point(touchPoint.X - oldTouchPoint.X, touchPoint.Y - oldTouchPoint.Y);
            GestureStatus state = GestureStatus.Started;
            isPanning = true;
            foreach (var listener in panGestureListeners)
            {
                if (wasPanStarted)
                    state = GestureStatus.Running;

                //// Pan velcoity will have value only on pan completed action that will be called after the manipulation inertia triggered.
                PanEventArgs eventArgs = new PanEventArgs(state, touchPoint, translationPoint, Point.Zero);
                listener.OnPan(eventArgs);
            }
            wasPanStarted = true;
        }

        void OnPinch(uint pointerID, Windows.Foundation.Point position)
        {
            if (pinchGestureListeners == null || pinchGestureListeners.Count == 0 || touchPointers.Count != 2 || !touchPointers.ContainsKey(pointerID))
                return;

            if (wasPanStarted)
                PanCompleted(false);

            List<uint> keys = touchPointers.Keys.ToList();

            //Pointers poistion before the pinch.
            Windows.Foundation.Point oldPointerPosition1 = touchPointers[keys[0]];
            Windows.Foundation.Point oldPointerPosition2 = touchPointers[keys[1]];

            touchPointers[pointerID] = position;

            //Pointers poistion after the pinch.
            Windows.Foundation.Point newPointerPosition1 = touchPointers[keys[0]];
            Windows.Foundation.Point newPointerPosition2 = touchPointers[keys[1]];

            keys.Clear();

            //Sum of two points before the pinch to calculate the changes in the distance and origin of zoom.
            Point oldCumulativePoint = new Point(oldPointerPosition1.X + oldPointerPosition2.X,
                oldPointerPosition1.Y + oldPointerPosition2.Y);

            //Distance between the two touch pointers before the pinch.
            double oldDistance = MathUtils.GetDistance(oldPointerPosition1.X, oldPointerPosition2.X,
                oldPointerPosition1.Y, oldPointerPosition2.Y);

            if (!wasPinchStarted)
                PerformPinch(new Point(oldCumulativePoint.X / 2, oldCumulativePoint.Y / 2),
                    Point.Zero, 1);

            //Sum of two points after the pinch to calculate the changes in the distance and origin of zoom.
            Point newCumulativePoint = new Point(newPointerPosition1.X + newPointerPosition2.X,
                newPointerPosition1.Y + newPointerPosition2.Y);

            //Distance between the two touch pointers after the pinch.
            double newDistance = MathUtils.GetDistance(newPointerPosition1.X, newPointerPosition2.X,
                newPointerPosition1.Y, newPointerPosition2.Y);

            //Mid point of two touch pointers.
            Point scalePoint = new Point(newCumulativePoint.X / 2, newCumulativePoint.Y / 2);
            //Calculate the x and y translation change before and after pinch.
            Point translationPoint = new Point(newCumulativePoint.X - oldCumulativePoint.X,
                newCumulativePoint.Y - oldCumulativePoint.Y);
            float scale = (float)(newDistance / oldDistance);

            PerformPinch(scalePoint, translationPoint, scale);
        }

        void PerformPinch(Point scalePoint, Point translationPoint, float scale)
        {
            if (pinchGestureListeners == null)
                return;
            GestureStatus state = GestureStatus.Started;
            double angle = MathUtils.GetAngle(scalePoint.X, translationPoint.X, scalePoint.Y, translationPoint.Y);
            isPinching = true;
            foreach (var listener in pinchGestureListeners)
            {
                if (wasPinchStarted)
                {
                    state = GestureStatus.Running;
                }
                PinchEventArgs eventArgs = new PinchEventArgs(state, scalePoint, angle, scale);
                listener.OnPinch(eventArgs);
            }
            wasPinchStarted = true;
        }

        void PinchCompleted(bool isCompleted, Point scalePoint = new Point(), double angle = double.NaN, float scale = 1)
        {
            if (pinchGestureListeners == null || pinchGestureListeners.Count == 0 || !isPinching) return;

            foreach (var listener in pinchGestureListeners)
            {
                PinchEventArgs eventArgs = new PinchEventArgs(isCompleted ? GestureStatus.Completed : GestureStatus.Canceled, scalePoint, angle, scale);
                listener.OnPinch(eventArgs);
            }
            isPinching = false;
            touchPointers.Clear();
            wasPinchStarted = false;
        }

        void PanCompleted(bool isCompleted)
        {
            if (panGestureListeners == null || panGestureListeners.Count == 0 || !isPanning) return;

            foreach (var listener in panGestureListeners)
            {
                PanEventArgs eventArgs = new PanEventArgs(isCompleted ? GestureStatus.Completed : GestureStatus.Canceled, new Point(touchMovePoint.X, touchMovePoint.Y), Point.Zero, panVelocity);
                listener.OnPan(eventArgs);
            }
            isPanning = false;
            wasPanStarted = false;
            panVelocity = Point.Zero;
        }

        private void PlatformView_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            if (longPressGestureListeners == null || longPressGestureListeners.Count == 0) return;

            var touchPoint = e.GetPosition(sender as UIElement);

            foreach (var listener in longPressGestureListeners)
            {
                if (e.HoldingState == Microsoft.UI.Input.HoldingState.Started)
                {
                    listener.OnLongPress(new LongPressEventArgs(new Point(touchPoint.X, touchPoint.Y)));
                }
            }

            if (longPressGestureListeners[0].IsTouchHandled)
            {
                e.Handled = true;
            }
        }

        private void PlatformView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            if (doubleTapGestureListeners == null || doubleTapGestureListeners.Count == 0) return;

            var touchPoint = e.GetPosition(sender as UIElement);

            foreach (var listener in doubleTapGestureListeners)
            {
                listener.OnDoubleTap(new TapEventArgs(new Point(touchPoint.X, touchPoint.Y), 2));
            }

            if (doubleTapGestureListeners[0].IsTouchHandled)
            {
                e.Handled = true;
            }
        }

        private void PlatformView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!MauiView.IsEnabled || MauiView.InputTransparent)
            {
                return;
            }

            if (tapGestureListeners == null || tapGestureListeners.Count == 0) return;

            var touchPoint = e.GetPosition(sender as UIElement);

            foreach (var listener in tapGestureListeners)
            {
                listener.OnTap(MauiView, new TapEventArgs(new Point(touchPoint.X, touchPoint.Y), 1));
                listener.OnTap(new TapEventArgs(new Point(touchPoint.X, touchPoint.Y), 1));
            }

            if (tapGestureListeners[0].IsTouchHandled)
            {
                e.Handled = true;
            }
        }

        internal void UnsubscribeNativeGestureEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                UIElement? nativeView = handler.PlatformView as UIElement;

                if (nativeView != null)
                {
                    nativeView.ManipulationMode = defaultManipulationMode;
                    nativeView.Tapped -= PlatformView_Tapped;
                    nativeView.DoubleTapped -= PlatformView_DoubleTapped;
                    nativeView.Holding -= PlatformView_Holding;
                    nativeView.PointerPressed -= PlatformView_PointerPressed;
                    nativeView.PointerMoved -= PlatformView_PointerMoved;
                    nativeView.PointerReleased -= PlatformView_PointerReleased;
                    nativeView.PointerCanceled -= PlatformView_PointerCanceled;
                    nativeView.PointerExited -= PlatformView_PointerExited;
                    nativeView.PointerCaptureLost -= PlatformView_PointerCaptureLost;
                    nativeView.ManipulationInertiaStarting -= OnNativeViewManipulationInertiaStarting;
                }
            }
            touchPointers.Clear();
        }
    }
}