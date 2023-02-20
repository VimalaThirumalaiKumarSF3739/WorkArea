using SampleBrowser.Maui.Base;
namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart;

public partial class BubbleChartAnimation : SampleView
{
	public BubbleChartAnimation()
	{
		InitializeComponent();
        if (!(BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile))
            viewModel.StartTimer();
    }

    public override void OnAppearing()
    {
        base.OnAppearing();
        if (BaseConfig.RunTimeDeviceLayout == SBLayout.Mobile)
        {
            viewModel.StopTimer();
            viewModel.StartTimer();
        }
    }

    public override void OnDisappearing()
    {
        base.OnDisappearing();
        if (viewModel != null)
            viewModel.StopTimer();

        bubbleChart.Handler?.DisconnectHandler();
    }
}
