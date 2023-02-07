using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PolarRadarSeriesBase : DataMarkerSeries, ISupportAxes
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>DataLabelSettings</c> dependency property.
        /// </value>  
        public static readonly DependencyProperty DataLabelSettingsProperty =
          DependencyProperty.Register(nameof(DataLabelSettings), typeof(PolarDataLabelSettings), typeof(PolarRadarSeriesBase),
          new PropertyMetadata(null, OnAdornmentsInfoChanged));

        /// <summary>
        /// Identifies the YBindingPath dependency property.
        /// </summary>
        /// <value>
        /// The identifier for YBindingPath dependency property.
        /// </value>   
        public static readonly DependencyProperty YBindingPathProperty =
            DependencyProperty.Register(
                nameof(YBindingPath), 
                typeof(string), 
                typeof(PolarRadarSeriesBase),
                new PropertyMetadata(null, OnYPathChanged));

        /// <summary>
        /// Identifies the IsClosed dependency property.
        /// </summary>
        /// <value>
        /// The identifier for IsClosed dependency property.
        /// </value>   
        public static readonly DependencyProperty IsClosedProperty =
            DependencyProperty.Register(
                nameof(IsClosed),
                typeof(bool), 
                typeof(PolarRadarSeriesBase),
                new PropertyMetadata(true, OnDrawValueChanged));

        /// <summary>
        /// Identifies the <c>DrawType</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>DrawType</c> dependency property.
        /// </value>   
        internal static readonly DependencyProperty DrawTypeProperty =
            DependencyProperty.Register(
                nameof(DrawType), 
                typeof(ChartSeriesDrawType),
                typeof(PolarRadarSeriesBase),
                new PropertyMetadata(ChartSeriesDrawType.Area, OnDrawValueChanged));
                
        /// <summary>
        /// Identifies the StrokeDashArray dependency property.
        /// </summary>
        /// <value>
        /// The identifier for StrokeDashArray dependency property.
        /// </value>   
        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register(
                nameof(StrokeDashArray),
                typeof(DoubleCollection),
                typeof(PolarRadarSeriesBase),
                new PropertyMetadata(null, OnDrawValueChanged));

        #endregion

        #region Fields

        #region Private Fields

        Storyboard sb;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarRadarSeriesBase"/> class.
        /// </summary>
        public PolarRadarSeriesBase()
        {
            YValues = new List<double>();
            DataLabelSettings = new PolarDataLabelSettings();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to customize the appearance of the displaying data labels in the polar series.
        /// </summary>
        /// <remarks>This allows us to change the look of a data point by displaying labels, shapes, and connector lines.</remarks>
        /// <value>
        /// It takes the <see cref="PolarDataLabelSettings" />.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <chart:SfPolarChart>
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:PolarAreaSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue" 
        ///                            ShowDataLabels="True">
        ///          <syncfusion:PolarAreaSeries.DataLabelSettings>
        ///              <chart:PolarDataLabelSettings />
        ///          <syncfusion:PolarAreaSeries.DataLabelSettings>
        ///     </chart:PolarAreaSeries> 
        ///
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// //omitted for brevity
        /// PolarAreaSeries series = new PolarAreaSeries();
        /// series.ItemsSource = viewmodel.Data;
        /// series.XBindingPath = "XValue";
        /// series.YBindingPath = "YValue";
        /// series.ShowDataLabels = true;
        /// chart.Series.Add(series);
        ///
        /// series.DataLabelSettings = new PolarDataLabelSettings();
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public PolarDataLabelSettings DataLabelSettings
        {
            get
            {
                return (PolarDataLabelSettings)GetValue(DataLabelSettingsProperty);
            }

            set
            {
                SetValue(DataLabelSettingsProperty, value);
            }
        }

        internal override ChartDataLabelSettings AdornmentsInfo
        {
            get
            {
                return (PolarDataLabelSettings)GetValue(DataLabelSettingsProperty);
            }
        }


        /// <summary>
        /// Gets or sets a path value on the source object to serve a y value to the series.
        /// </summary>
        /// <value> The string that represents the property name for the y plotting data, and its default value is null.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfPolarChart>
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:PolarAreaSeries ItemsSource="{Binding Data}" 
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"/>
        ///
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// //omitted for brevity
        /// PolarAreaSeries series = new PolarAreaSeries();
        /// series.ItemsSource = viewmodel.Data;
        /// series.XBindingPath = "XValue";
        /// series.YBindingPath = "YValue";
        /// chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string YBindingPath
        {
            get { return (string)GetValue(YBindingPathProperty); }
            set { SetValue(YBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the area path for the polar series should be closed or opened.
        /// </summary>
        /// <remarks> If its <c>true</c>, series path will be closed; otherwise opened.</remarks>
        /// <value>It accepts bool values and its default value is <c>True</c>.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfPolarChart>
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:PolarAreaSeries ItemsSource="{Binding Data}" 
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            IsClosed="True"/>
        ///
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// //omitted for brevity
        /// PolarAreaSeries series = new PolarAreaSeries();
        /// series.ItemsSource = viewmodel.Data;
        /// series.XBindingPath = "XValue";
        /// series.YBindingPath = "YValue";
        /// series.IsClosed = true;
        /// chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal ChartSeriesDrawType DrawType
        {
            get { return (ChartSeriesDrawType)GetValue(DrawTypeProperty); }
            set { SetValue(DrawTypeProperty, value); }
        }

        /// <summary>
        /// Gets the start and end range values of series x-axis. 
        /// </summary>
        public DoubleRange VisibleXRange { get; internal set; }

        /// <summary>
        /// Gets the start and end range values of series y-axis.
        /// </summary>
        public DoubleRange VisibleYRange { get; internal set; }

        /// <summary>
        /// Gets or sets the <see cref="DoubleCollection"/> value for stroke dash array to customize the stroke appearance of <see cref="PolarSeries"/>.
        /// </summary>
        /// <value>
        /// It takes <see cref="DoubleCollection"/> value and the default value is null.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfPolarChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:PolarLineSeries ItemsSource="{Binding Data}" 
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            StrokeDashArray="5,3"/>
        ///
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        /// 
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// // omitted for brevity
        /// PolarLineSeries series = new PolarLineSeries();
        /// series.ItemsSource = viewmodel.Data;
        /// series.XBindingPath = "XValue";
        /// series.YBindingPath = "YValue";
        /// chart.Series.Add(series);
        ///
        /// DoubleCollection doubleCollection = new DoubleCollection();
        /// doubleCollection.Add(5);
        /// doubleCollection.Add(3);
        /// series.StrokeDashArray = doubleCollection;
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        ///<inheritdoc/>
        ChartAxis ISupportAxes.ActualXAxis
        {
            get { return ActualXAxis; }
        }

        ///<inheritdoc/>
        ChartAxis ISupportAxes.ActualYAxis
        {
            get { return ActualYAxis; }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets or sets YValues to render the series.
        /// </summary>
        /// <value>It takes the collection of double values.</value>
        internal IList<double> YValues { get; set; }

        /// <summary>
        /// Gets or sets the chart segment.
        /// </summary>
        /// <value>It takes the chart segment value.</value>
        internal ChartSegment Segment { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Finds the nearest point in ChartSeries relative to the mouse point/touch position.
        /// </summary>
        /// <param name="point">The co-ordinate point representing the current mouse point /touch position.</param>
        /// <param name="x">x-value of the nearest point.</param>
        /// <param name="y">y-value of the nearest point.</param>
        /// <param name="stackedYValue">The stacked y value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal override void FindNearestChartPoint(Point point, out double x, out double y, out double stackedYValue)
        {
            x = double.NaN;
            y = double.NaN;
            stackedYValue = double.NaN;

            // Converting the mouse point to x chart value.
            double center = 0.5 * Math.Min(this.Chart.SeriesClipRect.Width, this.Chart.SeriesClipRect.Height);
            double radian = ChartTransform.PointToPolarRadian(point, center);
            double coeff = ChartTransform.RadianToPolarCoefficient(radian);
            double chartX = this.Chart.InternalPrimaryAxis.PolarCoefficientToValue(coeff);
            double xStart = this.ActualXAxis.VisibleRange.Start;
            double xEnd = this.ActualXAxis.VisibleRange.End;

            if (chartX <= xEnd && chartX >= xStart)
            {
                if (this.IsIndexed || !(this.ActualXValues is IList<double>))
                {
                    double nearestRange = Math.Round(chartX);
                    var xValues = this.GetXValues();

                    if (xValues != null && nearestRange < xValues.Count && nearestRange >= 0 && YValues != null)
                    {
                        x = NearestSegmentIndex = xValues.IndexOf((int)nearestRange);
                        y = YValues[NearestSegmentIndex];
                    }
                }
                else
                {
                    ChartPoint nearPoint = new ChartPoint();
                    IList<double> xValues = this.ActualXValues as IList<double>;
                    nearPoint.X = xStart;
                    nearPoint.Y = ActualYAxis.VisibleRange.Start;
                    int index = 0;

                    if (xValues != null && YValues != null)
                    {
                        foreach (var x1 in xValues)
                        {
                            double y1 = YValues[index];
                            if (Math.Abs(chartX - x1) <= Math.Abs(chartX - nearPoint.X))
                            {
                                nearPoint = new ChartPoint(x1, y1);
                                x = x1;
                                y = y1;
                                NearestSegmentIndex = index;
                            }

                            index++;
                        }
                    }
                }
            }
        }

        #endregion

        #region Internal Override Methods

        internal override void SetDataLabelsVisibility(bool isShowDataLabels)
        {
            if (DataLabelSettings != null)
            {
                DataLabelSettings.Visible = isShowDataLabels;
            }
        }

        internal override void ResetAdornmentAnimationState()
        {
            if (adornmentInfo != null)
            {
                foreach (FrameworkElement child in this.AdornmentPresenter.Children)
                {
                    child.ClearValue(FrameworkElement.RenderTransformProperty);
                    child.ClearValue(FrameworkElement.OpacityProperty);
                }
            }
        }

        internal override bool GetAnimationIsActive()
        {
            return sb != null && sb.GetCurrentState() == ClockState.Active;
        }

        internal override void Animate()
        {
            // WPF-25124 Animation not working properly when resize the window.
            if (sb != null)
            {
                sb.Stop();
                if (!canAnimate)
                {
                    ResetAdornmentAnimationState();
                    return;
                }
            }

            sb = new Storyboard();

            string propertyXPath = "(UIElement.RenderTransform).(ScaleTransform.ScaleX)";
            string propertyYPath = "(UIElement.RenderTransform).(ScaleTransform.ScaleY)";

            var panel = (ActualArea as ChartBase).GridLinesPanel;
            Point center = new Point(panel.ActualWidth / 2, panel.ActualHeight / 2);

            if (this.DrawType == ChartSeriesDrawType.Area)
            {
                var segmentCanvas = this.Segment.GetRenderedVisual();
                var path = (segmentCanvas as Canvas).Children[0];
                path.RenderTransform = new ScaleTransform() { CenterX = center.X, CenterY = center.Y };
                AnimateElement(path, propertyXPath, propertyYPath);
            }
            else
            {
                foreach (var segment in this.Segments)
                {
                    var lineSegment = segment.GetRenderedVisual();
                    lineSegment.RenderTransform = new ScaleTransform() { CenterX = center.X, CenterY = center.Y };
                    AnimateElement(lineSegment, propertyXPath, propertyYPath);
                }
            }

            AnimateAdornments();
            sb.Begin();
        }

        internal override void UpdateRange()
        {
            VisibleXRange = DoubleRange.Empty;
            VisibleYRange = DoubleRange.Empty;

            foreach (ChartSegment segment in Segments)
            {
                VisibleXRange += segment.XRange;
                VisibleYRange += segment.YRange;
            }
        }

        internal override void RemoveTooltip()
        {
            if (this.Chart == null)
            { 
                return; 
            }

            var canvas = this.Chart.GetAdorningCanvas();
            
            if (canvas != null && canvas.Children.Contains((this.Chart.Tooltip as ChartTooltip)))
                canvas.Children.Remove(this.Chart.Tooltip as ChartTooltip);
        }

        internal override int GetDataPointIndex(Point point)
        {
            Canvas canvas = Chart.GetAdorningCanvas();
            double left = Chart.ActualWidth - canvas.ActualWidth;
            double top = Chart.ActualHeight - canvas.ActualHeight;

            point.X = point.X - left - Chart.SeriesClipRect.Left + Chart.Margin.Left;
            point.Y = point.Y - top - Chart.SeriesClipRect.Top + Chart.Margin.Top;
            double xVal = 0;
            double xStart = ActualXAxis.VisibleRange.Start;
            double xEnd = ActualXAxis.VisibleRange.End;
            int index = -1;
            double center = 0.5 * Math.Min(this.Chart.SeriesClipRect.Width, this.Chart.SeriesClipRect.Height);
            double radian = ChartTransform.PointToPolarRadian(point, center);
            double coeff = ChartTransform.RadianToPolarCoefficient(radian);
            xVal = Math.Round(this.Chart.InternalPrimaryAxis.PolarCoefficientToValue(coeff));
            if (xVal <= xEnd && xVal >= xStart)
                index = this.GetXValues().IndexOf((int)xVal);
            return index;
        }

        internal override void UpdateTooltip(object originalSource)
        {
            if (EnableTooltip)
            {
                var shape = (originalSource as Shape);
                if (shape == null || (shape != null && shape.Tag == null))
                    return;
                SetTooltipDuration();
                var canvas = this.Chart.GetAdorningCanvas();

                object data = null;
                double x = double.NaN;
                double y = double.NaN; 
                double stackYValue = double.NaN;
                if (this.Chart.SeriesClipRect.Contains(mousePosition))
                {
                    var point = new Point(
                        mousePosition.X - this.Chart.SeriesClipRect.Left,
                        mousePosition.Y - this.Chart.SeriesClipRect.Top);
                   
                    FindNearestChartPoint(point, out x, out y, out stackYValue);

                    if (NearestSegmentIndex > -1 && NearestSegmentIndex < ActualData.Count)
                        data = this.ActualData[NearestSegmentIndex];
                }

                var chartTooltip = this.Chart.Tooltip as ChartTooltip;
                if (this.DrawType == ChartSeriesDrawType.Area)
                {
                    var areaSegment = shape.Tag as AreaSegment;
                    areaSegment.Item = data;
                    areaSegment.XData = x;
                    areaSegment.YData = y;
                }
                else
                {
                    var lineSegment = shape.Tag as LineSegment;
                    lineSegment.Item = data;
                    lineSegment.YData = y;
                }

                if (chartTooltip != null)
                {
                    var tag = shape.Tag;
                    ToolTipTag = tag;
                    chartTooltip.PolygonPath = " ";
                    chartTooltip.DataContext = tag;

                    if (canvas.Children.Count == 0 || (canvas.Children.Count > 0 && !IsTooltipAvailable(canvas)))
                    {
                        if (ChartTooltip.GetActualInitialShowDelay(ActualArea.TooltipBehavior, ChartTooltip.GetInitialShowDelay(this)) == 0)
                        {
                            canvas.Children.Add(chartTooltip);
                        }

                        chartTooltip.ContentTemplate = this.GetTooltipTemplate();
                        AddTooltip();

                        if (ChartTooltip.GetActualEnableAnimation(ActualArea.TooltipBehavior, ChartTooltip.GetEnableAnimation(this)))
                        {
                            SetDoubleAnimation(chartTooltip);
                            Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                            Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                        }
                        else
                        {
                            Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                            Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                        }
                    }
                    else
                    {
                        foreach (var child in canvas.Children)
                        {
                            var tooltip = child as ChartTooltip;
                            if (tooltip != null)
                                chartTooltip = tooltip;
                        }

                        AddTooltip();
                        Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                        Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                    }
                }
            }
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            return mousePosition;
        }

        internal override void Dispose()
        {
            if (sb != null)
            {
                sb.Stop();
                sb.Children.Clear();
                sb = null;
            }
            base.Dispose();
        }

        #endregion

        #region Protected Internal Override Methods

        /// <summary>
        /// Method used to generate the data points for Polar and Radar series.
        /// </summary>
        internal override void GenerateDataPoints()
        {
            GeneratePoints(new string[] { YBindingPath }, YValues);
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            YValues.Clear();
            Segment = null;
            GeneratePoints(new string[] { YBindingPath }, YValues);
            this.ScheduleUpdateChart();
        }

        /// <summary>
        /// Invoked when XBindingPath or YBindingPath properties changed.
        /// </summary>
        /// <see cref="ChartSeries.XBindingPath"/>
        /// <see cref="PolarRadarSeriesBase.YBindingPath"/>
        internal override void OnBindingPathChanged()
        {
            YValues.Clear();
            Segment = null;
            ResetData();
            GeneratePoints(new[] { YBindingPath }, YValues);
            if (this.Chart != null && this.Chart.PlotArea != null)
                this.Chart.PlotArea.ShouldPopulateLegendItems = true;
            base.OnBindingPathChanged();
        }

#if NETFX_CORE
        /// <inheritdoc/>
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Touch)
                UpdateTooltip(e.OriginalSource);
        }
#endif

        #endregion

        #region Private Static Methods

        private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PolarRadarSeriesBase).OnBindingPathChanged();
        }

        private static void OnDrawValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PolarRadarSeriesBase).ScheduleUpdateChart();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Timer Tick Handler for closing the Tooltip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        void timer_Tick(object sender, object e)
        {
            RemoveTooltip();
            Timer.Stop();
        }

        private void AnimateAdornments()
        {
            if (this.AdornmentsInfo != null && ShowDataLabels)
            {
                foreach (var child in this.AdornmentPresenter.Children)
                {
                    DoubleAnimationUsingKeyFrames keyFrames1 = new DoubleAnimationUsingKeyFrames();

                    SplineDoubleKeyFrame frame1 = new SplineDoubleKeyFrame();
                    frame1.KeyTime = frame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0));
                    frame1.Value = 0;
                    keyFrames1.KeyFrames.Add(frame1);

                    frame1 = new SplineDoubleKeyFrame();
                    frame1.KeyTime = frame1.KeyTime.GetKeyTime(AnimationDuration);
                    frame1.Value = 0;
                    keyFrames1.KeyFrames.Add(frame1);

                    frame1 = new SplineDoubleKeyFrame();
                    frame1.KeyTime = frame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(AnimationDuration.TotalSeconds + 1));
                    frame1.Value = 1;
                    keyFrames1.KeyFrames.Add(frame1);

                    KeySpline keySpline = new KeySpline();
                    keySpline.ControlPoint1 = new Point(0.64, 0.84);

                    keySpline.ControlPoint2 = new Point(0, 1); // Animation have to provide same easing effect in all platforms.
                    keyFrames1.EnableDependentAnimation = true;
                    Storyboard.SetTargetProperty(keyFrames1, "(Opacity)");
                    frame1.KeySpline = keySpline;

                    Storyboard.SetTarget(keyFrames1, child as FrameworkElement);
                    sb.Children.Add(keyFrames1);
                }
            }
        }

        private void AnimateElement(UIElement element, string propertyXPath, string propertyYPath)
        {
            DoubleAnimation animation_X = new DoubleAnimation();
            animation_X.From = 0;
            animation_X.To = 1;
            animation_X.Duration = new Duration().GetDuration(AnimationDuration);
            Storyboard.SetTarget(animation_X, element);
            Storyboard.SetTargetProperty(animation_X, propertyXPath);
            animation_X.EnableDependentAnimation = true;
            sb.Children.Add(animation_X);

            DoubleAnimation animation_Y = new DoubleAnimation();
            animation_Y.From = 0;
            animation_Y.To = 1;
            animation_Y.Duration = new Duration().GetDuration(AnimationDuration);
            Storyboard.SetTarget(animation_Y, element);
            Storyboard.SetTargetProperty(animation_Y, propertyYPath);
            animation_Y.EnableDependentAnimation = true;
            sb.Children.Add(animation_Y);
        }

#endregion

#endregion
    }
}
