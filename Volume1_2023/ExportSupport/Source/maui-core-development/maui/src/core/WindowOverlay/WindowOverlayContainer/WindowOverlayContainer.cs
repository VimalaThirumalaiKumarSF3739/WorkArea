using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    internal class WindowOverlayContainer : View
    {
        internal virtual bool canHandleTouch
        {
            get { return false; }
        }
    }
}
