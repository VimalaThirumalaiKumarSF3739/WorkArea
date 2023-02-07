using Microsoft.Maui.Graphics;
using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Charts;
using Syncfusion.Maui.Core;
using System.Collections.ObjectModel;
using System.Globalization;

namespace SampleBrowser.Maui.CircularChart.SfCircularChart
{
    public partial class CenterElevation : SampleView
    {
        public CenterElevation()
        {
            InitializeComponent();
        }
    }

    public class CornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return new CornerRadius((double)value / 2);
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

