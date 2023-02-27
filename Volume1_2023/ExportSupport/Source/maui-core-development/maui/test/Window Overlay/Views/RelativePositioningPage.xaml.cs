using Syncfusion.Maui.Core.Internals;

namespace WindowOverlay.Samples;

public partial class RelativePositioningPage : ContentPage
{
    public RelativePositioningPage()
    {
        InitializeComponent();
    }
}

public class SliderExt : Slider
{
    private readonly Label tooltip;

    private SfWindowOverlay? overlay;

    public SliderExt()
    {

        tooltip = new Label()
        {
            Padding = new Thickness(5, 0),
            TextColor = Colors.White,
            Background = Brush.DarkViolet,
        };

        ValueChanged += OnSliderExtValueChanged;

        DragCompleted += OnSliderExtDragCompleted;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler != null)
        {
            overlay = new SfWindowOverlay();
        }
    }

    private void OnSliderExtValueChanged(object? sender, ValueChangedEventArgs e)
    {
        tooltip.Text = e.NewValue.ToString("0.##");
        overlay?.AddOrUpdate(tooltip, this);
    }

    private void OnSliderExtDragCompleted(object? sender, EventArgs e)
    {
        //overlay?.Remove(tooltip);
    }
}