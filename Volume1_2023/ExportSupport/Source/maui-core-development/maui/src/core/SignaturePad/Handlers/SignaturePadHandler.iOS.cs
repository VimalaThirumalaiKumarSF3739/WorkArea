using Microsoft.Maui.Handlers;

namespace Syncfusion.Maui.Graphics.Internals
{
    public partial class SignaturePadHandler : ViewHandler<ISignaturePad, PlatformSignaturePad>
    {
        /// <inheritdoc/>
		protected override PlatformSignaturePad CreatePlatformView()
        {
            return new PlatformSignaturePad();
        }

        /// <summary>
        /// Updates the drawn signature maximum stroke thickness.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="virtualView"></param>
        public static void MapMaximumStrokeThickness(SignaturePadHandler handler, ISignaturePad virtualView)
        {
            handler.PlatformView.UpdateMaximumStrokeThickness(virtualView);
        }

        /// <summary>
        /// Updates the drawn signature minimum stroke thickness.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="virtualView"></param>
        public static void MapMinimumStrokeThickness(SignaturePadHandler handler, ISignaturePad virtualView)
        {
            handler.PlatformView.UpdateMinimumStrokeThickness(virtualView);
        }

        /// <summary>
        /// Updates the drawn signature stroke color.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="virtualView"></param>
        public static void MapStrokeColor(SignaturePadHandler handler, ISignaturePad virtualView)
        {
            handler.PlatformView.UpdateStrokeColor(virtualView);
        }

        /// <summary>
        /// Clears the drawn signature.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="virtualView"></param>
        /// <param name="arg"></param>
        public static void MapClear(ISignaturePadHandler handler, ISignaturePad virtualView, object? arg)
        {
            handler.PlatformView.Clear();
        }
    }
}

