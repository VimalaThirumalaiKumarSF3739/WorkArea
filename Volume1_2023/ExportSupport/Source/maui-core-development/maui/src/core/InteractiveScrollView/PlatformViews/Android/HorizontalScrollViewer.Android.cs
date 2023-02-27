using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace Syncfusion.Maui.Core.Internals
{
    internal class HorizontalScrollViewer : HorizontalScrollView
    {
        bool m_isBidirectional = true;
        internal PlatformScrollViewer? ParentScrollView;
        internal bool IsScrollingEnabled = true;

        internal HorizontalScrollViewer(Context? context) : base(context)
        {
            // If ClipToOutline is false, the contents will not be clipped and visible over the other controls.
            this.ClipToOutline = true;
        }

        public override bool OnInterceptTouchEvent(MotionEvent? ev)
        {
            if (!IsScrollingEnabled)
                return false;

            if (ev == null || ParentScrollView == null)
                return false;

            if (m_isBidirectional && ev.Action == MotionEventActions.Down)
            {
                ParentScrollView.LastY = ev.RawY;
                ParentScrollView.LastX = ev.RawX;
            }
            return base.OnInterceptTouchEvent(ev);
        }

        public override bool OnTouchEvent(MotionEvent? ev)
        {
            if (ev == null || ParentScrollView == null)
                return false;

            if (!ParentScrollView.Enabled)
                return false;

            // If the touch is caught by the horizontal scrollview, forward it to the parent 
            ParentScrollView.ShouldSkipOnTouch = true;
            ParentScrollView.OnTouchEvent(ev);

            if (m_isBidirectional)
            {
                float dY = ParentScrollView.LastY - ev.RawY;

                ParentScrollView.LastY = ev.RawY;
                ParentScrollView.LastX = ev.RawX;
                if (ev.Action == MotionEventActions.Move && ParentScrollView.IsScrollingEnabled)
                {
                    // Handle X scrolling when on bidirectional scrolling.
                    ParentScrollView.ScrollBy(0, (int)dY); 					
                }
            }
            return base.OnTouchEvent(ev);
        }

        internal void SetContent(View view)
        {
            if (this.ChildCount == 0)
            {
                this.AddView(view);
                this.FillViewport = true;
            }
        }

        internal int GetHorizontalScrollRange()
        {
            return this.ComputeHorizontalScrollRange();
        }
    }
}
