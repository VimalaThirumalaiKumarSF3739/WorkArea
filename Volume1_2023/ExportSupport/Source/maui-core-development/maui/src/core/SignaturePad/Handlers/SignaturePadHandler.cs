using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Graphics.Internals
{
    /// <summary>
    /// The <see cref="SignaturePadHandler"/> maps the cross-platform signature pad control API to the native view's API.
    /// </summary>
    public partial class SignaturePadHandler : ISignaturePadHandler
    {
        /// <summary>
        /// Maps the cross-platform property changed to the native property changed methods.
        /// </summary>
        public static PropertyMapper<ISignaturePad, SignaturePadHandler> Mapper =
            new((PropertyMapper)ViewMapper)
            {
                [nameof(ISignaturePad.MaximumStrokeThickness)] = MapMaximumStrokeThickness,
                [nameof(ISignaturePad.MinimumStrokeThickness)] = MapMinimumStrokeThickness,
                [nameof(ISignaturePad.StrokeColor)] = MapStrokeColor,
            };

        /// <summary>
        /// Maps the cross-platform methods to the native methods.
        /// </summary>
        public static CommandMapper<ISignaturePad, ISignaturePadHandler> CommandMapper = new(ViewCommandMapper)
        {
            [nameof(ISignaturePad.Clear)] = MapClear
        };

        /// <summary>
        /// Initializes a new instance of the SignaturePadHandler class.
        /// </summary>
        public SignaturePadHandler() : base(Mapper, CommandMapper)
        {
        }

        ISignaturePad ISignaturePadHandler.VirtualView => VirtualView;

        PlatformSignaturePad ISignaturePadHandler.PlatformView => PlatformView;

        /// <inheritdoc/>
        protected override void ConnectHandler(PlatformSignaturePad platformView)
        {
            platformView.Connect(VirtualView);

            base.ConnectHandler(platformView);
        }

        /// <inheritdoc/>
        protected override void DisconnectHandler(PlatformSignaturePad platformView)
        {
            platformView.Disconnect();

            base.DisconnectHandler(platformView);
        }

        /// <summary>
        /// Converts the drawn signature as an image.
        /// </summary>
        internal ImageSource? ToImageSource()
        {
            return PlatformView?.ToImageSource();
        }
    }
}

