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
	/// This class converts boolean value to opacity and vice versa.
    /// </summary>
    public class BoolToOpacityConverter : IValueConverter
    {
        /// <summary>
        /// The convert method is used to convert the boolean value to opacity. 
        /// If the boolean value is true it converts into 1 and if it is false it converts into false.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns>The value to be passed to the target property.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null
                ? (bool)value ? 1 : 0
                : throw new ArgumentNullException("Value should not be null" , nameof(value));
        }


        /// <summary>
        /// The convert back method is used to convert the opacity value to boolean. 
        /// If the opacity value is 1 it converts into true and if it is false it converts into 1.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns>The value to be passed to the source object.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null
                ? (double)value == 1
                : throw new ArgumentNullException("Value should not be null" ,nameof(value));

        }
    }
}
