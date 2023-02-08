using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Text;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Color = Microsoft.Maui.Graphics.Color;
using Microsoft.Maui.Platform;
using System;
using Android.Runtime;
using ARect = Android.Graphics.Rect;
using Rectangle = Microsoft.Maui.Graphics.Rect;
using Size = Microsoft.Maui.Graphics.Size;
using Android.Widget;

namespace Syncfusion.Maui.Core.Platform
{
    /// <summary>
    /// 
    /// </summary>
    internal class LayoutViewGroupExt : LayoutViewGroup
    {
        private int _width, _height;
        private PlatformCanvas? _canvas;
        private ScalingCanvas? _scalingCanvas;
        private IDrawable? _drawable;
        private float _scale = 1;
        private Color? _backgroundColor;
        private readonly Context _context;
        private DrawingOrder drawingOrder = DrawingOrder.NoDraw;
        readonly ARect _clipRect = new();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public LayoutViewGroupExt(Context context) : base(context)
        {
            this.Initialize();
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="drawable"></param>
        public LayoutViewGroupExt(Context context, IDrawable? drawable = null) : base(context)
        {
            this.Initialize();
            _context = context;
            Drawable = drawable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="javaReference"></param>
        /// <param name="transfer"></param>
        public LayoutViewGroupExt(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            var context = Context;
            ArgumentNullException.ThrowIfNull(context);
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        public LayoutViewGroupExt(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.Initialize();
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="drawable"></param>
        public LayoutViewGroupExt(Context context, IAttributeSet attrs, IDrawable? drawable = null) : base(context, attrs)
        {
            _context = context;
            Drawable = drawable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStyleAttr"></param>
        public LayoutViewGroupExt(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            this.Initialize();
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStyleAttr"></param>
        /// <param name="defStyleRes"></param>
        public LayoutViewGroupExt(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            this.Initialize();
            _context = context;
        }

        private void Initialize()
        {
            this.SetWillNotDraw(true);
            if (Resources != null && Resources.DisplayMetrics != null)
            {
                _scale = Resources.DisplayMetrics.Density;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal Func<double, double, Size>? CrossPlatformMeasure { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal Func<Rectangle, Size>? CrossPlatformArrange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Color? BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                Invalidate();
            }
        }

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
                this.UpdateDrawable();
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
                Invalidate();
            }
        }

        private void UpdateDrawable()
        {
            if(this.DrawingOrder == DrawingOrder.NoDraw)
            {
                this.SetWillNotDraw(true);
                if (_canvas != null)
                {
                    _canvas.Dispose();
                    _canvas = null;
                }
                if (_scalingCanvas != null)
                {
                    _scalingCanvas = null;
                }
            }
            else
            {
                this.SetWillNotDraw(false);
                if (_canvas == null)
                {
                    _canvas = new PlatformCanvas(_context);
                }
                if (_scalingCanvas == null)
                {
                    _scalingCanvas = new ScalingCanvas(_canvas);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        protected override void DispatchDraw(Canvas? canvas)
        {
            if (this.DrawingOrder == DrawingOrder.AboveContent)
            {
                base.DispatchDraw(canvas);
                this.DrawContent(canvas);
            }
            else
            {
                this.DrawContent(canvas);
                base.DispatchDraw(canvas);
            }
        }

        private void DrawContent(Canvas? androidCanvas)
        {
            if (_drawable == null) return;

            var dirtyRect = new Microsoft.Maui.Graphics.RectF(0, 0, _width, _height);

            if (_canvas != null)
            {
                _canvas.Canvas = androidCanvas;
                if (_backgroundColor != null)
                {
                    _canvas.FillColor = _backgroundColor;
                    _canvas.FillRectangle(dirtyRect);
                    _canvas.FillColor = Colors.White;
                }

                _scalingCanvas?.ResetState();
                _scalingCanvas?.Scale(_scale, _scale);
                //Since we are using a scaling canvas, we need to scale the rectangle
                dirtyRect.Height /= _scale;
                dirtyRect.Width /= _scale;
                _drawable.Draw(_scalingCanvas, dirtyRect);
                _canvas.Canvas = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="oldWidth"></param>
        /// <param name="oldHeight"></param>
        protected override void OnSizeChanged(int width, int height, int oldWidth, int oldHeight)
        {
            base.OnSizeChanged(width, height, oldWidth, oldHeight);
            _width = width;
            _height = height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="widthMeasureSpec"></param>
        /// <param name="heightMeasureSpec"></param>
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            if (CrossPlatformMeasure == null)
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
                return;
            }

            var deviceIndependentWidth = widthMeasureSpec.ToDouble(_context);
            var deviceIndependentHeight = heightMeasureSpec.ToDouble(_context);

            var widthMode = MeasureSpec.GetMode(widthMeasureSpec);
            var heightMode = MeasureSpec.GetMode(heightMeasureSpec);

            var measure = CrossPlatformMeasure(deviceIndependentWidth, deviceIndependentHeight);

            var width = widthMode == MeasureSpecMode.Exactly ? deviceIndependentWidth : measure.Width;
            var height = heightMode == MeasureSpecMode.Exactly ? deviceIndependentHeight : measure.Height;

            var platformWidth = _context.ToPixels(width);
            var platformHeight = _context.ToPixels(height);

            platformWidth = Math.Max(MinimumWidth, platformWidth);
            platformHeight = Math.Max(MinimumHeight, platformHeight);

            SetMeasuredDimension((int)platformWidth, (int)platformHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="changed"></param>
        /// <param name="l"></param>
        /// <param name="t"></param>
        /// <param name="r"></param>
        /// <param name="b"></param>
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            if (CrossPlatformArrange == null || _context == null)
            {
                return;
            }

            var destination = _context.ToCrossPlatformRectInReferenceFrame(l, t, r, b);

            CrossPlatformArrange(destination);

            if (ClipsToBounds)
            {
                _clipRect.Right = r - l;
                _clipRect.Bottom = b - t;
                ClipBounds = _clipRect;
            }
            else
            {
                ClipBounds = null;
            }
        }

    }
}
