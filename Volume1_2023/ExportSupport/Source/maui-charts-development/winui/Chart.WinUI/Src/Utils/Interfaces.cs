using System;
using System.Collections;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Interface implementation for IRangeAxis
    /// </summary>
    public interface IRangeAxis
    {
        /// <summary>
        /// Gets Range property
        /// </summary>
        DoubleRange Range { get; }
    }

    /// <summary>
    /// Interface implementation for ISupportAxes
    /// </summary>
    public interface ISupportAxes
    {
        /// <summary>
        /// Gets XRange property
        /// </summary>
        DoubleRange VisibleXRange
        {
            get;
        }

        /// <summary>
        /// Gets YRange property
        /// </summary>
        DoubleRange VisibleYRange
        {
            get;
        }

        /// <summary>
        /// Gets ActualXAxis property.
        /// </summary>
        /// <value>It takes the <c>ChartAxis</c> value.</value>
        ChartAxis ActualXAxis { get; }
        /// <summary>
        /// Gets ActualYAxis property.
        /// </summary>
        /// <value>It takes the <c>ChartAxis</c> value.</value>
        ChartAxis ActualYAxis { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISegmentSelectable
    {
        #region Properites

        DataPointSelectionBehavior SelectionBehavior
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISegmentSpacing
    {
        #region Properties

        /// <summary>
        /// Gets or sets SegmentSpacing property
        /// </summary>
        double SegmentSpacing
        {
            get;
            set;
        }

        #endregion

        #region Methods

        ///<summary>
        /// Method used to calculate the segment spacing.
        ///</summary>
        /// <param name="spacing">Segment spacing value.</param>
        /// <param name="Right">Segment right value.</param>
        /// <param name="Left">Segment left value.</param>
        /// <returns>Returns the calculated segment space.</returns>
        double CalculateSegmentSpacing(double spacing, double Right, double Left);

        #endregion
    }

}
