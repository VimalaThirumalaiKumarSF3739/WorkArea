using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Charts;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart;

	public partial class DefaultBubbleChart : SampleView
	{
		public DefaultBubbleChart()
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
                Text = "World Countries Details",
                TextColor = Colors.Black,
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                LineBreakMode = LineBreakMode.WordWrap,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            Chart.Title = label; yAxis.Title = new ChartAxisTitle() { Text = "A Global Perspective: GDP, and Literacy Rate by Country" }; xAxis.Title= new ChartAxisTitle() { Text = "Literacy rate" };
        }
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();
        Chart.Handler?.DisconnectHandler();
    }
}
