using System;
using Android.Content;
using Android.Views;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Custom handler for custom scroll layout.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "We require this")]
    internal class SnapLayoutHandler : LayoutHandler
    {
        /// <summary>
        /// Create a native view for snap layout.
        /// </summary>
        /// <returns>Native snap layout.</returns>
        protected override LayoutViewGroup CreatePlatformView()
        {
            if (this.VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(this.VirtualView)} must be set to create a LayoutViewGroup");
            }

            NativeSnapLayout viewGroup = new NativeSnapLayout(this.Context!);
            SnapLayout? scrollLayout = this.VirtualView as SnapLayout;
            if (scrollLayout != null)
            {
                //// Set the native intercept event property by Maui intercept method because in native we did not decide the whether the child scroll view reaches it end and try to scroll after the end so that we call the Maui method for intercept event.
                viewGroup.InterceptTouchEvent = scrollLayout.OnInterceptTouchEvent;
                viewGroup.DisAllowInterceptTouchEvent = scrollLayout.OnDisAllowInterceptTouchEvent;
                viewGroup.TouchEvent = scrollLayout.OnHandleTouch;
            }

            //// .NET MAUI layouts should not impose clipping on their children.
            viewGroup.SetClipChildren(false);
            return viewGroup;
        }
    }

    /// <summary>
    /// Custom native snap layout that handles the intercept event.
    /// </summary>
    internal class NativeSnapLayout : LayoutViewGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NativeSnapLayout"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public NativeSnapLayout(Context context)
            : base(context)
        {
            //// Pan or tap gestures on Maui does not triggered while we override intercept event and return value based on movement. In swiping, initial down action return false and move action return true value. So the pan or tap gestures not triggered. Using the touch to call Maui touch event to resolve the issue.
            this.Touch += this.Handle_Touch;
        }

        /// <summary>
        /// Gets or sets to decide whether hold/pass the touch to children.
        /// </summary>
        internal Func<Point, string, bool>? InterceptTouchEvent { get; set; }

        /// <summary>
        /// Gets or sets to decide whether hold/pass the touch to parent.
        /// </summary>
        internal Func<Point, string, bool?>? DisAllowInterceptTouchEvent { get; set; }

        /// <summary>
        /// Gets or sets to call the Maui touch event.
        /// </summary>
        internal Action<Point, string>? TouchEvent { get; set; }

        /// <summary>
        ///  Return false then pass the touch to its children.
        ///  Return true then it holds the touch to its own.
        /// </summary>
        /// <param name="motionEvent">Motion event details.</param>
        /// <returns> Return true to steal motion events from the children and have them dispatched to this ViewGroup through onTouchEvent(). The current target will receive an ACTION_CANCEL event, and no further messages will be delivered here.</returns>
        public override bool OnInterceptTouchEvent(MotionEvent? motionEvent)
        {
            if (this.InterceptTouchEvent == null || motionEvent == null)
            {
                return false;
            }

            int actionIndex = motionEvent.ActionIndex;
            //// The touch point value based on density.
            Point screenPoint = new Point(motionEvent.GetX(actionIndex), motionEvent.GetY(actionIndex));
            Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
            //// Calculate the touch point for Maui (without density value).
            Point point = new Point(fromPixels(screenPoint.X), fromPixels(screenPoint.Y));
            switch (motionEvent.Action)
            {
                case MotionEventActions.Down:
                    {
                        bool isIntercept = this.InterceptTouchEvent(point, "Down");
                        if (this.DisAllowInterceptTouchEvent != null)
                        {
                            bool? isDisAllowIntercept = this.DisAllowInterceptTouchEvent(point, "Down");
                            if (isDisAllowIntercept != null)
                            {
                                this.Parent?.RequestDisallowInterceptTouchEvent(isDisAllowIntercept.Value);
                            }
                        }

                        return isIntercept;
                    }

                case MotionEventActions.Move:
                    {
                        bool isIntercept = this.InterceptTouchEvent(point, "Move");
                        if (this.DisAllowInterceptTouchEvent != null)
                        {
                            bool? isDisAllowIntercept = this.DisAllowInterceptTouchEvent(point, "Move");
                            if (isDisAllowIntercept != null)
                            {
                                this.Parent?.RequestDisallowInterceptTouchEvent(isDisAllowIntercept.Value);
                            }
                        }

                        return isIntercept;
                    }

                case MotionEventActions.Up:
                    {
                        return this.InterceptTouchEvent(point, "Up");
                    }

                case MotionEventActions.Cancel:
                    {
                        return this.InterceptTouchEvent(point, "Cancel");
                    }
            }

            return false;
        }

        /// <summary>
        /// Dispose the object instances.
        /// </summary>
        /// <param name="disposing">The dispose.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Touch -= this.Handle_Touch;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Pass the touch and its position to the layout used for
        /// swiping layout swiping.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="touchEvent">The touch event.</param>
        private void Handle_Touch(object? sender, TouchEventArgs touchEvent)
        {
            if (this.TouchEvent == null || touchEvent.Event == null)
            {
                return;
            }

            int actionIndex = touchEvent.Event.ActionIndex;
            //// The touch point value based on density.
            Point screenPoint = new Point(touchEvent.Event.GetX(actionIndex), touchEvent.Event.GetY(actionIndex));
            Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
            //// Calculate the touch point for Maui (without density value).
            Point point = new Point(fromPixels(screenPoint.X), fromPixels(screenPoint.Y));
            switch (touchEvent.Event.Action)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    {
                        this.TouchEvent(point, "Down");
                        break;
                    }

                case MotionEventActions.Move:
                    {
                        this.TouchEvent(point, "Move");
                        break;
                    }

                case MotionEventActions.Up:
                case MotionEventActions.PointerUp:
                    {
                        this.TouchEvent(point, "Up");
                        break;
                    }

                case MotionEventActions.Cancel:
                    {
                        this.TouchEvent(point, "Cancel");
                        break;
                    }
            }
        }
    }
}