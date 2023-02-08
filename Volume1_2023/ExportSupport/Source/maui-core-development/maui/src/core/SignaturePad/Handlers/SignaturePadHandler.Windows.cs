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

        /// <inheritdoc/>
        public override void UpdateValue(string property)
        {
            base.UpdateValue(property);

            //When no background color is given in the PCL, the native view background is updated as null and thus no touch point is captured.
            //So updated the background as Colors.Transparent in case of null. Will remove once framework report is addressed.
            if (property == "Background")
            {
                PlatformView.UpdateBackground(VirtualView);
            }
        }
    }
}

