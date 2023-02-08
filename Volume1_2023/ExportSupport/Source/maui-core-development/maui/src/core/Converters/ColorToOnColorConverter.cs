using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Converters
{
    /// <summary>
	/// This class converts color value to color and vice versa.
    /// </summary>
    public class ColorToOnColorConverter : IValueConverter
    {
        /// <summary>
		/// Converts to black color for light color and white color for dark color type.
        /// </summary>
        /// <param name="value">The value must be the type of color</param>
        /// <param name="targetType">The type of target property</param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used</param>
        /// <param name="culture">The culture to use in the converter, not used</param>
        /// <returns>Returns the black color for light color and white color for dark color</returns>
        /// <exception cref="ArgumentException">Exception is thrown when the value type is null or not a type of color</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is Color color)
            {
                return GetColorForText(color);
            }
            throw new ArgumentException("Expected value to be a type of color", nameof(value));
        }

        /// <summary>
        /// Converts back is impossible to revert to original value
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay."); 
        }

        #region Private Methods
        /// <summary>
        /// This method return the TextColor suitable for its BackgroundColor
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>If background is light color, it returns black text color else it returns white text color</returns>
        private Color GetColorForText(Color color)
        {
            return color.GetLuminosity() > 0.72 ? Colors.Black : Colors.White;
        }
        #endregion

    }
}
