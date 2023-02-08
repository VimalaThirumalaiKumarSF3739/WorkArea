using Microsoft.Maui.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Converters
{
    /// <summary>
	/// This class converts the list value to boolean value
    /// </summary>
    public class IsListNotNullOrEmptyConverter : IValueConverter
    {
        /// <summary>
		/// Converts list value to an boolean value.
        /// </summary>
        /// <param name="value">The value must be the type of list </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used </param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>Returns the boolean value of the list</returns>
        /// <exception cref="ArgumentException">The exception is thrown when the value type is null or not a type of ICollection</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            else if (value is not ICollection)
            {
                throw new ArgumentException("Value cannot be casted to ICollection", nameof(value));
            }
            else
            {
                var collection = (ICollection)value;
                return collection.Count != 0;
            }

        }

        /// <summary>
        /// Converts back is impossible to revert to original value
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
        }
    }
}
