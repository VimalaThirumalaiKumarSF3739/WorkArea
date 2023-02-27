
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Core.Platform;
using System;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SfViewHandler : LayoutHandler
    {
        private LayoutViewGroupExt? layoutViewGroupExt;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected override LayoutViewGroup CreatePlatformView()
        {
            if (VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutViewGroup");
            }

            this.layoutViewGroupExt = new LayoutViewGroupExt(Context, (IDrawableLayout)VirtualView)
            {
                CrossPlatformMeasure = VirtualView.CrossPlatformMeasure,
                CrossPlatformArrange = VirtualView.CrossPlatformArrange
            };

            this.layoutViewGroupExt.SetClipChildren(true);

            return this.layoutViewGroupExt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public override void SetVirtualView(IView view)
        {
            base.SetVirtualView(view);

            _ = VirtualView ?? throw new InvalidOperationException($"{nameof(VirtualView)} should have been set by base class.");

            if (this.layoutViewGroupExt != null)
            {
                this.layoutViewGroupExt.CrossPlatformMeasure = VirtualView.CrossPlatformMeasure;
                this.layoutViewGroupExt.CrossPlatformArrange = VirtualView.CrossPlatformArrange;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Invalidate()
        {
            this.PlatformView?.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawingOrder"></param>
        public void SetDrawingOrder(DrawingOrder drawingOrder = DrawingOrder.NoDraw)
        {
            if (this.layoutViewGroupExt != null)
            {
                this.layoutViewGroupExt.DrawingOrder = drawingOrder;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateClipToBounds(bool clipToBounds)
        {
            if (this.layoutViewGroupExt != null)
            {
                this.layoutViewGroupExt.ClipsToBounds = clipToBounds;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformView"></param>
        protected override void DisconnectHandler(LayoutViewGroup platformView)
        {
            base.DisconnectHandler(platformView);
            foreach (var child in VirtualView)
            {
                if (child.Handler != null)
                {
                    child.Handler.DisconnectHandler();
                }
            }

            if (this.layoutViewGroupExt != null)
            {
                this.layoutViewGroupExt.Dispose();
                this.layoutViewGroupExt = null;
            }
        }
    }
}
