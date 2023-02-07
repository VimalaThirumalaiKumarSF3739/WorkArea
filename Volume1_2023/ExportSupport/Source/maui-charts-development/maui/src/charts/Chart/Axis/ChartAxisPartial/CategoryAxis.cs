﻿using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The CategoryAxis is an indexed based axis that plots values based on the index of the data point collection. It displays string values in axis labels.
    /// </summary>
    /// <remarks>
    /// 
    /// <para>Category axis supports only for the X(horizontal) axis. </para>
    /// 
    /// <para>To render an axis, add the category axis instance to the chart’s <see cref="SfCartesianChart.XAxes"/> collection as shown in the following code sample.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///  
    ///         <chart:SfCartesianChart.XAxes>
    ///             <chart:CategoryAxis/>
    ///         </chart:SfCartesianChart.XAxes>
    /// 
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// CategoryAxis xaxis = new CategoryAxis();
    /// chart.XAxes.Add(xaxis);	
    /// 
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para>The CategoryAxis supports the following features. Refer to the corresponding APIs, for more details and example codes.</para>
    /// 
    /// <para> <b>Title - </b> To render the title, refer to this <see cref="ChartAxis.Title"/> property.</para>
    /// <para> <b>Grid Lines - </b> To show and customize the grid lines, refer these <see cref="ChartAxis.ShowMajorGridLines"/>, and <see cref="ChartAxis.MajorGridLineStyle"/> properties.</para>
    /// <para> <b>Axis Line - </b> To customize the axis line using the <see cref="ChartAxis.AxisLineStyle"/> property.</para>
    /// <para> <b>Labels Customization - </b> To customize the axis labels, refer to this <see cref="ChartAxis.LabelStyle"/> property.</para>
    /// <para> <b>Inversed Axis - </b> Inverse the axis using the <see cref="ChartAxis.IsInversed"/> property.</para>
    /// <para> <b>Axis Crossing - </b> For axis crossing, refer these <see cref="ChartAxis.CrossesAt"/>, <see cref="ChartAxis.CrossAxisName"/>, and <see cref="ChartAxis.RenderNextToCrossingValue"/> properties.</para>
    /// <para> <b>Label Placement - </b> To place the axis labels in between or on the tick lines, refer to this <see cref="LabelPlacement"/> property.</para>
    /// <para> <b>Interval - </b> To define the interval between the axis labels, refer to this <see cref="Interval"/> property.</para>
    /// </remarks>
    public partial class CategoryAxis
    {
        #region Bindable Properties
        /// <summary>
        /// Identifies the <see cref="Interval"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
            nameof(Interval),
            typeof(double),
            typeof(CategoryAxis),
            double.NaN,
            BindingMode.Default,
            null,
            OnIntervalPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="LabelPlacement"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty LabelPlacementProperty = BindableProperty.Create(
            nameof(LabelPlacement),
            typeof(LabelPlacement),
            typeof(CategoryAxis),
            LabelPlacement.OnTicks,
            BindingMode.Default,
            null,
            OnLabelPlacementPropertyChanged);

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets a value that determines whether to place the axis label in between or on the tick lines.
        /// </summary>
        /// <remarks>
        /// <para> <b>BetweenTicks - </b> Used to place the axis label between the ticks.</para>
        /// <para> <b>OnTicks - </b> Used to place the axis label with the tick as the center.</para>
        /// </remarks>
        /// <value>It accepts the <see cref="Charts.LabelPlacement"/> values and the default value is <c>OnTicks</c>. </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis LabelPlacement="BetweenTicks" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis();
        /// xaxis.LabelPlacement = LabelPlacement.BetweenTicks;
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        public LabelPlacement LabelPlacement
        {
            get { return (LabelPlacement)GetValue(LabelPlacementProperty); }
            set { SetValue(LabelPlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that can be used to customize the interval between the axis labels.
        /// </summary>
        /// <remarks>If this property is not set, the interval will be calculated automatically.</remarks>
        /// <value>It accepts double values and the default value is double.NaN.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:CategoryAxis Interval="2" />
        ///         </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis(){ Interval = 2, };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        #endregion

        #region Private Methods
        private static void OnIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as CategoryAxis;
            if (axis != null)
            {
                axis.UpdateAxisInterval((double)newValue);
                axis.UpdateLayout();
            }
        }

        private static void OnLabelPlacementPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as CategoryAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        #endregion
    }
}
