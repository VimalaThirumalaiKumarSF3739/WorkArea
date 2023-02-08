using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;
using UIKit;

namespace Syncfusion.Maui.Core.Platform
{
    /// <summary>
    /// Native view for <see cref="SfDrawableView"/>.
    /// </summary>
    public class PlatformGraphicsViewExt : PlatformGraphicsView
    {
        #region Fields

        private View? mauiView;

        #endregion

        #region Properties

        /// <summary>
        /// Returns a boolean value indicating whether this object can become the first responder.
        /// </summary>
        public override bool CanBecomeFirstResponder => (mauiView! is IKeyboardListener) && (mauiView! as IKeyboardListener)!.CanBecomeFirstResponder;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformGraphicsViewExt"/> class.
        /// </summary>
        /// <param name="drawable">Instance of virtual view.</param>
        public PlatformGraphicsViewExt(IDrawable? drawable = null) : base(drawable)
        {
            mauiView = (View)drawable!;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Raised when a button is pressed.
        /// </summary>
        /// <param name="presses">A set of <see cref="UIPress"/> instances that represent the new presses that occurred.</param>
        /// <param name="evt">The event to which the presses belong.</param>
        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (this.mauiView != null && !this.mauiView.HandleKeyPress(presses, evt))
            {
                base.PressesBegan(presses, evt);
            }
        }

        /// <summary>
        /// Raised when a button is released.
        /// </summary>
        /// <param name="presses">A set of <see cref="UIPress"/> instances that represent the buttons that the user is no longer pressing.</param>
        /// <param name="evt">The event to which the presses belong.</param>
        public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (this.mauiView != null && !this.mauiView.HandleKeyRelease(presses, evt))
            {
                base.PressesEnded(presses, evt);
            }
        }

        #endregion
    }
}
