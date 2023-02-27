using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Converters
{
	/// <summary>
	/// This class converts the incoming value and performs more than one convertion operations.
	/// </summary>
	public class MultiConverter : List<IValueConverter>, IValueConverter
	{
		/// <summary>
		/// Uses the incoming converters to convert the value.
		/// </summary>
		/// <param name="value">The value must be the type of list </param>
		/// <param name="targetType"> The type of the target property </param>
		/// <param name="parameter">An additional parameter for the converter to handle, not used </param>
		/// <param name="culture"> The culture to use in the converter, not used </param>
		/// <returns>Returns the aggregate value of the incoming value</returns>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				throw new ArgumentNullException("value");
			if (parameter is IList<ParametersOfMultiConverter> propertiesOfConverter)
			{
				return this.Aggregate(value, (currentValue, currentConverter) => currentConverter.Convert(currentValue, targetType,
						 propertiesOfConverter.FirstOrDefault(x => x.TypeOfConverter == currentConverter.GetType())?.ValueOfConverter, culture));
			}
			else
			{
				return this.Aggregate(value, (currentValue, currentConverter) => currentConverter.Convert(currentValue, targetType, parameter, culture));
			}
		}

		/// <summary>
		/// Converts back is impossible to revert to original value
		/// </summary>
		/// <exception cref="NotSupportedException"></exception>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException("ConvertBack to original value is not possible. BindingMode must be set as OneWay.");
		}
	}

}
