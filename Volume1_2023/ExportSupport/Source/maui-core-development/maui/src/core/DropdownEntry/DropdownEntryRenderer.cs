using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal class FluentDropdownEntryRenderer : IDropdownRenderer
    {
        /// <summary>
        /// 
        /// </summary>
        internal float padding = 12;

        /// <summary>
        /// Draw border method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="rectF">The rect.</param>
        /// <param name="isFocused">The isfocused.</param>
        /// <param name="borderColor">The border color.</param>
        /// <param name="focusedBorderColor">the focused border color.</param>
        public void DrawBorder(ICanvas canvas, RectF rectF, bool isFocused, Color borderColor, Color focusedBorderColor)
        {
            canvas.StrokeColor = Color.FromRgba(240, 240, 240, 255);
            canvas.StrokeSize = 2;
            canvas.DrawRoundedRectangle(rectF, 5);
            canvas.ResetStroke();

            if (isFocused)
            {
                canvas.StrokeColor = focusedBorderColor;
                canvas.DrawLine(rectF.X + 2, rectF.Y + rectF.Height - 1.5f, rectF.X + rectF.Width - 2, rectF.Height - 1.5f);
            }
            else
            {
                canvas.StrokeColor = borderColor;
                canvas.DrawLine(rectF.X + 2, rectF.Y + rectF.Height - 1, rectF.X + rectF.Width - 2, rectF.Height - 1);
            }
            canvas.StrokeSize = 0.6f;
            canvas.DrawLine(rectF.X + 3, rectF.Y + rectF.Height - 0.6f, rectF.X + rectF.Width - 3, rectF.Height - 0.6f);
            canvas.DrawLine(rectF.X + 4, rectF.Y + rectF.Height, rectF.X + rectF.Width - 4, rectF.Height);

            canvas.StrokeSize = 1f;
        }

        /// <summary>
        /// Draw clear button method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="rectF">The rect.</param>
        public void DrawClearButton(ICanvas canvas, RectF rectF)
        {
            PointF A = new Point(0, 0);
            PointF B = new Point(0, 0);

            A.X = rectF.X + this.padding;
            A.Y = rectF.Y + this.padding;

            B.X = rectF.X + rectF.Width - this.padding;
            B.Y = rectF.Y + rectF.Height - this.padding;

            canvas.DrawLine(A, B);

            A.X = rectF.X + this.padding;
            A.Y = rectF.Y + rectF.Height - this.padding;

            B.X = rectF.X + rectF.Width - this.padding;
            B.Y = rectF.Y + this.padding;


            canvas.DrawLine(A, B);

        }

        /// <summary>
        /// Draw drop down button.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="rectF">The rect.</param>
        public void DrawDropDownButton(ICanvas canvas, RectF rectF)
        {
            var imageSize = (rectF.Width / 2) - (this.padding / 2);

            rectF.X = rectF.Center.X - imageSize/2;
            rectF.Y = rectF.Center.Y - imageSize/4;
            rectF.Width = imageSize;
            rectF.Height = imageSize/2;

            float x = rectF.X;
            float y = rectF.Y;
            float width = x + rectF.Width;
            float height = y + rectF.Height;
            float midWidth = x + (rectF.Width / 2);

            var path = new PathF();
            path.MoveTo(x, y);
            path.LineTo(midWidth, height);
            path.LineTo(width, y);
            canvas.DrawPath(path);

        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class CupertinoDropdownEntryRenderer : IDropdownRenderer
    {
        /// <summary>
        /// 
        /// </summary>
        internal float padding = 10;

        /// <summary>
        /// Draw border method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="rectF">The rect.</param>
        /// <param name="isFocused">The isfocused.</param>
        /// <param name="borderColor">The border color.</param>
        /// <param name="focusedBorderColor">The focused border color.</param>
        public void DrawBorder(ICanvas canvas, RectF rectF, bool isFocused, Color borderColor, Color focusedBorderColor)
        {
            if (!isFocused)
            {
                canvas.StrokeColor = borderColor;
                canvas.StrokeSize = 1;
            }
            else
            {
                canvas.StrokeColor = focusedBorderColor;
                canvas.StrokeSize = 2;
            }

            canvas.SaveState();
            canvas.DrawRoundedRectangle(rectF, 6);
            canvas.ResetStroke();
        }

        /// <summary>
        /// Draw clear button method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="rectF">The rect.</param>
        public void DrawClearButton(ICanvas canvas, RectF rectF)
        {
            PointF A = new Point(0, 0);
            PointF B = new Point(0, 0);

            A.X = rectF.X + this.padding;
            A.Y = rectF.Y + this.padding;

            B.X = rectF.X + rectF.Width - this.padding;
            B.Y = rectF.Y + rectF.Height - this.padding;

            canvas.DrawLine(A, B);

            A.X = rectF.X + this.padding;
            A.Y = rectF.Y + rectF.Height - this.padding;

            B.X = rectF.X + rectF.Width - this.padding;
            B.Y = rectF.Y + this.padding;


            canvas.DrawLine(A, B);

        }

        /// <summary>
        /// Draw dropdown button method..
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="rectF">The rect</param>
        public void DrawDropDownButton(ICanvas canvas, RectF rectF)
        {
            var imageSize = (rectF.Width / 2) - (this.padding / 2);

            rectF.X = rectF.Center.X - imageSize / 2;
            rectF.Y = rectF.Center.Y - imageSize / 4;
            rectF.Width = imageSize;
            rectF.Height = imageSize / 2;

            float x = rectF.X;
            float y = rectF.Y;
            float width = x + rectF.Width;
            float height = y + rectF.Height;
            float midWidth = x + (rectF.Width / 2);

            var path = new PathF();
            path.MoveTo(x, y);
            path.LineTo(midWidth, height);
            path.LineTo(width, y);
            canvas.DrawPath(path);

        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class MaterialDropdownEntryRenderer : IDropdownRenderer
    {
        /// <summary>
        /// 
        /// </summary>
        internal float padding = 7;
        internal float clearButtonPadding = 9;

        /// <summary>
        /// Draw border method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="rectF">The rect.</param>
        /// <param name="isFocused">The isfocused.</param>
        /// <param name="borderColor">The border color.</param>
        /// <param name="focusedBorderColor">The focued border color.</param>
        public void DrawBorder(ICanvas canvas, RectF rectF, bool isFocused, Color borderColor, Color focusedBorderColor)
        {
            if (isFocused)
            {
                canvas.StrokeColor = focusedBorderColor;
                canvas.StrokeSize = 2;
            }
            else
            {
                canvas.StrokeSize = 1;
                canvas.StrokeColor = borderColor;
            }
            canvas.DrawLine(rectF.X + 4, rectF.Y + rectF.Height - 7, rectF.X + rectF.Width - 4, rectF.Height - 7);
            canvas.StrokeSize = 1;

        }

        /// <summary>
        /// Draw clear button method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="rectF">The rect</param>
        public void DrawClearButton(ICanvas canvas, RectF rectF)
        {
            PointF A = new Point(0, 0);
            PointF B = new Point(0, 0);

            A.X = rectF.X + this.clearButtonPadding;
            A.Y = rectF.Y + this.clearButtonPadding;

            B.X = rectF.X + rectF.Width - this.clearButtonPadding;
            B.Y = rectF.Y + rectF.Height - this.clearButtonPadding;

            canvas.DrawLine(A, B);

            A.X = rectF.X + this.clearButtonPadding;
            A.Y = rectF.Y + rectF.Height - this.clearButtonPadding;

            B.X = rectF.X + rectF.Width - this.clearButtonPadding;
            B.Y = rectF.Y + this.clearButtonPadding;

            canvas.DrawLine(A, B);

        }

        /// <summary>
        /// Draw dropdown button method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="rectF">The rect.</param>
        public void DrawDropDownButton(ICanvas canvas, RectF rectF)
        {
            rectF.X = rectF.Center.X - this.padding;
            rectF.Y = rectF.Center.Y - (this.padding / 2);
            rectF.Width = (this.padding * 2);
            rectF.Height = this.padding;

            float x = rectF.X;
            float y = rectF.Y;
            float width = x + rectF.Width;
            float height = y + rectF.Height;
            float midWidth = x + (rectF.Width / 2);

            var path = new PathF();
            path.MoveTo(x, y);
            path.LineTo(width, y);
            path.LineTo(midWidth, height);
            path.LineTo(x, y);
            path.Close();
            canvas.FillPath(path);

        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal interface IDropdownRenderer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rectF"></param>
        void DrawDropDownButton(ICanvas canvas, RectF rectF);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rectF"></param>
        void DrawClearButton(ICanvas canvas, RectF rectF);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rectF"></param>
        /// <param name="isFocused"></param>
        /// <param name="borderColor"></param>
        /// <param name="focusedBorderColor"></param>
        void DrawBorder(ICanvas canvas, RectF rectF, bool isFocused, Color borderColor, Color focusedBorderColor);
    }

}
