using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Converters
{
    /// <summary>
    /// This class converts the color value into gray scale color value
    /// </summary>
    public class ColorToGrayScaleColorConverter : IValueConverter
    {
        /// <summary>
		/// Converts color value to an gray scale color type.
        /// </summary>
        /// <param name="value"> The value must be the type of color </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used</param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>GrayScaleColor</returns>
        /// <exception cref="ArgumentException">Exception thrown when the value type is null or not a type of Color</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is Color color)
            {
                return GetGrayScaleColor(color);
            }
            throw new ArgumentException("Expected value to be a type of color", nameof(value));
        }

        /// <summary>
		/// Converts back the  gray scale color to color type
        /// </summary>
		/// <param name="value">The value must be the type of SolidColorBrush</param>
        /// <param name="targetType">The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used</param>
        /// <param name="culture">The culture to use in the converter, not used</param>
        /// <exception cref="NotSupportedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Converting back to the original value is not possible. Binding mode must be set as one way.");
        }

        #region Private Method

        /// <summary>
        /// This method perform the conversion opertion of Color value to GrayScale Color value
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>The GrayScale color value of the given Color</returns>
        private Color GetGrayScaleColor(Color color)
        {
            float grayValue = (color.Red + color.Green + color.Blue)/3;
            return new Color(grayValue);
        }

        #endregion
    }

}
