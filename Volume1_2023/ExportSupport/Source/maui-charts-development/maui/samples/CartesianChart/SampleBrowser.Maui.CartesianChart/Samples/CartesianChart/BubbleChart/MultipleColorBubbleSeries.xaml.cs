using SampleBrowser.Maui.Base;

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
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();
        Chart2.Handler?.DisconnectHandler();
    }
}