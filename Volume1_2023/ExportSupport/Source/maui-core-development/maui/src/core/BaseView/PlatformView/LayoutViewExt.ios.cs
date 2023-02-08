
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Color = Microsoft.Maui.Graphics.Color;
using Microsoft.Maui.Platform;
using CoreGraphics;
using UIKit;
using System;
using CoreAnimation;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Foundation;
using Syncfusion.Maui.Core.Internals;

namespace Syncfusion.Maui.Core.Platform
{
    /// <summary>
    /// 
    /// </summary>
    internal class LayoutViewExt : LayoutView
    {
        private IGraphicsRenderer? _renderer;
        private CGColorSpace? _colorSpace;
        private IDrawable? _drawable;
        private CGRect _lastBounds;
        private DrawingOrder drawingOrder = DrawingOrder.NoDraw;
        private PlatformGraphicsView? nativeGraphicsView;
        private readonly View? mauiView;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawable"></param>
        /// <param name="renderer"></param>
        public LayoutViewExt(IDrawable? drawable = null, IGraphicsRenderer? renderer = null)
        {
            Drawable = drawable;
            mauiView = (View)drawable!;
            Renderer = renderer;
            BackgroundColor = UIColor.Clear;
            this.Opaque = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aPtr"></param>
        public LayoutViewExt(IntPtr aPtr)
        {
            BackgroundColor = UIColor.Clear;
        }

        /// <summary>
        /// Returns a boolean value indicating whether this object can become the first responder.
        /// </summary>
        public override bool CanBecomeFirstResponder => (mauiView! is IKeyboardListener) && (mauiView! as IKeyboardListener)!.CanBecomeFirstResponder;

        internal Func<double, double, Size>? CrossPlatformMeasure { get; set; }
        internal Func<Rect, Size>? CrossPlatformArrange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal DrawingOrder DrawingOrder
        {
            get
            {
                return this.drawingOrder;
            }
            set
            {
                this.drawingOrder = value;
                this.InitializeNativeGraphicsView();
                this.SetNeedsDisplay();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal IGraphicsRenderer? Renderer
        {
            get => _renderer;

            set
            {
                this.UpdateRenderer(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal IDrawable? Drawable
        {
            get => _drawable;
            set
            {
                _drawable = value;
                if (_renderer != null)
                {
                    _renderer.Drawable = _drawable;
                    _renderer.Invalidate();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateRenderer(IGraphicsRenderer? graphicsRenderer = null)
        {
            if(_renderer != null)
            {
                _renderer.Drawable = null;
                _renderer.GraphicsView = null;
                _renderer.Dispose();
                _renderer = null;
            }

            if(this.DrawingOrder == DrawingOrder.BelowContent)
            {
                _renderer = graphicsRenderer ?? new DirectRenderer();

                _renderer.GraphicsView = new PlatformGraphicsView();
                _renderer.Drawable = this.Drawable;
                _renderer.SizeChanged((float)Bounds.Width, (float)Bounds.Height);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void InvalidateDrawable()
        {
            _renderer?.Invalidate();
            this.nativeGraphicsView?.InvalidateDrawable();
            this.SetNeedsDisplay();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void InvalidateDrawable(float x, float y, float w, float h)
        {
            _renderer?.Invalidate(x, y, w, h);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void InitializeNativeGraphicsView()
        {
            this.UpdateRenderer();
            if (this.DrawingOrder == DrawingOrder.AboveContent || this.DrawingOrder == DrawingOrder.AboveContentWithTouch)
            {
                if (nativeGraphicsView == null)
                {
                    nativeGraphicsView = new PlatformGraphicsView
                    {
                        BackgroundColor = UIColor.Clear,
                        Drawable = this.Drawable
                    };
                    if (this.DrawingOrder == DrawingOrder.AboveContent)
                    {
                        this.nativeGraphicsView.UserInteractionEnabled = false;
                    }
                }

                this.Add(nativeGraphicsView);
            }
            else if (nativeGraphicsView != null)
            {
                this.nativeGraphicsView.RemoveFromSuperview();
                SetNeedsDisplay();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var bounds = AdjustForSafeArea(Bounds).ToRectangle();
            CrossPlatformMeasure?.Invoke(bounds.Width, bounds.Height);
            CrossPlatformArrange?.Invoke(bounds);
			this.UpdateGraphicsViewBounds();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public override CGSize SizeThatFits(CGSize size)
        {
            if (CrossPlatformMeasure == null)
            {
                return base.SizeThatFits(size);
            }

            var width = size.Width;
            var height = size.Height;

            var crossPlatformSize = CrossPlatformMeasure(width, height);

            return crossPlatformSize.ToCGSize();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirtyRect"></param>
        public override void Draw(CGRect dirtyRect)
        {
            if (this.DrawingOrder == DrawingOrder.BelowContent)
            {
                base.Draw(dirtyRect);
                var coreGraphics = UIGraphics.GetCurrentContext();

                if (_drawable == null) return;

                if (_colorSpace == null)
                {
                    _colorSpace = CGColorSpace.CreateDeviceRGB();
                }

                coreGraphics.SetFillColorSpace(_colorSpace);
                coreGraphics.SetStrokeColorSpace(_colorSpace);
                coreGraphics.SetPatternPhase(PatternPhase);
                _renderer?.Draw(coreGraphics, dirtyRect.AsRectangleF());
            }
        }

        /// <summary>
        /// Raised when a button is pressed.
        /// </summary>
        /// <param name="presses">A set of <see cref="UIPress"/> instances that represent the new presses that occurred.</param>
        /// <param name="evt">The event to which the presses belong.</param>
        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (this.mauiView != null && !this.mauiView.HandleKeyPress(presses, evt))
            {
                base.PressesBegan(presses, evt);
            }
        }

        /// <summary>
        /// Raised when a button is released.
        /// </summary>
        /// <param name="presses">A set of <see cref="UIPress"/> instances that represent the buttons that the user is no longer pressing.</param>
        /// <param name="evt">The event to which the presses belong.</param>
        public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            if (this.mauiView != null && !this.mauiView.HandleKeyRelease(presses, evt))
            {
                base.PressesEnded(presses, evt);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override CGRect Bounds
        {
            get => base.Bounds;

            set
            {
                var newBounds = value;
                if (_lastBounds.Width != newBounds.Width || _lastBounds.Height != newBounds.Height)
                {
                    base.Bounds = value;
                    _renderer?.SizeChanged((float)newBounds.Width, (float)newBounds.Height);
                    _renderer?.Invalidate();
                    this.UpdateGraphicsViewBounds();
                    _lastBounds = newBounds;
                    // Draw method not getting called on resizing the window
                    this.SetNeedsDisplay();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (nativeGraphicsView != null)
                {
                    nativeGraphicsView.Dispose();
                    nativeGraphicsView = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual CGSize PatternPhase
        {
            get
            {
                var px = Frame.X;
                var py = Frame.Y;
                return new CGSize(px, py);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateGraphicsViewBounds()
        {
            if (this.nativeGraphicsView != null)
            {
                if(this.nativeGraphicsView.Bounds != this.Bounds)
                    this.nativeGraphicsView.Frame = this.Bounds;
                this.nativeGraphicsView.InvalidateDrawable();
            }
        }
    }
}
