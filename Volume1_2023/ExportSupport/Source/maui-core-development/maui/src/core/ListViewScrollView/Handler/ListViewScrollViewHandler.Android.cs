using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Widget;
using Java.Lang;
using Java.Lang.Reflect;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Views.View;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// ScrollViewHandler for <see cref="ListViewScrollViewExt"/>.
    /// </summary>
    // Todo - Reverting SfInteractiveScrollView - internal partial class ListViewScrollViewHandler : SfInteractiveScrollViewHandler
    internal partial class ListViewScrollViewHandler : ScrollViewHandler
    {
        #region Fields

        /// <summary>
        /// Native view when orientation of the ScrollView is <see cref="Orientation.Vertical"/>.
        /// </summary>
        private NativeListViewScrollView? nativeScrollView;

        /// <summary>
        /// Get the <see cref="OverScroller"/> for <see cref="nativeScrollView"/>.
        /// </summary>
        private OverScroller? scroller;

        /// <summary>
        /// Gets the <see cref="HorizontalScrollView"/> when orientation is <see cref="ScrollOrientation.Horizontal"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        private HorizontalScrollView? nativeHorizontalScrollView;

        /// <summary>
        /// Gets the <see cref="OverScroller"/> for <see cref="HorizontalScrollView"/>.
        /// </summary>
        private OverScroller? horizontalScroller;

        /// <summary>
        /// Used to check whether touch is removed or not, Because when touch is up there is no chance for Dragging.
        /// Used for ScrollState handling.
        /// </summary>
        private bool hasTouch = false;

        /// <summary>
        /// Keeps track when ScrollChanged method invoked.
        /// Used this to detect whether scrollChanged is invoked for certain ticks to detect idle state.
        /// </summary>
        private long scrolledTime = -1;

        /// <summary>
        /// Keeps track of previous ScrollChanged method invokation ticks.
        /// Used this to detect whether scrollChanged is invoked for certain ticks to detect idle state.
        /// </summary>
        private long lastScrolledTime = -1;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the Orientation is <see cref="ScrollOrientation.Vertical"/>.
        /// </summary>
        private bool IsVerticalOrientation
        {
            get { return this.ScrollView!.Orientation == ScrollOrientation.Vertical; }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Sets the VirtualView.
        /// </summary>
        /// <param name="view">Instance of virtualView.</param>
        public override void SetVirtualView(IView view)
        {
            base.SetVirtualView(view);
            if (this.ScrollView != null && this.ScrollView.ScrollingEnabled)
            {
                this.ScrollView.PropertyChanged += this.OnListViewScrollViewPropertyChanged;
            }
        }

        /// <summary>
        /// Creates native view.
        /// </summary>
        /// <returns>Instance of native view.</returns>
        //  Todo - Reverting SfInteractiveScrollView - protected override PlatformScrollViewer CreatePlatformView()
        protected override MauiScrollView CreatePlatformView()
        {
            if (this.ScrollView != null && !this.ScrollView.ScrollingEnabled)
            {
                return base.CreatePlatformView();
            }
            else
            {
                this.nativeScrollView = new NativeListViewScrollView(this.Context);
                this.nativeScrollView.listViewScrollViewExt = this.VirtualView as ListViewScrollViewExt;

                // Set to avoid overlapping control issue when scrolling.
                this.nativeScrollView.ClipToOutline = true;
                this.nativeScrollView.ChildViewAdded += this.MauiScrollView_ChildViewAdded;
                this.nativeScrollView.Touch += this.ScrollView_Touch;
                if (Build.VERSION.SdkInt > BuildVersionCodes.LollipopMr1)
                {
#pragma warning disable CA1416
                    this.nativeScrollView.ScrollChange += this.OnScrollChange;
#pragma warning restore CA1416
                }

                this.GetScroller(this.nativeScrollView!.Class!.Superclass!.Superclass!.CanonicalName, this.nativeScrollView, this.scroller!);
                this.nativeScrollView.Scroller = this.scroller;
                return this.nativeScrollView;
            }
        }

        /// <summary>
        /// Disconnects the handler.
        /// </summary>
        /// <param name="nativeView">Instance of nativeView.</param>
        //  Todo - Reverting SfInteractiveScrollView - protected override void DisconnectHandler(PlatformScrollViewer nativeView)
        protected override void DisconnectHandler(MauiScrollView nativeView)
        {
            if (this.ScrollView!.ScrollingEnabled)
            {
                this.ScrollView.PropertyChanged -= this.OnListViewScrollViewPropertyChanged;
                if (this.nativeScrollView != null)
                {
                    this.nativeScrollView.ChildViewAdded -= this.MauiScrollView_ChildViewAdded;
                    this.nativeScrollView.Touch -= this.ScrollView_Touch;
                    if (Build.VERSION.SdkInt > BuildVersionCodes.LollipopMr1)
                    {
#pragma warning disable CA1416
                        this.nativeScrollView.ScrollChange -= this.OnScrollChange;
#pragma warning restore CA1416
                    }
                }

                if (this.nativeHorizontalScrollView != null)
                {
                    if (Build.VERSION.SdkInt > BuildVersionCodes.LollipopMr1)
                    {
#pragma warning disable CA1416
                        // Todo - Reverting SfInteractiveScrollView - this.nativeHorizontalScrollView.ScrollChange -= this.OnScrollChange;
                        this.nativeHorizontalScrollView.ScrollChange -= this.HScrollChange;
#pragma warning restore CA1416
                    }

                    this.nativeHorizontalScrollView.Touch -= this.ScrollView_Touch;
                }
            }

            base.DisconnectHandler(nativeView);
        }

        /// <summary>
        /// Arranges the childrens.
        /// </summary>
        /// <param name="frame">Bounds value.</param>
        public override void PlatformArrange(Rect frame)
        {
            base.PlatformArrange(frame);
            if (this.ScrollView != null && this.nativeHorizontalScrollView != null)
            {
                if (this.ScrollView.IsViewLoadedAndHasHorizontalRTL())
                {
                    this.ScrollView.SetIsHorizontalRTLViewLoaded(true, ContextExtensions.FromPixels(this.Context, this.nativeHorizontalScrollView.ScrollX));
                }
            }
        }

        /// <summary>
        /// Gets the required size for the scrollview.
        /// </summary>
        /// <param name="widthConstraint">Available width to the scrollview.</param>
        /// <param name="heightConstraint">Available height to the scrollview.</param>
        /// <returns>Returns the measured size of scrollview.</returns>
        public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            // MAUI-3853,4368 - ForceLayout() ensures to include that view to be included compulsorly in layout pass.
            // Check the Issue 1,2 solution in fix details attached in the task for more details.
            bool isInvalidatedTillContainer = false;
            ViewGroup? view = this.PlatformView;
            while (!isInvalidatedTillContainer && view != null)
            {
                if (view!.ChildCount > 0)
                {
                    if (view is ViewGroup)
                    {
                        view = (view.GetChildAt(0) as ViewGroup)!;
                        view.ForceLayout();
                    }
                    else
                    {
                        view = null;
                    }
                }

                if (view != null && view is LayoutViewGroupExt && (view as LayoutViewGroupExt)!.Drawable != null && (view as LayoutViewGroupExt)!.Drawable!.ToString()!.Contains("VisualContainer"))
                {
                    isInvalidatedTillContainer = true;
                }
            }

            view = null;
            return base.GetDesiredSize(widthConstraint, heightConstraint);
        }

        #endregion

        #region private Methods

        /// <summary>
        /// Sets the <see cref="scroller"/> and <see cref="horizontalScroller"/>.
        /// </summary>
        /// <param name="assemblyname">Represents the assembly name of ScrollView.</param>
        /// <param name="scrollview">Represents the type of ScrollView.</param>
        /// <param name="overscroller">Represents the type of OverScroller.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
        private void GetScroller(string? assemblyname, Java.Lang.Object scrollview, OverScroller overscroller)
        {
            try
            {
                if (scrollview == null || scrollview.Handle == IntPtr.Zero)
                {
                    return;
                }

                Class parentClass = scrollview.Class;

                if (parentClass == null || parentClass.Handle == IntPtr.Zero)
                {
                    return;
                }

                do
                {
                    parentClass = parentClass.Superclass!;
                }
                while (!parentClass.Name.Equals(assemblyname));

                Field scrollerField = parentClass.GetDeclaredField("mScroller");

                if (scrollerField == null || scrollerField.Handle == IntPtr.Zero)
                {
                    return;
                }

                scrollerField.Accessible = true;
                if (overscroller == this.scroller)
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    this.scroller = (OverScroller)scrollerField.Get(scrollview);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                }
                else if (overscroller == this.horizontalScroller)
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    this.horizontalScroller = (OverScroller)scrollerField.Get(scrollview);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                }
            }
            catch (NoSuchFieldException e)
            {
                e.PrintStackTrace();
            }
            catch (IllegalArgumentException e)
            {
                e.PrintStackTrace();
            }
            catch (IllegalAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// A recursive method to determine whether scrolling has been stopped.
        /// </summary>
        private void Run()
        {
            if (this.lastScrolledTime == this.scrolledTime && !this.hasTouch && !this.IsInFling())
            {
                this.scrolledTime = -1;
                this.lastScrolledTime = -1;
                this.OnScrollEnd();
            }
            else
            {
                this.lastScrolledTime = this.scrolledTime;
                this.nativeScrollView!.PostDelayed(this.Run, 100);
            }
        }

        /// <summary>
        /// Method raises when scrolling stops.
        /// </summary>
        private void OnScrollEnd()
        {
            this.ScrollView!.IsProgrammaticScrolling = false;
            this.ScrollView.SetScrollState("Idle");
        }

        /// <summary>
        /// Sets the CurrentScrollState for <see cref="ListViewScrollViewExt"/>.
        /// </summary>
        private void ProcessScrollState(OverScroller overScroller)
        {
            if (!this.ScrollView!.IsProgrammaticScrolling)
            {
                if (this.hasTouch)
                {
                    this.ScrollView.SetScrollState("Dragging");
                }
                else
                {
                    this.ScrollView.SetScrollState("Fling");
                }
            }
            else
            {
                this.ScrollView.SetScrollState("Programmatic");
            }

            if (this.scrolledTime == -1)
            {
                this.nativeScrollView!.PostDelayed(this.Run, 100);
            }

            this.scrolledTime = System.DateTime.Now.Millisecond;
        }

        /// <summary>
        /// Helper method to find whether the fling action is completed or not.
        /// </summary>
        /// <returns>Returns true, if fling is in progress otherwise returns false.</returns>
        private bool IsInFling()
        {
            if (this.IsVerticalOrientation)
            {
                return this.scroller != null && this.scroller.Handle != IntPtr.Zero && !this.scroller.IsFinished;
            }
            else
            {
                return this.horizontalScroller != null && this.horizontalScroller.Handle != IntPtr.Zero && !this.horizontalScroller.IsFinished;
            }
        }

        #endregion

        #region CallBack Methods

        /// <summary>
        /// Raises when scroll value changes in vertical orientation.
        /// </summary>
        /// <param name="sender">Instance of <see cref="NativeListViewScrollView"/>.</param>
        /// <param name="e">Corresponding <see cref="ScrollChangeEventArgs"/> args.</param>
        // Todo - Reverting SfInteractiveScrollView - private void OnScrollChange(object? sender, ScrollChangeEventArgs e)
        private void OnScrollChange(object? sender, NestedScrollView.ScrollChangeEventArgs e)
        {
            this.ProcessScrollState(this.scroller!);
        }

        /// <summary>
        /// Raises when scroll value changes in vertical orientation.
        /// </summary>
        /// <param name="sender">Instance of <see cref="NativeListViewScrollView"/>.</param>
        /// <param name="e">Corresponding <see cref="ScrollChangeEventArgs"/> args.</param>
        // Todo - Reverting SfInteractiveScrollView - Remove this.
        private void HScrollChange(object? sender, ScrollChangeEventArgs e)
        {
            this.ProcessScrollState(this.scroller!);
        }

        /// <summary>
        /// Raised when child added.
        /// </summary>
        /// <param name="sender">Instance of <see cref="NativeListViewScrollView"/>.</param>
        /// <param name="e">Child added event args.</param>
        private void MauiScrollView_ChildViewAdded(object? sender, ViewGroup.ChildViewAddedEventArgs e)
        {
            if (!this.IsVerticalOrientation && e.Child is HorizontalScrollView && e.Child != this.nativeHorizontalScrollView)
            {
                this.nativeHorizontalScrollView = (HorizontalScrollView)e.Child;
                this.GetScroller("android.widget.HorizontalScrollView", this.nativeHorizontalScrollView, this.horizontalScroller!);
                this.nativeScrollView!.HScrollView = this.nativeHorizontalScrollView;
                this.nativeScrollView.Hscroller = this.horizontalScroller;
                if (Build.VERSION.SdkInt > BuildVersionCodes.LollipopMr1)
                {
#pragma warning disable CA1416
                    // Todo - Reverting SfInteractiveScrollView - this.nativeHorizontalScrollView.ScrollChange += this.OnScrollChange;
                    this.nativeHorizontalScrollView.ScrollChange += this.HScrollChange;
#pragma warning restore CA1416
                }

                this.nativeHorizontalScrollView.Touch += this.ScrollView_Touch;
            }
        }

        /// <summary>
        /// Raised when <see cref="NativeListViewScrollView"/> is touched.
        /// </summary>
        /// <param name="sender">Instance of <see cref="nativeScrollView"/>.</param>
        /// <param name="e">Touch event args.</param>
        private void ScrollView_Touch(object? sender, TouchEventArgs e)
        {
            e.Handled = false;
            if (e.Event!.Action == MotionEventActions.Up || e.Event.Action == MotionEventActions.Cancel)
            {
                this.hasTouch = false;
            }

            if (e.Event.Action == MotionEventActions.Down || e.Event.Action == MotionEventActions.Move)
            {
                this.hasTouch = true;
            }
        }

        /// <summary>
        /// Raises when <see cref="ListViewScrollViewExt"/> property changes.
        /// </summary>
        /// <param name="sender">Instance of <see cref="ListViewScrollViewExt"/>.</param>
        /// <param name="e">Property chnaged event args.</param>
        private void OnListViewScrollViewPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ScrollPosition")
            {
                var position = (int)(this.ScrollView!.ScrollPosition * this.nativeScrollView!.Resources!.DisplayMetrics!.Density);

                // Called the ScrollTo method from Runnable because scroll to position not works when called on ListView loading.
                if (this.IsVerticalOrientation)
                {
                    if (this.ScrollView != null && this.ScrollView.DisableAnimation && !this.ScrollView.IsNativeScrollViewLoaded())
                    {
                        if (this.scroller != null && this.scroller.Handle != IntPtr.Zero)
                        {
                            this.nativeScrollView.ScrollTo(0, position);
                        }
                    }
                    else
                    {
                        this.nativeScrollView.Post(new Runnable(() =>
                        {
                            if (this.scroller != null && this.scroller.Handle != IntPtr.Zero)
                            {
                                this.nativeScrollView.ScrollTo(0, position);
                            }
                        }));
                    }
                }
                else if (this.nativeHorizontalScrollView != null && this.nativeHorizontalScrollView.Handle != IntPtr.Zero)
                {
                    this.nativeHorizontalScrollView.Post(new Runnable(() =>
                    {
                        if (this.horizontalScroller != null && this.horizontalScroller.Handle != IntPtr.Zero)
                        {
                            this.nativeHorizontalScrollView.ScrollTo(position, 0);
                        }
                    }));
                }
            }
            else if ((e.PropertyName == "Width" || e.PropertyName == "Height") && this.ScrollView!.Height > 0 && this.ScrollView!.Width > 0)
            {
                // MAUI-3853,4368 this will add the method in UI thread queue, this was added a precaution.
                // Check issue 1,2 in fix details attached in the task for more details.
                this.PlatformView.Post(this.ScrollView.InvalidateContainerIfRequired);
            }
        }

        #endregion
    }

    /// <summary>
    /// Native view of <see cref="ListViewScrollViewExt"/>.
    /// </summary>
    //  Todo - Reverting SfInteractiveScrollView - internal class NativeListViewScrollView : PlatformScrollViewer
    internal class NativeListViewScrollView : MauiScrollView
    {
        #region Fields

        /// <summary>
        /// First pointer index of the touch X coordinate.
        /// </summary>
        private float touchDownXPosition = 0;

        /// <summary>
        /// XAMARIN-29343 First pointer index of the touch Y coordinate.
        /// </summary>
        private float touchDownYPosition = 0;
        /// <summary>
        /// Get the <see cref="OverScroller"/> for <see cref="NativeListViewScrollView"/>.
        /// </summary>
        internal OverScroller? Scroller;

        /// <summary>
        /// Get the <see cref="OverScroller"/> for <see cref="HorizontalScrollView"/>.
        /// </summary>
        internal OverScroller? Hscroller;

        /// <summary>
        /// Get the <see cref="HorizontalScrollView"/> when <see cref="ScrollOrientation"/> is <see cref="Orientation.Horizontal"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        internal HorizontalScrollView? HScrollView;

        /// <summary>
        /// Instance of <see cref="ListViewScrollViewExt"/>.
        /// </summary>
        internal ListViewScrollViewExt? listViewScrollViewExt;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Verified")]
        private Handler? mainHandler;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeListViewScrollView" /> class.
        /// </summary>
        /// <param name="context">The <see cref="Context"/>.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Reviewed. Suppression is OK here.")]
        public NativeListViewScrollView(Context context)
            : base(context)
        {
            this.mainHandler = new Handler(Looper.MainLooper!);
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Raises when the touch is made inside <see cref="ListViewScrollViewExt"/>.
        /// </summary>
        /// <param name="ev"><see cref="MotionEvent "/>.</param>
        /// <returns>True, if touch is handled by this view otherwise false.</returns>
        public override bool OnInterceptTouchEvent(MotionEvent? ev)
        {
            bool isVertical = this.listViewScrollViewExt!.Orientation == ScrollOrientation.Vertical;
            if (ev!.Action == MotionEventActions.Down)
            {
                this.touchDownXPosition = isVertical ? ev.GetX() : ev.GetY();
                this.touchDownYPosition = isVertical ? ev.GetY() : ev.GetX();
            }
            if (ev.Action == MotionEventActions.Move)
            {
                var moveX = isVertical ? ev.GetX() : ev.GetY();
                var moveY = isVertical ? ev.GetY() : ev.GetX();
                var xDiff = (this.touchDownXPosition - moveX) / this.Resources!.DisplayMetrics!.Density;
                var yDiff = (this.touchDownYPosition - moveY) / this.Resources.DisplayMetrics!.Density;
                if (System.Math.Abs(yDiff) > 10 && System.Math.Abs(xDiff) <= System.Math.Abs(yDiff))
                {
                    // Vertical scrolling does not works after horizontal scroll when using nested list view. Touch is not handled by scroll view after horizontal scroll. Hence handled touch based on when touch is moved to scroll in vertical direction.
                    if (isVertical)
                    {
                        return true;
                    }
                }

                else if (this.listViewScrollViewExt!.IsSwipingEnabled() && !isVertical && System.Math.Abs(xDiff) > 10 && System.Math.Abs(yDiff) <= System.Math.Abs(xDiff))
                {
                    //// Swiping not working properly in Horizontal ListView.
                    //// Returned touch when we are trying to scroll vertically for horizontal listView. touch is not needed in scrollview in this case, 
                    //// handling touch in scrollView is restricting swiping by canceling touch in item renderer.
                    return false;
                }
            }
            return base.OnInterceptTouchEvent(ev);
        }

        /// <summary>
        /// Handle the scrolling when scroll offset changed and which will run in Handler for avoiding the delay of loading.
        /// </summary>
        public override void ComputeScroll()
        {
            if (this.listViewScrollViewExt!.Orientation == ScrollOrientation.Vertical)
            {
                if (this.Scroller != null && this.Scroller.Handle != IntPtr.Zero && !this.Scroller.ComputeScrollOffset())
                {
                    return;
                }

                Runnable myRunnable = new Runnable(() =>
                {
                    if (this.Scroller != null && this.Scroller.Handle != IntPtr.Zero)
                    {
                        // XAMARIN-33151 When trying to select items when listview reaches the end of the list on fling, the first touch does not pass to the list view item, touches only passes after few seconds. Hence added condition to check whether list reaches the end and handles the touch.
                        double finalYPosition = this.Scroller.FinalY;
                        var maxScrollOffset = this.GetChildAt(0)!.Height - this.Height;
                        if (finalYPosition > maxScrollOffset)
                        {
                            finalYPosition = maxScrollOffset;
                        }
                        else if (finalYPosition < 0)
                        {
                            finalYPosition = 0;
                        }

                        // On fling action, ScrollTo is fired repeatedly even after reach the scroll value to the end. So swiping is not happened when stop the scrolling suddenly. So added the below code for fix this issue.
                        // XAMARIN-9955 - Need to abort the scrolling animation, when the reverse scrolling is occured if items are added at end of listview and it occurs due to scrollY is greater than scroller's FinalY and end is reached.
                        if ((!this.Scroller.IsFinished && finalYPosition == this.ScrollY) ||
                            (finalYPosition < this.ScrollY && this.Scroller.FinalY > 0 && this.Scroller.IsOverScrolled))
                        {
                            this.Scroller.AbortAnimation();
                        }
                        else
                        {
                            this.ScrollTo(this.Scroller.CurrX, this.Scroller.CurrY);
                            this.PostInvalidate();
                        }
                    }
                });
                this.mainHandler!.Post(myRunnable);
            }
            else if (this.Hscroller != null && this.Hscroller.Handle != IntPtr.Zero)
            {
                // XAMARIN - 13560 - Need to abort the scrolling animation, when the reverse scrolling is occurred if extent is changed at runtime with  and it occurs due to scrollX is greater than hscroller's FinalX and end is reached.
                if (!this.Hscroller.ComputeScrollOffset())
                {
                    return;
                }

                Runnable myRunnable = new Runnable(() =>
                {
                    if (this.Hscroller != null && this.HScrollView != null && this.Hscroller.Handle != IntPtr.Zero && this.HScrollView.Handle != IntPtr.Zero)
                    {
                        if ((!this.Hscroller.IsFinished && this.Hscroller.FinalX == this.HScrollView.ScrollX) ||
                            (this.Hscroller.FinalX < this.HScrollView.ScrollX && this.Hscroller.FinalX > 0 && this.Hscroller.IsOverScrolled))
                        {
                            this.Hscroller.AbortAnimation();
                        }
                        else
                        {
                            this.HScrollView.ScrollTo(this.Hscroller.CurrX, this.Hscroller.CurrY);
                            this.PostInvalidate();
                        }
                    }
                });
                this.mainHandler!.Post(myRunnable);
            }
        }

        #endregion
    }
}