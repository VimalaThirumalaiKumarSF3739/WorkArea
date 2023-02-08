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
	/// This class converts number value to boolean and vice versa.
    /// </summary>
    public class NumberToBoolConverter : IValueConverter
    {
        /// <summary>
		/// Converts number value to an boolean type.
        /// </summary>
        /// <param name="value">The value must be the type of number </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used </param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>Returns the boolean value of the number</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null
                ? System.Convert.ToBoolean(value)
                : throw new ArgumentNullException("Value should not be null" , nameof(value));
        }

        /// <summary>
		///Converts back the boolean value to number 
        /// </summary>
        /// <param name="value">The value be the type of boolean </param>
        /// <param name="targetType"> The type of the target property</param>
        /// <param name="parameter"> An additional parameter for the converter to handle, not used</param>
        /// <param name="culture"> The culture to use in the converter, not used</param>
        /// <returns>Returns the boolean value of the number</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null
                ? !(bool)value ? 0 : 1
                : throw new ArgumentNullException("Value should not be null" , nameof(value));

        }


    }
}
