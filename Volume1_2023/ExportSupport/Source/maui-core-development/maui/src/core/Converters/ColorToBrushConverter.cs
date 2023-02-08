using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Converters
{
    /// <summary>
	/// This class converts the color value into the brush and vice versa.
    /// </summary>
    public class ColorToBrushConverter : IValueConverter
    {
        /// <summary>
		/// Converts color value to an brush type.
        /// </summary>
        /// <param name="value">The value must be the type of color</param>
        /// <param name="targetType">The type of the target property</param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used</param>
        /// <param name="culture">The culture to use in the converter, not used</param>
        /// <returns>SolidColorBrush</returns>
        /// <exception cref="ArgumentException">Exception thrown when the value type is null or not a type of Color</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is Color color)
            {
                return new SolidColorBrush(color);
            }
            throw new ArgumentException("Expected value to be a type of color", nameof(value));
        }

        /// <summary>
		/// Converts back the brush to color type.
        /// </summary>
        /// <param name="value">The value must be the type of SolidColorBrush</param>
        /// <param name="targetType">The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used</param>
        /// <param name="culture">The culture to use in the converter, not used</param>
        /// <returns>Color</returns>
        /// <exception cref="ArgumentException">Exception thrown when the value type is null or not a type of SolidColorBrush</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!Brush.IsNullOrEmpty(value as SolidColorBrush))
            {
                SolidColorBrush brush = (SolidColorBrush)value;
                return brush.Color;
            }
            throw new ArgumentException("Expected value to be a type of brush", nameof(value));
        }
    }
}
