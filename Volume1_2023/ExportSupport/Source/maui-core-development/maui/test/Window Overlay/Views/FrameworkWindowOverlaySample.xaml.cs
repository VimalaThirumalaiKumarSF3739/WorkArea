namespace WindowOverlay.Samples;

public partial class FrameworkWindowOverlaySample : ContentPage
{
	private FrameworkWindowOverlay? overlay;

	public FrameworkWindowOverlaySample()
	{
		InitializeComponent();
	}

    void OnAddOverlayButtonClicked(System.Object sender, System.EventArgs e)
    {
		if (Window != null)
		{
			overlay ??= new FrameworkWindowOverlay(Window);

			Window.AddOverlay(overlay);
		}
    }
}
