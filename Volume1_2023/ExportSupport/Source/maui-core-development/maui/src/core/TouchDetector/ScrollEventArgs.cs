﻿using System;
using System.Collections.Generic;
using Microsoft.Maui.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// This class serves as an event data for the mouse wheel action on the view.
    /// </summary>
    public class ScrollEventArgs
    {
        /// <summary>
        ///  Returns pointer Id.
        /// </summary>
        public long PointerID { private set; get; }

        /// <summary>
        /// Returns mouse wheel moving delta.
        /// </summary>
        public double ScrollDelta { private set; get; }

        /// <summary>
        ///  Returns actual touch point.
        /// </summary>
        public Point TouchPoint { private set; get; }

        /// <summary>
        /// Gets or sets a value that marks the routed event as handled, and prevents most handlers along the event route from handling the same event again. 
        /// </summary>
        /// <remarks>
        /// It is applicable only for routed events which are supported in Windows.
        /// </remarks>
        internal bool Handled { set; get; }

        /// <summary>
        /// Initializes when <see cref="ScrollEventArgs"/>
        /// </summary>
        public ScrollEventArgs(long id, Point origin, double direction)
        {
            PointerID = id;
            TouchPoint = origin;
            ScrollDelta = direction;
        }
    }
}
