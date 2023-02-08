using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Core.Internals;
using System;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IArea
    {
        /// <summary>
        /// 
        /// </summary>
        Rect AreaBounds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool NeedsRelayout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IPlotArea PlotArea { get; }

        /// <summary>
        /// 
        /// </summary>
        void ScheduleUpdateArea();
    }
}
