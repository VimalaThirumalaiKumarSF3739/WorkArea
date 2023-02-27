using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Shimmer;

namespace SampleBrowser.Maui.Shimmer.SfShimmer;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class GettingStarted : SampleView
{
	public GettingStarted()
	{
		InitializeComponent();
        picker.SelectedIndex = 4;
        waveDirectionPicker.SelectedIndex = 0;
        repeatLabel.Text = shimmer.RepeatCount.ToString();
    }

    private void waveDirectionPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        shimmer.WaveDirection = (ShimmerWaveDirection)waveDirectionPicker.SelectedIndex;
    }

    private void picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        shimmer.Type = (ShimmerType)picker.SelectedIndex;
    }

    private void ShimmerColor_Clicked(object sender, EventArgs e)
    {
        viewModel.ShimmerColor = ((Button)sender).BackgroundColor;
        int index = viewModel.ShimmerColors.IndexOf(viewModel.ShimmerColor);
        viewModel.WaveColor = viewModel.WaveColors[index];
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        string text = ((Button)sender).Text;
        if (text == "-")
        {
            shimmer.RepeatCount = shimmer.RepeatCount - 1;
        }
        else
        {
            shimmer.RepeatCount = shimmer.RepeatCount + 1;
        }

        if (shimmer.RepeatCount >= 4)
        {
            shimmer.RepeatCount = 4;
        }
        if (shimmer.RepeatCount <= 1)
        {
            shimmer.RepeatCount = 1;
        }

        repeatLabel.Text = shimmer.RepeatCount.ToString();
    }
}