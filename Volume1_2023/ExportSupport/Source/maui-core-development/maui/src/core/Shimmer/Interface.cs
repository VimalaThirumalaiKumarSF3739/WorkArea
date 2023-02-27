using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Shimmer
{
    /// <summary>
    /// Interface that holds the properties to render the <see cref="SfShimmer"/> view.
    /// </summary>
    internal interface IShimmer
    {
        /// <summary>
        /// Gets the duration of the wave animation in milliseconds.
        /// </summary>
        public double AnimationDuration { get; }

        /// <summary>
        /// Gets the background color of shimmer view.
        /// </summary>
        public Brush Fill { get; }

        /// <summary>
        /// Gets a value indicating whether to load actual content of shimmer.
        /// </summary>
        public bool IsActive { get; }

        /// <summary>
        /// Gets the shimmer wave color.
        /// </summary>
        public Color WaveColor { get; }

        /// <summary>
        /// Gets the width of the wave.
        /// </summary>
        public double WaveWidth { get; }

        /// <summary>
        /// Gets the built-in shimmer view type. 
        /// </summary>
        public ShimmerType Type { get; }

        /// <summary>
        /// Gets the animation direction for Shimmer.
        /// </summary>
        public ShimmerWaveDirection WaveDirection { get; }

        /// <summary>
        /// Gets the number of times the shimmer view should be repeated.
        /// </summary>
        public int RepeatCount { get; }

        /// <summary>
        /// Gets the custom view that is used for loading view.
        /// </summary>
        public View CustomView { get; }
    }
}
