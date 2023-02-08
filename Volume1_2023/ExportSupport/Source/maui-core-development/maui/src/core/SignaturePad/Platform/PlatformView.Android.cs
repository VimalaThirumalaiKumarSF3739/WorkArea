using System;
using System.IO;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Microsoft.Maui.Graphics.Platform;

namespace Syncfusion.Maui.Graphics.Internals
{
    public partial class PlatformSignaturePad : View
    {
        #region Fields

        private readonly float density;

        private Paint paint = null!;

        private Bitmap? bitmap;

        private Canvas? canvas;

        private int width;

        private int height;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformSignaturePad"/> class.
        /// </summary>
        /// <param name="context"></param>
        public PlatformSignaturePad(Context context) : base(context)
        {
            if (context.Resources != null && context.Resources.DisplayMetrics != null)
            {
                density = context.Resources.DisplayMetrics.Density;
            }
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <inheritdoc/>
        public override bool OnTouchEvent(MotionEvent? e)
        {
            if (virtualView != null && !virtualView.IsEnabled)
            {
                return false;
            }

            return OnTouch(e);
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override void OnDraw(Canvas? canvas)
        {
            base.OnDraw(canvas);

            if (canvas != null && bitmap != null)
            {
                canvas.DrawBitmap(bitmap, 0, 0, paint);
            }
        }

        protected override void OnSizeChanged(int newWidth, int newHeight, int oldWidth, int oldHeight)
        {
            base.OnSizeChanged(newWidth, newHeight, oldWidth, oldHeight);

            width = newWidth;
            height = newHeight;

            CreateBitmap();
        }

        #endregion

        #region Internal Methods

        internal void Connect(ISignaturePad mauiView)
        {
            virtualView = mauiView;

            paint = new Paint
            {
                AntiAlias = true,
                Color = Color.Black,
                StrokeCap = Paint.Cap.Round,
                StrokeJoin = Paint.Join.Bevel,
            };
            paint.SetStyle(Paint.Style.Stroke);
        }

        internal void Disconnect()
        {
            virtualView = null;

            paint?.Dispose();

            canvas?.Dispose();

            bitmap?.Dispose();
        }

        internal void UpdateMinimumStrokeThickness(ISignaturePad mauiView)
        {
            minimumStrokeWidth = (float)mauiView.MinimumStrokeThickness * density;
            previousStrokeWidth = (minimumStrokeWidth + maximumStrokeWidth) / 2;

            Redraw();
        }

        internal void UpdateMaximumStrokeThickness(ISignaturePad mauiView)
        {
            maximumStrokeWidth = (float)mauiView.MaximumStrokeThickness * density;
            previousStrokeWidth = (minimumStrokeWidth + maximumStrokeWidth) / 2;

            Redraw();
        }

        internal void UpdateStrokeColor(ISignaturePad mauiView)
        {
            if (paint != null)
            {
                paint.Color = mauiView.StrokeColor.AsColor();

                if (canvas != null)
                {
                    canvas.DrawColor(paint.Color, PorterDuff.Mode.SrcIn!);
                    Invalidate();
                }
            }
        }

        internal Microsoft.Maui.Controls.ImageSource ToImageSource()
        {
            using MemoryStream stream = new();
            bitmap?.Compress(Bitmap.CompressFormat.Png, 0, stream);

            return Microsoft.Maui.Controls.ImageSource.FromStream(() => new MemoryStream(stream.ToArray()));
        }

        internal void Clear()
        {
            Reset();
            foreach (var cyclePoints in drawPointsCache)
            {
                cyclePoints.Clear();
            }

            WipeOut();
        }

        #endregion

        #region Private Methods

        private void CreateBitmap()
        {
            if (width > 0 && height > 0)
            {
                if (Bitmap.Config.Argb8888 != null)
                {
                    bitmap?.Dispose();
                    bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
                }

                if (bitmap != null)
                {
                    canvas?.Dispose();
                    canvas = new Canvas(bitmap);
                }
            }
        }

        private bool OnTouch(MotionEvent? motionEvent)
        {
            if (motionEvent == null)
            {
                return false;
            }

            // To ignore the touch interception from interactive parent controls like scroll viewer.
            Parent?.RequestDisallowInterceptTouchEvent(true);
            switch (motionEvent.Action)
            {
                case MotionEventActions.Down:
                    if (virtualView == null || !virtualView.StartInteraction())
                    {
                        return false;
                    }

                    OnInteractionStart(motionEvent.GetX(), motionEvent.GetY());
                    break;

                case MotionEventActions.Move:
                    OnInteractionMove(motionEvent.GetX(), motionEvent.GetY());
                    break;

                case MotionEventActions.Up:
                    OnInteractionEnd(motionEvent.GetX(), motionEvent.GetY());
                    virtualView?.EndInteraction();
                    break;
            }

            Invalidate();

            return true;
        }

        private void AddBezier(Bezier curve, float startWidth, float endWidth)
        {
            float widthDelta = endWidth - startWidth;
            float drawSteps = (float)Math.Ceiling(curve.Length());

            for (int i = 0; i < drawSteps; i++)
            {
                ComputePointDetails(curve, startWidth, widthDelta, drawSteps, i, out float x, out float y, out float width);

                paint.StrokeWidth = width;
                canvas?.DrawPoint(x, y, paint);
            }
        }

        private void DrawPoint(float x, float y, float width)
        {
            paint.StrokeWidth = width;
            canvas?.DrawPoint(x, y, paint);
        }

        private void WipeOut()
        {
            if (canvas != null)
            {
                canvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear!);
            }

            Invalidate();
        }

        #endregion

        #endregion
    }
}
