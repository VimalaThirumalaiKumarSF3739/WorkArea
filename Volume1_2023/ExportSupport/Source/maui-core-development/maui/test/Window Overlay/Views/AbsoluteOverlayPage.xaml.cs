using Syncfusion.Maui.Core.Internals;

namespace WindowOverlay.Samples;

public partial class AbsoluteOverlayPage : ContentPage
{
    public AbsoluteOverlayPage()
    {
        InitializeComponent();

        ContentView.Page = this;
    }

    internal Tuple<WindowOverlayHorizontalAlignment, WindowOverlayVerticalAlignment> GetAlignment(Point position)
    {
        if (TopLeftBoxView.Bounds.Contains(position))
        {
            return Tuple.Create(WindowOverlayHorizontalAlignment.Left, WindowOverlayVerticalAlignment.Top);
        }
        else if (TopCenterBoxView.Bounds.Contains(position))
        {
            return Tuple.Create(WindowOverlayHorizontalAlignment.Center, WindowOverlayVerticalAlignment.Top);
        }
        else if (TopRightBoxView.Bounds.Contains(position))
        {
            return Tuple.Create(WindowOverlayHorizontalAlignment.Right, WindowOverlayVerticalAlignment.Top);
        }
        else if (CenterLeftBoxView.Bounds.Contains(position))
        {
            return Tuple.Create(WindowOverlayHorizontalAlignment.Left, WindowOverlayVerticalAlignment.Center);
        }
        else if (CenterBoxView.Bounds.Contains(position))
        {
            return Tuple.Create(WindowOverlayHorizontalAlignment.Center, WindowOverlayVerticalAlignment.Center);
        }
        else if (CenterRightBoxView.Bounds.Contains(position))
        {
            return Tuple.Create(WindowOverlayHorizontalAlignment.Right, WindowOverlayVerticalAlignment.Center);
        }
        else if (BottomLeftBoxView.Bounds.Contains(position))
        {
            return Tuple.Create(WindowOverlayHorizontalAlignment.Left, WindowOverlayVerticalAlignment.Bottom);
        }
        else if (BottomCenterBoxView.Bounds.Contains(position))
        {
            return Tuple.Create(WindowOverlayHorizontalAlignment.Center, WindowOverlayVerticalAlignment.Bottom);
        }
        else if (BottomRightBoxView.Bounds.Contains(position))
        {
            return Tuple.Create(WindowOverlayHorizontalAlignment.Right, WindowOverlayVerticalAlignment.Bottom);
        }

        return Tuple.Create(WindowOverlayHorizontalAlignment.Center, WindowOverlayVerticalAlignment.Center);
    }

    private void OnRemoveOverlayButtonClicked(object sender, EventArgs e)
    {
        ContentView.RemoveOverlay();
    }

    private void RemoveAllOverlays()
    {
        if (Window != null)
        {
            IReadOnlyCollection<IWindowOverlay> overlays = Window.Overlays;
            int length = overlays.Count;
            for (int i = 0; i < length; i++)
            {
                Window.RemoveOverlay(overlays.ElementAt(i));
            }
        }
    }
}

public class ContentViewExt : ContentView, ITouchListener
{
    private SfWindowOverlay? windowOverlay;

    public ContentViewExt()
    {
        this.AddTouchListener(this);
    }

    public AbsoluteOverlayPage? Page { get; set; }

    public bool IsTouchHandled
    {
        get { return true; }
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler != null)
        {
            windowOverlay ??= new SfWindowOverlay();
        }
    }

    internal void RemoveOverlay()
    {
        if (windowOverlay != null)
        {
            windowOverlay.RemoveFromWindow();
        }
    }

    void ITouchListener.OnTouch(PointerEventArgs e)
    {
        if (Window != null && e.Action == PointerActions.Released && windowOverlay != null)
        {
            Tuple<WindowOverlayHorizontalAlignment, WindowOverlayVerticalAlignment>? alignment = Page?.GetAlignment(e.TouchPoint);
            if (alignment != null)
            {
                //windowOverlay.Arrange(e.TouchPoint, alignment.Item1, alignment.Item2);
            }
        }
    }
}