using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Converters
{
	/// <summary>
	/// This converter having the properties of MultiConverter.
	/// </summary>
	public class ParametersOfMultiConverter : BindableObject
	{
		/// <summary>
		/// The type of object of this parameter.
		/// </summary>
		public Type? TypeOfConverter { get; set; }

		/// <summary>
		/// The value of this parameter.
		/// </summary>
		public object? ValueOfConverter { get; set; }
	}
}
