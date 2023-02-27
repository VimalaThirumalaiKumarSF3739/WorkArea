
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

using WSize = global::Windows.Foundation.Size;
using WRect = global::Windows.Foundation.Rect;
using Microsoft.UI.Xaml.Media;

namespace Syncfusion.Maui.Core.Platform
{
    /// <summary>
    /// 
    /// </summary>
    internal class LayoutPanelExt : LayoutPanel
    {
        private W2DGraphicsView? nativeGraphicsView;
        internal Func<double, double, Size>? CrossPlatformMeasure { get; set; }
        internal Func<Rect, Size>? CrossPlatformArrange { get; set; }
        private DrawingOrder drawingOrder = DrawingOrder.NoDraw;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layout"></param>
        public LayoutPanelExt(IDrawableLayout layout)
        {
            this.Drawable = layout;
            //ListView Focus rect not shown when navigated using keyboard arrow keys
            //this.IsTabStop = true; 
            this.AllowFocusOnInteraction = true;
            this.UseSystemFocusVisuals = true;
            SizeChanged += ContentPanelExt_SizeChanged;

        }

        private void ContentPanelExt_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            nativeGraphicsView?.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        public DrawingOrder DrawingOrder
        {
            get
            {
                return this.drawingOrder;
            }
            set
            {
                this.drawingOrder = value;
                if (this.DrawingOrder == DrawingOrder.NoDraw)
                {
                    this.RemoveDrawableView();
                }
                else
                {
                    this.InitializeNativeGraphicsView();
                    this.ArrangeNativeGraphicsView();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public IDrawable Drawable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal void InitializeNativeGraphicsView()
        {
            if (!Children.Contains(nativeGraphicsView))
            {
                nativeGraphicsView = new W2DGraphicsView
                {
                    Drawable = this.Drawable
                };
            }

            if (nativeGraphicsView != null)
            {
                if (this.DrawingOrder == DrawingOrder.AboveContentWithTouch || this.DrawingOrder == DrawingOrder.BelowContent)
                {
                    nativeGraphicsView.IsHitTestVisible = true;
                }
                else
                {
                    nativeGraphicsView.IsHitTestVisible = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void RemoveDrawableView()
        {
            if (nativeGraphicsView != null && Children.Contains(nativeGraphicsView))
            {
                Children.Remove(nativeGraphicsView);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void ArrangeNativeGraphicsView()
        {
            if (nativeGraphicsView != null)
            {
                if (Children.Contains(nativeGraphicsView))
                {
                    Children.Remove(nativeGraphicsView);
                }

                if (this.DrawingOrder == DrawingOrder.AboveContentWithTouch || this.DrawingOrder == DrawingOrder.AboveContent)
                {
                    Children.Add(nativeGraphicsView);
                }
                else
                {
                    Children.Insert(0, nativeGraphicsView);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void Invalidate()
        {
            this.nativeGraphicsView?.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override WSize ArrangeOverride(WSize finalSize)
        {
            if (CrossPlatformArrange == null)
            {
                return base.ArrangeOverride(finalSize);
            }

            var width = finalSize.Width;
            var height = finalSize.Height;

            CrossPlatformArrange(new Rect(0, 0, width, height));

            if(ClipsToBounds)
            {
                if(Clip != null && (Clip.Bounds.Width != finalSize.Width || Clip.Bounds.Height != finalSize.Height))
                {
                    Clip = new RectangleGeometry { Rect = new WRect(0, 0, finalSize.Width, finalSize.Height) };
                }
            }

            if (nativeGraphicsView != null)
            {
                nativeGraphicsView.Arrange(new Windows.Foundation.Rect(0, 0, width, height));
            }

            return finalSize;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override WSize MeasureOverride(WSize availableSize)
        {
            if (CrossPlatformMeasure == null)
            {
                return base.MeasureOverride(availableSize);
            }

            var width = availableSize.Width;
            var height = availableSize.Height;

            var crossPlatformSize = CrossPlatformMeasure(width, height);

            width = crossPlatformSize.Width;
            height = crossPlatformSize.Height;

            if (nativeGraphicsView != null)
            {
                nativeGraphicsView.Measure(availableSize);
            }

            return new WSize(width, height);
        }

        internal void Dispose()
        {
            if(this.nativeGraphicsView != null)
            {
                this.nativeGraphicsView = null;
            }
        }
    }
}
