using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Graphics.Internals
{
    /// <summary>
    /// Interface for the signature pad virtual view.
    /// </summary>
    public interface ISignaturePad : IView
    {
        /// <summary>
        /// Gets or sets the maximum stroke thickness of the signature.
        /// </summary>
        double MaximumStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the minimum stroke thickness of the signature.
        /// </summary>
        double MinimumStrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the stroke color of the signature.
        /// </summary>
        Color StrokeColor { get; set; }

        /// <summary>
        /// Gets or sets the background of the signature.
        /// </summary>
        new Brush Background { get; set; }

        /// <summary>
        /// Calls to convert the drawn signature as an image.
        /// </summary>
        /// <returns></returns>
        ImageSource? ToImageSource();

        /// <summary>
        /// Calls to clear the drawn signature.
        /// </summary>
        void Clear();

        /// <summary>
        /// Calls when the drawing starts.
        /// </summary>
        bool StartInteraction();

        /// <summary>
        /// Calls when the drawing ends.
        /// </summary>
        void EndInteraction();
    }
}

