using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Graphics.Internals
{
    /// <summary>
    /// Platform view for the signature pad control.
    /// </summary>
    public partial class PlatformSignaturePad
    {
        #region Fields

        private readonly List<List<TimedPoint>> drawPointsCache = new();

        private readonly List<TimedPoint> cachePoints = new();

        private readonly List<TimedPoint> memoryPoints = new();

        private readonly float velocityWeight = 0.9f;

        private float previousTouchVelocity = 0;

        private float previousStrokeWidth;

#pragma warning disable 0649
        private float minimumStrokeWidth;

        private float maximumStrokeWidth;
#pragma warning restore 0649

        private Bezier bezier = new();

        private TimedPoint firstControlTimedPoint = new();

        private TimedPoint secondControlTimedPoint = new();

        private List<TimedPoint> currentInteractionCyclePoints = null!;

#pragma warning disable 0169
        private ISignaturePad? virtualView;
#pragma warning restore 0169

        #endregion

        #region Methods

        private void OnInteractionStart(float x, float y)
        {
            memoryPoints.Clear();
            cachePoints.Clear();

            currentInteractionCyclePoints = new List<TimedPoint>();
            drawPointsCache.Add(currentInteractionCyclePoints);

            TimedPoint point = GetNewPoint(x, y);
            currentInteractionCyclePoints.Add(point);
            AddPoint(point);
        }

        private void OnInteractionMove(float x, float y)
        {
            TimedPoint point = GetNewPoint(x, y);
            currentInteractionCyclePoints.Add(point);
            AddPoint(point);
        }

        private void OnInteractionEnd(float x, float y)
        {
            TimedPoint point = GetNewPoint(x, y);
            currentInteractionCyclePoints.Add(point);
            AddPoint(point);
        }

        private void AddPoint(TimedPoint newPoint)
        {
            memoryPoints!.Add(newPoint);

            int pointsCount = memoryPoints.Count;
            if (pointsCount > 3)
            {
                CalculateCurveControlPoints(memoryPoints[0], memoryPoints[1], memoryPoints[2]);
                TimedPoint point1 = secondControlTimedPoint;
                RecyclePoint(firstControlTimedPoint);

                CalculateCurveControlPoints(memoryPoints[1], memoryPoints[2], memoryPoints[3]);
                TimedPoint point2 = firstControlTimedPoint;
                RecyclePoint(secondControlTimedPoint);

                bezier.Update(memoryPoints[1], point1, point2, memoryPoints[2]);

                TimedPoint startPoint = bezier.StartPoint;
                TimedPoint endPoint = bezier.EndPoint;
                float velocity = endPoint.VelocityFrom(startPoint);
                velocity = float.IsNaN(velocity) ? 0.0f : velocity;
                velocity = velocityWeight * velocity + (1 - velocityWeight) * previousTouchVelocity;
                float newWidth = GetStrokeWidth(velocity);
                AddBezier(bezier, previousStrokeWidth, newWidth);

                previousTouchVelocity = velocity;
                previousStrokeWidth = newWidth;

                RecyclePoint(memoryPoints[0]);
                memoryPoints.RemoveAt(0);

                RecyclePoint(point1);
                RecyclePoint(point2);
            }
            else if (pointsCount == 1)
            {
                TimedPoint firstPoint = memoryPoints[0];
                memoryPoints.Add(GetNewPoint(firstPoint.X, firstPoint.Y));
                DrawPoint(firstPoint.X, firstPoint.Y, (maximumStrokeWidth + minimumStrokeWidth) / 2);
            }
        }

        private float GetStrokeWidth(float velocity)
        {
            return (float)Math.Max(maximumStrokeWidth / (velocity + 1), minimumStrokeWidth);
        }

        private void RecyclePoint(TimedPoint point)
        {
            cachePoints.Add(point.Copy());
        }

        private void CalculateCurveControlPoints(TimedPoint s1, TimedPoint s2, TimedPoint s3)
        {
            float dx1 = s1.X - s2.X;
            float dy1 = s1.Y - s2.Y;
            float dx2 = s2.X - s3.X;
            float dy2 = s2.Y - s3.Y;

            float m1X = (s1.X + s2.X) / 2.0f;
            float m1Y = (s1.Y + s2.Y) / 2.0f;
            float m2X = (s2.X + s3.X) / 2.0f;
            float m2Y = (s2.Y + s3.Y) / 2.0f;

            float l1 = (float)Math.Sqrt(dx1 * dx1 + dy1 * dy1);
            float l2 = (float)Math.Sqrt(dx2 * dx2 + dy2 * dy2);

            float dxm = m1X - m2X;
            float dym = m1Y - m2Y;
            float k = l2 / (l1 + l2);
            if (float.IsNaN(k))
            {
                k = 0.0f;
            }

            float cmX = m2X + dxm * k;
            float cmY = m2Y + dym * k;

            float tx = s2.X - cmX;
            float ty = s2.Y - cmY;

            firstControlTimedPoint = GetNewPoint(m1X + tx, m1Y + ty);
            secondControlTimedPoint = GetNewPoint(m2X + tx, m2Y + ty);
        }

        private TimedPoint GetNewPoint(float x, float y)
        {
            int cacheSize = cachePoints.Count;
            TimedPoint timedPoint;
            if (cacheSize == 0)
            {
                timedPoint = new TimedPoint();
            }
            else
            {
                int lastIndex = cacheSize - 1;
                timedPoint = cachePoints[lastIndex];
                cachePoints.RemoveAt(lastIndex);
            }

            timedPoint.Update(x, y);
            return timedPoint;
        }

        private void ComputePointDetails(Bezier curve, float startWidth, float widthDelta,
            float drawSteps, int i, out float x, out float y, out float width)
        {
            float t = i / drawSteps;
            float tt = t * t;
            float ttt = tt * t;
            float u = 1 - t;
            float uu = u * u;
            float uuu = uu * u;

            x = uuu * curve.StartPoint.X;
            x += 3 * uu * t * curve.FirstControlPoint.X;
            x += 3 * u * tt * curve.SecondControlPoint.X;
            x += ttt * curve.EndPoint.X;

            y = uuu * curve.StartPoint.Y;
            y += 3 * uu * t * curve.FirstControlPoint.Y;
            y += 3 * u * tt * curve.SecondControlPoint.Y;
            y += ttt * curve.EndPoint.Y;

            // Calculating the incremental stroke width.
            width = startWidth + ttt * widthDelta;
        }

        private void Redraw()
        {
            int pointsLength = drawPointsCache.Count;

            if (pointsLength > 0)
            {
                Reset();
                WipeOut();

                for (int i = 0; i < pointsLength; i++)
                {
                    cachePoints.Clear();
                    memoryPoints.Clear();

                    List<TimedPoint> cyclePoints = drawPointsCache[i];
                    int cyclePointsLength = drawPointsCache[i].Count;

                    for (int j = 0; j < cyclePointsLength; j++)
                    {
                        AddPoint(cyclePoints[j]);
                        Invalidate();
                    }
                }
            }
        }

        private void Reset()
        {
            cachePoints.Clear();
            memoryPoints.Clear();

            bezier = new();
            previousTouchVelocity = 0;
            previousStrokeWidth = (minimumStrokeWidth + maximumStrokeWidth) / 2f;
        }

        #endregion
    }
}
