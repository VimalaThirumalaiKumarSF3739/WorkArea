using System;

namespace WindowOverlay.Samples
{
	public class FrameworkWindowOverlay : Microsoft.Maui.WindowOverlay
    {
		public FrameworkWindowOverlay(IWindow window) : base(window)
		{
			AddWindowElement(new WindowOverlayElement());
		}
	}

    public class WindowOverlayElement : IWindowOverlayElement
    {
        public bool Contains(Point point)
        {
            return true;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.Red;
            canvas.FillRectangle(new Rect(dirtyRect.X, 100, 100, 100));
        }
    }
}

