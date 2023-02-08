using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// ScrollViewHandler for <see cref="ListViewScrollViewExt"/>.
    /// </summary>
    // Todo - Reverting SfInteractiveScrollView - internal partial class ListViewScrollViewHandler : SfInteractiveScrollViewHandler
    internal partial class ListViewScrollViewHandler : ScrollViewHandler
    {
        #region

        /// <summary>
        /// Native instance of <see cref="ListViewScrollViewExt"/>'s content.
        /// </summary>
        private FrameworkElement? content;

        /// <summary>
        /// Has the previous scroll offset to compute current offset based on ManipulationDelta cumulative translation value.
        /// </summary>
        private double previousOffset = 0;

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
                if (this.PlatformView.Content != null)
                {
                    this.content = this.PlatformView.Content! as FrameworkElement;
                    this.content!.ManipulationMode = ManipulationModes.All;
                    this.WireEvents();
                }
            }
        }

        /// <summary>
        /// Disconnects the handler.
        /// </summary>
        /// <param name="nativeView">Instance of nativeView.</param>
        protected override void DisconnectHandler(ScrollViewer nativeView)
        {
            if (this.ScrollView != null && this.ScrollView.ScrollingEnabled)
            {
                this.ScrollView.PropertyChanged -= this.OnListViewScrollViewPropertyChanged;
                this.UnWireEvents();
            }

            base.DisconnectHandler(nativeView);
        }

        #endregion

        #region CallBack Methods

        /// <summary>
        /// Raised when manipulation such as Zoom or Pan causes the view to change.
        /// </summary>
        /// <param name="sender">Instance of nativeView.</param>
        /// <param name="e">args corresponding to <see cref="ScrollViewer"/> ViewChanging.</param>
        private void ScrollViewer_ViewChanging(object? sender, ScrollViewerViewChangingEventArgs e)
        {
            if (this.ScrollView!.IsProgrammaticScrolling)
            {
                this.ScrollView.SetScrollState("Programmatic");

                // Based on this Idle state will be set for ProgrammaticScrolling.
                this.ScrollView.ScrollEndPosition = this.ScrollView.Orientation == ScrollOrientation.Vertical ? e.FinalView.VerticalOffset : e.FinalView.HorizontalOffset;
            }
        }

        /// <summary>
        /// Raised when manipulation such as Zoom or Pan causes the view to change.
        /// </summary>
        /// <param name="sender">Instance of nativeView.</param>
        /// <param name="e">args corresponding to <see cref="ScrollViewer"/> ViewChange.</param>
        private void ScrollViewer_ViewChanged(object? sender, ScrollViewerViewChangedEventArgs e)
        {
            this.ScrollView!.SetIsHorizontalRTLViewLoaded(true, 0);
            if (this.ScrollView.GetScrollState() == "Programmatic")
            {
                if (this.ScrollView.Orientation == ScrollOrientation.Vertical && this.ScrollView.ScrollEndPosition == this.PlatformView.VerticalOffset)
                {
                    this.ScrollView.IsProgrammaticScrolling = false;
                    this.ScrollView.SetScrollState("Idle");
                }
                else if (this.ScrollView.Orientation == ScrollOrientation.Horizontal && this.ScrollView.ScrollEndPosition == this.PlatformView.HorizontalOffset)
                {
                    this.ScrollView.IsProgrammaticScrolling = false;
                    this.ScrollView.SetScrollState("Idle");
                }
            }
        }

        /// <summary>
        /// Raises when the mouse wheel is rotated.
        /// </summary>
        /// <param name="sender">Instance of scrollViewer's content.</param>
        /// <param name="e">Contains all the information of event.</param>
        private void Content_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            var isVertical = this.ScrollView!.Orientation == ScrollOrientation.Vertical;

            var isVerticalExtent = this.ScrollView.Height == this.ScrollView.GetContainerTotalExtent();
            var isHorizontalExtent = this.ScrollView.Width == this.ScrollView.GetContainerTotalExtent();

            // Focus is not passed to parent when listview has non scrollable content.
            if ((isVerticalExtent && isVertical) || (isHorizontalExtent && !isVertical))
            {
                return;
            }
            var offset = e.GetCurrentPoint(this.content).Properties.MouseWheelDelta;
            var currentOffset = isVertical ? this.PlatformView.VerticalOffset : this.PlatformView.HorizontalOffset;

            // To-do : MAUI-2939- Items are not arranged from right side with RTL and Horizontal orientation in VS2022 v17.4.0 Preview 1 (MAUI SDK v6.0.486).
            // because getting ScrollOffset is 0 at right side in WinUI.
            // Scrolling occured very fastly.So set the ScrollOffset value based on CurrentOffset.
            var scrolloffset = (double)currentOffset - offset;

            // Calculates the maximum scrollable extent.
            double maxExtent = this.ScrollView.GetContainerTotalExtent() - (isVertical ? this.ScrollView.Height : this.ScrollView.Width);

            // if computed offset is less than 0, ex: -5 computed offset will be set to 0.
            scrolloffset = (scrolloffset < 0) ? 0 : scrolloffset;

            // if computed offset is greater than scrollable extent, ex: 500 when maximum scrollable extent is 450 computed offset will be set to 450.
            scrolloffset = (scrolloffset > maxExtent) ? maxExtent : scrolloffset;

            // The above restriction will prevent from missing resetting scrollstate to Idle, because when computed offset is -5
            // we will set ScrollEndPosition to -5 in viewChanged event offset will be 0.

            // The below restriction will prevent unwanted setting scroll state to programmatic when scrolled to same offset.
            if (scrolloffset == currentOffset)
            {
                e.Handled = true;
                return;
            }

            // Todo Since there is no ScrollState for mouse wheel, considered it as programmatic as followed in xamarin.
            this.ScrollView.SetScrollState("Programmatic");

            this.ScrollView.ProcessAutoOnScroll();

            // Since we are scrolling only in single orientation,no zooming support and no need to handle animation ScrollToVerticalOffset/ScrollToHorizontalOffset is a better option to use.
            // also sometimes viewchanged, VirtualView.scrolled not triggering in WinUI while using changeView.
            if (this.ScrollView.Orientation == ScrollOrientation.Vertical)
            {
                this.PlatformView.ScrollToVerticalOffset(scrolloffset);
            }
            else
            {
                this.PlatformView.ScrollToHorizontalOffset(scrolloffset);
            }

            // XAMARIN-25829, ScrollState does not change on scrolling listview by mouse wheel or touch pad. Hence change the ScrollEndPosition which update the scroll state.
            this.ScrollView.ScrollEndPosition = scrolloffset;
            e.Handled = true;
        }

        /// <summary>
        /// Raised when fling action starts.
        /// </summary>
        /// <param name="sender">Instance of nativeView.</param>
        /// <param name="e">args corresponding to <see cref="ScrollViewer"/> ViewChanging.</param>
        private void Content_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            // Handled here to avoid SfInteractiveScrollViewHandler scrolling, Since it scrolls even on pointer fling.
            e.Handled = true;
        }

        /// <summary>
        /// Raises when a input manipulation starts in scrollViewer's content.
        /// </summary>
        /// <param name="sender">Instance of scrollViewer's content.</param>
        /// <param name="e">Provides data for the ManipulationCompleted event.</param>
        private void Content_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            // For cumulative offset calculation we need the offset that is been set before even the manipulation started.
            this.previousOffset = (this.ScrollView!.Orientation == ScrollOrientation.Vertical) ? this.PlatformView.VerticalOffset : this.PlatformView.HorizontalOffset;
        }

        /// <summary>
        /// Raises when the touch input position gets changed after a manipulation starts.
        /// </summary>
        /// <param name="sender">Instance of scrollViewer's content.</param>
        /// <param name="e">Contains all the information of event.</param>
        private void Content_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var isVertical = this.ScrollView!.Orientation == ScrollOrientation.Vertical;

            // To skip scrolling when panned using mouse.
            if (e.PointerDeviceType == Microsoft.UI.Input.PointerDeviceType.Mouse)
            {
                e.Handled = true;
                return;
            }

            // To skip scrolling when swiping on the touch interaction.
            if (this.ScrollView.IsSwipingEnabled())
            {
                if (this.ScrollView.IsListViewInSwiping())
                {
                    return;
                }
                var x = isVertical ? e.Cumulative.Translation.X : e.Cumulative.Translation.Y;
                var y = isVertical ? e.Cumulative.Translation.Y : e.Cumulative.Translation.X;
                if (Math.Abs(y) <= Math.Abs(x))
                {
                    return;
                }
            }

            if (e.IsInertial && Math.Abs(e.Velocities.Linear.X) > 0.2)
            {
                this.ScrollView.SetScrollState("Fling");
            }
            else if (this.ScrollView.GetScrollState() != "Fling")
            {
                this.ScrollView.SetScrollState("Dragging");
            }

            // With the delta calcuation we need the current offset, since for some manipulation viewchanged,VirtualView.scrolled not triggering
            // so container offset not updating properly. Resulting in a lag in the view while fling. so we have used cumulative translation values.
            // Since we are scrolling only in single orientation,no zooming support and no need to handle animation ScrollToVerticalOffset/ScrollToHorizontalOffset is a better option to use.
            // also sometimes viewchanged, VirtualView.scrolled not triggering in WinUI while using changeView.
            if (isVertical)
            {
                var verticalOffset = this.previousOffset - (this.PlatformView.FlowDirection == Microsoft.UI.Xaml.FlowDirection.RightToLeft && !isVertical ? (e.Cumulative.Translation.Y * -1) : e.Cumulative.Translation.Y);
                this.PlatformView.ScrollToVerticalOffset(verticalOffset);
            }
            else
            {
                var horizontalOffset = this.previousOffset - (this.PlatformView.FlowDirection == Microsoft.UI.Xaml.FlowDirection.RightToLeft && !isVertical ? (e.Cumulative.Translation.X * -1) : e.Cumulative.Translation.X);
                this.PlatformView.ScrollToHorizontalOffset(horizontalOffset);
            }
        }

        /// <summary>
        /// Raises when the manipulation and inertia on the scrollViewer's content is completed.
        /// </summary>
        /// <param name="sender">Instance of scrollViewer's content.</param>
        /// <param name="e">Provides data for the ManipulationCompleted event.</param>
        private void Content_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.ScrollView!.SetScrollState("Idle");
        }

        /// <summary>
        /// Raises when <see cref="ListViewScrollViewExt"/> property changes.
        /// </summary>
        /// <param name="sender">Instance of <see cref="ListViewScrollViewExt"/>.</param>
        /// <param name="e">Property changed event args.</param>
        private void OnListViewScrollViewPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ScrollPosition")
            {
                if (this.ScrollView!.Orientation == ScrollOrientation.Vertical)
                {
                    this.PlatformView.ChangeView(0, this.ScrollView.ScrollPosition, null, this.ScrollView!.DisableAnimation);
                }
                else
                {
                    this.PlatformView.ChangeView(this.ScrollView.ScrollPosition, 0, null, this.ScrollView!.DisableAnimation);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Wires all the the events needed for processing manipulations.
        /// </summary>
        private void WireEvents()
        {
            if (this.content != null)
            {
                this.PlatformView.ViewChanging += this.ScrollViewer_ViewChanging;
                this.PlatformView.ViewChanged += this.ScrollViewer_ViewChanged;
                this.content.ManipulationStarting += this.Content_ManipulationStarting;
                this.content.ManipulationDelta += this.Content_ManipulationDelta;
                this.content.ManipulationCompleted += this.Content_ManipulationCompleted;
                this.content.PointerWheelChanged += this.Content_PointerWheelChanged;
                this.content.ManipulationInertiaStarting += this.Content_ManipulationInertiaStarting;
            }
        }

        /// <summary>
        /// UnWires all the the events wired for processing manipulations.
        /// </summary>
        private void UnWireEvents()
        {
            if (this.content != null)
            {
                this.PlatformView.ViewChanging -= this.ScrollViewer_ViewChanging;
                this.PlatformView.ViewChanged -= this.ScrollViewer_ViewChanged;
                this.content.ManipulationStarting -= this.Content_ManipulationStarting;
                this.content.ManipulationDelta -= this.Content_ManipulationDelta;
                this.content.ManipulationCompleted -= this.Content_ManipulationCompleted;
                this.content.PointerWheelChanged -= this.Content_PointerWheelChanged;
                this.content.ManipulationInertiaStarting -= this.Content_ManipulationInertiaStarting;
            }
        }

        #endregion
    }
}
