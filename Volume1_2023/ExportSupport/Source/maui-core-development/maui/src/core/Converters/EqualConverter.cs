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
	/// This class checks whether the value and parameter it's equal or not
    /// </summary>
    public class EqualConverter : IValueConverter
    {
        #region methods
        /// <summary>
        /// This method is used to compare the value and parameter
        /// </summary>
        /// <param name="value">The value must be the type of any value </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used </param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>Returns the bool value of the comparison</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool checkEqual = ((value != null && value.Equals(parameter)) || (value == null && parameter == null));

            return checkEqual;
        }

        /// <summary>
        /// Converts back is impossible to revert to original value
        /// </summary>
        /// <param name="value"> value be the type of InverseColor </param>
        /// <param name="targetType"> The type of the target property</param>
        /// <param name="parameter"> An additional parameter for the converter to handle, not used</param>
        /// <param name="culture"> The culture to use in the converter, not used</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
        }
        #endregion
    }
}
