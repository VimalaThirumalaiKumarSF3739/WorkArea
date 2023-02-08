using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Converters
{
    /// <summary>
	/// This class converts enum value to int and vice versa.
    /// </summary>
    public class EnumToIntConverter : IValueConverter
    {

        #region methods
        /// <summary>
        /// Converts enum value to an int type.
        /// </summary>
        /// <param name="value">The value must be the type of enum </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used </param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>Returns the int value of the enum</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value is Enum)
                {
                    return System.Convert.ToInt32(value);
                }
                throw new ArgumentException($"Value is not of enum type, it is of {nameof(value)}");
            }
            throw new ArgumentException($"Value is null");
        }

        /// <summary>
		/// Converts back the int value to enum type
        /// </summary>
        /// <param name="value"> The value be the type of int </param>
        /// <param name="targetType"> The type of the target property</param>
        /// <param name="parameter"> An additional parameter for the converter to handle, not used</param>
        /// <param name="culture"> The culture to use in the converter, not used</param>
        /// <returns>Returns the enum value of the int value</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value is int enumNumber && Enum.IsDefined(targetType, enumNumber))
                {
                    return Enum.ToObject(targetType, enumNumber);
                }
                throw new ArgumentException($"Value is not of enummeration type");
            }
            throw new ArgumentException($"Value is null");
        }
        #endregion
    }
}
