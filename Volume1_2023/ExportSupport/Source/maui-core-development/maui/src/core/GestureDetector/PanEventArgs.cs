using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// This class serves as an event data for the panning action on the view.
    /// </summary>
    public class PanEventArgs
    {
        private readonly Point _touchPoint;
        private readonly Point _translatePoint;
        private readonly GestureStatus _status;
        private readonly Point _velocity;

        /// <summary>
        /// Initializes when <see cref="PanEventArgs"/>
        /// </summary>
        public PanEventArgs(GestureStatus status, Point touchPoint, Point translatePoint, Point velocity)
        {
            _status = status;
            _touchPoint = touchPoint;
            _translatePoint = translatePoint;
            _velocity = velocity;
        }

        /// <summary>
        /// Returns <see cref="GestureStatus"/> on pan interaction.
        /// </summary>
        public GestureStatus Status { get { return _status; } }

        /// <summary>
        /// Returns translate distance point on panning.
        /// </summary>
        public Point TranslatePoint { get { return _translatePoint; } }

        /// <summary>
        /// Returns actual touch point on panning.
        /// </summary>
        public Point TouchPoint { get { return _touchPoint; } }

        /// <summary>
        /// Returns the pan velocity values in X and Y direction.
        /// </summary>
        /// <value>
        /// Velocity values start from 0, 1000, 2000... ranges based on X and Y directions.
        /// While panning with less friction the velocity values are in the range of 0 to 1000 based on X and Y directions.
        /// </value>
        public Point Velocity { get { return _velocity; } }
    }
}
