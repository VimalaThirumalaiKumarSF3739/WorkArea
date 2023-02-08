using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace Syncfusion.Maui.Core.Internals
{
    internal class UIKeyEventArgs : EventArgs
    {
        internal NSSet<UIPress> Presses { get; }
        internal UIPressesEvent PressesEvent { get; }
        internal bool Handled { get; set; }

        internal UIKeyEventArgs(NSSet<UIPress> presses, UIPressesEvent pressesEvent)
        {
            this.Presses = presses;
            this.PressesEvent = pressesEvent;
        }
    }

    internal partial class PlatformScrollViewer : UIScrollView
    {
        bool m_canBecomeFirstResponder;

        internal event EventHandler<UIKeyEventArgs>? KeyPressesBegan;
        internal event EventHandler<UIKeyEventArgs>? KeyPressesEnded;
        internal event EventHandler<EventArgs>? LayoutChanged;

        internal PlatformScrollViewer()
        {
            Bounces = BouncesZoom = false;
        }

        public override bool CanBecomeFirstResponder => m_canBecomeFirstResponder;

        internal void SetCanBecomeFirstResponder(bool value)
        {
            m_canBecomeFirstResponder = value;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            LayoutChanged?.Invoke(this, EventArgs.Empty);
        }

        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (KeyPressesBegan != null)
            {
                UIKeyEventArgs eventArgs = new UIKeyEventArgs(presses, evt);
                KeyPressesBegan?.Invoke(this, eventArgs);
                if (eventArgs.Handled)
                    return;
            }
            base.PressesBegan(presses, evt);
        }

        public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (KeyPressesEnded != null)
            {
                UIKeyEventArgs eventArgs = new UIKeyEventArgs(presses, evt);
                KeyPressesEnded?.Invoke(this, eventArgs);
                if (eventArgs.Handled)
                    return;
            }
            base.PressesEnded(presses, evt);
        }

        internal void ScrollToVerticalOffset(double offset, bool animated)
        {
            offset = GetMaxScrollOffset(offset, ContentSize.Height, Frame.Height);
            SetContentOffset(new CGPoint(ContentOffset.X, offset), animated);
        }

        internal void ScrollToHorizontalOffset(double offset, bool animated)
        {
            offset = GetMaxScrollOffset(offset, ContentSize.Width, Frame.Width);
            SetContentOffset(new CGPoint(offset, ContentOffset.Y), animated);
        }

        internal void ScrollTo(double horizontalOffset, double verticalOffset, bool animated)
        {
            horizontalOffset = GetMaxScrollOffset(horizontalOffset, ContentSize.Width, Frame.Width);
            verticalOffset = GetMaxScrollOffset(verticalOffset, ContentSize.Height, Frame.Height);
            SetContentOffset(new CGPoint(horizontalOffset, verticalOffset), animated);
        }

        /// <summary>
        /// Provides the maximum scroll offset, that the control can be scrolled to display the last frame of the contents.
        /// </summary>
        /// <param name="offset">current offset</param>
        /// <param name="contentEnd">vertical/horizontal end of the content</param>
        /// <param name="frameLength">ScrollViewer's (frame) width/height</param>
        /// <returns>Maximum scroll offset that a scroll viewer can scroll</returns>
        double GetMaxScrollOffset(double offset, double contentEnd, double frameLength)
        {
            var maxScrollOffset = contentEnd - frameLength;
            return offset <= maxScrollOffset ? offset : maxScrollOffset;
        }
    }
}
