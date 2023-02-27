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
    /// This class converts boolean value to object and vice versa.
    /// </summary>
    public class BoolToObjectConverter : IValueConverter
    {

        #region Property

        /// <summary>
        /// Gets or sets the value to be returned when the boolean is true
        /// </summary>
        public object? TrueValueObject { get; set; }

        /// <summary>
        /// Gets or sets the value to be returned when the boolean is false
        /// </summary>
        public object? FalseValueObject { get; set; }

        #endregion

        #region method

        /// <summary>
        /// Converts boolean value to an object type.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns>The value to be passed to the target property.</returns>
		/// <exception cref="ArgumentException"></exception>
        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value != null)
            {
                if (value is bool outputValue)
                {
                    return outputValue ? TrueValueObject : FalseValueObject;
                }

                throw new ArgumentException($"Value is not a boolean type", nameof(value));
            }

            throw new ArgumentException($"Value is null", nameof(value));
        }

        /// <summary>
        /// Converts back the object to boolean type
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns>The value to be passed to the source object.</returns>
		/// <exception cref="ArgumentException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.Equals(TrueValueObject))
                    return true;
                else if (value.Equals(FalseValueObject))
                    return false;
                throw new ArgumentException($"Value is not valid", nameof(value));
            }
            else if(value == null && TrueValueObject == null)
            {
                return true;
            }

            throw new ArgumentException($"Value is null", nameof(value));

        }
        
        #endregion 
    }
}
