using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Converters
{
    /// <summary>
	/// This class converts string value to list
    /// </summary>
    public class StringToListConverter : IValueConverter
    {
        #region fields
        /// <summary>
        /// Gets or sets the value string that separates the substrings in this string.
        /// </summary>
        public string Separator { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value strings that separates the substrings in this string.
        /// </summary>
        public IList<string> Separators { get; } = new List<string>();

        /// <summary>
        /// Gets or sets the value decides whether to trim substrings and include empty substrings.
        /// </summary>
        public StringSplitOptions SplitOptions { get; set; } = StringSplitOptions.None;
        #endregion

        #region methods
        /// <summary>
        /// This method is used to convert string to list
        /// Converts string value to an list type.
        /// </summary>
        /// <param name="value">The value must be the type of string </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used </param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>Returns the list value of the string</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if(value is string stringValue)
                {
                    if (Separators.Count == 0 )
                    {
                        string[] substrings = stringValue.Split(Separator, SplitOptions);
                        List<string> listString = substrings.ToList<string>();
                        return listString;
                    }
                    else if (Separators.Count >= 1)
                    {
                        string[] substrings = stringValue.Split(Separators.ToArray(), SplitOptions);
                        List<string> listString = substrings.ToList<string>();
                        return listString;
                    }
                    else
                    {
                        return string.Empty;
                    }
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
