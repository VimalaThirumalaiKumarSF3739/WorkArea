using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters
{

    /// <summary>
    /// This class converts string value to formatted value
    /// </summary>
    public class FormatStringConverter : IValueConverter
    {

        /// <summary>
		/// Converts string value to formatted string.
        /// </summary>
        /// <param name="value">The value must be the type of string </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used </param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>Returns the Formatted string</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentException("Value is Null");
            }
            else
            {
                if (value is not IFormattable formattableString || parameter is not string stringFormat)
                    throw new ArgumentException("value is not of type Iformattable or Format is null", nameof(value));
                else
                    return formattableString.ToString(stringFormat, null);
            }
        }

        /// <summary>
        /// Converts back is impossible to revert to original value
        /// </summary>
        /// <param name="value"> value be the type of InverseColor </param>
        /// <param name="targetType"> The type of the target property</param>
        /// <param name="parameter"> An additional parameter for the converter to handle, not used</param>
        /// <param name="culture"> The culture to use in the converter, not used</param>
        /// <returns>Not implemented</returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
        }
    }
}
