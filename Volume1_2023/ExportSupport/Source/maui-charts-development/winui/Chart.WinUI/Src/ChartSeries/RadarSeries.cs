using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    internal class RadarSeries : PolarRadarSeriesBase
    {
        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the Segments of RadarSeries.
        /// </summary>
        internal override void GenerateSegments()
        {
            Segments.Clear(); Segment = null;

            if (DrawType == ChartSeriesDrawType.Area)
            {
                double Origin = 0;
                List<double> xValues = GetXValues().ToList();
                List<double> tempYValues = new List<double>();
                tempYValues = (from val in YValues select val).ToList();

                if (xValues != null)
                {
                    if (!IsClosed)
                    {
                        xValues.Insert((int)PointsCount - 1, xValues[(int)PointsCount - 1]);
                        xValues.Insert(0, xValues[0]);
                        tempYValues.Insert(0, Origin);
                        tempYValues.Insert(tempYValues.Count, Origin);
                    }
                    else
                    {
                        xValues.Insert(0, xValues[0]);
                        tempYValues.Insert(0, YValues[0]);
                        xValues.Insert(0, xValues[(int)PointsCount]);
                        tempYValues.Insert(0, YValues[(int)PointsCount - 1]);
                    }

                    if (Segment == null)
                    {
                        Segment = CreateSegment() as AreaSegment;
                        if (Segment != null)
                        {
                            Segment.Series = this;
                            Segment.SetData(xValues, tempYValues);
                            Segments.Add(Segment);
                        }
                    }
                    else
                        Segment.SetData(xValues, tempYValues);
                    if (AdornmentsInfo != null && ShowDataLabels)
                        AddAreaAdornments(YValues);
                }
            }
            else if (DrawType == ChartSeriesDrawType.Line)
            {
                int index = -1;
                int i = 0;
                double xIndexValues = 0d;
                List<double> xValues = ActualXValues as List<double>;

                if (IsIndexed || xValues == null)
                {
                    xValues = xValues != null ? (from val in (xValues) select (xIndexValues++)).ToList()
                          : (from val in (ActualXValues as List<string>) select (xIndexValues++)).ToList();
                }

                if (xValues != null)
                {
                    for (i = 0; i < this.PointsCount; i++)
                    {
                        index = i + 1;
                        if (index < this.PointsCount)
                        {
                            if (i < Segments.Count)
                            {
                                (Segments[i]).SetData(xValues[i], YValues[i], xValues[index], YValues[index]);
                            }
                            else
                            {
                                CreateSegment(new[] { xValues[i], YValues[i], xValues[index], YValues[index] });
                            }
                        }

                        if (AdornmentsInfo != null && ShowDataLabels)
                        {
                            if (i < Adornments.Count)
                            {
                                Adornments[i].SetData(xValues[i], YValues[i], xValues[i], YValues[i]);
                            }
                            else
                            {
                                Adornments.Add(this.CreateAdornment(this, xValues[i], YValues[i], xValues[i], YValues[i]));
                            }

                            Adornments[i].Item = ActualData[i];
                        }
                    }

                    if (IsClosed)
                    {
                        CreateSegment(new[] { xValues[0], YValues[0], xValues[i - 1], YValues[i - 1] });
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add the <see cref="LineSegment"/> into the Segments collection.
        /// </summary>
        /// <param name="values">The values.</param>
        private void CreateSegment(double[] values)
        {
            LineSegment segment = CreateSegment() as LineSegment;
            if (segment != null)
            {
                segment.Series = this;
                segment.Item = this;
                segment.SetData(values);
                Segment = segment;
                Segments.Add(segment);
            }
        }


        #endregion

        #region Protected Internal Override Methods

        /// <inheritdoc/>
        internal override IChartTransformer CreateTransformer(Size size, bool create)
        {
            if (create || ChartTransformer == null)
            {
                ChartTransformer = ChartTransform.CreatePolar(new Rect(new Point(), size), this);
            }

            return ChartTransformer;
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        internal override ChartSegment CreateSegment()
        {
            if (DrawType == ChartSeriesDrawType.Area)
            {
                return new AreaSegment();
            }
            else
            {
                return new LineSegment();
            }
        }

        #endregion

        #endregion
    }
}
