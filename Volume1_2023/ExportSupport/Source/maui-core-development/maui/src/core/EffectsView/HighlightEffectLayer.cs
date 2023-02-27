// <copyright file="HighlightEffectLayer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;
    using Syncfusion.Maui.Graphics.Internals;

    /// <summary>
    /// Represents the HighlightEffectLayer class.
    /// </summary>
    internal class HighlightEffectLayer 
    {
        #region Fields

        /// <summary>
        /// Represents the highlight transparency factor.
        /// </summary>
        private const float HighlightTransparencyFactor = 0.04f;

        /// <summary>
        /// Represents highlight bounds.
        /// </summary>
        private Rect highlightBounds;

        /// <summary>
        /// Represents the highlight color.
        /// </summary>
        private Brush highlightColor = new SolidColorBrush(Colors.Black);

        private readonly IDrawable drawable;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightEffectLayer"/> class.
        /// </summary>
        /// <param name="highlightColor">The Highlight Color</param>
        /// <param name="drawable">drawable</param>
        public HighlightEffectLayer(Brush highlightColor,IDrawable drawable)
        {
            this.highlightColor = highlightColor;
            this.drawable = drawable;
        }

        #endregion

        #region Properties
        internal double Width { get; set; }

        internal double Height { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rectF"></param>
        /// <param name="highlightColorValue"></param>
        internal void DrawHighlight(ICanvas canvas, RectF rectF, Brush highlightColorValue)
        {
            if (this.highlightColor != null)
            {
                canvas.SetFillPaint(highlightColorValue, rectF);
                canvas.FillRectangle(rectF);
            }
        }

        /// <summary>
        /// The draw method of HighlightEffectLayer
        /// </summary>
        /// <param name="canvas"></param>
        internal void DrawHighlight(ICanvas canvas)
        {
            if (this.highlightColor != null)
            {
                canvas.Alpha = HighlightTransparencyFactor;
                DrawHighlight(canvas, this.highlightBounds, this.highlightColor);
            }
        }

        /// <summary>
        /// Update highlight bounds method.
        /// </summary>
        /// <param name="width">The width property.</param>
        /// <param name="height">The height property.</param>
        /// <param name="highlightColor">The highlight color.</param>
        internal void UpdateHighlightBounds(double width = 0, double height = 0, Brush? highlightColor = null)
        {
            highlightColor ??= new SolidColorBrush(Colors.Transparent);

            this.highlightColor = highlightColor;
            this.highlightBounds = new Rect(0, 0, width, height);
            if (drawable is IDrawableLayout drawableLayout)
                drawableLayout.InvalidateDrawable();
            else if (drawable is IDrawableView drawableView)
                drawableView.InvalidateDrawable();
        }
    }

    #endregion
}
