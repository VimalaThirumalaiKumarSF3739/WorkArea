using SampleBrowser.Maui.Base;
using System;

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

        if (!IsCardView)
        {
            var label = new Label()
            {
                Text = "Bubble Chart Dynamic Animation",
                TextColor = Colors.Black,
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                LineBreakMode = LineBreakMode.WordWrap,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            bubbleChart.Title = label;
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
