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
	/// This class converts index to to array item and vice versa.
    /// </summary>
    public class IndexToArrayItemConverter : IValueConverter
    {
        /// <summary>
		/// Converts index value to an array item.
        /// </summary>
        /// <param name="value">The value must be the type of int </param>
        /// <param name="targetType"> The type of the target property </param>
        /// <param name="parameter">An additional parameter for the converter to handle, not used </param>
        /// <param name="culture"> The culture to use in the converter, not used </param>
        /// <returns>Returns the array item corresponds to the index</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException("Value should not null", nameof(value));

            if (parameter == null)
                throw new ArgumentNullException("Parameter should not null", nameof(parameter));

            if (value is not int indexValue)
            {
                if (value is not double)
                {
                    throw new ArgumentException("Value is not a valid integer", nameof(value));
                }
                indexValue = System.Convert.ToInt32(value);
            }

            if (parameter is not ICollection<object> collection)
                throw new ArgumentException("Parameter is not a valid array", nameof(parameter));

            var list = collection.ToList();

            if (indexValue < 0 || indexValue >= list.Count)
                throw new ArgumentOutOfRangeException("Index was out of range", nameof(value));

            return list[indexValue];
        }

        /// <summary>
		/// Converts back the array item to index value
        /// </summary>
        /// <param name="value">The value be the type of array item </param>
        /// <param name="targetType"> The type of the target property</param>
        /// <param name="parameter"> An additional parameter for the converter to handle, not used</param>
        /// <param name="culture"> The culture to use in the converter, not used</param>
        /// <returns>Returns the index of the selected array item</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException("Value should not be null", nameof(value));

            if (parameter == null)
                throw new ArgumentNullException("Parameter should not null", nameof(parameter));

            if (parameter is not ICollection<object> collection)
                throw new ArgumentException("Parameter is not a valid array", nameof(parameter));

            var list = collection.ToList();
            if (list.Contains(value))
                return list.IndexOf(value);

            throw new ArgumentException("Value does not exist in the array", nameof(value));
        }
    }
}
