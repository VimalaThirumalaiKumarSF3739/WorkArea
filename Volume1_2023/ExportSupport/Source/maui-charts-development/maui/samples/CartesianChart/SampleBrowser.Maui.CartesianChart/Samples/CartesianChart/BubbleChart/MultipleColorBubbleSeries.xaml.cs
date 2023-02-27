using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Charts;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart;
public partial class MultipleColorBubbleSeries : SampleView
{
	public MultipleColorBubbleSeries()
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
                Text = "Top Cross-Genre Movie Hits: A Performance Overview",
                TextColor = Colors.Black,
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                LineBreakMode = LineBreakMode.WordWrap,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            Chart2.Title = label; yAxis.Title = new ChartAxisTitle() { Text = "Gross Amount in millions" }; xAxis.Title = new ChartAxisTitle() { Text = "Movie Budget" };
        }
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();
        Chart2.Handler?.DisconnectHandler();
    }
}