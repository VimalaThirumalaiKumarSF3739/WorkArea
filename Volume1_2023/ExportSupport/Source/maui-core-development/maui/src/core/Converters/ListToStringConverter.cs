using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Converters
{
    /// <summary>
	/// This class converts list value to string
    /// </summary>
    public class ListToStringConverter :IValueConverter
    {
        #region fields

        /// <summary>
        /// Gets or sets the value the string that separates the substrings
        /// </summary>
        public string Separator { get; set; } = string.Empty;

        #endregion

        #region methods
        /// <summary>
        /// This method is used to convert list to string
		/// Converts list value to an string value type.
        /// </summary>
        /// <param name="value">The value must be the type of list </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used </param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>Returns the string value of the list</returns>
        /// <exception cref="ArgumentException">The exception is thrown when the value type is null or not a type of color</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value is ICollection listOfValues)
                {
                    var listElement = from val in listOfValues.OfType<object>()
                                      select val.ToString();

                    return string.Join(Separator, listElement);
                }

                throw new ArgumentException($"Value is of {value.GetType}");
            }
            else
            {
                return string.Empty;
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

        #endregion
    }
}
