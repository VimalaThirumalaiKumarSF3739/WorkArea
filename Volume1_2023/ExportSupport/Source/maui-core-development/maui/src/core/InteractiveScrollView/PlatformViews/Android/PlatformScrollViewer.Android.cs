using Android.Animation;
using Android.Content;
using Android.Views;
using Android.Widget;
using Microsoft.Maui;
using System;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// Represents the scroll viewer that can scroll both horizontally and vertically.
    /// </summary>
    /// <remarks>
    /// Horizontal scrollbar will be appears only at the bottom of the content.
    /// </remarks>
    internal partial class PlatformScrollViewer : ScrollView
    {
        HorizontalScrollViewer? m_horizontalScrollViewer;
        int m_viewPortWidth;
        int m_viewPortHeight;
        bool m_isBidirectional = true;

        internal float LastX { get; set; } = 0;
        internal float LastY { get; set; } = 0;
        internal bool ShouldSkipOnTouch { get; set; }
        internal bool IsScrollingEnabled { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of PlatformScrollViewer class.
        /// </summary>
        /// <param name="context"></param>
        /// <remarks>
        /// Since the horizontal scrollbar will be visible only at the bottom,
        /// it is not recommended to set "ScrollbarStyles.InsideInset" which might provide wrong viewport size,
        /// because it occupies some size for the scrollbar with the scroll view and we have remark with horizontal scrollbar.
        /// </remarks>
        internal PlatformScrollViewer(Context? context) : base(context)
        {
            // ClipToOutline - This property is needed to clip the contents which are scrolled out of the viewport.
            // If false, the contents will not be clipped and visible over the other controls.
            // This can be checked by adding the scroll view as one of the children of another View Group (Linear layout).
            this.ClipToOutline = true;
            if (OperatingSystem.IsAndroidVersionAtLeast(23)) 
                this.ScrollChange += VerticalScrollChange;
            this.LayoutChange += OnLayoutChange;
        }

        internal int ViewPortHeight
        {
            get
            {
                return m_viewPortHeight;
            }
        }

        internal int ViewPortWidth
        {
            get
            {
                return m_viewPortWidth;
            }
        }

        internal event EventHandler<ScrollChangeEventArgs>? ScrollChanged;

        internal void SetVerticalScrollingEnabled(bool isEnabled)
        {
            IsScrollingEnabled = isEnabled;
            this.SetVerticalScrollBarEnabled(false);
        }

        internal void SetHorizontalScrollingEnabled(bool isEnabled)
        {
            if (m_horizontalScrollViewer != null)
            {
                m_horizontalScrollViewer.IsScrollingEnabled = isEnabled;
                this.SetHorizontalScrollBarEnabled(false);
            }
        }

        public override bool OnInterceptTouchEvent(MotionEvent? ev)
        {
            if (!IsScrollingEnabled)
                return false;

            if (ev == null)
                return false;

            // set the start point for the bidirectional scroll; 
            if (m_isBidirectional && ev.Action == MotionEventActions.Down)
            {
                LastY = ev.RawY;
                LastX = ev.RawX;
            }
            return base.OnInterceptTouchEvent(ev);
        }

        public override bool OnTouchEvent(MotionEvent? ev)
        {
            if (ev == null || !Enabled)
                return false;

            if (ShouldSkipOnTouch)
            {
                ShouldSkipOnTouch = false;
                return false;
            }

            if (m_isBidirectional)
            {
                float dX = LastX - ev.RawX;

                LastY = ev.RawY;
                LastX = ev.RawX;
                if (ev.Action == MotionEventActions.Move && m_horizontalScrollViewer != null && m_horizontalScrollViewer.IsScrollingEnabled)
                {
                    m_horizontalScrollViewer.ScrollBy((int)dX, 0);
                }
            }
            return base.OnTouchEvent(ev);
        }

        private void OnLayoutChange(object? sender, LayoutChangeEventArgs e)
        {
            // View port size is equal to the size of the control, because there is no impact of the scrollbars size in viewport, as the scrollbars are present overlay the control.
            if (e.OldLeft != e.Left || e.OldTop != e.Top || e.OldRight != e.Right || e.OldBottom != e.Bottom)
            {
                m_viewPortHeight = this.ComputeVerticalScrollExtent();
                m_viewPortWidth = this.ComputeHorizontalScrollExtent();
            }
        }

        internal void SetVerticalScrollBarFadingEnabled(bool enabled)
        {
            this.ScrollbarFadingEnabled = enabled;
            this.Invalidate();
        }

        internal void SetVerticalScrollBarEnabled(bool enabled)
        {
            this.VerticalScrollBarEnabled = enabled;
            this.Invalidate();
        }

        internal void SetHorizontalScrollBarFadingEnabled(bool enabled)
        {
            if (m_horizontalScrollViewer != null)
            {
                m_horizontalScrollViewer.ScrollbarFadingEnabled = enabled;
                m_horizontalScrollViewer.Invalidate();
            }
        }

        internal void SetHorizontalScrollBarEnabled(bool enabled)
        {
            if (m_horizontalScrollViewer != null)
            {
                m_horizontalScrollViewer.HorizontalScrollBarEnabled = enabled;
                m_horizontalScrollViewer.Invalidate();
            }
        }

        private void VerticalScrollChange(object? sender, ScrollChangeEventArgs e)
        {
            if (m_horizontalScrollViewer != null)
                ExecuteScrollChangeEvent(m_horizontalScrollViewer.ScrollX, e.ScrollY, m_horizontalScrollViewer.ScrollX, e.OldScrollY);
            else
                ExecuteScrollChangeEvent(e.ScrollX, e.ScrollY, e.ScrollX, e.OldScrollY);
        }

        private void HorizontalScrollChange(object? sender, ScrollChangeEventArgs e)
        {
            ExecuteScrollChangeEvent(e.ScrollX, this.ScrollY, e.OldScrollX, this.ScrollY);
        }

        internal void SetContent(View view)
        {
            if (m_horizontalScrollViewer == null)
            {
                m_horizontalScrollViewer = new HorizontalScrollViewer(Context)
                {
                    ParentScrollView = this
                };
                if (OperatingSystem.IsAndroidVersionAtLeast(23))
                    m_horizontalScrollViewer.ScrollChange += HorizontalScrollChange;
                this.AddView(m_horizontalScrollViewer, ViewGroup.LayoutParams.MatchParent);
                this.FillViewport = true;
            }
            m_horizontalScrollViewer?.SetContent(view);
        }

        internal void DisconnectViews()
        {
            m_horizontalScrollViewer?.RemoveAllViews();
            this.RemoveAllViews();
        }

        internal void ScrollToOffset(int x, int y, bool animated, ScrollOrientation scrollOrientation, Action finished)
        {
            x = GetMaxHorizontalScrollOffset(x);
            y = GetMaxVerticalScrollOffset(y);
            if (!animated)
            {
                JumpToOffset(x, y, scrollOrientation, finished);
            }
            else
            {
                SmoothScrollToOffset(x, y, scrollOrientation, finished);
            }
        }

        private int GetMaxVerticalScrollOffset(int offSet)
        {
            if (offSet < 0)
                return -1;
            if (offSet > 0)
            {
                int maxScrollOffset = ComputeVerticalScrollRange() - ViewPortHeight;
                return offSet <= maxScrollOffset ? offSet : maxScrollOffset;
            }
            else
                return offSet;
        }

        private int GetMaxHorizontalScrollOffset(int offSet)
        {
            if (offSet < 0)
                return -1;
            if (m_horizontalScrollViewer != null && offSet > 0)
            {
                int maxScrollOffset = m_horizontalScrollViewer.GetHorizontalScrollRange() - ViewPortWidth;
                return offSet <= maxScrollOffset ? offSet : maxScrollOffset;
            }
            else
                return offSet;
        }

        void JumpToOffset(int x, int y, ScrollOrientation scrollOrientation, Action finished)
        {
            switch (scrollOrientation)
            {
                case ScrollOrientation.Vertical:
                    ScrollTo(x, y);
                    break;
                case ScrollOrientation.Horizontal:
                    m_horizontalScrollViewer?.ScrollTo(x, y);
                    break;
                case ScrollOrientation.Both:
                    UnwireScrollEvents();
                    if (m_horizontalScrollViewer != null)
                    {
                        int oldScrollX = m_horizontalScrollViewer.ScrollX;
                        int oldScrollY = ScrollY;
                        m_horizontalScrollViewer?.ScrollTo(x, y);
                        ScrollTo(x, y);
                        ExecuteScrollChangeEvent(x, y, oldScrollX, oldScrollY);
                    }
                    WireScrollEvents();
                    break;
                case ScrollOrientation.Neither:
                    break;
            }

            finished();
        }

        void ExecuteScrollChangeEvent(int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            ScrollChangeEventArgs eventArgs = new ScrollChangeEventArgs(this, scrollX, scrollY, oldScrollX, oldScrollY);
            OnScrollChanged(eventArgs);
        }

        void UnwireScrollEvents()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(23))
            {
                if (m_horizontalScrollViewer != null)
                    m_horizontalScrollViewer.ScrollChange -= HorizontalScrollChange;
                this.ScrollChange -= VerticalScrollChange;
            }
        }

        void WireScrollEvents()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(23))
            {
                if (m_horizontalScrollViewer != null)
                    m_horizontalScrollViewer.ScrollChange += HorizontalScrollChange;
                this.ScrollChange += VerticalScrollChange;
            }
        }

        void SmoothScrollToOffset(int x, int y, ScrollOrientation scrollOrientation, Action finished)
        {
            int currentX = scrollOrientation == ScrollOrientation.Horizontal || scrollOrientation == ScrollOrientation.Both ? m_horizontalScrollViewer!.ScrollX : ScrollX;
            int currentY = scrollOrientation == ScrollOrientation.Vertical || scrollOrientation == ScrollOrientation.Both ? ScrollY : m_horizontalScrollViewer!.ScrollY;

            ValueAnimator? animator = ValueAnimator.OfFloat(0f, 1f);
            animator!.SetDuration(1000);

            animator.Update += (o, animatorUpdateEventArgs) =>
            {
                var animatedValue = (double)(animatorUpdateEventArgs.Animation!.AnimatedValue!);
                int distX = GetDistance(currentX, x, animatedValue);
                int distY = GetDistance(currentY, y, animatedValue);

                switch (scrollOrientation)
                {
                    case ScrollOrientation.Horizontal:
                        m_horizontalScrollViewer?.ScrollTo(distX, distY);
                        break;
                    case ScrollOrientation.Vertical:
                        ScrollTo(distX, distY);
                        break;
                    default:
                        m_horizontalScrollViewer?.ScrollTo(distX, distY);
                        ScrollTo(distX, distY);
                        break;
                }
            };

            animator.AnimationEnd += delegate
            {
                finished();
            };

            animator.Start();
        }

        int GetDistance(double start, double position, double animatedValue)
        {
            return (int)(start + (position - start) * animatedValue);
        }

        internal void OnScrollChanged(ScrollChangeEventArgs args)
        {
            ScrollChanged?.Invoke(this, args);
        }
    }
}