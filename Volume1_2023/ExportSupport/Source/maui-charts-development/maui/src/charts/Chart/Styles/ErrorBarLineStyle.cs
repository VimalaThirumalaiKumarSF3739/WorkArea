using Microsoft.Maui.Controls;
using System;

namespace Syncfusion.Maui.Charts
{

    /// <summary>
    /// 
    /// </summary>
    public class ErrorBarLineStyle : ChartLineStyle
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty StrokeCapProperty =
            BindableProperty.Create(
                nameof(StrokeCap),
                typeof(ErrorBarStrokeCap),
                typeof(ErrorBarLineStyle),
                ErrorBarStrokeCap.Flat);

        /// <summary>
        /// 
        /// </summary>
        public ErrorBarStrokeCap StrokeCap
        {
            get { return (ErrorBarStrokeCap)GetValue(StrokeCapProperty); }
            set { SetValue(StrokeCapProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ErrorBarLineStyle()
        {
            Stroke = Brush.Black;
            StrokeWidth = 1;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ErrorBarCapLineStyle : ChartLineStyle
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty CapLineSizeProperty =
            BindableProperty.Create(
                nameof(CapLineSize),
                typeof(double),
                typeof(ErrorBarCapLineStyle),
                1d);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty IsVisibleProperty =
            BindableProperty.Create(
                nameof(IsVisible),
                typeof(bool),
                typeof(ErrorBarCapLineStyle),
                true);

        /// <summary>
        /// 
        /// </summary>
        public double CapLineSize
        {
            get { return (double)GetValue(CapLineSizeProperty); }
            set { SetValue(CapLineSizeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ErrorBarCapLineStyle()
        {
            Stroke = Brush.Black;
            StrokeWidth = 1;
        }
    }
}

