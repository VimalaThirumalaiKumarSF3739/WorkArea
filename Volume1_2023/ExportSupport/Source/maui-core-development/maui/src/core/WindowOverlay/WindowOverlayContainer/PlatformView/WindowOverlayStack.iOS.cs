using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace Syncfusion.Maui.Core.Internals
{
    internal class WindowOverlayStack : UIView
    {
        internal bool canHandleTouch = false;

        public override UIView HitTest(CGPoint point, UIEvent? uievent)
        {
            UIView view = base.HitTest(point, uievent);
            // TODO: Check possibility to pass null and remove suppressing the warning.
#pragma warning disable CS8603 // Possible null reference return.
            return !canHandleTouch && view == this ? null : view;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
