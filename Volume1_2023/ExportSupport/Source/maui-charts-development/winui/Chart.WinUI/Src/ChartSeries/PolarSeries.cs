using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Microsoft.UI.Xaml;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// It is the base class for all types of polar series.
    /// </summary>
    public abstract class PolarSeries : PolarRadarSeriesBase
    {
        #region Methods

        #region Public Override Methods
        
        /// <summary>
        /// Creates the segments of PolarSeries.
        /// </summary>
        internal override void GenerateSegments()
        {
            List<double> tempYValues = new List<double>();
            Segments.Clear(); Segment = null;

            if (DrawType == ChartSeriesDrawType.Area)
            {
                double Origin = 0;
                List<double> xValues = GetXValues().ToList();
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

                        if (index < PointsCount)
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
                        }
                    }

                    if (IsClosed)
                    {
                        CreateSegment(new[] { xValues[0], YValues[0], xValues[i - 1], YValues[i - 1] });
                    }
                }
            }
        }

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

    /// <summary>
    /// The <see cref="PolarLineSeries"/> is a series that displays data in terms of values and angles by using a collection of straight lines. It allows for the visual comparison of several quantitative or qualitative aspects of a situation.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of polar line series class, and add it to the <see cref="SfPolarChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="ChartSeries.StrokeThickness"/>, <see cref="PolarRadarSeriesBase.StrokeDashArray"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para><b>Data Label</b></para>
    /// <para> To customize the appearance of data labels, refer to the <see cref="DataMarkerSeries.ShowDataLabels"/>, and <see cref="PolarRadarSeriesBase.DataLabelSettings"/> properties.</para>
    /// 
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///         <!--omitted for brevity-->
    ///
    ///         <chart:PolarLineSeries ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"
    ///                                ShowDataLabels="True">
    ///             <chart:PolarLineSeries.DataLabelSettings>
    ///                 <chart:PolarDataLabelSettings />
    ///             </chart:PolarLineSeries.DataLabelSettings>
    ///         </chart:PolarLineSeries>
    ///
    ///     </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     PolarLineSeries series = new PolarLineSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.ShowDataLabels = true;
    ///     series.DataLabelSettings = new PolarDataLabelSettings();
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// *** 
    ///
    /// <para><b>Animation</b></para>
    /// <para>To animate the series, refer to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    ///
    /// # [Xaml](#tab/tabid-3)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///         <!--omitted for brevity-->
    ///
    ///         <chart:PolarLineSeries ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"
    ///                                EnableAnimation="True"/>
    ///
    ///     </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-4)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     PolarLineSeries series = new PolarLineSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.EnableAnimation = true;
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <para><b>LegendIcon</b></para>
    /// <para>To customize the legend icon, refer to the <see cref="ChartSeries.LegendIcon"/>, <see cref="ChartSeries.LegendIconTemplate"/>, and <see cref="ChartSeries.VisibilityOnLegend"/> properties.</para>
    ///
    /// # [Xaml](#tab/tabid-5)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///         <!--omitted for brevity-->
    ///
    ///         <chart:SfPolarChart.Legend>
    ///             <chart:ChartLegend/>
    ///         </chart:SfPolarChart.Legend>
    ///
    ///         <chart:PolarLineSeries ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"
    ///                                LegendIcon="Diamond"/>
    ///
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-6)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     chart.Legend = new ChartLegend();
    /// 
    ///     PolarLineSeries series = new PolarLineSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.LegendIcon = ChartLegendIcon.Diamond;
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    /// </remarks>
    /// <seealso cref="LineSegment"/>
    public class PolarLineSeries : PolarSeries
    {
        /// <summary>
        /// Initializes a new instance of the PolarLineSeries.
        /// </summary>
        public PolarLineSeries()
        {
            DrawType = ChartSeriesDrawType.Line;
        }
    }

    /// <summary>
    /// The <see cref="PolarAreaSeries"/> is a series that displays data in terms of values and angles using a filled polygon shape. It allows for the visual comparison of several quantitative or qualitative aspects of a situation.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of polar area series class, and add it to the <see cref="SfPolarChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="ChartSeries.StrokeThickness"/>, <see cref="ChartSeries.Stroke"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para><b>Data Label</b></para>
    /// <para>To customize the appearance of data labels, refer to the <see cref="DataMarkerSeries.ShowDataLabels"/>, and <see cref="PolarRadarSeriesBase.DataLabelSettings"/> properties.</para>
    ///
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///         <!--omitted for brevity-->
    ///
    ///         <chart:PolarAreaSeries ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"
    ///                                ShowDataLabels="True">
    ///             <chart:PolarAreaSeries.DataLabelSettings>
    ///                 <chart:PolarDataLabelSettings />
    ///             </chart:PolarAreaSeries.DataLabelSettings>
    ///         </chart:PolarAreaSeries>
    ///           
    ///     </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///     
    ///     PolarAreaSeries series = new PolarAreaSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.ShowDataLabels = true;
    ///     series.DataLabelSettings = new PolarDataLabelSettings();
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// *** 
    ///
    /// <para><b>Animation</b></para>
    /// <para>To animate the series, refer to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    ///
    /// # [Xaml](#tab/tabid-3)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///         <!--omitted for brevity-->
    ///
    ///         <chart:PolarAreaSeries ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"
    ///                                EnableAnimation="True"/>
    ///
    ///     </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-4)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     PolarAreaSeries series = new PolarAreaSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.EnableAnimation = true;
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <para><b>LegendIcon</b></para>
    /// <para>To customize the legend icon, refer to the <see cref="ChartSeries.LegendIcon"/>, <see cref="ChartSeries.LegendIconTemplate"/>, and <see cref="ChartSeries.VisibilityOnLegend"/> properties.</para>
    ///
    /// # [Xaml](#tab/tabid-5)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///         <!--omitted for brevity-->
    /// 
    ///         <chart:SfPolarChart.Legend>
    ///             <chart:ChartLegend/>
    ///         </chart:SfPolarChart.Legend>
    ///
    ///         <chart:PolarAreaSeries ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"
    ///                                LegendIcon="Diamond"/>
    ///
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-6)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     chart.Legend = new ChartLegend();
    ///
    ///     PolarAreaSeries series = new PolarAreaSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.LegendIcon = ChartLegendIcon.Diamond;
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    /// </remarks>
    /// <seealso cref="AreaSegment"/>
    public class PolarAreaSeries : PolarSeries
    {
        /// <summary>
        /// Initializes a new instance of the PolarAreaSeries.
        /// </summary>
        public PolarAreaSeries()
        {
            DrawType = ChartSeriesDrawType.Area;
        }
    }
}