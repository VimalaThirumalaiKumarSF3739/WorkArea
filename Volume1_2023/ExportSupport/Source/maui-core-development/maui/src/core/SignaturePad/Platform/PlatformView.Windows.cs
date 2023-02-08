using System;
using System.IO;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using Microsoft.UI;
using Microsoft.UI.Composition;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Storage.Streams;
using Windows.UI;

namespace Syncfusion.Maui.Graphics.Internals
{
    public partial class PlatformSignaturePad : UserControl
    {
        #region Fields

        private int pointerID = -1;

        private bool isPressed;

        private SpriteVisual? visual;

        private Compositor? compositor;

        private CompositionSurfaceBrush? viewBrush;

        private CompositionDrawingSurface? viewSurface;

        private CompositionGraphicsDevice? graphicsDevice;

        private CanvasRenderTarget? renderTarget;

        private Color strokeColor;

        private Color backgroundColor;

        private float dpi;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformSignaturePad"/> class.
        /// </summary>
        public PlatformSignaturePad()
        {
            Content = new Grid()
            {
                Background = new SolidColorBrush(Colors.Transparent)
            };
        }

        #endregion

        #region Methods

        #region Internal Methods

        internal void Connect(ISignaturePad mauiView)
        {
            virtualView = mauiView;

            SizeChanged += OnSizeChanged;
            PointerPressed += OnPointerPressed;
            PointerMoved += OnPointerMoved;
            PointerReleased += OnPointerReleased;
        }

        internal void Disconnect()
        {
            virtualView = null;

            SizeChanged -= OnSizeChanged;
            PointerPressed -= OnPointerPressed;
            PointerMoved -= OnPointerMoved;
            PointerReleased -= OnPointerReleased;

            visual?.Dispose();
            compositor?.Dispose();
            viewBrush?.Dispose();
            viewSurface?.Dispose();
            graphicsDevice?.Dispose();
            renderTarget?.Dispose();
        }

        internal void UpdateMaximumStrokeThickness(ISignaturePad virtualView)
        {
            maximumStrokeWidth = (float)virtualView.MaximumStrokeThickness;
            previousStrokeWidth = (minimumStrokeWidth + maximumStrokeWidth) / 2;

            Redraw();
        }

        internal void UpdateMinimumStrokeThickness(ISignaturePad virtualView)
        {
            minimumStrokeWidth = (float)virtualView.MinimumStrokeThickness;
            previousStrokeWidth = (minimumStrokeWidth + maximumStrokeWidth) / 2;

            Redraw();
        }

        internal void UpdateStrokeColor(ISignaturePad virtualView)
        {
            strokeColor = virtualView.StrokeColor.ToWindowsColor();

            Redraw();
        }

        internal void UpdateBackground(ISignaturePad virtualView)
        {
            //When no background color is given in the PCL, the native view background is updated as null and thus no touch point is captured.
            //So updated the background as Colors.Transparent in case of null. Will remove once framework report is addressed.
            Brush? brush = virtualView.Background?.ToBrush();
            if (brush is SolidColorBrush solidColorBrush && solidColorBrush?.Color != null)
            {
                backgroundColor = solidColorBrush.Color;
            }
            else
            {
                backgroundColor = Colors.Transparent;
            }

            Invalidate();
        }

        internal Microsoft.Maui.Controls.ImageSource? ToImageSource()
        {
            InMemoryRandomAccessStream stream = new();
            renderTarget?.SaveAsync(stream, CanvasBitmapFileFormat.Png).GetAwaiter().GetResult();
            return Microsoft.Maui.Controls.ImageSource.FromStream(stream.AsStream);
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

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (visual == null)
            {
                CreateRenderTarget(e.NewSize);
                return;
            }

            Clear();
            viewBrush?.Dispose();
            renderTarget?.Dispose();

            visual.Size = ActualSize;

            viewSurface?.Resize(new Windows.Graphics.SizeInt32() { Width = (int)ActualSize.X, Height = (int)ActualSize.Y });
            viewBrush = compositor?.CreateSurfaceBrush(viewSurface);
            visual.Brush = viewBrush;
            dpi = (float)(96 * XamlRoot.RasterizationScale);
            renderTarget = new CanvasRenderTarget(CanvasDevice.GetSharedDevice(), ActualSize.X, ActualSize.Y, dpi);
            // Invalidated surface, since the drawing is rendered on resizing from cache.
            Invalidate();
        }

        private void CreateRenderTarget(Windows.Foundation.Size size)
        {
            if (compositor == null)
            {
                compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            }

            if (visual == null)
            {
                visual = compositor.CreateSpriteVisual();
                ElementCompositionPreview.SetElementChildVisual(this, visual);
            }

            visual.IsVisible = true;
            if (graphicsDevice == null)
            {
                graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(compositor, CanvasDevice.GetSharedDevice());
            }

            if (viewSurface == null)
            {
                viewSurface = graphicsDevice.CreateDrawingSurface(size,
                    Microsoft.Graphics.DirectX.DirectXPixelFormat.B8G8R8A8UIntNormalized,
                    Microsoft.Graphics.DirectX.DirectXAlphaMode.Premultiplied);
            }

            visual.Size = new Vector2((float)size.Width, (float)size.Height);
            viewBrush = compositor.CreateSurfaceBrush(viewSurface);
            visual.Brush = viewBrush;
            dpi = (float)(96 * XamlRoot.RasterizationScale);
            renderTarget = new CanvasRenderTarget(CanvasDevice.GetSharedDevice(), (float)size.Width, (float)size.Height, dpi);
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (virtualView != null && virtualView.StartInteraction() && pointerID == -1)
            {
                isPressed = true;
                pointerID = (int)e.Pointer.PointerId;
                CapturePointer(e.Pointer);
                memoryPoints.Clear();

                PointerPoint pointerPoint = e.GetCurrentPoint(this);
                OnInteractionStart((float)pointerPoint.Position.X, (float)pointerPoint.Position.Y);
            }
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (isPressed && e.Pointer.PointerId == pointerID)
            {
                PointerPoint pointerPoint = e.GetCurrentPoint(this);
                OnInteractionMove((float)pointerPoint.Position.X, (float)pointerPoint.Position.Y);
            }
        }

        private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (isPressed && e.Pointer.PointerId == pointerID)
            {
                pointerID = -1;
                isPressed = false;

                PointerPoint pointerPoint = e.GetCurrentPoint(this);
                OnInteractionEnd((float)pointerPoint.Position.X, (float)pointerPoint.Position.Y);
                virtualView?.EndInteraction();
            }
        }

        private void AddBezier(Bezier curve, float startWidth, float endWidth)
        {
            if (renderTarget != null)
            {
                float widthDelta = endWidth - startWidth;
                float drawSteps = (float)Math.Ceiling(curve.Length());

                using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
                {
                    drawingSession.Antialiasing = CanvasAntialiasing.Antialiased;
                    for (int i = 0; i < drawSteps; i++)
                    {
                        ComputePointDetails(curve, startWidth, widthDelta, drawSteps,
                            i, out float x, out float y, out float width);
                        drawingSession.FillCircle((float)x, (float)y, (float)width, strokeColor);
                    }
                }

                Invalidate();
            }
        }

        private void DrawPoint(float x, float y, float width)
        {
            if (renderTarget != null)
            {
                using (CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession())
                {
                    drawingSession.Antialiasing = CanvasAntialiasing.Antialiased;
                    drawingSession.FillCircle((float)x, (float)y, (float)width, strokeColor);
                }
                Invalidate();
            }
        }

        private void Invalidate()
        {
            if (viewSurface != null)
            {
                using CanvasDrawingSession drawingSession = CanvasComposition.CreateDrawingSession(viewSurface);
                drawingSession.Antialiasing = CanvasAntialiasing.Antialiased;
                drawingSession.Clear(backgroundColor);
                drawingSession.DrawImage(renderTarget);
            }
        }

        private void WipeOut()
        {
            if (renderTarget != null)
            {
                using CanvasDrawingSession drawingSession = renderTarget.CreateDrawingSession();
                drawingSession.Antialiasing = CanvasAntialiasing.Antialiased;
                drawingSession.Clear(Colors.Transparent);

                Invalidate();
            }
        }

        #endregion

        #endregion
    }
}

