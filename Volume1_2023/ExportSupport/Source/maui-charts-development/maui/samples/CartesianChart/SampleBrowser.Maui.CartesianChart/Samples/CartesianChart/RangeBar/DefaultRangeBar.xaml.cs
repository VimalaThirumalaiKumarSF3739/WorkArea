using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Charts;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart
{
    public partial class DefaultRangeBar : SampleView
    {
        public DefaultRangeBar()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            hyperLinkLayout.IsVisible = !IsCardView;
            if (!IsCardView)
            {
                var label = new Label()
                {
                    Text = "Annual Temperature Variations in Rome",
                    TextColor = Colors.Black,
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(0, 0, 0, 20),
                    LineBreakMode = LineBreakMode.WordWrap,
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                
                Chart.Title = label;
                yAxis.Title = new ChartAxisTitle() { Text = "Temperature [°C]" };
            }
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart.Handler?.DisconnectHandler();
        }
    }
}
