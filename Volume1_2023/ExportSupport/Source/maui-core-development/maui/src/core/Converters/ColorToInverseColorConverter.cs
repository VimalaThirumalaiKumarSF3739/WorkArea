using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Converters
{
    /// <summary>
	/// This class converts color value to inverse color and vice versa.
    /// </summary>
    public class ColorToInverseColorConverter : IValueConverter
    {
        /// <summary>
		/// Converts color value to an inverse color type.
        /// </summary>
        /// <param name="value">The value must be the type of color </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used </param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>Returns the inverse color value of the color</returns>
        /// <exception cref="ArgumentException">The exception is thrown when the value type is null or not a type of color</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertColorToInverseColor(value);
        }

        /// <summary>
		/// Converts back the inverse color to color type
        /// </summary>
        /// <param name="value">The value be the type of InverseColor </param>
        /// <param name="targetType"> The type of the target property</param>
        /// <param name="parameter"> An additional parameter for the converter to handle, not used</param>
        /// <param name="culture"> The culture to use in the converter, not used</param>
        /// <returns>Returns the color value of the inverse color</returns>
        /// <exception cref="ArgumentException">The exception is thrown when the value type is null or not a type of inverse color</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertColorToInverseColor(value);
        }
        
        /// <summary>
        /// This method convert Color to its Inverse Color
        /// </summary>
        /// <param name="value">value to be the type of Color</param>
        /// <returns>Inversed color of given color</returns>
        /// <exception cref="ArgumentException">Exception thrown when value is null or other than color type</exception>
        private Color ConvertColorToInverseColor(object value)
        {
            if (value != null && value is Color color)
            {
                return new Color(1 - color.Red, 1 - color.Green, 1 - color.Blue);
            }
            throw new ArgumentException("Expected value to be a type of color", nameof(value));
        }
    }
}
