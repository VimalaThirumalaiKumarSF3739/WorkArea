using System;
using Syncfusion.Maui.Core.Internals;

namespace Syncfusion.Maui.Core.Internals
{
    internal class ScrollToParameters
    {
        internal ScrollToParameters(double scrollX, double scrollY, bool animated = false)
        {
            ScrollX = scrollX;
            ScrollY = scrollY;
            Animated = animated;
        }

        internal double ScrollY { get; }
        internal double ScrollX { get; }
        internal bool Animated { get; }
    }

    /// <summary>
    /// Provides data for the <see cref="SfInteractiveScrollView.ScrollChanged"/> event.
    /// </summary>
    public class ScrollChangedEventArgs : EventArgs
    {
        internal ScrollChangedEventArgs(double scrollX, double scrollY, double oldScrollX, double oldScrollY)
        {
            OldScrollX = oldScrollX;
            OldScrollY = oldScrollY;
            ScrollX = scrollX;
            ScrollY = scrollY;
            HorizontalChange = scrollX - oldScrollX;
            VerticalChange = scrollY - oldScrollY;
        }

        internal double OldScrollX { get; }
        internal double OldScrollY { get; }
        internal double ScrollX { get; }
        internal double ScrollY { get; }

        /// <summary>
        /// Gets a value that indicates the change in the vertical scroll distance.
        /// </summary>
        public double VerticalChange { get; private set; }

        /// <summary>
        /// Gets a value that indicates the change in the horizontal scroll distance.
        /// </summary>
        public double HorizontalChange { get; private set; }
    }
}