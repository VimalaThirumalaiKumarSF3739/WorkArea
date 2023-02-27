using Syncfusion.Maui.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Localization resource for <see cref="SfCartesianChart"/>.
    /// </summary>
    public class SfCartesianChartResources : LocalizationResourceAccessor
    {
        internal static string High
        {
            get
            {
                var high = GetString("High");
                return string.IsNullOrEmpty(high) ? "High" : high;
            }
        }

        internal static string Low
        {
            get
            { 
                var low = GetString("Low");
                return string.IsNullOrEmpty(low) ? "Low" : low;
            }
        }
    }
}
