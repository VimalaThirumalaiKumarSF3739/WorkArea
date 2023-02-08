

using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Syncfusion.Maui.Core.Platform;
using System;
using System.Reflection;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SfViewHandler : LayoutHandler
    {
        private LayoutPanelExt? layoutPanelExt;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected override LayoutPanel CreatePlatformView()
        {
            if (VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutViewGroup");
            }

            this.layoutPanelExt = new LayoutPanelExt((IDrawableLayout)VirtualView)
            {
                CrossPlatformMeasure = VirtualView.CrossPlatformMeasure,
                CrossPlatformArrange = VirtualView.CrossPlatformArrange
            };

            return this.layoutPanelExt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public override void SetVirtualView(IView view)
        {
            base.SetVirtualView(view);

            _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

            if (this.layoutPanelExt != null)
            {
                this.layoutPanelExt.CrossPlatformMeasure = VirtualView.CrossPlatformMeasure;
                this.layoutPanelExt.CrossPlatformArrange = VirtualView.CrossPlatformArrange;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void Invalidate()
        {
            if (this.layoutPanelExt != null)
            {
                this.layoutPanelExt.Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawingOrder"></param>
        public void SetDrawingOrder(DrawingOrder drawingOrder = DrawingOrder.NoDraw)
        {
            if (this.layoutPanelExt != null)
            {
                this.layoutPanelExt.DrawingOrder = drawingOrder;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateClipToBounds(bool clipToBounds)
        {
            if (this.layoutPanelExt != null)
            {
                this.layoutPanelExt.ClipsToBounds = clipToBounds;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="child"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public new void Add(IView child)
        {
            _ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
            _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
            _ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

            var index = this.PlatformView.Children.Count;
            if (this.layoutPanelExt != null)
            {
                if (this.layoutPanelExt?.DrawingOrder == DrawingOrder.AboveContent)
                {
                    PlatformView.Children.Insert(index - 1, child.ToPlatform(MauiContext));
                }
                else
                {
                    PlatformView.Children.Insert(index, child.ToPlatform(MauiContext));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="child"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public new void Insert(int index, IView child)
        {
            _ = PlatformView ?? throw new InvalidOperationException($"{nameof(PlatformView)} should have been set by base class.");
            _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");
            _ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

            if (this.layoutPanelExt != null)
            {
                if (this.layoutPanelExt?.DrawingOrder == DrawingOrder.BelowContent)
                {
                    PlatformView.Children.Insert(index + 1, child.ToPlatform(MauiContext));
                }
                else
                {
                    PlatformView.Children.Insert(index, child.ToPlatform(MauiContext));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformView"></param>
        protected override void DisconnectHandler(LayoutPanel platformView)
        {
            base.DisconnectHandler(platformView);

            foreach (var child in VirtualView)
            {
                if (child.Handler != null)
                {
                    child.Handler.DisconnectHandler();
                }
            }
            if (this.layoutPanelExt != null)
            {
                this.layoutPanelExt.Dispose();
                this.layoutPanelExt = null;
            }

        }
    }
}
