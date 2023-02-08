using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;
using Syncfusion.Maui.Core.Platform;
using UIKit;

namespace Syncfusion.Maui.Graphics.Internals
{
    public partial class SfDrawableViewHandler : ViewHandler<IDrawableView, PlatformGraphicsView>
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override PlatformGraphicsView CreatePlatformView()
        {
            return new PlatformGraphicsViewExt(VirtualView) { BackgroundColor = UIColor.Clear };
        }

        /// <summary>
        /// 
        /// </summary>
        public void Invalidate()
        {
            this.PlatformView?.InvalidateDrawable();
        }

        #endregion
    }
}
