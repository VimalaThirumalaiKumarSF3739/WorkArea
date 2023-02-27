using CoreGraphics;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Core.Platform;
using System;
using System.Linq;
using UIKit;
using PlatformView = UIKit.UIView;

namespace Syncfusion.Maui.Core
{
	/// <summary>
	/// 
	/// </summary>
    public partial class SfViewHandler : LayoutHandler
    {
        private LayoutViewExt? layoutViewExt;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected override LayoutView CreatePlatformView()
		{
			if (VirtualView == null)
			{
				throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutViewGroup");
			}

            this.layoutViewExt = new LayoutViewExt((IDrawable)VirtualView)
            {
                CrossPlatformMeasure = VirtualView.CrossPlatformMeasure,
                CrossPlatformArrange = VirtualView.CrossPlatformArrange
            };

			return this.layoutViewExt;
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

            if (this.layoutViewExt != null)
            {
                this.layoutViewExt.CrossPlatformMeasure = VirtualView.CrossPlatformMeasure;
                this.layoutViewExt.CrossPlatformArrange = VirtualView.CrossPlatformArrange;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Invalidate()
		{
            if (this.layoutViewExt != null)
            {
                this.layoutViewExt.InvalidateDrawable();
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="drawingOrder"></param>
		public void SetDrawingOrder(DrawingOrder drawingOrder = DrawingOrder.NoDraw)
		{
            if (this.layoutViewExt != null)
            {
                this.layoutViewExt.DrawingOrder = drawingOrder;
            }
		}

        /// <summary>
        /// 
        /// </summary>
        public void UpdateClipToBounds(bool clipToBounds)
        {
            if (this.layoutViewExt != null)
            {
                this.layoutViewExt.ClipsToBounds = clipToBounds;
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

            var index = this.PlatformView.Subviews.Length;
            if (this.layoutViewExt != null)
            {
                if (this.layoutViewExt.DrawingOrder == DrawingOrder.AboveContent)
                {
                    PlatformView.InsertSubview(child.ToPlatform(MauiContext), index - 1);
                }
                else
                {
                    PlatformView.InsertSubview(child.ToPlatform(MauiContext), index);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformView"></param>
        protected override void DisconnectHandler(LayoutView platformView)
        {
            base.DisconnectHandler(platformView);
            foreach (var child in VirtualView)
            {
                if (child.Handler != null)
                {
                    child.Handler.DisconnectHandler();
                }
            }

            if (this.layoutViewExt != null)
            {
                this.layoutViewExt.Dispose();
                this.layoutViewExt = null;
            }
        }
    }
}
