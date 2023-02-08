using Syncfusion.Maui.Core.Internals;

namespace WindowOverlay.Samples;

public partial class FillOverlayPage : ContentPage
{
    private readonly ConstrainedLabelOverlay constrainedLabelOverlay;

    private SfWindowOverlay? overlay;

    public FillOverlayPage()
    {
        InitializeComponent();

        constrainedLabelOverlay = new ConstrainedLabelOverlay();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler != null)
        {
            overlay = new SfWindowOverlay();
        }
    }

    private void OnPushPageAsModelButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new ModelPage());
    }

    private void OnPushPageAsNormalButtonClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ModelPage());
    }

    private void OnAddOverlayButtonClicked(object sender, EventArgs e)
    {
        overlay?.AddOrUpdate(constrainedLabelOverlay, (Button)sender,
            horizontalAlignment: WindowOverlayHorizontalAlignment.Right, verticalAlignment: WindowOverlayVerticalAlignment.Top);
    }

    private void OnRemoveOverlayButtonClicked(object sender, EventArgs e)
    {
        overlay?.Remove(constrainedLabelOverlay);
    }

    private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        constrainedLabelOverlay.Text = e.NewValue.ToString("0.##");
        overlay?.AddOrUpdate(constrainedLabelOverlay, (View)sender, horizontalAlignment: WindowOverlayHorizontalAlignment.Right, verticalAlignment: WindowOverlayVerticalAlignment.Top);
    }

    private void OnSliderDragCompleted(object sender, EventArgs e)
    {
        overlay?.Remove(constrainedLabelOverlay);
    }
}