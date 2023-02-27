using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Charts;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart
{
    public partial class WaterFallChart : SampleView
    {
        public WaterFallChart()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            if (!IsCardView)
            {
                var label = new Label()
                {
                    Text = "Net Cash Flow (2021-2022)",
                    HorizontalOptions = LayoutOptions.Center,
                };

                Chart1.Title = label;
                myYaxis.Title = new ChartAxisTitle() { Text = "Dollar (USD)" }; 
                myXAxis.Title= new ChartAxisTitle() { Text = "Months" };
            }
        }
        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart1.Handler?.DisconnectHandler();
        }
    }
}
