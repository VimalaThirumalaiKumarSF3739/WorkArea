using Syncfusion.Maui.Core.Internals;

namespace WindowOverlay.Samples;

public partial class OverlayPage : ContentPage
{
    private SfWindowOverlay? overlay;

    private ConstrainedLabelOverlay? constrainedLabelOverlay;

    private ConstrainedScrollViewOverlay? constrainedScrollViewOverlay;

    private UnconstrainedCollectionViewOverlay? unconstrainedListViewOverlay;

    public OverlayPage()
    {
        InitializeComponent();

        constrainedLabelOverlay = new ConstrainedLabelOverlay();

        constrainedScrollViewOverlay = new ConstrainedScrollViewOverlay();

        unconstrainedListViewOverlay = new UnconstrainedCollectionViewOverlay();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler != null)
        {
            overlay = new SfWindowOverlay();
        }
    }

    private void OnConstrainedLabelTargetTapped(object sender, EventArgs e)
    {
        if (overlay != null && constrainedLabelOverlay != null)
        {
            overlay.AddOrUpdate(constrainedLabelOverlay, 100, 100, WindowOverlayHorizontalAlignment.Left, WindowOverlayVerticalAlignment.Center);
        }
    }

    private void OnConstrainedScrollViewTargetTapped(object sender, EventArgs e)
    {
        if (overlay != null && constrainedScrollViewOverlay != null)
        {
            overlay.AddOrUpdate(constrainedScrollViewOverlay, 200, 300, WindowOverlayHorizontalAlignment.Right, WindowOverlayVerticalAlignment.Top);
        }
    }

    private void OnUnconstraniedListViewTargetTapped(object sender, EventArgs e)
    {
        if (overlay != null && unconstrainedListViewOverlay != null)
        {
            overlay.AddOrUpdate(unconstrainedListViewOverlay, 0, 0);
        }
    }

    private void OnRemoveOverlayButtonClicked(object sender, EventArgs e)
    {
        RemoveAllOverlays();
    }

    private void RemoveAllOverlays()
    {
        if (overlay != null)
        {
            if (constrainedLabelOverlay != null)
            {
                overlay.Remove(constrainedLabelOverlay);
            }

            if (constrainedScrollViewOverlay != null)
            {
                overlay.Remove(constrainedScrollViewOverlay);
            }

            if (unconstrainedListViewOverlay != null)
            {
                overlay.Remove(unconstrainedListViewOverlay);
            }
        }
    }
}

