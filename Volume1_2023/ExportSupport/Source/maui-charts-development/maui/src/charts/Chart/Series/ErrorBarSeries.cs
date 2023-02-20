using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="ErrorBarSeries"/> displays a set of Error bar lines  for the given data point values.
    /// </summary>
    public class ErrorBarSeries : XYDataSeries
    {
        #region Properties

        #region Bindable Properties

        /// <summary>
        /// Gets or sets the value for HorizontalErrorPath. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty HorizontalErrorPathProperty =
            BindableProperty.Create(
                nameof(HorizontalErrorPath),
                typeof(string),
                typeof(ErrorBarSeries),
                null,
                BindingMode.Default, propertyChanged: OnHorizontalErrorPathChanged);

        /// <summary>
        /// Gets or sets the value for the VerticalErrorPath. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty VerticalErrorPathProperty =
            BindableProperty.Create(
                nameof(VerticalErrorPath),
                typeof(string),
                typeof(ErrorBarSeries),
                null,
                BindingMode.Default, propertyChanged: OnVerticalErrorPathChanged);

        /// <summary>
        /// Gets or sets the value that defines HorizontalErrorValue. This ia a bindable property.
        /// </summary>
        public static readonly BindableProperty HorizontalErrorValueProperty =
            BindableProperty.Create(
                nameof(HorizontalErrorValue),
                typeof(double),
                typeof(ErrorBarSeries),
                0.0,
                BindingMode.Default, propertyChanged: OnHorizontalErrorValueChanged);

        /// <summary>
        /// Gets or sets the value that defines VerticalErrorValue. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty VerticalErrorValueProperty =
            BindableProperty.Create(
                nameof(VerticalErrorValue),
                typeof(double),
                typeof(ErrorBarSeries),
                0.0, BindingMode.Default, propertyChanged: OnVerticalErrorValueChanged);

        /// <summary>
        /// Gets or sets the error bar mode for <see cref="ErrorBarSeries"/>. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty ModeProperty =
            BindableProperty.Create(
                nameof(Mode),
                typeof(ErrorBarMode),
                typeof(ErrorBarSeries),
                ErrorBarMode.Both, BindingMode.Default, propertyChanged: OnModePropertyChanged);

        /// <summary>
        /// Gets or sets the error bar type for <see cref="ErrorBarSeries"/>. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create(
                nameof(Type),
                typeof(ErrorBarType),
                typeof(ErrorBarSeries),
                ErrorBarType.Fixed, BindingMode.Default, propertyChanged: OnTypePropertyChanged);

        /// <summary>
        /// Gets or sets the error bar horizontal Direction for <see cref="ErrorBarSeries"/>. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty HorizontalDirectionProperty =
            BindableProperty.Create(
                nameof(HorizontalDirection),
                typeof(ErrorBarDirection),
                typeof(ErrorBarSeries),
                ErrorBarDirection.Both, BindingMode.Default, propertyChanged: OnHorizontalDirectionChaged);

        /// <summary>
        /// Gets or sets the error bar vertical Direction for <see cref="ErrorBarSeries"/>. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty VerticalDirectionProperty =
            BindableProperty.Create(
                nameof(VerticalDirection),
                typeof(ErrorBarDirection),
                typeof(ErrorBarSeries),
                ErrorBarDirection.Both, BindingMode.Default, propertyChanged: OnVerticalDirectionChanged);

        /// <summary>
        /// Gets or sets the style for horizontal error line in <see cref="ErrorBarSeries"/>. This is a bindable property.
        ///</summary>
        public static readonly BindableProperty HorizontalLineStyleProperty =
            BindableProperty.Create(
                nameof(HorizontalLineStyle),
                typeof(ErrorBarLineStyle),
                typeof(ErrorBarSeries),
                null, BindingMode.Default,
                propertyChanged: OnHorizontalLineStyleChanged);

        /// <summary>
        /// Gets or sets the style for vertical error line in <see cref="ErrorBarSeries"/>. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty VerticalLineStyleProperty =
            BindableProperty.Create(
                nameof(VerticalLineStyle),
                typeof(ErrorBarLineStyle),
                typeof(ErrorBarSeries),
                null, BindingMode.Default,
                propertyChanged: OnVerticalLineStyleChanged);

        /// <summary>
        /// Gets or sets the style for horizontal cap line in <see cref="ErrorBarSeries"/>. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty HorizontalCapLineStyleProperty =
            BindableProperty.Create(
                nameof(HorizontalCapLineStyle),
                typeof(ErrorBarCapLineStyle),
                typeof(ErrorBarSeries),
                null, BindingMode.Default, propertyChanged: OnHorizontalCapLineStyleChanged);

        /// <summary>
        /// Gets or sets the style for vertical cap line in <see cref="ErrorBarSeries"/>. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty VerticalCapLineStyleProperty =
            BindableProperty.Create(
                nameof(VerticalCapLineStyle),
                typeof(ErrorBarCapLineStyle),
                typeof(ErrorBarSeries),
                null, BindingMode.Default, propertyChanged: OnVerticalCapLineStyleChanged);

        #endregion

        #region Public  Properties

        /// <summary>
        /// Gets or sets the value for HorizontalErrorPath.
        /// </summary>
        public string HorizontalErrorPath
        {
            get { return (string)GetValue(HorizontalErrorPathProperty); }
            set { SetValue(HorizontalErrorPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the VerticalErrorPath.
        /// </summary>
        public string VerticalErrorPath
        {
            get { return (string)GetValue(VerticalErrorPathProperty); }
            set { SetValue(VerticalErrorPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that defines HorizontalErrorValue.
        /// </summary>
        public double HorizontalErrorValue
        {
            get { return (double)GetValue(HorizontalErrorValueProperty); }
            set { SetValue(HorizontalErrorValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that defines VerticalErrorValue.
        /// </summary>
        public double VerticalErrorValue
        {
            get { return (double)GetValue(VerticalErrorValueProperty); }
            set { SetValue(VerticalErrorValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the  error bar mode for <see cref="ErrorBarSeries"/>.
        /// </summary>
        public ErrorBarMode Mode
        {
            get { return (ErrorBarMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the error bar type for <see cref="ErrorBarSeries"/>.
        /// </summary>
        public ErrorBarType Type
        {
            get { return (ErrorBarType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal direction for <see cref="ErrorBarSeries"/>.
        /// </summary>
        public ErrorBarDirection HorizontalDirection
        {
            get { return (ErrorBarDirection)GetValue(HorizontalDirectionProperty); }
            set { SetValue(HorizontalDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical direction for <see cref="ErrorBarSeries"/>.
        /// </summary>
        public ErrorBarDirection VerticalDirection
        {
            get { return (ErrorBarDirection)GetValue(VerticalDirectionProperty); }
            set { SetValue(VerticalDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for horizontal error line in <see cref="ErrorBarSeries"/>.
        /// </summary>
        public ErrorBarLineStyle HorizontalLineStyle
        {
            get { return (ErrorBarLineStyle)GetValue(HorizontalLineStyleProperty); }
            set { SetValue(HorizontalLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for vertical error line in <see cref="ErrorBarSeries"/>.
        /// </summary>
        public ErrorBarLineStyle VerticalLineStyle
        {
            get { return (ErrorBarLineStyle)GetValue(VerticalLineStyleProperty); }
            set { SetValue(VerticalLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for horizontal cap line in <see cref="ErrorBarSeries"/>.
        /// </summary>
        public ErrorBarCapLineStyle HorizontalCapLineStyle
        {
            get { return (ErrorBarCapLineStyle)GetValue(HorizontalCapLineStyleProperty); }
            set { SetValue(HorizontalCapLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for vertical cap line in <see cref="ErrorBarSeries"/>. 
        /// </summary>
        public ErrorBarCapLineStyle VerticalCapLineStyle
        {
            get { return (ErrorBarCapLineStyle)GetValue(VerticalCapLineStyleProperty); }
            set { SetValue(VerticalCapLineStyleProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal IList<double> HorizontalErrorValues { get; set; }
        internal IList<double> VerticalErrorValues { get; set; }

        #endregion

        #endregion

        #region Constructor

        #region Public  Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBarSeries"/> class.
        /// </summary>
        public ErrorBarSeries()
        {
            HorizontalErrorValues = new List<double>();
            VerticalErrorValues = new List<double>();

            HorizontalLineStyle = new ErrorBarLineStyle();
            VerticalLineStyle = new ErrorBarLineStyle();
            HorizontalCapLineStyle = new ErrorBarCapLineStyle();
            VerticalCapLineStyle = new ErrorBarCapLineStyle();
        }

        #endregion

        #endregion

        #region Methods

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            if (xValues == null || xValues.Count == 0)
            {
                return;
            }
            if (Segments.Count == 0)
            {
                var segment = CreateSegment() as ErrorBarSegment;
                if (segment != null)
                {
                    segment.Series = this;
                    segment.SeriesView = seriesView;
                    segment.SetData(xValues, (IList)YValues);
                    Segments.Add(segment);
                }
            }
        }

        internal void GeneratePoints()
        {
            HorizontalErrorValues?.Clear();
            VerticalErrorValues?.Clear();
            YValues.Clear();
            if (YBindingPath is not null && HorizontalErrorPath is not null && VerticalErrorPath is not null)
            {
                GeneratePoints(new[] { YBindingPath, HorizontalErrorPath, VerticalErrorPath }, YValues, HorizontalErrorValues!, VerticalErrorValues!);
            }
        }

        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            GeneratePoints();
            this.ScheduleUpdateChart();
        }

        internal override void OnBindingPathChanged()
        {
            ResetData();
            GeneratePoints();
            SegmentsCreated = false;
            this.ScheduleUpdateChart();
        }

        internal override bool IsMultipleYPathRequired
        {
            get
            {
                bool yPathDecision = Type is ErrorBarType.Custom ? true : false;
                return yPathDecision;
            }
        }

        internal override void RemoveData(int index, NotifyCollectionChangedEventArgs e)
        {
            if (XValues is IList<double>)
            {
                ((IList<double>)XValues).RemoveAt(index);
                PointsCount--;
            }
            else if (XValues is IList<string>)
            {
                ((IList<string>)XValues).RemoveAt(index);
                PointsCount--;
            }
            
            SeriesYValues?[0].RemoveAt(index);
            ActualData?.RemoveAt(index);
        }

        #endregion

        #region Protected  Methods

        /// <summary>
        /// Creates the Error Bar segments.
        /// </summary>
        /// <returns></returns>
        protected override ChartSegment CreateSegment()
        {
            return new ErrorBarSegment();
        }

        #endregion

        #region Private Methods


        private static void OnHorizontalErrorPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnBindingPathChanged();
            }
        }

        private static void OnVerticalErrorPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnBindingPathChanged();
            }
        }

        private static void OnHorizontalErrorValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnVerticalErrorValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnBindingPathChanged();
            }
        }

        private static void OnHorizontalDirectionChaged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnVerticalDirectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnHorizontalLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
            }
        }

        private static void OnVerticalLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
            }
        }

        private static void OnHorizontalCapLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
            }
        }

        private static void OnVerticalCapLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ErrorBarSeries series)
            {
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
            }
        }

        private void OnStylePropertyChanged(ChartLineStyle? oldValue, ChartLineStyle? newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= ErrorBarLineStyles_PropertyChanged;
            }

            if (newValue != null)
            {
                newValue.PropertyChanged += ErrorBarLineStyles_PropertyChanged; ;
                SetInheritedBindingContext(newValue, BindingContext);
            }

            if (AreaBounds != Rect.Zero)
            {
                InvalidateSeries();
            }
        }

        private void ErrorBarLineStyles_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateSeries();
        }

        #endregion

        #endregion
    }
}
