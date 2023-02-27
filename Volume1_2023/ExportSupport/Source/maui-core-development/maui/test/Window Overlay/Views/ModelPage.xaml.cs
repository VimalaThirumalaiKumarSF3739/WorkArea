namespace WindowOverlay.Samples;

public partial class ModelPage : ContentPage
{
	public ModelPage()
	{
		InitializeComponent();
	}

	private void OnPopCurrentModelPageButtonClicked(object sender, EventArgs e)
	{
		Navigation.PopModalAsync();
	}

	private void OnPopCurrentPageButtonClicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}