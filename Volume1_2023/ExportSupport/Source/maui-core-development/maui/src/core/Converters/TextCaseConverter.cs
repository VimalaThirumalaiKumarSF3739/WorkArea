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
	/// This class converts text value to casing text value
    /// </summary>
    public class TextCaseConverter : IValueConverter
    {
        #region Property
        /// <summary>
        /// Get and sets the required text case using the CasingMode enum class. 
        /// </summary>
        public CasingMode CasingMode { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Converts text value to an casing text value.
        /// </summary>
        /// <param name="value">The value must be the type of string </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used </param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>Returns the casing string value of the string</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentException("Value is null");

            if (value is string inputString)                
                return this.GetCase(inputString, CasingMode);

            throw new ArgumentException("Input value  is not of type string ",nameof(value));

        }

        /// <summary>
        /// Method Returns the Desired Case of the Input string.
        /// </summary>
        /// <param name="Inputstring">The Input string to convert</param>
        /// <param name="CasingMode">Selected TextCaseType enum value</param>
        /// <returns>Returns the Desired Case of the Input string.</returns>
        private string GetCase(string Inputstring, CasingMode CasingMode)
        {
            switch (CasingMode)
            {
                case CasingMode.LowerCase:
                    return Inputstring.ToLowerInvariant();
                case CasingMode.UpperCase:
                    return Inputstring.ToUpperInvariant();
                case CasingMode.ParagraphCase:
                    return Inputstring.Substring(0, 1).ToUpperInvariant() + Inputstring.ToString().Substring(1).ToLowerInvariant();
                case CasingMode.PascalCase:
                    string outputString = string.Join("", Inputstring.Select(inputChar => 
                    char.IsWhiteSpace(inputChar) & char.IsLetterOrDigit(inputChar) ? inputChar.ToString().ToLower() : inputChar.ToString()).ToArray());
                    var stringArray = (outputString.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries).Select(splitedString =>
                    $"{splitedString.Substring(0, 1).ToUpper()}{splitedString.Substring(1)}"));
                    outputString = string.Join("", stringArray);
                    return outputString;

                default:
                    return Inputstring;
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

    /// <summary>
    /// Enum class for the selecting the Text Case type
    /// </summary>
    public enum CasingMode
    {

        /// <summary>
        /// Convert to uppercase
        /// </summary>
        UpperCase,

        /// <summary>
        /// Convert to lowercase
        /// </summary>
        LowerCase,

        /// <summary>
        /// Converts the first letter of the paragraph to upper
        /// </summary>
        ParagraphCase,

        /// <summary>
        /// Converts the Pascal case
        /// </summary>
        PascalCase,
    }
}
