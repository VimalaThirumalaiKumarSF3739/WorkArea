using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// Represents a customized <see cref="SfInteractiveScrollView"/> needed for SfListView requirements.
    /// </summary>
    //  Todo - Reverting SfInteractiveScrollView - public abstract class ListViewScrollViewExt : SfInteractiveScrollView
    public abstract class ListViewScrollViewExt : ScrollView
    {
        #region Fields

        /// <summary>
        /// Indicates whether the scrolling happens on programmatic call.
        /// </summary>
        internal bool IsProgrammaticScrolling = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the scrolling is enabled or disabled for listview.
        /// </summary>
        internal abstract bool ScrollingEnabled { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the animation is disabled in programmatic scrolling.
        /// </summary>
        internal abstract bool DisableAnimation { get; set; }

        /// <summary>
        /// Gets or sets the scrolled position value when programmatic scrolling is performed.
        /// </summary>
        internal double ScrollEndPosition { get; set; }

        /// <summary>
        /// Gets or sets the value need to do programmatic scrolling when AutoFitMode is not None or SfListView.HasGroups is enabled or animation is disabled.
        /// </summary>
        internal abstract double ScrollPosition { get; set; }

        #endregion

        #region Virtual Helper Methods

        /// <summary>
        /// This methods returns the total extent of ListView.VisualContainer.
        /// </summary>
        /// <returns>Returns the total extent of ListView.VisualContainer.</returns>
        internal abstract double GetContainerTotalExtent();

        /// <summary>
        /// This method invokes ListView.ProcessAutoOnScroll().
        /// </summary>
        internal abstract void ProcessAutoOnScroll();

        /// <summary>
        /// This methods gets whether listview swiping is in progress or not.
        /// </summary>
        /// <returns>Returns the value of ListView.SwipeController.IsInSwiping.</returns>
        internal abstract bool IsListViewInSwiping();

        /// <summary>
        /// This methods gets whether listview.AllowSwiping is true or not.
        /// </summary>
        /// <returns>Returns the value of ListView.AllowSwiping.</returns>
        internal abstract bool IsSwipingEnabled();

        /// <summary>
        /// This method returns whether listView.IsNativeScrollViewLoaded flag is true or not.
        /// </summary>
        /// <returns>Returns the value of listView.IsNativeScrollViewLoaded.</returns>
        internal abstract bool IsNativeScrollViewLoaded();

        /// <summary>
        /// This method returns true if listView.IsHorizontalRTLViewLoaded flag and listView.HasHorizontalRTL() is true.
        /// </summary>
        /// <returns>Returns true if both the listView.IsHorizontalRTLViewLoaded and listView.HasHorizontalRTL() is true else false.</returns>
        internal abstract bool IsViewLoadedAndHasHorizontalRTL();

        /// <summary>
        /// This method sets the value to listView.IsHorizontalRTLViewLoaded.
        /// </summary>
        internal abstract void SetIsHorizontalRTLViewLoaded(bool IsHorizontalRTLViewLoaded, double scrollX);

        /// <summary>
        /// This method sets the ScrollState for ListViewScrollView.
        /// </summary>
        /// <param name="scrollState">Represent the currentScrollState.</param>
        internal abstract void SetScrollState(string scrollState);

        /// <summary>
        /// This method gets the ScrollState for ListViewScrollView.
        /// </summary>
        internal abstract string GetScrollState();

        /// <summary>
        /// This method invalidates the VisualContainer.
        /// </summary>
        internal abstract void InvalidateContainerIfRequired();

        #endregion
    }
}
