using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Runtime.Versioning;

namespace Syncfusion.Maui.Graphics.Internals
{
    /// <summary>
    /// Represents a view that can be drawn on using native drawing options. 
    /// </summary>
    public class SfDrawableView : View, IDrawableView
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="dirtyRect"></param>
        void IDrawable.Draw(ICanvas canvas, RectF dirtyRect)
        {
            OnDraw(canvas, dirtyRect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="dirtyRect"></param>
        protected virtual void OnDraw(ICanvas canvas, RectF dirtyRect)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void InvalidateDrawable()
        {
            if (this.Handler is SfDrawableViewHandler handler)
                handler.Invalidate();
        }
    }
}
