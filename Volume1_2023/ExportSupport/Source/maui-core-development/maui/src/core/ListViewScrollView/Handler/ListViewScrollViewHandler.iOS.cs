using CoreGraphics;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// ScrollViewHandler for <see cref="ListViewScrollViewExt"/>.
    /// </summary>
    // Todo - Reverting SfInteractiveScrollView - internal partial class ListViewScrollViewHandler : SfInteractiveScrollViewHandler
    internal partial class ListViewScrollViewHandler : ScrollViewHandler
    {
        #region Overrides

        /// <summary>
        /// Connects the handler.
        /// </summary>
        /// <param name="platformView">Instance of platformView.</param>
        // Todo - Reverting SfInteractiveScrollView - protected override void ConnectHandler(PlatformScrollViewer platformView)
        protected override void ConnectHandler(UIScrollView platformView)
        {
            base.ConnectHandler(platformView);
            if (this.PlatformView != null && this.ScrollView!.ScrollingEnabled)
            {
                this.PlatformView.Scrolled += this.NativeScrollView_Scrolled;
                this.PlatformView.DraggingStarted += this.NativeScrollView_DraggingStarted;
                this.PlatformView.DraggingEnded += this.NativeScrollView_DraggingEnded;
                this.PlatformView.DecelerationStarted += this.NativeScrollView_DecelerationStarted;
                this.PlatformView.DecelerationEnded += this.NativeScrollView_DecelerationEnded;
                this.PlatformView.ScrollAnimationEnded += this.NativeScrollView_ScrollAnimationEnded;
            }
        }

        /// <summary>
        /// Sets the VirtualView.
        /// </summary>
        /// <param name="view">Instance of virtualView.</param>
        public override void SetVirtualView(IView view)
        {
            base.SetVirtualView(view);
            if (this.ScrollView!.ScrollingEnabled)
            {
                this.ScrollView!.PropertyChanged += this.OnListViewScrollViewPropertyChanged;
            }
        }

        /// <summary>
        /// Disconnects the handler.
        /// </summary>
        /// <param name="nativeView">Instance of nativeView.</param>
        // Todo - Reverting SfInteractiveScrollView - protected override void DisconnectHandler(PlatformScrollViewer nativeView)
        protected override void DisconnectHandler(UIScrollView nativeView)
        {
            if (this.ScrollView != null && this.ScrollView!.ScrollingEnabled)
            {
                if (this.PlatformView != null)
                {
                    this.PlatformView.Scrolled -= this.NativeScrollView_Scrolled;
                    this.PlatformView.DraggingStarted -= this.NativeScrollView_DraggingStarted;
                    this.PlatformView.DraggingEnded -= this.NativeScrollView_DraggingEnded;
                    this.PlatformView.DecelerationStarted -= this.NativeScrollView_DecelerationStarted;
                    this.PlatformView.DecelerationEnded -= this.NativeScrollView_DecelerationEnded;
                    this.PlatformView.ScrollAnimationEnded -= this.NativeScrollView_ScrollAnimationEnded;
                }
            }

            base.DisconnectHandler(nativeView);
        }

        /// <summary>
        /// Arranges the childrens.
        /// </summary>
        /// <param name="rect">Bounds value.</param>
        public override void PlatformArrange(Rect rect)
        {
            base.PlatformArrange(rect);
            if (this.ScrollView!.IsViewLoadedAndHasHorizontalRTL())
            {
                // Todo - Directly setted contentOffset because when calling ListView.ScrollTo with animate false - Animation occurs.
                this.PlatformView!.ContentOffset = new CGPoint(this.ScrollView.GetContainerTotalExtent() - this.PlatformView.Frame.Width, 0);
                this.ScrollView.SetIsHorizontalRTLViewLoaded(true, 0);
            }

        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Raised when UIScrollView gets scrolled.
        /// </summary>
        /// <param name="sender">Intsance of <see cref="PlatformScrollViewer"/>.</param>
        /// <param name="e">The event args.</param>
        private void NativeScrollView_Scrolled(object? sender, EventArgs e)
        {
            if (this.ScrollView!.IsProgrammaticScrolling)
            {
                this.ScrollView.SetScrollState("Programmatic");
                if (this.ScrollView.DisableAnimation)
                {
                    this.ScrollView.IsProgrammaticScrolling = false;
                    this.ScrollView.DisableAnimation = false;
                    this.ScrollView.SetScrollState("Idle");
                }
            }
        }

        /// <summary>
        /// Raises when the fling action is started on the <see cref="PlatformScrollViewer"/>.
        /// </summary>
        /// <param name="sender">The <see cref="PlatformScrollViewer"/>.</param>
        /// <param name="e">The event args.</param>
        private void NativeScrollView_DecelerationStarted(object? sender, EventArgs e)
        {
            this.ScrollView!.SetScrollState("Fling");
        }

        /// <summary>
        /// Raises when the dragging action is started on the <see cref="PlatformScrollViewer"/>.
        /// </summary>
        /// <param name="sender">The <see cref="PlatformScrollViewer"/>.</param>
        /// <param name="e">The event args.</param>
        private void NativeScrollView_DraggingStarted(object? sender, EventArgs e)
        {
            this.ScrollView!.SetScrollState("Dragging");
        }

        /// <summary>
        /// Raises when the dragging action is completed on the <see cref="PlatformScrollViewer"/>.
        /// </summary>
        /// <param name="sender">The <see cref="PlatformScrollViewer"/>.</param>
        /// <param name="e">The event args.</param>
        private void NativeScrollView_DraggingEnded(object? sender, UIKit.DraggingEventArgs e)
        {
            if (!e.Decelerate)
            {
                this.ScrollView!.SetScrollState("Idle");
            }
        }

        /// <summary>
        /// Raises when the fling action is completed on the <see cref="UIScrollView"/>.
        /// </summary>
        /// <param name="sender">The <see cref="PlatformScrollViewer"/>.</param>
        /// <param name="e">The event args.</param>
        private void NativeScrollView_DecelerationEnded(object? sender, EventArgs e)
        {
            this.ScrollView!.SetScrollState("Idle");
        }


        /// <summary>
        /// Raised when UIScrollView scroll animation ended.
        /// </summary>
        /// <param name="sender">Intsance of <see cref="PlatformScrollViewer"/>.</param>
        /// <param name="e">The event args.</param>
        private void NativeScrollView_ScrollAnimationEnded(object? sender, EventArgs e)
        {
            if (this.ScrollView!.IsProgrammaticScrolling)
            {
                this.ScrollView.IsProgrammaticScrolling = false;
                this.ScrollView!.SetScrollState("Idle");
            }
        }

        /// <summary>
        /// Raises when <see cref="ListViewScrollViewExt"/> property changes.
        /// </summary>
        /// <param name="sender">Instance of <see cref="ListViewScrollViewExt"/>.</param>
        /// <param name="e">Property changed event args.</param>
        private void OnListViewScrollViewPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ScrollPosition")
            {
                var position = this.ScrollView!.ScrollPosition;
                if (this.ScrollView!.Orientation == ScrollOrientation.Vertical)
                {
                    this.PlatformView.SetContentOffset(new CGPoint(0, position), false);
                }
                else
                {
                    this.PlatformView.SetContentOffset(new CGPoint(position, 0), false);
                }
            }
        }

        #endregion
    }
}