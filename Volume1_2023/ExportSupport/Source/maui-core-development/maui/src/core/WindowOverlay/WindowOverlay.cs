using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
#if MONOANDROID
using PlatformView = Android.Views.View;
#elif __IOS__ || MACCATALYST
using PlatformView = UIKit.UIView;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.FrameworkElement;
#else
using PlatformView = System.Object;
#endif

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// The <see cref="SfWindowOverlay"/> allows the users to add or update an independent 
    /// <see cref="Microsoft.Maui.Controls.View"/> to float above the application window. The AddOrUpdate() methods 
    /// allows you to position it both absolutely and relatively.
    /// 
    /// The passed view is eliminated from the floating window via the Remove() function. Using the RemoveFromWindow() 
    /// method, you can also delete all floating views. 
    /// </summary>
    internal partial class SfWindowOverlay
    {
        #region Fields

        private IWindow? window;

        private bool hasOverlayStackInRoot = false;

        private readonly Dictionary<PlatformView, PositionDetails> positionDetails;

        private WindowOverlayContainer? overlayStackView;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfWindowOverlay"/> class.
        /// </summary>
        internal SfWindowOverlay()
        {
            positionDetails = new Dictionary<PlatformView, PositionDetails>();
        }

        #endregion

        #region Methods

        internal void SetWindowOverlayContainer(WindowOverlayContainer view)
        {
            overlayStackView = view;
        }

        /// <summary>
        /// Gets the application windows and adds a new overlay stack for including the independent views.
        /// </summary>
        public void AddToWindow()
        {
            if (!hasOverlayStackInRoot)
            {
                window = WindowOverlayHelper.window;
                Initialize();
            }
        }

        /// <summary>
        /// Calculates a new absolute position based on the given alignment and size.
        /// </summary>
        /// <param name="horizontalAlignment"></param>
        /// <param name="verticalAlignment"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void AlignPosition(
            WindowOverlayHorizontalAlignment horizontalAlignment,
            WindowOverlayVerticalAlignment verticalAlignment,
            float width,
            float height,
            ref float x,
            ref float y)
        {
            switch (horizontalAlignment)
            {
                case WindowOverlayHorizontalAlignment.Right:
                    x -= width;
                    break;

                case WindowOverlayHorizontalAlignment.Center:
                    x -= width / 2;
                    break;
            }

            switch (verticalAlignment)
            {
                case WindowOverlayVerticalAlignment.Bottom:
                    y -= height;
                    break;

                case WindowOverlayVerticalAlignment.Center:
                    y -= height / 2;
                    break;
            }
        }

        /// <summary>
        /// Calculates a new relative position based on the given alignment, relative view size, and child size.
        /// </summary>
        /// <param name="horizontalAlignment"></param>
        /// <param name="verticalAlignment"></param>
        /// <param name="childWidth"></param>
        /// <param name="childHeight"></param>
        /// <param name="relativeViewWidth"></param>
        /// <param name="relativeViewHeight"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void AlignPositionToRelative(
            WindowOverlayHorizontalAlignment horizontalAlignment,
            WindowOverlayVerticalAlignment verticalAlignment,
            float childWidth,
            float childHeight,
            float relativeViewWidth,
            float relativeViewHeight,
            ref float x,
            ref float y)
        {
            switch (horizontalAlignment)
            {
                case WindowOverlayHorizontalAlignment.Right:
                    x += relativeViewWidth;
                    break;

                case WindowOverlayHorizontalAlignment.Center:
                    x += relativeViewWidth / 2 - childWidth / 2;
                    break;

                case WindowOverlayHorizontalAlignment.Left:
                    x += -childWidth;
                    break;
            }

            switch (verticalAlignment)
            {
                case WindowOverlayVerticalAlignment.Bottom:
                    y += relativeViewHeight;
                    break;

                case WindowOverlayVerticalAlignment.Center:
                    y += relativeViewHeight / 2 - childHeight / 2;
                    break;

                case WindowOverlayVerticalAlignment.Top:
                    y += -childHeight;
                    break;
            }
        }

        #endregion
    }

    /// <summary>
    /// Holds the <see cref="SfWindowOverlay"/> child positioning details for
    /// re-layouting during the window resize and orientation changes.
    /// </summary>
    internal class PositionDetails
    {
        #region Properties

        internal PlatformView? Relative { get; set; }

        internal float X { get; set; }

        internal float Y { get; set; }

        internal WindowOverlayHorizontalAlignment HorizontalAlignment { get; set; }

        internal WindowOverlayVerticalAlignment VerticalAlignment { get; set; }

        #endregion
    }
}