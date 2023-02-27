using Microsoft.Maui.Handlers;
using UIKit;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Represents a class which contains the information of scroll view handlers.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "We require this")]
    internal class CustomScrollLayoutHandler : ScrollViewHandler
    {
        /// <summary>
        /// Method used to create the native view.
        /// </summary>
        /// <returns>The UI scroll view.</returns>
        protected override UIScrollView CreatePlatformView()
        {
            //// DirectionalLockEnabled - While scroll orientation is set to both direction and if scroll offset changes in both direction then this will disable the minimum scroll offset direction.
            //// Thus it enables the scroll only on the higher offset change direction.
            return new NativeCustomScrollLayout() { Bounces = false, DirectionalLockEnabled = true };
        }
    }

    /// <summary>
    /// Represents a class which contains the information of native scroll view.
    /// </summary>
    internal class NativeCustomScrollLayout : UIScrollView
    {
        /// <summary>
        /// Method decide to hold or pass the touch from parent.
        /// If it return true then it holds the touch.
        /// Else it pass the touch to its parent.
        /// </summary>
        /// <param name="gestureRecognizer">The gesture recognizer.</param>
        /// <returns>True, If gesture is recognized or else false.</returns>
        public override bool GestureRecognizerShouldBegin(UIGestureRecognizer gestureRecognizer)
        {
            var recognizer = gestureRecognizer as UIPanGestureRecognizer;
            //// Return base touch begin action when the method triggered from gesture recognizer other than pan gesture.
            if (recognizer == null)
            {
                return base.GestureRecognizerShouldBegin(gestureRecognizer);
            }

            var velocity = recognizer.VelocityInView(this);
            //// Return touch to it's parent for below scenarios
            //// Return touch to parent when the scroll view reaches it end scroll position and try to scroll after the end scroll position.
            //// Return touch to parent when the scroll view reaches it minimum scroll position(0) and try to scroll before the minimum scroll position.
            if ((this.ContentOffset.X >= this.ContentSize.Width - this.Frame.Width && velocity.X < 0) || (this.ContentOffset.X <= 0 && velocity.X > 0))
            {
                return false;
            }

            return true;
        }
    }
}