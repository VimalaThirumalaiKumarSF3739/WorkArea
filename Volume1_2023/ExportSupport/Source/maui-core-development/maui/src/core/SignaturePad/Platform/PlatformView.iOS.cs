using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using UIKit;

namespace Syncfusion.Maui.Graphics.Internals
{
    public partial class PlatformSignaturePad : UIView
    {
        #region Fields

        private bool isPressed;

        private CGColor strokeColor = null!;

        private CAShapeLayer rootLayer = null!;

        private UIBezierPath bezierPath = null!;

        private NSError? error;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformSignaturePad"/> class.
        /// </summary>
        public PlatformSignaturePad()
        {
            ClipsToBounds = true;
        }

        #endregion

        #region Methods

        #region Public Methods

        ///<Inhertitdoc/>
        public override void TouchesBegan(NSSet touches, UIEvent? evt)
        {
            base.TouchesBegan(touches, evt);

            if (virtualView == null || !virtualView.IsEnabled)
            {
                UserInteractionEnabled = false;
                return;
            }
            else if (touches.AnyObject is UITouch touch && virtualView.StartInteraction())
            {
                isPressed = true;
                CGPoint point = touch.LocationInView(this);
                OnInteractionStart((float)point.X, (float)point.Y);
            }
        }

        ///<Inhertitdoc/>
        public override void TouchesMoved(NSSet touches, UIEvent? evt)
        {
            base.TouchesMoved(touches, evt);
            if (virtualView != null && !virtualView.IsEnabled)
            {
                return;
            }

            if (touches.AnyObject is UITouch touch && isPressed)
            {
                CGPoint point = touch.LocationInView(this);
                OnInteractionMove((float)point.X, (float)point.Y);
            }
        }

        ///<Inhertitdoc/>
        public override void TouchesEnded(NSSet touches, UIEvent? evt)
        {
            base.TouchesEnded(touches, evt);
            if (virtualView != null && !virtualView.IsEnabled)
            {
                return;
            }

            if (touches.AnyObject is UITouch touch && isPressed)
            {
                virtualView?.EndInteraction();
                CGPoint point = touch.LocationInView(this);
                OnInteractionEnd((float)point.X, (float)point.Y);
            }

            isPressed = false;
        }

        #endregion

        #region Internal Methods

        internal void Connect(ISignaturePad mauiView)
        {
            virtualView = mauiView;

            BackgroundColor = UIColor.Clear;

            AddRootLayer();

            bezierPath = new UIBezierPath();
        }

        internal void Disconnect()
        {
            virtualView = null;

            RemoveAllSublayers();
            rootLayer.RemoveFromSuperLayer();

            rootLayer.Dispose();
            bezierPath.Dispose();
            strokeColor.Dispose();

            error?.Dispose();
        }

        internal void UpdateMinimumStrokeThickness(ISignaturePad mauiView)
        {
            minimumStrokeWidth = (float)mauiView.MinimumStrokeThickness;
            previousStrokeWidth = (minimumStrokeWidth + maximumStrokeWidth) / 2f;

            Redraw();
        }

        internal void UpdateMaximumStrokeThickness(ISignaturePad mauiView)
        {
            maximumStrokeWidth = (float)mauiView.MaximumStrokeThickness;
            previousStrokeWidth = (minimumStrokeWidth + maximumStrokeWidth) / 2f;

            Redraw();
        }

        internal void UpdateStrokeColor(ISignaturePad mauiView)
        {
            strokeColor = mauiView.StrokeColor.ToCGColor();

            Redraw();
        }

        internal void UpdateBackground(ISignaturePad virtualView)
        {
            BrushExtensions.UpdateBackground(this, virtualView.Background);
        }

        internal ImageSource ToImageSource()
        {
            UIGraphics.BeginImageContextWithOptions(new CGSize(Frame.Width, Frame.Height), false, 0);
            CGColor? previousBackgroundColor = Layer.BackgroundColor;
            // Removed background color to avoid converting the signature to image with its background. 
            Layer.BackgroundColor = (UIColor.Clear).CGColor;
            Layer.RenderInContext(UIGraphics.GetCurrentContext());

            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            Layer.BackgroundColor = previousBackgroundColor;

            return ImageSource.FromStream(() => image.AsPNG().AsStream());
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

        private void AddBezier(Bezier curve, float startWidth, float endWidth)
        {
            float widthDelta = endWidth - startWidth;
            float drawSteps = (float)Math.Ceiling(curve.Length());

            bezierPath.RemoveAllPoints();

            if (bezierPath != null)
            {
                CGPoint point = new();
                for (int i = 0; i < drawSteps; i++)
                {
                    ComputePointDetails(curve, startWidth, widthDelta, drawSteps,
                        i, out float x, out float y, out float width);

                    bezierPath.LineWidth = (float)width;

                    point.X = x;
                    point.Y = y;
                    bezierPath.MoveTo(point);
                    bezierPath.AddLineTo(point);
                }
            }

            AddRootLayerCopy();
        }

        private void DrawPoint(float x, float y, float width)
        {
            bezierPath.RemoveAllPoints();
            if (bezierPath != null)
            {
                bezierPath.LineWidth = (float)width;
                CGPoint point = new();
                point.X = x;
                point.Y = y;
                bezierPath.MoveTo(point);
                bezierPath.AddLineTo(point);
                AddRootLayerCopy();
            }
        }

        private void AddRootLayerCopy()
        {
            rootLayer.Path = bezierPath.CGPath;
            rootLayer.LineJoin = CAShapeLayer.JoinRound;
            rootLayer.LineCap = CAShapeLayer.CapRound;
            rootLayer.LineWidth = bezierPath.LineWidth;
            rootLayer.Opacity = 1.0f;
            rootLayer.StrokeColor = strokeColor;
            using NSData? archeivedData = NSKeyedArchiver.GetArchivedData(rootLayer, false, out error);

            CAShapeLayer? copy = null;
            if (archeivedData != null)
            {
                copy = NSKeyedUnarchiver.GetUnarchivedObject(rootLayer.Class,
                    data: archeivedData, error: out error) as CAShapeLayer;
            }

            if (copy != null)
            {
                Layer.AddSublayer(copy);
            }
        }

        private void AddRootLayer()
        {
            rootLayer = new CAShapeLayer();
            Layer.AddSublayer(rootLayer);
        }

        private void RemoveAllSublayers()
        {
            if (Layer.Sublayers != null)
            {
                foreach (CALayer sublayer in Layer.Sublayers)
                {
                    sublayer.RemoveFromSuperLayer();
                    sublayer.Dispose();
                }
            }
        }

        private void Invalidate()
        {
            SetNeedsDisplay();
        }

        private void WipeOut()
        {
            RemoveAllSublayers();
            bezierPath.RemoveAllPoints();

            AddRootLayer();
        }

        #endregion

        #endregion
    }
}
