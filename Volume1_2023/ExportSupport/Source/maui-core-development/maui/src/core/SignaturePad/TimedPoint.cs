using System;

namespace Syncfusion.Maui.Graphics.Internals
{
    internal class TimedPoint
    {
        #region Properties

        internal float X { get; set; }

        internal float Y { get; set; }

        internal long TimeStamp { get; set; }

        #endregion

        #region Methods

        internal void Update(float x, float y)
        {
            X = x;
            Y = y;
            TimeStamp = DateTime.Now.Millisecond;
        }

        internal float VelocityFrom(TimedPoint start)
        {
            long timeDifference = TimeStamp - start.TimeStamp;
            if (timeDifference <= 0)
            {
                timeDifference = 1;
            }

            float velocity = DistanceTo(start) / timeDifference;
            if (float.IsInfinity(velocity) || float.IsNaN(velocity))
            {
                velocity = 0;
            }

            return velocity;
        }

        internal float DistanceTo(TimedPoint point)
        {
            return (float)Math.Sqrt(Math.Pow(point.X - X, 2) + Math.Pow(point.Y - Y, 2));
        }

        internal TimedPoint Copy()
        {
            return new TimedPoint() { X = X, Y = Y, TimeStamp = TimeStamp };
        }

        #endregion
    }
}
