using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Maui.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Converters
{
    /// <summary>
	/// This class converts double value to opacity and vice versa.
    /// </summary>
    public class InverseOpacityConverter : IValueConverter
    {
        #region methods
        /// <summary>
		/// Converts double value to an inverse opacity type.
        /// </summary>
        /// <param name="value">The value must be the type of numeric </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used </param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>Returns the inverse opacity value of the double value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           double inverseOpacityValue = ReturnOpacityValue(value);

           return inverseOpacityValue;
        }

        /// <summary>
        /// Converts back the inverse opacity to double type
        /// </summary>
        /// <param name="value">The value be the type of int</param>
        /// <param name="targetType"> The type of the target property</param>
        /// <param name="parameter"> An additional parameter for the converter to handle, not used</param>
        /// <param name="culture"> The culture to use in the converter, not used</param>
        /// <returns>Returns the double value of the inverse opacity</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double opacityValue = ReturnOpacityValue(value);

            return opacityValue;
        }

        /// <summary>
        /// This method is used to return inverse opacity value whether the value is inverse or not
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private double ReturnOpacityValue(object value)
        {
            if (value != null)
            {
                double opacityValue = System.Convert.ToDouble(value);

                double inverseOpacityValue = 1 - opacityValue;

                if (opacityValue >= 0 && opacityValue <= 1)
                {
                    return Math.Round(inverseOpacityValue, 1);
                }
                else if (opacityValue < 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            throw new ArgumentException("Value is null");
        }
        #endregion
    }
}
