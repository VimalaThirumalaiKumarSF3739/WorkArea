using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// Manages the pan interactions of <see cref="SfInteractiveScrollView"/> control.
    /// </summary>
    /// <remarks>
    /// The pan handling is currently applicable only for Windows, as for iOS/Android pan was handled in the platform level.
    /// </remarks>
    internal class PanGestureManager
    {
        Point? m_translationPositionAtStart = null;
        Point m_totalTranslatedPosition = Point.Zero;
        Point m_scrollOffsetAtStart = Point.Zero;

        PanZoomListener m_panListener;
        SfInteractiveScrollView m_scrollView;

        internal PanGestureManager(SfInteractiveScrollView scrollView, PanZoomListener panListener)
        {
            m_scrollView = scrollView;
            m_panListener = panListener;
            SubscribePanEvents();
        }

        void SubscribePanEvents()
        {
            if (m_panListener != null)
            {
                m_panListener.PanUpdated += OnPanUpdated;
            }
        }

        internal void OnPanUpdated(object? sender, PanEventArgs e)
        {
            if (m_scrollView == null)
                return;
            View? content = m_scrollView.Content;
            if (content != null)
            {
                if (e.Status == GestureStatus.Started && !m_scrollView.IsScrolling)
                {
                    m_translationPositionAtStart = new Point(content.TranslationX, content.TranslationY);
                    m_scrollOffsetAtStart.X = m_scrollView.ScrollX;
                    m_scrollOffsetAtStart.Y = m_scrollView.ScrollY;
                }
                if (e.Status == GestureStatus.Running && m_translationPositionAtStart != null)
                {
                    m_totalTranslatedPosition.X += e.TranslatePoint.X;
                    m_totalTranslatedPosition.Y += e.TranslatePoint.Y;
                    if (!m_scrollView.IsScrolling)
                    {
                        if (e.TranslatePoint.X != 0 || e.TranslatePoint.Y != 0)
                        {
                            if (m_scrollView.ContentSize.Width >= m_scrollView.Width)
                                content.TranslationX = m_translationPositionAtStart.Value.X +
                                System.Math.Clamp(m_totalTranslatedPosition.X, m_scrollOffsetAtStart.X + m_scrollView.Width - m_scrollView.ContentSize.Width,
                                m_scrollOffsetAtStart.X);

                            if (m_scrollView.ContentSize.Height >= m_scrollView.Height)
                                content.TranslationY = m_translationPositionAtStart.Value.Y +
                                System.Math.Clamp(m_totalTranslatedPosition.Y, m_scrollOffsetAtStart.Y + m_scrollView.Height - m_scrollView.ContentSize.Height,
                                m_scrollOffsetAtStart.Y);

                            ScrollChangedEventArgs eventArgs = new ScrollChangedEventArgs(
                                m_scrollOffsetAtStart.X + (m_translationPositionAtStart.Value.X - content.TranslationX),
                                m_scrollOffsetAtStart.Y + (m_translationPositionAtStart.Value.Y - content.TranslationY),
                                m_scrollView.ScrollX, m_scrollView.ScrollY);
                            m_scrollView.OnScrollChanged(eventArgs);
                        }
                    }
                }
                if (e.Status == GestureStatus.Completed && m_translationPositionAtStart != null)
                {
                    m_scrollView.ScrollTo(m_scrollOffsetAtStart.X + (m_translationPositionAtStart.Value.X - content.TranslationX),
                        m_scrollOffsetAtStart.Y + (m_translationPositionAtStart.Value.Y - content.TranslationY), false);
                    content.TranslationX = m_translationPositionAtStart.Value.X;
                    content.TranslationY = m_translationPositionAtStart.Value.Y;
                    ResetValues();
                }
            }
        }

        void ResetValues()
        {
            m_translationPositionAtStart = null;
            m_totalTranslatedPosition = Point.Zero;
            m_scrollOffsetAtStart = Point.Zero;
        }

        void UnsubscribePanEvents()
        {
            if (m_panListener != null)
            {
                m_panListener.PanUpdated -= OnPanUpdated;
            }
        }

        internal void Dispose()
        {
            UnsubscribePanEvents();
        }
    }
}