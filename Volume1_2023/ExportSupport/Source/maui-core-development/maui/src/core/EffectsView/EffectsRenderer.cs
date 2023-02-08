using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Maui.Controls.Internals.GIFBitmap;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal class EffectsRenderer :  ITouchListener
    {
        #region Fields

        private readonly IDrawable? drawable;

        private ICanvas? currentCanvas;

        private bool shouldDrawHighlight =false;

        private bool shouldDrawRipple = false;
        
        private readonly HighlightEffectLayer? highlightEffectLayer;

        private readonly RippleEffectLayer? rippleEffectLayer;

        private RectF rippleBounds;

        private RectF highlightBounds;

        private ObservableCollection<RectF> rippleBoundsCollection = new();

        private ObservableCollection<RectF> highlightBoundsCollection = new();
        
        private Brush rippleColorBrush = new SolidColorBrush(Color.FromRgba(150, 150, 150, 75));
        
        private Brush highlightBrush = new SolidColorBrush(Color.FromRgba(200, 200, 200, 50));

        #endregion

        #region Properties



        public Brush HighlightBrush
        {
            get
            {
                return highlightBrush;
            }
            set
            {
                highlightBrush = value;
            }
        }



        public Brush RippleColorBrush
        {
            get
            {
                return rippleColorBrush;
            }
            set
            {
                rippleColorBrush = value;
            }
        }


        public double RippleAnimationDuration { get; set; }

        public double RippleStart { get; set; }


        internal bool ShouldDrawHighlight
        {
            get
            {
                return shouldDrawHighlight;
            }
            private set
            {
                if (ShouldDrawHighlight != value)
                {
                    shouldDrawHighlight = value;
                    this.InvalidateDrawable();
                }
            }
        }

        internal bool ShouldDrawRipple
        {
            get
            {
                return shouldDrawRipple;
            }
            private set
            {
                if (shouldDrawRipple != value)
                {
                    shouldDrawRipple = value;
                    this.InvalidateDrawable();
                }
            }

        }

        internal RectF RippleBounds
        {
            get
            {
                return rippleBounds;
            }
            private set
            {
                if (rippleBounds != value)
                {
                    rippleBounds = value;
                    InvalidateDrawable();
                }
            }
        }



        internal RectF HighlightBounds
        {
            get
            {
                return highlightBounds;
            }
            private set
            {
                if (highlightBounds != value)
                {
                    highlightBounds = value;
                    this.InvalidateDrawable();
                }
            }
        }

        public ObservableCollection<RectF> RippleBoundsCollection
        {
            get
            {
                return rippleBoundsCollection;
            }
            set
            {
                rippleBoundsCollection = value;
            }
        }


        public ObservableCollection<RectF> HighlightBoundsCollection
        {
            get
            {
                return highlightBoundsCollection;
            }
            set
            {
                highlightBoundsCollection = value;
            }
        }

        #endregion

        #region Constructor


        /// <summary>
        /// 
        /// </summary>
        public EffectsRenderer(View drawableView)
        {
            drawableView.AddTouchListener(this);

            if(drawableView is IDrawable dView)
            {
                this.drawable = dView;
                this.rippleEffectLayer = new RippleEffectLayer(RippleColorBrush, 1000, dView, drawableView);
                this.highlightEffectLayer = new HighlightEffectLayer(this.HighlightBrush, dView);
            }
        }

        #endregion

        #region Methods

        internal void DrawEffects(ICanvas canvas)
        {
            this.currentCanvas = canvas;
            if(HighlightBounds.Width > 0 && HighlightBounds.Height > 0)
                this.DrawHighlight(HighlightBounds);
            if (RippleBounds.Width > 0 && RippleBounds.Height > 0)
                this.DrawRipple(RippleBounds);
        }

        private void DrawRipple(RectF rectF)
        {
            if (this.ShouldDrawRipple && this.rippleEffectLayer != null &&this. currentCanvas != null)
                rippleEffectLayer.DrawRipple(currentCanvas, rectF, this.RippleColorBrush, true);
        }

        private void DrawHighlight(RectF rectF)
        {
           if (this.ShouldDrawHighlight && this.highlightEffectLayer != null && this.currentCanvas != null)
                highlightEffectLayer.DrawHighlight(currentCanvas,rectF, this.HighlightBrush);
        }

        private void InvalidateDrawable()
        {
            if (drawable is IDrawableLayout drawableLayout)
                drawableLayout.InvalidateDrawable();
            else if (drawable is IDrawableView drawableView)
                drawableView.InvalidateDrawable();
        }

        public void OnTouch(PointerEventArgs e)
        {
            if (e.Action == PointerActions.Moved)
            {
                this.CheckBoundsContainsPoint(e.TouchPoint, HighlightBoundsCollection, false);
            }
            else if (e.Action == PointerActions.Pressed)
            {
                this.CheckBoundsContainsPoint(e.TouchPoint, RippleBoundsCollection, true);
                if (this.ShouldDrawRipple && this.rippleEffectLayer != null)
                {
                    this.rippleEffectLayer.StartRippleAnimation(e.TouchPoint, this.RippleColorBrush, 2000, 0f, true);
                }
                else
                {
                    if (this.rippleBounds.Width > 0 && this.rippleBounds.Height > 0)
                        this.RemoveRipple();
                }
            }
            else if (e.Action == PointerActions.Released)
            {
                if (this.rippleBounds.Width > 0 && this.rippleBounds.Height > 0)
                    this.RemoveRipple();
#if ANDROID
                if (this.highlightBounds.Width > 0 && this.highlightBounds.Height > 0)
                    this.RemoveHighlight();
#endif
            }
            else if (e.Action == PointerActions.Cancelled || e.Action == PointerActions.Exited)
            {
                this.ShouldDrawHighlight = false;
                this.ShouldDrawRipple = false;
                if (this.rippleBounds.Width > 0 && this.rippleBounds.Height > 0)
                    this.RemoveRipple();
                if (this.highlightBounds.Width > 0 && this.highlightBounds.Height > 0)
                    this.RemoveHighlight();
            }
        }

        private void RemoveRipple()
        {
            if (this.rippleEffectLayer != null)
            {
                this.rippleEffectLayer.OnRippleAnimationFinished();
            }
        }

        private void RemoveHighlight()
        {
            if (this.highlightEffectLayer != null)
            {
                this.HighlightBounds = new RectF(0, 0, 0, 0);
                this.highlightEffectLayer.UpdateHighlightBounds();
            }
        }

        private void CheckBoundsContainsPoint(Point p, ObservableCollection<RectF> bounds,bool isRipple )
        {
            foreach (var item in bounds)
            {
                if (item.Contains(p))
                {
                    if (isRipple)
                    {
                        this.RippleBounds = item;
                        this.ShouldDrawRipple = true;
                    }
                    else
                    {
                        this.HighlightBounds = item;
                        this.ShouldDrawHighlight = true;
                    }
                    return;
                }
            }

            if (isRipple)
            {
                this.ShouldDrawRipple = false;
            }
            else
            {
                this.ShouldDrawHighlight = false;
            }

        }

        #endregion
    }
}
