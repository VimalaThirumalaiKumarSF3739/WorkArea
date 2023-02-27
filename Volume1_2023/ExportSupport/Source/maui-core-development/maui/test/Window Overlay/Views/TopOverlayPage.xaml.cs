using Syncfusion.Maui.Core.Internals;

namespace WindowOverlay.Samples;

public partial class TopOverlayPage : ContentPage
{
    private readonly ConstrainedLabelOverlay topWindowOverlayContent;

    private readonly ConstrainedLabelOverlay bottomWindowOverlayContent;

    private SfWindowOverlay? overlay;

    public TopOverlayPage()
    {
        InitializeComponent();

        topWindowOverlayContent = new ConstrainedLabelOverlay();

        bottomWindowOverlayContent = new ConstrainedLabelOverlay();
    }

    private void OnRemoveOverlayButtonClicked(object sender, EventArgs e)
    {
        overlay?.Remove(topWindowOverlayContent);

        overlay?.Remove(bottomWindowOverlayContent);
    }

    private void OnTopButtonClicked(object sender, EventArgs e)
    {
        Initialize();
        overlay?.AddOrUpdate(topWindowOverlayContent, TopButton,
            horizontalAlignment: WindowOverlayHorizontalAlignment.Left, verticalAlignment: WindowOverlayVerticalAlignment.Top);
    }

    private void OnBottomButtonClicked(object sender, EventArgs e)
    {
        Initialize();
        overlay?.AddOrUpdate(bottomWindowOverlayContent, BottomButton,
            horizontalAlignment: WindowOverlayHorizontalAlignment.Right, verticalAlignment: WindowOverlayVerticalAlignment.Bottom);
    }

    private void Initialize()
    {
        if (overlay == null)
        {
            overlay = new SfWindowOverlay();
        }
    }

}