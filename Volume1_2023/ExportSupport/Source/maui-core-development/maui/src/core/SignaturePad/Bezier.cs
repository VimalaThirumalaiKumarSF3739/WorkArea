using System;

namespace Syncfusion.Maui.Graphics.Internals
{
    internal class Bezier
    {
        #region Properties

        internal TimedPoint StartPoint { get; set; } = null!;

        internal TimedPoint FirstControlPoint { get; set; } = null!;

        internal TimedPoint SecondControlPoint { get; set; } = null!;

        internal TimedPoint EndPoint { get; set; } = null!;

        #endregion

        #region Methods

        internal void Update(TimedPoint startPoint, TimedPoint firstControlPoint,
            TimedPoint secondControlPoint, TimedPoint endPoint)
        {
            StartPoint = startPoint;
            FirstControlPoint = firstControlPoint;
            SecondControlPoint = secondControlPoint;
            EndPoint = endPoint;
        }

        internal double Length()
        {
            int steps = 10;
            double length = 0;
            double cx, cy, px = 0, py = 0, xDiff, yDiff;

            for (int i = 0; i <= steps; i++)
            {
                float t = (float)i / steps;
                cx = Point(t, StartPoint.X, FirstControlPoint.X, SecondControlPoint.X, EndPoint.X);
                cy = Point(t, StartPoint.Y, FirstControlPoint.Y, SecondControlPoint.Y, EndPoint.Y);
                if (i > 0)
                {
                    xDiff = cx - px;
                    yDiff = cy - py;
                    length += Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
                }

                px = cx;
                py = cy;
            }

            return length;

        }

        internal double Point(float t, float start, float c1, float c2, float end)
        {
            return start * (1.0 - t) * (1.0 - t) * (1.0 - t)
                + 3.0 * c1 * (1.0 - t) * (1.0 - t) * t
                + 3.0 * c2 * (1.0 - t) * t * t
                + end * t * t * t;
        }

        #endregion
    }
}
