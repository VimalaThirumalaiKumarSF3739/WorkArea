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
	/// This class converts to decimal value
    /// </summary>
    public class DecimalValueConverter : IValueConverter
    {
        /// <summary>
        /// Gets and sets the decimal places 
        /// </summary>
        public int NumberDecimalDigits { get; set; }

        /// <summary>
        /// Gets and sets the output format
        /// </summary>
        public OutputType OutputType { get; set; }

        /// <summary>
        /// Converts input value with required decimal type.
        /// </summary>
        /// <param name="value">The value must be the type of color</param>
        /// <param name="targetType">The type of target property</param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used</param>
        /// <param name="culture">The culture to use in the converter, not used</param>
        /// <returns>Returns the decimal value</returns>
        /// <exception cref="ArgumentException"></exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string outputZeroString = "0";
            if (value != null)
            {
                if (value.ToString() == "")
                {
                    outputZeroString = new string('0', NumberDecimalDigits);
                    return $"0.{outputZeroString}";
                }

                decimal decimalValue = decimal.Parse(value.ToString()!);
                switch (OutputType)
                {
                    case OutputType.String:

                        if (decimalValue % 1 == 0)
                        {
                            outputZeroString = new string('0', NumberDecimalDigits);
                            return $"{Math.Round(decimalValue)}.{outputZeroString}";
                        }

                        else
                        {
                            string decimalString = decimalValue.ToString();
                            int length = decimalString.Substring(decimalString.IndexOf(".")).Length;

                            if ((length - 1) >= 1 && ((length - 1) <= NumberDecimalDigits))
                            {
                                outputZeroString = new string('0', NumberDecimalDigits - (length - 1));

                                return $"{decimalValue}{outputZeroString}";
                            }
                            return Math.Round(decimalValue, NumberDecimalDigits).ToString();
                        }

                    case OutputType.Decimal:
                    default:
                        return Math.Round(decimalValue, NumberDecimalDigits);
                }
            }

            else
            {
                throw new ArgumentException("Value cannot be null", nameof(value));
            }
        }

        /// <summary>
        /// Converts back are impossible to revert to the original value
        /// </summary>
        /// <param name="value">The value must be the type of color</param>
        /// <param name="targetType">The type of target property</param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used</param>
        /// <param name="culture">The culture to use in the converter, not used</param>
        /// <returns>Not Implemented</returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
        }
    }

    /// <summary>
    /// Enum for the Showing output in the required format.
    /// </summary>
    public enum OutputType
    {
        /// <summary>
        /// Returns output as String
        /// </summary>
        String,

        /// <summary>
        /// Returns output as decimal
        /// </summary>
        Decimal,
    }
}
