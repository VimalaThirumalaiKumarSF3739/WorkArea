namespace Syncfusion.Maui.Core
{
    using Android.Content;
    using Android.OS;
    using Android.Views;
    using Android.Widget;
    using Microsoft.Maui.Handlers;
    using Microsoft.Maui.Platform;
    using System.Linq;
    using static Android.Views.View;

    /// <summary>
    /// Represents a class which contains the information of scroll view handlers.
    /// TODO: As vertical scroll is not working in Android while using scroll orientation as Both, native handler added for scroll view.
    /// Issue report-https://github.com/dotnet/maui/issues/5759.
    /// </summary>
    internal class CustomScrollLayoutHandler : ScrollViewHandler
    {
        /// <summary>
        /// The horizontal scroll view, which is added while scroll orientation is set to horizontal or both.
        /// </summary>
        private HorizontalScrollView? horizontalScrollView;

        /// <inheritdoc/>
        protected override MauiScrollView CreatePlatformView()
        {
            var nativeScrollView = new NativeCustomScrolLayout(this.Context, this.VirtualView);
            // Set to avoid overlapping control issue when scrolling.
            nativeScrollView.ClipToOutline = true;
            
            return nativeScrollView;
        }

        /// <inheritdoc/>
        protected override void ConnectHandler(MauiScrollView platformView)
        {
            base.ConnectHandler(platformView);
            platformView.ChildViewAdded += this.OnNativeScrollViewChildViewAdded;
            platformView.ChildViewRemoved += this.OnNativeScrollViewChildViewRemoved;
            platformView.ScrollChange += this.OnNativeScrollViewScrollChange;
        }

        // <inheritdoc/>
        protected override void DisconnectHandler(MauiScrollView platformView)
        {
            base.DisconnectHandler(platformView);
            platformView.ChildViewAdded -= this.OnNativeScrollViewChildViewAdded;
            platformView.ChildViewRemoved -= this.OnNativeScrollViewChildViewRemoved;
            platformView.ScrollChange -= this.OnNativeScrollViewScrollChange;
            if (this.horizontalScrollView != null)
            {
                if (Build.VERSION.SdkInt > BuildVersionCodes.LollipopMr1)
                {
#pragma warning disable CA1416
                    this.horizontalScrollView.ScrollChange -= this.OnHorizontalScrollViewScrollChange;
#pragma warning restore CA1416
                }

                this.horizontalScrollView = null;
            }
        }

        /// <summary>
        /// Invoks on child view added to native scroll view.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        private void OnNativeScrollViewChildViewAdded(object? sender, ViewGroup.ChildViewAddedEventArgs e)
        {
            if (e.Child is HorizontalScrollView nativeHorizontalScrollView)
            {
                if (Build.VERSION.SdkInt > BuildVersionCodes.LollipopMr1)
                {
#pragma warning disable CA1416
                    nativeHorizontalScrollView.ScrollChange += this.OnHorizontalScrollViewScrollChange;
#pragma warning restore CA1416
                }

                this.horizontalScrollView = nativeHorizontalScrollView;
            }
        }

        /// <summary>
        /// Invoks on child view removed from native scroll view.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event args.</param>
        private void OnNativeScrollViewChildViewRemoved(object? sender, ViewGroup.ChildViewRemovedEventArgs e)
        {
            if (e.Child is HorizontalScrollView && this.horizontalScrollView != null)
            {
                if (Build.VERSION.SdkInt > BuildVersionCodes.LollipopMr1)
                {
#pragma warning disable CA1416
                    this.horizontalScrollView.ScrollChange -= this.OnHorizontalScrollViewScrollChange;
#pragma warning restore CA1416
                }

                this.horizontalScrollView = null;
            }
        }

        /// <summary>
        /// Invoks on horizontal scroll change.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The scroll event args.</param>
        private void OnHorizontalScrollViewScrollChange(object? sender, ScrollChangeEventArgs e)
        {
            this.VirtualView.HorizontalOffset = ContextExtensions.FromPixels(this.Context, e.ScrollX);
        }

        /// <summary>
        /// Invoks on vertical scroll change.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The scroll event args.</param>
        private void OnNativeScrollViewScrollChange(object? sender, AndroidX.Core.Widget.NestedScrollView.ScrollChangeEventArgs e)
        {
            if (this.horizontalScrollView != null)
            {
                //// In framework scroll change event, the horizontal scroll offset is reset to 0 while vertically scrolling. Hence here resetting the offset value. 
                this.VirtualView.HorizontalOffset = ContextExtensions.FromPixels(this.Context, this.horizontalScrollView.ScrollX);
            }

            //// On scrolled to bottom and again scrolling(for example scroll bounce at bottom), the scroll y value changed to 0 in maui view.
            //// But in native view the scrollY values remains same, hence reset Maui view ScrollY value using native view's Scroll offset value.
            if (e.V != null && e.ScrollY != e.V.ScrollY)
            {
                this.VirtualView.VerticalOffset = this.Context.FromPixels(e.V.ScrollY);
            }
        }
    }

    /// <summary>
    /// Represents a class which contains the information of native scroll view.
    /// </summary>
    internal class NativeCustomScrolLayout : MauiScrollView
    {
        /// <summary>
        /// The MAUI parenet scroll view.
        /// </summary>
        private Microsoft.Maui.IScrollView scrollView;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeCustomScrolLayout" /> class.
        /// </summary>
        /// <param name="context">The <see cref="Context"/>.</param>
        /// <param name="scrollView">The parent scroll view.</param>
        public NativeCustomScrolLayout(Context context, Microsoft.Maui.IScrollView scrollView) : base(context)
        {
            this.scrollView = scrollView;
        }

        /// <inheritdoc/>
        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);

            //// While scroll orientatio is set to both the vertical scroll is not working. Hence using measured height for horizontal scroll view to render in actual height.
            if (this.scrollView != null && this.scrollView.Orientation == Microsoft.Maui.ScrollOrientation.Both)
            {
                var hScrollViews = this.GetChildrenOfType<HorizontalScrollView>();
                var horizontalScrollView = hScrollViews!.FirstOrDefault();
                if (horizontalScrollView != null)
                {
                    horizontalScrollView.Layout(0, 0, right - left, horizontalScrollView.MeasuredHeight);
                }
            }
        }
    }
}
