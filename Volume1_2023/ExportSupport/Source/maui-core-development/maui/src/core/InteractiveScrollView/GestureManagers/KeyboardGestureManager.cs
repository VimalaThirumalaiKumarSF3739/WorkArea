namespace Syncfusion.Maui.Core.Internals
{
    internal class KeyboardGestureManager : IKeyboardListener
    {
        SfInteractiveScrollView m_scrollView;

        internal KeyboardGestureManager(SfInteractiveScrollView scrollView)
        {
            m_scrollView = scrollView;
            m_scrollView.AddKeyboardListener(this);
        }

        public void OnKeyDown(KeyEventArgs args)
        {
            if (m_scrollView == null)
                return;

            float scrollDelta = 60;
            switch (args.Key)
            {
                case KeyboardKey.Down:
                    m_scrollView.ScrollToY(m_scrollView.ScrollY + scrollDelta, true);
                    break;
                case KeyboardKey.Up:
                    m_scrollView.ScrollToY(m_scrollView.ScrollY - scrollDelta, true);
                    break;
                case KeyboardKey.Left:
                    m_scrollView.ScrollToX(m_scrollView.ScrollX - scrollDelta, true);
                    break;
                case KeyboardKey.Right:
                    m_scrollView.ScrollToX(m_scrollView.ScrollX + scrollDelta, true);
                    break;
                case KeyboardKey.Home:
                    if (args.IsShiftKeyPressed)
                        m_scrollView.ScrollToX(0, true);
                    else
                        m_scrollView.ScrollToY(0, true);
                    break;
                case KeyboardKey.End:
                    if (args.IsShiftKeyPressed)
                        m_scrollView.ScrollToX(m_scrollView.ContentSize.Width, true);
                    else
                        m_scrollView.ScrollToY(m_scrollView.ContentSize.Height, true);
                    break;
                case KeyboardKey.PageUp:
                    if (args.IsShiftKeyPressed)
                        m_scrollView.ScrollToX(m_scrollView.ScrollX - m_scrollView.ViewportWidth, true);
                    else
                        m_scrollView.ScrollToY(m_scrollView.ScrollY - m_scrollView.ViewportHeight, true);
                    break;
                case KeyboardKey.PageDown:
                    if (args.IsShiftKeyPressed)
                        m_scrollView.ScrollToX(m_scrollView.ScrollX + m_scrollView.ViewportWidth, true);
                    else
                        m_scrollView.ScrollToY(m_scrollView.ScrollY + m_scrollView.ViewportHeight, true);
                    break;
            }
        }

        public void OnKeyUp(KeyEventArgs args)
        {
            // The method implemented as a part of the interface.
        }

        internal void Dispose()
        {
            m_scrollView?.RemoveKeyboardListener(this);
        }
    }
}
