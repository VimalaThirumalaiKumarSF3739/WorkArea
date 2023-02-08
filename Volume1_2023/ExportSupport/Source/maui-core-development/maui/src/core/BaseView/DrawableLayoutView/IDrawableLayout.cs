using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDrawableLayout : IDrawable, IAbsoluteLayout
    {
        /// <summary>
        /// 
        /// </summary>
        void InvalidateDrawable();

        /// <summary>
        /// 
        /// </summary>
        DrawingOrder DrawingOrder { get; set;}
    }
}
