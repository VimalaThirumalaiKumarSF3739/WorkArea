namespace Syncfusion.Maui.Core.Hosting
{
    using System;
    using Android.Views;
    using Microsoft.Maui.Handlers;
    using Microsoft.Maui.Platform;
    using static Microsoft.Maui.Layouts.LayoutExtensions;
    using Android.Animation;
    using Android.Content;
    using Android.Widget;
    using AndroidX.Core.Widget;
    using Java.Lang;
    using PlatformView = Syncfusion.Maui.Core.Hosting.ExtMauiScrollView;
    using Android.Graphics;
    using Microsoft.Maui;
    using Syncfusion.Maui.Core.Internals;
    using System.Reflection;
    using System.Linq;

    internal partial class DataGridScrollViewHandler : ViewHandler<IScrollView, ExtMauiScrollView>, IScrollViewHandler
    {
        #region Fields

        const string InsetPanelTag = "MAUIContentInsetPanel";
        private HorizontalScrollView? m_nativeHorizontalScrollView;
        private OverScroller? m_horizontalScroller;
        private OverScroller? m_verticalScroller;
        private ExtMauiScrollView? m_mauiScrollView;

        #endregion

        #region Static Properties

        public static IPropertyMapper<IScrollView, IScrollViewHandler> Mapper = new PropertyMapper<IScrollView, IScrollViewHandler>(ViewMapper)
        {
            [nameof(IScrollView.Content)] = MapContent,
            [nameof(IScrollView.HorizontalScrollBarVisibility)] = MapHorizontalScrollBarVisibility,
            [nameof(IScrollView.VerticalScrollBarVisibility)] = MapVerticalScrollBarVisibility,
            [nameof(IScrollView.Orientation)] = MapOrientation,
        };

        public static CommandMapper<IScrollView, IScrollViewHandler> CommandMapper = new(ViewCommandMapper)
        {
            [nameof(IScrollView.RequestScrollTo)] = MapRequestScrollTo
        };

        #endregion

        #region Constructor

        public DataGridScrollViewHandler() : base(Mapper, CommandMapper)
        {

        }

        #endregion

        #region Override Properties

        IScrollView IScrollViewHandler.VirtualView => VirtualView;

        PlatformView IScrollViewHandler.PlatformView => PlatformView;

        #endregion

        #region Override Methods

        protected override ExtMauiScrollView CreatePlatformView()
        {
            m_mauiScrollView = new ExtMauiScrollView(this.Context, this.VirtualView);
            m_mauiScrollView.ChildViewAdded += ScrollView_ChildViewAdded;
            m_mauiScrollView.ClipToOutline = true;

            this.GetScroller(m_mauiScrollView!.Class!.Superclass!.CanonicalName, m_mauiScrollView, this.m_verticalScroller!);
            m_mauiScrollView.m_verticalScroller = this.m_verticalScroller;
            
            return m_mauiScrollView;
        }

        protected override void ConnectHandler(ExtMauiScrollView nativeView)
        {
            base.ConnectHandler(nativeView);
            nativeView.ScrollChange += ScrollChange;
        }

        protected override void DisconnectHandler(ExtMauiScrollView nativeView)
        {
            base.DisconnectHandler(nativeView);
            nativeView.ScrollChange -= ScrollChange;
        }

        public override Microsoft.Maui.Graphics.Size GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            var result = base.GetDesiredSize(widthConstraint, heightConstraint);

            if (FindInsetPanel(this) == null)
            {
                VirtualView.CrossPlatformMeasure(widthConstraint, heightConstraint);
            }

            return result;
        }

        #endregion

        #region Static Methods

        public static void MapContent(IScrollViewHandler handler, IScrollView scrollView)
        {
            if (handler.PlatformView == null || handler.MauiContext == null)
                return;

            if (NeedsInsetView(scrollView))
            {
                UpdateInsetView(scrollView, handler);
            }
            else
            {
                handler.PlatformView.UpdateContent(scrollView.PresentedContent, handler.MauiContext);
            }
        }

        public static void MapHorizontalScrollBarVisibility(IScrollViewHandler handler, IScrollView scrollView)
        {
            handler.PlatformView.SetHorizontalScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
        }

        public static void MapVerticalScrollBarVisibility(IScrollViewHandler handler, IScrollView scrollView)
        {
            handler.PlatformView.SetVerticalScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
        }

        public static void MapOrientation(IScrollViewHandler handler, IScrollView scrollView)
        {
            handler.PlatformView.SetOrientation(scrollView.Orientation);
        }

        public static void MapRequestScrollTo(IScrollViewHandler handler, IScrollView scrollView, object? args)
        {
            if (args is not ScrollToRequest request)
            {
                return;
            }

            var context = handler.PlatformView.Context;

            if (context == null)
            {
                return;
            }

#if NET6_0
            var horizontalOffsetDevice = (int)context.ToPixels(request.HoriztonalOffset);
#else
            var horizontalOffsetDevice = (int)context.ToPixels(request.HorizontalOffset);
#endif
            var verticalOffsetDevice = (int)context.ToPixels(request.VerticalOffset);

            handler.PlatformView.ScrollTo(horizontalOffsetDevice, verticalOffsetDevice,
                request.Instant, () => handler.VirtualView.ScrollFinished());
        }

        static bool NeedsInsetView(IScrollView scrollView)
        {
            if (scrollView.PresentedContent == null)
            {
                return false;
            }

            if (scrollView.Padding != Thickness.Zero)
            {
                return true;
            }

            if (scrollView.PresentedContent.Margin != Thickness.Zero)
            {
                return true;
            }

            return false;
        }

        static ContentViewGroup? FindInsetPanel(IScrollViewHandler handler)
        {
            return handler.PlatformView.FindViewWithTag(InsetPanelTag) as ContentViewGroup;
        }

        static void UpdateInsetView(IScrollView scrollView, IScrollViewHandler handler)
        {
            if (scrollView.PresentedContent == null || handler.MauiContext == null)
            {
                return;
            }

            var nativeContent = scrollView.PresentedContent.ToPlatform(handler.MauiContext);

            if (FindInsetPanel(handler) is ContentViewGroup currentPaddingLayer)
            {
                if (currentPaddingLayer.ChildCount == 0 || currentPaddingLayer.GetChildAt(0) != nativeContent)
                {
                    currentPaddingLayer.RemoveAllViews();
                    currentPaddingLayer.AddView(nativeContent);
                }
            }
            else
            {
                InsertInsetView(handler, scrollView, nativeContent);
            }
        }

        static void InsertInsetView(IScrollViewHandler handler, IScrollView scrollView, Android.Views.View nativeContent)
        {
            if (scrollView.PresentedContent == null || handler.MauiContext?.Context == null)
            {
                return;
            }

            var paddingShim = new ContentViewGroup(handler.MauiContext.Context)
            {
                Tag = InsetPanelTag
            };

            handler.PlatformView.RemoveAllViews();
            paddingShim.AddView(nativeContent);
            handler.PlatformView.SetContent(paddingShim);
        }

        static Func<double, double, Microsoft.Maui.Graphics.Size> IncludeScrollViewInsets(Func<double, double, Microsoft.Maui.Graphics.Size> internalMeasure, IScrollView scrollView)
        {
            return (widthConstraint, heightConstraint) =>
            {
                return InsetScrollView(widthConstraint, heightConstraint, internalMeasure, scrollView);
            };
        }

        static Microsoft.Maui.Graphics.Size InsetScrollView(double widthConstraint, double heightConstraint, Func<double, double, Microsoft.Maui.Graphics.Size> internalMeasure, IScrollView scrollView)
        {
            var padding = scrollView.Padding;

            if (scrollView.PresentedContent == null)
            {
                return new Microsoft.Maui.Graphics.Size(padding.HorizontalThickness, padding.VerticalThickness);
            }

            var measurementWidth = widthConstraint - padding.HorizontalThickness;
            var measurementHeight = heightConstraint - padding.VerticalThickness;

            var result = internalMeasure.Invoke(measurementWidth, measurementHeight);

            var fullSize = new Microsoft.Maui.Graphics.Size(result.Width + padding.HorizontalThickness, result.Height + padding.VerticalThickness);

            if (double.IsInfinity(widthConstraint))
            {
                widthConstraint = result.Width;
            }

            if (double.IsInfinity(heightConstraint))
            {
                heightConstraint = result.Height;
            }


            return fullSize.AdjustForFill(new Microsoft.Maui.Graphics.Rect(0, 0, widthConstraint, heightConstraint), scrollView.PresentedContent);
        }

        #endregion

        #region Helper Methods

        private void ScrollView_ChildViewAdded(object? sender, ViewGroup.ChildViewAddedEventArgs e)
        {
            if (e.Child is ExtMauiHorizontalScrollView && e.Child != this.m_nativeHorizontalScrollView)
            {
                this.m_nativeHorizontalScrollView = (ExtMauiHorizontalScrollView)e.Child;
                this.GetScroller("android.widget.HorizontalScrollView", this.m_nativeHorizontalScrollView, this.m_horizontalScroller!);

#pragma warning disable CA1416
                this.m_nativeHorizontalScrollView.ScrollChange += NativeHorizontalScrollView_ScrollChange;
#pragma warning restore CA1416

                this.m_mauiScrollView!.m_nativeHorizontalScrollView = this.m_nativeHorizontalScrollView;
                this.m_mauiScrollView.m_horizontalScroller = this.m_horizontalScroller;
            }
        }

        private void NativeHorizontalScrollView_ScrollChange(object? sender, Android.Views.View.ScrollChangeEventArgs e)
        {
            this.VirtualView.HorizontalOffset = Microsoft.Maui.Platform.ContextExtensions.FromPixels(this.Context, e.ScrollX);
        }


        private void GetScroller(string? assemblyname, Java.Lang.Object scrollview, OverScroller overscroller)
        {
            try
            {

                if (scrollview == null || scrollview.Handle == IntPtr.Zero)
                {
                    return;
                }

                Class parentClass = scrollview.Class;

                if (parentClass == null || parentClass.Handle == IntPtr.Zero)
                {
                    return;
                }

                do
                {
                    parentClass = parentClass.Superclass!;
                }
                while (!parentClass.Name.Equals(assemblyname));

                Java.Lang.Reflect.Field scrollerField = parentClass.GetDeclaredField("mScroller");

                if (scrollerField == null || scrollerField.Handle == IntPtr.Zero)
                {
                    return;
                }

                scrollerField.Accessible = true;
                if (overscroller == this.m_verticalScroller)
                {
                    this.m_verticalScroller = (OverScroller)scrollerField.Get(scrollview)!;
                }
                else if (overscroller == this.m_horizontalScroller)
                {
                    this.m_horizontalScroller = (OverScroller)scrollerField.Get(scrollview)!;
                }
            }
            catch (NoSuchFieldException e)
            {
                e.PrintStackTrace();
            }
            catch (IllegalArgumentException e)
            {
                e.PrintStackTrace();
            }
            catch (IllegalAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        void ScrollChange(object? sender, AndroidX.Core.Widget.NestedScrollView.ScrollChangeEventArgs e)
        {
            var context = (sender as Android.Views.View)?.Context;

            if (context == null)
            {
                return;
            }

            // Need to pass native horizontal scroll view's ScrollX position to the virtual view to resolve
            // the zero scroll offset issue since the framework returns the improper scroll x position. 
            var newScrollX = this.m_nativeHorizontalScrollView!.ScrollX;
            VirtualView.VerticalOffset = Context.FromPixels(e.ScrollY);
            VirtualView.HorizontalOffset = Context.FromPixels(newScrollX);
        }

        #endregion
    }

    internal class ExtMauiHorizontalScrollView : HorizontalScrollView, IExtScrollBarView
    {
        #region Internal Properties

        internal bool IsBidirectional;
        internal bool IsScrollingEnabled = true;
        internal ExtMauiScrollView? m_mauiScrollView;

        #endregion

        #region Constructor

        public ExtMauiHorizontalScrollView(Context? context, ExtMauiScrollView parentScrollView) : base(context)
        {
            m_mauiScrollView = parentScrollView;
        }

        #endregion

        #region Override Properties

        public bool ScrollBarsInitialized { get; set; } = false;

        #endregion

        #region Override Methods

        public override void Draw(Canvas? canvas)
        {
            try
            {
                if (canvas != null)
                    canvas.ClipRect(canvas.ClipBounds);

                base.Draw(canvas);
            }
            catch (Java.Lang.NullPointerException)
            {
                this.HandleScrollBarVisibilityChange();
            }
        }

        public override bool OnInterceptTouchEvent(MotionEvent? ev)
        {
            if (!IsScrollingEnabled)
                return false;

            if (ev == null || m_mauiScrollView == null)
                return false;

            if (IsBidirectional && ev.Action == MotionEventActions.Down)
            {
                m_mauiScrollView.LastY = ev.RawY;
                m_mauiScrollView.LastX = ev.RawX;
            }

            return base.OnInterceptTouchEvent(ev);
        }

        public override bool OnTouchEvent(MotionEvent? ev)
        {
            if (ev == null || m_mauiScrollView == null)
                return false;

            if (!m_mauiScrollView.Enabled)
                return false;

            m_mauiScrollView.ShouldSkipOnTouch = true;
            m_mauiScrollView.OnTouchEvent(ev);

            if (IsBidirectional)
            {
                float dY = m_mauiScrollView.LastY - ev.RawY;

                m_mauiScrollView.LastY = ev.RawY;
                m_mauiScrollView.LastX = ev.RawX;
                if (ev.Action == MotionEventActions.Move && m_mauiScrollView.IsScrollingEnabled)
                {
                    m_mauiScrollView.ScrollBy(0, (int)dY);				
                }
            }
            return base.OnTouchEvent(ev);
        }

        public override bool HorizontalScrollBarEnabled
        {
            get => base.HorizontalScrollBarEnabled;
            set
            {
                base.HorizontalScrollBarEnabled = value;
                this.HandleScrollBarVisibilityChange();
            }
        }

        void IExtScrollBarView.AwakenScrollBars()
        {
            base.AwakenScrollBars();
        }

        #endregion
    }

    public class ExtMauiScrollView : NestedScrollView, IExtScrollBarView
    {
        #region Fields

        internal bool ShouldSkipOnTouch;
        internal bool _isBidirectional;
        private bool isInitialLoad = true;

        Android.Views.View? _content;
        private Microsoft.Maui.IScrollView scrollView;
        private Android.OS.Handler? mainHandler;
        internal OverScroller? m_verticalScroller;
        internal OverScroller? m_horizontalScroller;
        internal HorizontalScrollView? m_nativeHorizontalScrollView;
        internal ExtMauiHorizontalScrollView? m_horizontalScrollView;

        ScrollOrientation _scrollOrientation = ScrollOrientation.Vertical;
        ScrollBarVisibility _defaultHorizontalScrollVisibility = 0;
        ScrollBarVisibility _defaultVerticalScrollVisibility = 0;
        ScrollBarVisibility _horizontalScrollVisibility = 0;

        #endregion

        #region Internal Properties

        internal float LastX { get; set; }

        internal float LastY { get; set; }

        internal bool IsScrollingEnabled { get; set; } = true;

        /// <summary>
        /// Gets the vertical scroll range.
        /// </summary>
        internal int VerticalScrollRange
        {
            get => this.ComputeVerticalScrollRange() - this.Height;
        }

        internal int HorizontalScrollRange
        {
            get => this.ComputeHorizontalScrollRange() - this.Width;
        }

        #endregion

        #region Constructor

        public ExtMauiScrollView(Context context, Microsoft.Maui.IScrollView scrollView) : base(context)
        {
            this.mainHandler = new Android.OS.Handler(Android.OS.Looper.MainLooper!);
            this.scrollView = scrollView;
        }

        #endregion

        #region Helper Methods

        public void SetHorizontalScrollBarVisibility(ScrollBarVisibility scrollBarVisibility)
        {
            _horizontalScrollVisibility = scrollBarVisibility;
            if (m_horizontalScrollView == null)
            {
                return;
            }

            if (_defaultHorizontalScrollVisibility == 0)
            {
                _defaultHorizontalScrollVisibility = m_horizontalScrollView.HorizontalScrollBarEnabled ? ScrollBarVisibility.Always : ScrollBarVisibility.Never;
            }

            if (scrollBarVisibility == ScrollBarVisibility.Default)
            {
                scrollBarVisibility = _defaultHorizontalScrollVisibility;
            }

            m_horizontalScrollView.HorizontalScrollBarEnabled = scrollBarVisibility == ScrollBarVisibility.Always;
        }

        public void SetVerticalScrollBarVisibility(ScrollBarVisibility scrollBarVisibility)
        {
            if (_defaultVerticalScrollVisibility == 0)
                _defaultVerticalScrollVisibility = VerticalScrollBarEnabled ? ScrollBarVisibility.Always : ScrollBarVisibility.Never;

            if (scrollBarVisibility == ScrollBarVisibility.Default)
                scrollBarVisibility = _defaultVerticalScrollVisibility;

            VerticalScrollBarEnabled = scrollBarVisibility == ScrollBarVisibility.Always;

            this.HandleScrollBarVisibilityChange();
        }

        public void SetContent(Android.Views.View content)
        {
            _content = content;
            SetOrientation(_scrollOrientation);
        }

        public void SetOrientation(ScrollOrientation orientation)
        {
            _scrollOrientation = orientation;

            if (orientation == ScrollOrientation.Horizontal || orientation == ScrollOrientation.Both)
            {
                if (m_horizontalScrollView == null)
                {
                    m_horizontalScrollView = new ExtMauiHorizontalScrollView(Context, this);
                    m_horizontalScrollView.HorizontalFadingEdgeEnabled = HorizontalFadingEdgeEnabled;
                    m_horizontalScrollView.SetFadingEdgeLength(HorizontalFadingEdgeLength);
                    SetHorizontalScrollBarVisibility(_horizontalScrollVisibility);
                }

                m_horizontalScrollView.IsBidirectional = _isBidirectional = orientation == ScrollOrientation.Both;

                if (m_horizontalScrollView.Parent != this)
                {
                    if (_content != null)
                    {
                        _content.RemoveFromParent();
                        m_horizontalScrollView.AddView(_content);
                    }

                    AddView(m_horizontalScrollView);
                }
            }
            else
            {
                if (_content != null && _content.Parent != this)
                {
                    _content.RemoveFromParent();
                    if (m_horizontalScrollView != null)
                        m_horizontalScrollView.RemoveFromParent();
                    AddView(_content);
                }
            }
        }


        private bool IsVerticalFling()
        {
            return this.m_verticalScroller != null && this.m_verticalScroller.Handle != IntPtr.Zero && !this.m_verticalScroller.IsFinished;
        }

        private bool IsHorizontalFling()
        {
            return this.m_horizontalScroller != null && this.m_horizontalScroller.Handle != IntPtr.Zero && !this.m_horizontalScroller.IsFinished;
        }

        public override void ComputeScroll()
        {
            if (this.IsHorizontalFling())
            {
                Runnable myRunnable = new Runnable(() =>
                {
                    if (this.m_horizontalScroller != null && this.m_horizontalScroller.Handle != IntPtr.Zero)
                    {
                        double finalXPosition = this.m_horizontalScroller.FinalX;
                        var maxScrollOffset = this.m_nativeHorizontalScrollView!.GetChildAt(0)!.Width - this.m_nativeHorizontalScrollView!.Width;
                        if (finalXPosition > maxScrollOffset)
                        {
                            finalXPosition = maxScrollOffset;
                        }
                        else if (finalXPosition < 0)
                        {
                            finalXPosition = 0;
                        }

                        if ((!this.m_horizontalScroller.IsFinished && finalXPosition == this.m_nativeHorizontalScrollView!.ScrollX) ||
                            (finalXPosition < this.m_nativeHorizontalScrollView!.ScrollX && this.m_horizontalScroller.FinalX > 0 && this.m_horizontalScroller.IsOverScrolled))
                        {
                            this.m_horizontalScroller.AbortAnimation();
                        }
                        else
                        {
                            this.m_nativeHorizontalScrollView.ScrollTo(this.m_horizontalScroller.CurrX, this.m_horizontalScroller.CurrY);
                            this.PostInvalidate();
                        }
                    }
                });

                this.mainHandler!.Post(myRunnable);
            }
            else if (this.IsVerticalFling())
            {
                if (this.m_verticalScroller != null && this.m_verticalScroller.Handle != IntPtr.Zero && !this.m_verticalScroller.ComputeScrollOffset())
                {
                    return;
                }

                Runnable myRunnable = new Runnable(() =>
                {
                    if (this.m_verticalScroller != null && this.m_verticalScroller.Handle != IntPtr.Zero)
                    {
                        double finalYPosition = this.m_verticalScroller.FinalY;
                        var maxScrollOffset = this.GetChildAt(0)!.Height - this.Height;
                        if (finalYPosition > maxScrollOffset)
                        {
                            finalYPosition = maxScrollOffset;
                        }
                        else if (finalYPosition < 0)
                        {
                            finalYPosition = 0;
                        }

                        if ((!this.m_verticalScroller.IsFinished && finalYPosition == this.ScrollY) ||
                            (finalYPosition < this.ScrollY && this.m_verticalScroller.FinalY > 0 && this.m_verticalScroller.IsOverScrolled))
                        {
                            this.m_verticalScroller.AbortAnimation();
                        }
                        else
                        {
                            this.ScrollTo(this.m_verticalScroller.CurrX, this.m_verticalScroller.CurrY);
                            this.PostInvalidate();
                        }
                    }
                });
                this.mainHandler!.Post(myRunnable);
            }
            else
            {
                if (this.isInitialLoad && this.LayoutDirection == Android.Views.LayoutDirection.Rtl)
                {
                    this.scrollView!.Content!.GetType().GetRuntimeProperties()!.FirstOrDefault(x => x.Name.Equals("HorizontalOffset"))!.SetValue(this.scrollView.Content, ContextExtensions.FromPixels(this.Context, this.m_nativeHorizontalScrollView!.ScrollX));
                    this.isInitialLoad = false;
                }
            }
        }

        public override bool OnInterceptTouchEvent(MotionEvent? ev)
        {
            if (!IsScrollingEnabled)
                return false;

            if (ev == null)
                return false;
		
            if (_isBidirectional && ev.Action == MotionEventActions.Down)
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

            if (_isBidirectional)
            {
                float dX = LastX - ev.RawX;

                LastY = ev.RawY;
                LastX = ev.RawX;
                if (ev.Action == MotionEventActions.Move && m_horizontalScrollView != null && m_horizontalScrollView.IsScrollingEnabled)
                {
                    m_horizontalScrollView.ScrollBy((int)dX, 0);
                }
            }

            return base.OnTouchEvent(ev);
        }

        void IExtScrollBarView.AwakenScrollBars()
        {
            base.AwakenScrollBars();
        }

        bool IExtScrollBarView.ScrollBarsInitialized { get; set; } = false;

        public void ScrollTo(int x, int y, bool instant, Action finished)
        {
            if (instant)
            {
                JumpTo(x, y, finished);
            }
            else
            {
                SmoothScrollTo(x, y, finished);
            }
        }

        void JumpTo(int x, int y, Action finished)
        {
            switch (_scrollOrientation)
            {
                case ScrollOrientation.Vertical:
                    ScrollTo(x, y);
                    break;
                case ScrollOrientation.Horizontal:
                    m_horizontalScrollView?.ScrollTo(x, y);
                    break;
                case ScrollOrientation.Both:
                    m_horizontalScrollView?.ScrollTo(x, y);
                    ScrollTo(x, y);
                    break;
                case ScrollOrientation.Neither:
                    break;
            }

            finished();
        }

        static int GetDistance(double start, double position, double v)
        {
            return (int)(start + (position - start) * v);
        }

        void SmoothScrollTo(int x, int y, Action finished)
        {
            int currentX = _scrollOrientation == ScrollOrientation.Horizontal || _scrollOrientation == ScrollOrientation.Both ? m_horizontalScrollView!.ScrollX : ScrollX;
            int currentY = _scrollOrientation == ScrollOrientation.Vertical || _scrollOrientation == ScrollOrientation.Both ? ScrollY : m_horizontalScrollView!.ScrollY;

            ValueAnimator? animator = ValueAnimator.OfFloat(0f, 1f);
            animator!.SetDuration(1000);

            animator.Update += (o, animatorUpdateEventArgs) =>
            {
                var v = (double)(animatorUpdateEventArgs.Animation!.AnimatedValue!);
                int distX = GetDistance(currentX, x, v);
                int distY = GetDistance(currentY, y, v);

                switch (_scrollOrientation)
                {
                    case ScrollOrientation.Horizontal:
                        m_horizontalScrollView?.ScrollTo(distX, distY);
                        break;
                    case ScrollOrientation.Vertical:
                        ScrollTo(distX, distY);
                        break;
                    default:
                        m_horizontalScrollView?.ScrollTo(distX, distY);
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

        #endregion
    }

    public static class ScrollViewExtensions
    {
        #region Static Methods

        internal static void HandleScrollBarVisibilityChange(this IExtScrollBarView scrollView)
        {
            if (scrollView.ScrollBarsInitialized)
                scrollView.AwakenScrollBars();

            if (!scrollView.ScrollbarFadingEnabled)
            {
                scrollView.ScrollbarFadingEnabled = true;
                scrollView.AwakenScrollBars();
                scrollView.ScrollbarFadingEnabled = false;
            }
            else
            {
                scrollView.AwakenScrollBars();
            }

            scrollView.ScrollBarsInitialized = true;
        }

        public static void UpdateContent(this ExtMauiScrollView scrollView, IView? content, IMauiContext context)
        {
            var nativeContent = content == null ? null : content.ToPlatform(context);

            scrollView.RemoveAllViews();

            if (nativeContent != null)
            {
                scrollView.SetContent(nativeContent);

            }
        }

        #endregion
    }

    internal interface IExtScrollBarView
    {
        #region Properties

        bool ScrollBarsInitialized { get; set; }

        bool ScrollbarFadingEnabled { get; set; }

        #endregion

        #region

        void AwakenScrollBars();

        #endregion
    }
}