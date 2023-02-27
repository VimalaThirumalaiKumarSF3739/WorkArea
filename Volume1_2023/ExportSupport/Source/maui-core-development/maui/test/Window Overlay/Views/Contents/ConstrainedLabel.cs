using System;

namespace WindowOverlay.Samples
{
	public class ConstrainedLabelOverlay : Label
	{
		public ConstrainedLabelOverlay()
		{
			Text = "Label as window overlay";

			Padding = new Thickness(5, 0);
            TextColor = Colors.White;
            Background = Brush.DarkViolet;

			HorizontalOptions = LayoutOptions.Start;
			VerticalOptions = LayoutOptions.Start;
        }
	}
}