using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Converters
{
    /// <summary>
	/// This class converts double value to int value and vice versa.
    /// </summary>
    public class DoubleToIntConverter : IValueConverter
    {
        /// <summary>
		/// Converts double value to an int type.
        /// </summary>
        /// <param name="value">The value must be the type of color</param>
        /// <param name="targetType">The type of target property</param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used</param>
        /// <param name="culture">The culture to use in the converter, not used</param>
        /// <returns>Returns the int value</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is double)
                return System.Convert.ToInt32(value);
            throw new ArgumentException("Value is not a valid double", nameof(value));
        }

        /// <summary>
		/// Converts back the int value to double type
        /// </summary>
        /// <param name="value">The value must be the type of color</param>
        /// <param name="targetType">The type of target property</param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used</param>
        /// <param name="culture">The culture to use in the converter, not used</param>
        /// <returns>Returns the double value</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is int)
                return System.Convert.ToDouble(value);
            throw new ArgumentException("Value is not a valid integer", nameof(value));
        }
    }
}
