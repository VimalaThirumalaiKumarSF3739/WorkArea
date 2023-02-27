using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Defines the animation type for animation of busy indicator.
    /// </summary>
    public enum AnimationType
    {
        /// <summary>
        /// Defines the animation type as circular material for busy indicator.
        /// </summary>
        CircularMaterial,

        /// <summary>
        /// Defines the animation type as linear material for busy indicator.
        /// </summary>
        LinearMaterial,

        /// <summary>
        /// Defines the animation type as Cupertino for busy indicator.
        /// </summary>
        Cupertino,

        /// <summary>
        /// Defines the animation type as single circle for busy indicator.
        /// </summary>
        SingleCircle,

        /// <summary>
        /// Defines the animation type as double circle for busy indicator.
        /// </summary>
        DoubleCircle,

    }

    /// <summary>
    /// Defines the position of title.
    /// </summary>
    public enum BusyIndicatorTitlePlacement
    {
        /// <summary>
        /// Places the title at the top of the indicator
        /// </summary>
        Top,
        /// <summary>
        /// Places the title to the bottom of the indicator
        /// </summary>
        Bottom,
        /// <summary>
        /// Hides the title of the indicator.
        /// </summary>
        None,
        
    }
}
