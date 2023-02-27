using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class WaterfallSeries : XYDataSeries
    {

        #region  Private Fields

        private double bottomValue;

        #endregion

        #region  Properties

        #region Bindable Properties

        /// <summary>
        /// Gets or sets a value indicating whether this WaterfallSeries allow auto sum.
        /// </summary>
        public static readonly BindableProperty AllowAutoSumProperty =
            BindableProperty.Create(
                nameof(AllowAutoSum),
                typeof(bool),
                typeof(WaterfallSeries),
                true,
                BindingMode.Default,
                null, propertyChanged: OnAllowAutoSumChanged);

        /// <summary>
        /// Gets or sets a value indicating whether this WaterfallSeries show connector line.
        /// </summary>
        public static readonly BindableProperty ShowConnectorProperty =
            BindableProperty.Create(
                nameof(ShowConnectorLine),
                typeof(bool),
                typeof(WaterfallSeries),
                defaultValue: true, propertyChanged: OnShowConnectorChanged);

        /// <summary>
        /// Gets the style value that indicates the segments connector line visual representation.
        /// </summary>
        public static readonly BindableProperty ConnectorLineStyleProperty =
            BindableProperty.Create(
                nameof(ConnectorLineStyle),
                typeof(ChartLineStyle),
                typeof(WaterfallSeries),
                null, propertyChanged: OnConnectorLineStyleChanged);

        /// <summary>
        /// Gets or sets string that indicates model property name which holds the collection of boolean values.
        /// </summary>
        public static readonly BindableProperty SummaryBindingPathProperty =
            BindableProperty.Create(
                nameof(SummaryBindingPath),
                typeof(string),
                typeof(WaterfallSeries),
                defaultValue: string.Empty,
                propertyChanged: OnSummaryBindingPathChanged);

        /// <summary>
        /// Gets or sets the color value that indicates the consolidated segment's interior.
        /// </summary>
        public static readonly BindableProperty SummaryPointsBrushProperty =
            BindableProperty.Create(
                nameof(SummaryPointsBrush),
                typeof(Brush),
                typeof(WaterfallSeries),
                null,
                BindingMode.Default,
                null, propertyChanged: OnSummaryPointsBrushChanged);

        /// <summary>
        /// Gets or sets the color value that indicates the consolidated segment's interior.
        /// </summary>
        public static readonly BindableProperty NegativePointsBrushProperty =
            BindableProperty.Create(
                nameof(NegativePointsBrush),
                typeof(Brush),
                typeof(WaterfallSeries),
                null,
                BindingMode.Default,
                null, propertyChanged: OnNegativePointsBrushChanged);


        /// <summary>
        /// Gets or sets the width of Waterfall series.
        /// </summary>  
        public static readonly BindableProperty WidthProperty =
            BindableProperty.Create(
                nameof(Width),
                typeof(double),
                typeof(WaterfallSeries),
                defaultValue: 0.8d,
                BindingMode.Default,
                null, propertyChanged: OnWidthChanged);

        /// <summary>
        /// Gets or sets the spacing between the segments across the series in cluster mode.
        /// </summary>        
        public static readonly BindableProperty SpacingProperty =
            BindableProperty.Create(
                nameof(Spacing),
                typeof(double),
                typeof(WaterfallSeries),
                0d,
                BindingMode.Default,
                null, propertyChanged: OnSpacingChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether this WaterfallSeries allow auto sum.
        /// </summary>
        public bool AllowAutoSum
        {
            get { return (bool)GetValue(AllowAutoSumProperty); }
            set { SetValue(AllowAutoSumProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this WaterfallSeries show connector line.
        /// </summary>
        public bool ShowConnectorLine
        {
            get { return (bool)GetValue(ShowConnectorProperty); }
            set { SetValue(ShowConnectorProperty, value); }
        }

        /// <summary>
        /// Gets the style value that indicates the segments connector line visual representation.
        /// </summary>
        public ChartLineStyle ConnectorLineStyle
        {
            get { return (ChartLineStyle)GetValue(ConnectorLineStyleProperty); }
            set { SetValue(ConnectorLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets string that indicates model property name which holds the collection of boolean values.
        /// </summary>
        public string SummaryBindingPath
        {
            get { return (string)GetValue(SummaryBindingPathProperty); }
            set { SetValue(SummaryBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color value that indicates the consolidated segment's interior.
        /// </summary>
        public Brush SummaryPointsBrush
        {
            get { return (Brush)GetValue(SummaryPointsBrushProperty); }
            set { SetValue(SummaryPointsBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color value that indicates the consolidated segment's interior.
        /// </summary>
        public Brush NegativePointsBrush
        {
            get { return (Brush)GetValue(NegativePointsBrushProperty); }
            set { SetValue(NegativePointsBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of Waterfall series.
        /// </summary>
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        ///  Gets or sets the spacing between the segments across the series in cluster mode.
        /// </summary>
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }


        #endregion

        #region Internal Property

        /// <summary>
        /// Gets a value indicating whether this WaterfallSeries is side by side.
        /// </summary>
        /// <value><c>true</c> if is side by side; otherwise, <c>false</c>.</value>
        internal override bool IsSideBySide => true;

        #endregion

        #region Private Property

        private List<bool> summaryValues;

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the WaterfallSeries class.
        /// </summary>
        public WaterfallSeries()
        {
            ConnectorLineStyle = new ChartLineStyle()
            {
                Stroke = Color.FromArgb("#ABABAB"),
                StrokeWidth = 1
            };

            summaryValues = new List<bool>();

        }
        #endregion

        #region Methods

        #region Internal Override Methods

        internal override void GeneratePoints(string[] yPaths, params IList<double>[] yValueLists)
        {
            base.GeneratePoints(yPaths, yValueLists);

            GetSummaryValues();
        }

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            double x1, x2, y1, y2;

            if (ActualXAxis != null)
            {
                bottomValue = double.IsNaN(ActualXAxis.ActualCrossingValue) ? 0 : ActualXAxis.ActualCrossingValue;
            }
            
            if (IsIndexed || xValues == null)
            {
                for (var i = 0; i < PointsCount; i++)
                {
                    if (xValues != null)
                    {
                        OnCalculateSegmentValues(out x1, out x2, out y1, out y2, i, bottomValue, xValues[i]);

                        if (i < Segments.Count && Segments[i] is WaterfallSegment)
                        {
                            ((WaterfallSegment)Segments[i]).SetData(new[] { x1, x2, y1, y2, i, YValues[i] });
                        }
                        else
                        {
                            CreateSegment(seriesView, new[] { x1, x2, y1, y2, i, YValues[i] }, i);
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < PointsCount; i++)
                {
                    var x = xValues[i];

                    OnCalculateSegmentValues(out x1, out x2, out y1, out y2, i, bottomValue, xValues[i]);

                    if (i < Segments.Count && Segments[i] is WaterfallSegment)
                    {
                        ((WaterfallSegment)Segments[i]).SetData(new[] { x1, x2, y1, y2, x, YValues[i] });
                    }
                    else
                    {
                        CreateSegment(seriesView, new[] { x1, x2, y1, y2, x, YValues[i] }, i);
                    }
                }
            }
        }

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            TooltipInfo? tooltipInfo = base.GetTooltipInfo(tooltipBehavior, x, y);
            if (tooltipInfo != null)
            {
                if (Segments[tooltipInfo.Index] is WaterfallSegment waterfallSegment)
                {
                    if (waterfallSegment.SegmentType == WaterfallSegmentType.Sum)
                    {
                        tooltipInfo.Text = waterfallSegment.Sum.ToString();
                    }
                }
            }

            return tooltipInfo;
        }

        internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            WaterfallSegment? waterfallSegment = Segments[tooltipInfo.Index] as WaterfallSegment;
           
            if (waterfallSegment != null)
            {
                RectF targetRect = waterfallSegment.SegmentBounds;
                float xPosition = tooltipInfo.X;
                float yPosition;
                float width = targetRect.Width;
                float height = targetRect.Height;

                if (ChartArea != null && ChartArea.IsTransposed)
                {
                    xPosition = waterfallSegment.SegmentBounds.Center.X;
                    yPosition = waterfallSegment.SegmentBounds.Top;
                }
                else
                {
                    yPosition = waterfallSegment.Top;
                }

                targetRect = new Rect(xPosition - width / 2, yPosition, width, height);
                tooltipInfo.TargetRect = targetRect;
            }
        }

        internal override double GetActualWidth()
        {
            return Width;
        }

        internal override double GetActualSpacing()
        {
            return Spacing;
        }

        internal override double GetDataLabelPositionAtIndex(int index)
        {
            double dataLabelPositionAtIndex = 0;
            if (Segments.Count >= index)
            {
                WaterfallSegment? segment = Segments[index] as WaterfallSegment;

                if (segment != null)
                {
                    double median = segment.y1 + ((segment.y2 - segment.y1) / 2);
                    var segmentType = segment.SegmentType;
                    double waterfallSum = segment.WaterfallSum;
                    if (segmentType is WaterfallSegmentType.Sum)
                    {
                        dataLabelPositionAtIndex = AllowAutoSum ? 
                            (DataLabelSettings.BarAlignment == DataLabelAlignment.Middle) ? (waterfallSum / 2) : 
                            (waterfallSum >= 0) ? segment.y1 : segment.y2 : (DataLabelSettings.BarAlignment == DataLabelAlignment.Middle) ? median : (YValues[index] >= 0) ? segment.y1 : segment.y2;
                    }
                    else if (DataLabelSettings.BarAlignment == DataLabelAlignment.Top)
                    {
                        dataLabelPositionAtIndex = (segmentType is WaterfallSegmentType.Positive) ? segment.y1 : segment.y2;
                    }
                    else if (DataLabelSettings.BarAlignment == DataLabelAlignment.Bottom)
                    {
                        dataLabelPositionAtIndex = (segmentType is WaterfallSegmentType.Positive) ? segment.y2 : segment.y1;
                    }
                    else
                        dataLabelPositionAtIndex = median;
                }
            }
            
            return dataLabelPositionAtIndex;
        }

        internal override void CalculateDataPointPosition(int index, ref double x, ref double y)
        {
            if (ChartArea == null) return;
            var x1 = SbsInfo.Start + x;
            var x2 = SbsInfo.End + x;
            var xCal = x1 + ((x2 - x1) / 2);
            var yCal = y;
            
            if (ActualYAxis != null && ActualXAxis != null && !double.IsNaN(yCal))
            {
                y = ChartArea.IsTransposed ? ActualXAxis.ValueToPoint(xCal) : ActualYAxis.ValueToPoint(yCal);
            }

            if (ActualXAxis != null && ActualYAxis != null && !double.IsNaN(x))
            {
                x = ChartArea.IsTransposed ? ActualYAxis.ValueToPoint(yCal) : ActualXAxis.ValueToPoint(xCal);
            }
        }

        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            if (ChartArea == null) return labelPosition;

            if (ChartArea.IsTransposed)
            {
                return DataLabelSettings.GetLabelPositionForTransposedRectangularSeries(this, dataLabel.Index, labelSize, labelPosition, padding, DataLabelSettings.BarAlignment);
            }

            return DataLabelSettings.GetLabelPositionForRectangularSeries(this, dataLabel.Index, labelSize, labelPosition, padding, DataLabelSettings.BarAlignment);
        }

        internal override void DrawDataLabels(ICanvas canvas)
        {
            var dataLabelSettings = ChartDataLabelSettings;

            if (dataLabelSettings == null) return;

            ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;

            foreach (ChartSegment datalabel in Segments)
            {

                if (!datalabel.IsEmpty)
                {
                    UpdateDataLabelAppearance(canvas, datalabel, dataLabelSettings, labelStyle);
                }
            }
        }

        internal override Brush? GetFillColor(int index)
        {
            Brush? fillColor = base.GetFillColor(index);

            if(fillColor == Chart?.GetSelectionBrush(this) || fillColor == GetSelectionBrush(index))
            {
                return fillColor;
            }

            if (Segments[index] is WaterfallSegment segment)
            {
                switch (segment.SegmentType)
                {
                    case WaterfallSegmentType.Negative:
                        return NegativePointsBrush != null ? NegativePointsBrush : fillColor;
                    case WaterfallSegmentType.Sum:
                        return SummaryPointsBrush != null ? SummaryPointsBrush : fillColor;
                }
            }

            return fillColor;
        }

        internal override void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos, ref bool isSidebySide)
        {
            var xValues = GetXValues();
            float xPosition = 0f;
            float yPosition = 0f;
            if (nearestDataPoints != null && ActualData != null && xValues != null && SeriesYValues != null)
            {
                IList<double> yValues = SeriesYValues[0];
                foreach (object point in nearestDataPoints)
                {
                    int index = ActualData.IndexOf(point);
                    var xValue = xValues[index];
                    WaterfallSegment? segment = Segments[index] as WaterfallSegment;
                    double yValue = yValues[index];
                    
                    if (segment != null)
                    {
                        yValue = segment.y1;
                        if (segment.SegmentType == WaterfallSegmentType.Negative)
                            yValue = segment.y2;
                    }
                    
                    string label = yValue.ToString();
                    
                    if (IsSideBySide)
                    {
                        isSidebySide = true;
                        double xMidVal = xValue + SbsInfo.Start + ((SbsInfo.End - SbsInfo.Start) / 2);
                        xPosition = TransformToVisibleX(xMidVal, yValue);
                        yPosition = TransformToVisibleY(xMidVal, yValue);
                    }

                    TrackballPointInfo? chartPointInfo = CreateTrackballPointInfo(xPosition, yPosition, label, point);

                    if (chartPointInfo != null)
                    {
                        chartPointInfo.XValue = xValue;
                        pointInfos.Add(chartPointInfo);
                    }
                }
            }
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Creates the Waterfall segments.
        /// </summary>
        protected override ChartSegment? CreateSegment()
        {
            return new WaterfallSegment();
        }

        #endregion

        #region Private Methods

        private void GetSummaryValues()
        {
            var enumerable = ItemsSource as IEnumerable;
            var enumerator = enumerable?.GetEnumerator();

            if (enumerable != null && enumerator != null && enumerator.MoveNext())
            {
                var currObj = enumerator.Current;

                FastReflection summaryProperty = new FastReflection();

                if (!summaryProperty.SetPropertyName(SummaryBindingPath, currObj) || summaryProperty.IsArray(currObj))
                {
                    return;
                }

                summaryValues ??= new List<bool>();

                if (summaryValues != null)
                {
                    do
                    {
                        var summaryVal = summaryProperty.GetValue(enumerator.Current);
                        summaryValues.Add(Convert.ToBoolean(summaryVal));
                    }
                    while (enumerator.MoveNext());
                }
            }
        }

        private void OnCalculateSegmentValues(out double x1, out double x2, out double y1, out double y2, int i, double bottomValue, double xVal)
        {
            x1 = xVal + SbsInfo.Start;
            x2 = xVal + SbsInfo.End;
            y1 = y2 = double.NaN;
            
            //Calculation for First Segment
            if (i == 0)
            {
                if (YValues[i] >= 0)
                {
                    y1 = YValues[i];
                    y2 = bottomValue;
                }
                else if (double.IsNaN(YValues[i]))
                {
                    y2 = bottomValue;
                    y1 = bottomValue;
                }
                else
                {
                    y2 = YValues[i];
                    y1 = bottomValue;
                }
            }
            else
            {
                if (Segments[i - 1] is WaterfallSegment prevSegment)
                {
                    // Positive value calculation                       
                    if (YValues[i] >= 0)
                    {
                        if (YValues[i - 1] >= 0 || prevSegment.SegmentType == WaterfallSegmentType.Sum)
                        {
                            if (!AllowAutoSum && prevSegment.SegmentType == WaterfallSegmentType.Sum && YValues[i - 1] < 0)
                            {
                                y1 = YValues[i] + prevSegment.y2;
                                y2 = prevSegment.y2;
                            }
                            else
                            {
                                y1 = YValues[i] + prevSegment.y1;
                                y2 = prevSegment.y1;
                            }
                        }
                        else if (double.IsNaN(YValues[i - 1]))
                        {
                            y1 = YValues[i] == 0 ? prevSegment.y2
                                : prevSegment.y2 + YValues[i];
                            y2 = prevSegment.y2;
                        }
                        else
                        {
                            y1 = YValues[i] + prevSegment.y2;
                            y2 = prevSegment.y2;
                        }
                    }
                    else if (double.IsNaN(YValues[i]))
                    {
                        // Empty value calculation
                        if (YValues[i - 1] >= 0 || prevSegment.SegmentType == WaterfallSegmentType.Sum)
                            y1 = y2 = prevSegment.y1;
                        else
                            y1 = y2 = prevSegment.y2;
                    }
                    else
                    {
                        // Negative value calculation
                        if (YValues[i - 1] >= 0 || prevSegment.SegmentType == WaterfallSegmentType.Sum)
                        {
                            if (!AllowAutoSum && prevSegment.SegmentType == WaterfallSegmentType.Sum && YValues[i - 1] < 0)
                            {
                                y1 = prevSegment.y2;
                                y2 = YValues[i] + prevSegment.y2;
                            }
                            else
                            {
                                y1 = prevSegment.y1;
                                y2 = YValues[i] + prevSegment.y1;
                            }
                        }
                        else
                        {
                            y1 = prevSegment.y2;
                            y2 = YValues[i] + prevSegment.y2;
                        }
                    }
                }
            }
        }

        private void CreateSegment(SeriesView seriesView, double[] values, int index)
        {
            var segment = CreateSegment() as WaterfallSegment;

            if (segment != null)
            {
                segment.Series = this;
                segment.SeriesView = seriesView;
                segment.SetData(values);
                segment.Index = index;

                //Updating the Values for Summary Segments
                OnUpdateSumSegmentValues(segment, index);
                
                Segments.Add(segment);
            }
        }

        private void OnUpdateSumSegmentValues(WaterfallSegment segment, int index)
        {
            if ((index - 1) >= 0)
            {
                segment.PreviousWaterfallSegment = Segments[index - 1] as WaterfallSegment;
            }

            if (summaryValues != null && summaryValues.Count > index && summaryValues[index] == true)
            {
                segment.SegmentType = WaterfallSegmentType.Sum;

                if (segment.PreviousWaterfallSegment != null)
                {
                    segment.WaterfallSum = segment.PreviousWaterfallSegment.Sum;
                }
                else
                {
                    segment.WaterfallSum = YValues[index];
                }

                //Assigning the values for Summary Segment
                if (AllowAutoSum && segment.PreviousWaterfallSegment != null)
                {
                    segment.y1 = segment.WaterfallSum;
                    segment.y2 = bottomValue;
                }
                else
                {
                    if (YValues[index] >= 0)
                    {
                        segment.y1 = YValues[index];
                        segment.y2 = bottomValue;
                    }
                    else if (double.IsNaN(YValues[index]))
                    {
                        segment.Bottom = segment.Top = (float)bottomValue;
                    }
                    else
                    {
                        segment.y1 = bottomValue;
                        segment.y2 = YValues[index];
                    }
                }

                YRange += new DoubleRange(segment.y1, segment.y2);
            }
            else
            {
                if (YValues[index] < 0)
                {
                    segment.SegmentType = WaterfallSegmentType.Negative;
                }
                else
                {
                    segment.SegmentType = WaterfallSegmentType.Positive;
                }
            }

            //Sum Value Calculation
            var sum = double.NaN;
            if (AllowAutoSum == false && segment.SegmentType == WaterfallSegmentType.Sum)
                sum = YValues[index];
            else if (segment.PreviousWaterfallSegment != null && segment.SegmentType != WaterfallSegmentType.Sum) //If segment is positive or negative
                sum = YValues[index] + segment.PreviousWaterfallSegment.Sum;
            else if (segment.PreviousWaterfallSegment != null) //If segment is sum type
                sum = segment.PreviousWaterfallSegment.Sum;
            else
                sum = YValues[index];
            
            segment.Sum = sum;
        }

        private static void OnWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.UpdateSbsSeries();
            }
        }

        private static void OnAllowAutoSumChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnShowConnectorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.ScheduleUpdateChart();
            }
        }

        private static void OnSummaryBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.OnBindingPathChanged();
            }
        }

        private static void OnSummaryPointsBrushChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnNegativePointsBrushChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnSpacingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as WaterfallSeries;

            if (series != null && series.ChartArea != null)
            {
                series.InvalidateSeries();
            }
        }


        private static void OnConnectorLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
            }
        }

        private void OnStylePropertyChanged(ChartLineStyle? oldValue, ChartLineStyle? newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= ConnectorLineStyles_PropertyChanged;
            }

            if (newValue != null)
            {
                newValue.PropertyChanged += ConnectorLineStyles_PropertyChanged; ;
                SetInheritedBindingContext(newValue, BindingContext);
            }

            if (AreaBounds != Rect.Zero)
            {
                InvalidateSeries();
            }
        }

        private void ConnectorLineStyles_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateSeries();
        }

        #endregion

        #endregion
    }
}
