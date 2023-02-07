using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;


namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DataMarkerSeries : ChartSeries
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <see cref="ShowDataLabels"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ShowDataLabels</c> dependency property and its default value is false.
        /// </value>   
        public static readonly DependencyProperty ShowDataLabelsProperty =
            DependencyProperty.Register(nameof(ShowDataLabels), typeof(bool), typeof(DataMarkerSeries),
                new PropertyMetadata(false, new PropertyChangedCallback(OnShowDataLabelsChanged)));

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates to enable the data labels for the series.
        /// </summary>
        /// <value>It accepts bool values and the default value is <c>False</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:LineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            ShowDataLabels="True"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///     
        ///     LineSeries series = new LineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowDataLabels = true,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ShowDataLabels
        {
            get
            {
                return (bool)GetValue(ShowDataLabelsProperty);
            }

            set
            {
                SetValue(ShowDataLabelsProperty, value);
            }
        }

       
        internal virtual ChartDataLabelSettings AdornmentsInfo
        {
            get
            {
                return null;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// An abstract method which will be called over to create segments.
        /// </summary>
        internal override void GenerateSegments()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Internal Override Methods

        internal override void UpdateOnSeriesBoundChanged(Size size)
        {
            if (AdornmentPresenter != null && AdornmentsInfo != null)
            {
                AdornmentsInfo.UpdateElements();
            }

            base.UpdateOnSeriesBoundChanged(size);

            if (AdornmentPresenter != null && AdornmentsInfo != null)
            {
                AdornmentPresenter.Update(size);
                AdornmentPresenter.Arrange(size);
            }
        }

        internal override void CalculateSegments()
        {
            base.CalculateSegments();

            // VisibleAdornments need to be cleared when segments are newly build while Zooming 
            if (VisibleAdornments.Count > 0)
            {
                VisibleAdornments.Clear();
            }
            if (PointsCount == 0)
            {
                if (AdornmentsInfo != null)
                {
                    BarLabelAlignment markerPosition = this.adornmentInfo.GetAdornmentPosition();
                    if (markerPosition == BarLabelAlignment.Middle)
                        ClearUnUsedAdornments(this.PointsCount * 4);
                    else
                        ClearUnUsedAdornments(this.PointsCount * 2);
                }
            }
        }

        #endregion

        #region Protected Internal Override Methods

        /// <summary>
        /// Method implementation for GeneratePoints for series.
        /// </summary>
        internal override void GenerateDataPoints()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Method implementation for create DataMarkers.
        /// </summary>
        /// <param name="series">series</param>
        /// <param name="xVal">xvalue</param>
        /// <param name="yVal">yvalue</param>
        /// <param name="xPos">xposition</param>
        /// <param name="yPos">yposition</param>
        /// <returns>ChartAdornment</returns>
        internal virtual ChartDataLabel CreateDataMarker(DataMarkerSeries series, double xVal, double yVal, double xPos, double yPos)
        {
            ChartDataLabel adornment = new ChartDataLabel(xVal, yVal, xPos, yPos, series);
            adornment.XData = xVal;
            adornment.YData = yVal;
            adornment.XPos = xPos;
            adornment.YPos = yPos;
            adornment.Series = series;
            return adornment;
        }

        internal virtual ChartDataLabel CreateAdornment(DataMarkerSeries series, double xVal, double yVal, double xPos, double yPos)
        {
            return CreateDataMarker(series, xVal, yVal, xPos, yPos);
        }

        /// <summary>
        /// Method implementation for add ColumnAdornments in Chart.
        /// </summary>
        /// <param name="values">values</param>
        internal virtual void AddColumnAdornments(params double[] values)
        {
            ////values[0] -->   xData
            ////values[1] -->   yData
            ////values[2] -->   xPos
            ////values[3] -->   yPos
            ////values[4] -->   data point index
            ////values[5] -->   Median value.

            double adornposX = values[2] + values[5], adornposY = values[3];
            int pointIndex = (int)values[4];
            if (pointIndex < Adornments.Count)
            {
                Adornments[pointIndex].SetData(values[0], values[1], adornposX, adornposY);
            }
            else
            {
                Adornments.Add(CreateAdornment(this, values[0], values[1], adornposX, adornposY));
            }

            {
                if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed
                    && this.GroupedActualData.Count > 0)
                    Adornments[pointIndex].Item = this.GroupedActualData[pointIndex];
                else
                    Adornments[pointIndex].Item = ActualData[pointIndex];
            }
        }

        /// <summary>
        /// Method implementation for add Adornments at XY.
        /// </summary>
        /// <param name="x">xvalue</param>
        /// <param name="y">yvalue</param>
        /// <param name="pointindex">index</param>
        internal virtual void AddAdornmentAtXY(double x, double y, int pointindex)
        {
            double adornposX = x, adornposY = y;

            if (pointindex < Adornments.Count)
            {
                Adornments[pointindex].SetData(x, y, adornposX, adornposY);
            }
            else
            {
                Adornments.Add(CreateAdornment(this, x, y, adornposX, adornposY));
            }

            if (pointindex < ActualData.Count)
                Adornments[pointindex].Item = ActualData[pointindex];
        }

        /// <summary>
        /// Method implementation for add AreaAdornments in Chart.
        /// </summary>
        /// <param name="values">values</param>
        internal virtual void AddAreaAdornments(params IList<double>[] values)
        {
            IList<double> yValues = values[0];
            List<double> xValues = new List<double>();
            if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();

            if (values.Length == 1)
            {
                int i;
                for (i = 0; i < PointsCount; i++)
                {
                    if (i < xValues.Count && i < yValues.Count)
                    {
                        double adornX = xValues[i];
                        double adornY = yValues[i];

                        if (i < Adornments.Count)
                        {
                            Adornments[i].SetData(xValues[i], yValues[i], adornX, adornY);
                        }
                        else
                        {
                            Adornments.Add(CreateAdornment(this, xValues[i], yValues[i], adornX, adornY));
                        }

                        Adornments[i].Item = ActualData[i];
                    }
                }
            }
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AdornmentPresenter = new ChartDataMarkerPresenter();
            AdornmentPresenter.Series = this;

            if (Chart != null && AdornmentsInfo != null && ShowDataLabels)
            {
                Chart.DataLabelPresenter.Children.Add(AdornmentPresenter);
                AdornmentsInfo.PanelChanged(AdornmentPresenter);
            }
        }

        /// <inheritdoc/>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            if (AdornmentsInfo != null)
            {
                VisibleAdornments.Clear();
                Adornments.Clear();
                AdornmentsInfo.UpdateElements();
            }

            base.OnDataSourceChanged(oldValue, newValue);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Method implementation for clear unused adornments.
        /// </summary>
        /// <param name="startIndex"></param>
        internal void ClearUnUsedAdornments(int startIndex)
        {
            if (Adornments.Count > startIndex)
            {
                int count = Adornments.Count;

                for (int i = startIndex; i < count; i++)
                {
                    Adornments.RemoveAt(startIndex);
                }
            }
        }

        #endregion

        #region Private Static Methods

        internal static void OnAdornmentsInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as DataMarkerSeries;

            if (e.OldValue != null)
            {
                var adornmentInfo = e.OldValue as ChartDataLabelSettings;
                if (series != null)
                {
                    series.Adornments.Clear();
                    series.VisibleAdornments.Clear();
                }

                if (adornmentInfo != null)
                {
                    adornmentInfo.ClearChildren();
                    adornmentInfo.Series = null;
                }
            }

            if (e.NewValue != null)
            {
                if (series != null)
                {
                    series.adornmentInfo = e.NewValue as ChartDataLabelSettings;
                    series.AdornmentsInfo.Series = series;
                    if (series.Chart != null && series.AdornmentsInfo != null)
                    {
                        ////Panel panel = series.Area.GetMarkerPresenter();
                        Panel panel = series.AdornmentPresenter;
                        if (panel != null)
                        {
                            series.AdornmentsInfo.PanelChanged(panel);
                            series.Chart.ScheduleUpdate();
                        }
                    }
                }
            }
        }

        private static void OnShowDataLabelsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataMarkerSeries series = d as DataMarkerSeries;
            series.SetDataLabelsVisibility(series.ShowDataLabels);
            if (series.Chart == null)
                return;

            Panel panel = series.AdornmentPresenter;
            Canvas chartLabelPresenter = series.Chart.DataLabelPresenter;
            if (panel == null || chartLabelPresenter == null)
                return;

            if ((bool)e.NewValue)
            {
                if (!chartLabelPresenter.Children.Contains(panel))
                    chartLabelPresenter.Children.Add(panel);

                if (series.AdornmentPresenter != null && series.AdornmentsInfo != null && series.Adornments.Count > 0)
                {
                    series.AdornmentPresenter.Update(series.GetAvailableSize());
                    series.AdornmentPresenter.Arrange(series.GetAvailableSize());
                }
                else if (series.Adornments != null && series.Adornments.Count == 0)
                {
                    series.Invalidate();

                    if (panel != null)
                    {
                        series.AdornmentsInfo?.PanelChanged(panel);
                    }
                    series.AdornmentsInfo?.OnAdornmentPropertyChanged();
                }
            }
            else
            {
                if (chartLabelPresenter.Children.Contains(panel))
                    chartLabelPresenter.Children.Remove(panel);
            }
        }

        #endregion

        #endregion
    }

}