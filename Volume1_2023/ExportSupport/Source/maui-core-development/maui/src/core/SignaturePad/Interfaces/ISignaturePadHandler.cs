using Microsoft.Maui;

namespace Syncfusion.Maui.Graphics.Internals
{
    /// <summary>
    /// Interface for the <see cref="SignaturePadHandler"/> class. 
    /// </summary>
    public interface ISignaturePadHandler : IViewHandler
    {
        /// <summary>
        /// Virtual view of the signature pad control.
        /// </summary>
        new ISignaturePad VirtualView { get; }

        /// <summary>
        /// platform view of the signature pad control.
        /// </summary>
        new PlatformSignaturePad PlatformView { get; }
    }
}