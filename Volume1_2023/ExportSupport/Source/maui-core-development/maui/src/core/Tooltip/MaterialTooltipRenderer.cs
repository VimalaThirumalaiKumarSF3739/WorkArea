using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using Rect = Microsoft.Maui.Graphics.Rect;
using Font = Microsoft.Maui.Font;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Show the tooltip based on target rectangle.
    /// </summary>
    internal class MaterialToolTipRenderer : ITextElement
    {
        #region Fields

        private Point nosePoint = Point.Zero;

        private float noseOffset = 2f;

        private float noseHeight = 0f;

        private float noseWidth = 4f;

        #endregion

        private Font font = Font.Default;

        public Font Font
        {
            get
            {
                return font;
            }
            set
            {
                if (font != value)
                    font = value;
            }
        }

        private double fontSize = 14d;

        public double FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                if (fontSize != value)
                    fontSize = value;
                Font = font.WithSize(fontSize);
            }
        }

        private string fontFamily = string.Empty;

        public string FontFamily
        {
            get
            {
                return fontFamily;
            }
            set
            {
                if (fontFamily != value)
                    fontFamily = value;
                Font = Font.OfSize(fontFamily, fontSize, enableScaling: false).WithAttributes(fontAttributes);
            }
        }

        private FontAttributes fontAttributes = FontAttributes.None;

        public FontAttributes FontAttributes
        {
            get
            {
                return fontAttributes;
            }
            set
            {
                if (fontAttributes != value)
                    fontAttributes = value;
                Font = font.WithAttributes(fontAttributes);
            }
        }

        private Color textColor = Colors.White;

        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                if (textColor != value)
                    textColor = value;
            }
        }

        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {
        }

        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {
        }

        double ITextElement.FontSizeDefaultValueCreator()
        {
            return 14d;
        }

        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {
        }

        void ITextElement.OnFontChanged(Font oldValue, Font newValue)
        {
        }

        #region internal properties

        /// <summary>
        /// Gets or sets a value that indicates the position of tooltip.
        /// </summary>
        internal TooltipPosition Position { get; set; } = TooltipPosition.Auto;

        /// <summary>
        /// Gets or sets a value that indicates the padding of tooltip.
        /// </summary>
        internal Thickness Padding { get; set; } = new Thickness(15.0, 8.0);

        /// <summary>
        /// Gets or sets a value that indicates the corner radius of tooltip.
        /// </summary>
        internal float CornerRadius { get; set; } = 3f;

        internal Size ContentSize { get; set; }

        #endregion

        #region internal Methods

        /// <summary>
        /// The tooltip show the additional information about the control.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="containerRect"></param>
        /// <param name="targetRects"></param>
        /// <param name="texts"></param>
        /// <param name="animationValue"></param>
        /// <param name="fill"></param>
        /// <param name="stroke"></param>
        /// <param name="strokeSize"></param>
        internal void Show(ICanvas canvas, Rect containerRect, Rect[] targetRects, string[] texts, double[] animationValue, Brush fill, Brush stroke, double strokeSize)
        {
            Rect[] toolTipRects = GetToolTipRects(containerRect, targetRects, texts);
            bool canDrawOverlappingStroke = false;
            if (toolTipRects.Length == 2)
            {
                canDrawOverlappingStroke = toolTipRects[1].IntersectsWith(toolTipRects[0]);
            }

            for (int i = 0; i < toolTipRects.Length; i++)
            {
                SetNosePoint(Position, targetRects[i], toolTipRects[i]);

                PathGeometry geometry = GetClipPathGeometry(GetGeometryPoints(Position, toolTipRects[i]), Position);
                PathF path = new PathF();
                Size size = new Size(toolTipRects[i].Width, toolTipRects[i].Height);
                Rect textRect;
                if (Position == TooltipPosition.Right)
                {
                    Point translatePoint = new Point(0, ((LineSegment)geometry.Figures[0].Segments[7]).Point.Y);
                    textRect =
                        new Rect(((noseOffset + noseHeight) * animationValue[i]) - ((size.Width - (noseOffset + noseHeight)) / 2 * (1.0 - animationValue[i])), -translatePoint.Y,
                        size.Width - (noseOffset + noseHeight), size.Height);
                    AppendPath(path, geometry.Figures, translatePoint);
                }
                else if (Position == TooltipPosition.Left)
                {
                    Point translatePoint = new Point(((LineSegment)geometry.Figures[0].Segments[3]).Point.X + noseOffset, ((LineSegment)geometry.Figures[0].Segments[3]).Point.Y);
                    textRect =
                      new Rect((-translatePoint.X * animationValue[i]) - ((size.Width - (noseOffset + noseHeight)) / 2 * (1.0 - animationValue[i])), -translatePoint.Y,
                      size.Width - (noseOffset + noseHeight), size.Height);
                    AppendPath(path, geometry.Figures, translatePoint);
                }
                else
                {
                    Point translatePoint = new Point(((LineSegment)geometry.Figures[0].Segments[5]).Point.X, toolTipRects[i].Height);
                    textRect =
                       new Rect(-translatePoint.X, (-size.Height * animationValue[i]) - ((size.Height - (noseOffset + noseHeight)) / 2 * (1.0 - animationValue[i])),
                       size.Width, size.Height - (noseOffset + noseHeight));
                    AppendPath(path, geometry.Figures, translatePoint);
                }

                canvas.SaveState();
                canvas.Translate((float)targetRects[i].X, (float)targetRects[i].Y);
                canvas.Scale((float)animationValue[i], (float)animationValue[i]);

                if (((Paint)fill).ToColor() != Colors.Transparent)
                {
                    canvas.FillColor = Colors.Transparent;
                    canvas.SetFillPaint(fill, path.Bounds);
                    canvas.StrokeColor = Colors.Transparent;
                    canvas.FillPath(path);
                    canvas.DrawPath(path);
                }

                Paint strokePaint = stroke;
                if (strokePaint.ToColor() != Colors.Transparent)
                {
                    canvas.FillColor = Colors.Transparent;
                    canvas.StrokeColor = strokePaint.ToColor();
                    canvas.StrokeSize = (float)strokeSize;
                    canvas.FillPath(path);
                    canvas.DrawPath(path);
                }

                if (canDrawOverlappingStroke && strokePaint.ToColor() == Colors.Transparent)
                {
                    canvas.FillColor = Colors.Transparent;
                    canvas.StrokeColor = Colors.White;
                    canvas.StrokeSize = 0.5f;
                    canvas.FillPath(path);
                    canvas.DrawPath(path);
                }

                canvas.RestoreState();

                if (animationValue[i] == 1)
                {
                    canvas.DrawText(texts[i],
                        new Rect(textRect.X + (float)targetRects[i].X, textRect.Y + (float)targetRects[i].Y, textRect.Width, textRect.Height),
                        HorizontalAlignment.Center, VerticalAlignment.Center, this);
                }
                else if (animationValue[i] > 0)
                {
                    TooltipFont iTextElement = new TooltipFont(this);
                    iTextElement.FontSize = iTextElement.FontSize * animationValue[i];
                    canvas.DrawText(texts[i],
                        new Rect(textRect.X + (float)targetRects[i].X, textRect.Y + (float)targetRects[i].Y, textRect.Width, textRect.Height),
                        HorizontalAlignment.Center, VerticalAlignment.Center, iTextElement);
                }
            }
        }

        internal void UpdateDefaultVisual(float noseHeight, float noseWidth, float noseOffset, float cornerRadius)
        {
            this.noseHeight = noseHeight;
            this.noseWidth = noseWidth;
            this.noseOffset = noseOffset;
            CornerRadius = cornerRadius;
        }

        internal Rect[] GetToolTipRects(Rect containerRect, Rect[] targetRects, string[] texts)
        {
            int textLength = texts.Length;
            Rect[] toolTipRects = new Rect[textLength];
            for (int i = 0; i < textLength; i++)
            {
                string text = texts[i];
                if (text == null) break;
                Size size = text.Measure((float)fontSize, fontAttributes, fontFamily);
#if WINDOWS
                ContentSize = new Size(size.Width + Padding.HorizontalThickness, size.Height / 2 + Padding.VerticalThickness);
#else
                ContentSize = new Size(size.Width + Padding.HorizontalThickness, size.Height + Padding.VerticalThickness);
#endif
                toolTipRects[i] = GetToolTipRect(Position, targetRects[i], containerRect);
            }

            return toolTipRects;
        }

        internal void SetNosePoint(TooltipPosition position, Rect targetRect, Rect toolTipRect)
        {
            double noseOrigin = 0d;
            switch (position)
            {
                case TooltipPosition.Bottom:
                case TooltipPosition.Top:
                case TooltipPosition.Auto:
                    if (toolTipRect.Width < targetRect.Width)
                    {
                        noseOrigin = this.ContentSize.Width / 2;
                    }
                    else
                    {
                        noseOrigin = Math.Abs(toolTipRect.X - targetRect.X) + targetRect.Width / 2;
                    }

                    nosePoint = new Point(noseOrigin, (position == TooltipPosition.Auto || position == TooltipPosition.Top) ? toolTipRect.Height - noseOffset : noseOffset);
                    break;

                case TooltipPosition.Left:
                case TooltipPosition.Right:
                    if (toolTipRect.Height < targetRect.Height)
                    {
                        noseOrigin = this.ContentSize.Height / 2;
                    }
                    else
                    {
                        noseOrigin = Math.Abs(toolTipRect.Y - targetRect.Y) + targetRect.Height / 2;
                    }

                    nosePoint = new Point(position == TooltipPosition.Right ? noseOffset : toolTipRect.Width - noseOffset, noseOrigin);
                    break;
            }
        }

        internal Rect GetToolTipRect(TooltipPosition position, Rect targetRect, Rect containerRect)
        {
            double xPos = 0, yPos = 0;
            double width = ContentSize.Width;
            double height = ContentSize.Height;

            switch (position)
            {
                case TooltipPosition.Top:
                case TooltipPosition.Auto:
                    xPos = targetRect.Center.X - width / 2;
                    yPos = targetRect.Y - height - noseOffset - noseHeight;
                    height += noseOffset + noseHeight;
                    break;

                case TooltipPosition.Bottom:
                    xPos = targetRect.Center.X - width / 2;
                    yPos = targetRect.Bottom;
                    height += noseOffset + noseHeight;
                    break;

                case TooltipPosition.Right:
                    xPos = targetRect.Right;
                    yPos = targetRect.Center.Y - height / 2;
                    width += noseOffset + noseHeight;
                    break;

                case TooltipPosition.Left:
                    width += noseOffset + noseHeight;
                    xPos = targetRect.X - width;
                    yPos = targetRect.Center.Y - height / 2;
                    break;
            }

            var positionRect = new Rect(xPos, yPos, width, height);
            EdgedDetection(ref positionRect, containerRect);
            return positionRect;
        }

        internal PointCollection GetGeometryPoints(TooltipPosition position, Rect toolTipRect)
        {
            PointCollection points = new PointCollection();
            switch (position)
            {
                case TooltipPosition.Auto:
                case TooltipPosition.Top:
                    points.Add(new Point(0, 0));
                    points.Add(new Point(toolTipRect.Width, 0));
                    points.Add(new Point(toolTipRect.Width, toolTipRect.Height - noseHeight - noseOffset));
                    points.Add(new Point(nosePoint.X + noseWidth, nosePoint.Y - noseHeight));
                    points.Add(nosePoint);
                    points.Add(new Point(nosePoint.X - noseWidth, nosePoint.Y - noseHeight));
                    points.Add(new Point(0, toolTipRect.Height - noseHeight - noseOffset));
                    points.Add(new Point(0, 0));
                    points.Add(new Point(toolTipRect.Width, 0));
                    break;

                case TooltipPosition.Bottom:
                    points.Add(new Point(0, nosePoint.Y + noseHeight));
                    points.Add(new Point(nosePoint.X - noseWidth, nosePoint.Y + noseHeight));
                    points.Add(nosePoint);
                    points.Add(new Point(nosePoint.X + noseWidth, nosePoint.Y + noseHeight));
                    points.Add(new Point(toolTipRect.Width, nosePoint.Y + noseHeight));
                    points.Add(new Point(toolTipRect.Width, toolTipRect.Height));
                    points.Add(new Point(0, toolTipRect.Height));
                    points.Add(new Point(0, nosePoint.Y + noseHeight));
                    points.Add(new Point(nosePoint.X - noseWidth, nosePoint.Y + noseHeight));
                    break;

                case TooltipPosition.Right:
                    points.Add(new Point(nosePoint.X + noseHeight, 0));
                    points.Add(new Point(toolTipRect.Width, 0));
                    points.Add(new Point(toolTipRect.Width, toolTipRect.Height));
                    points.Add(new Point(nosePoint.X + noseHeight, toolTipRect.Height));
                    points.Add(new Point(nosePoint.X + noseHeight, nosePoint.Y + noseWidth));
                    points.Add(nosePoint);
                    points.Add(new Point(nosePoint.X + noseHeight, nosePoint.Y - noseWidth));
                    points.Add(new Point(nosePoint.X + noseHeight, 0));
                    points.Add(new Point(toolTipRect.Width, 0));
                    break;

                case TooltipPosition.Left:
                    points.Add(new Point(0, 0));
                    points.Add(new Point(toolTipRect.Width - noseHeight - noseOffset, 0));
                    points.Add(new Point(toolTipRect.Width - noseHeight - noseOffset, toolTipRect.Height / 2 - noseWidth));
                    points.Add(nosePoint);
                    points.Add(new Point(toolTipRect.Width - noseHeight - noseOffset, toolTipRect.Height / 2 + noseWidth));
                    points.Add(new Point(toolTipRect.Width - noseHeight - noseOffset, toolTipRect.Height));
                    points.Add(new Point(0, toolTipRect.Height));
                    points.Add(new Point(0, 0));
                    points.Add(new Point(toolTipRect.Width - noseHeight - noseOffset, 0));
                    break;
            }

            return points;
        }

        #endregion

        #region Private Methods

        private void EdgedDetection(ref Rect positionRect, Rect containerRect)
        {
            if (positionRect.X < 0)
            {
                positionRect.X = 0;
            }
            else if (positionRect.Right > containerRect.Width)
            {
                positionRect.X = containerRect.Width - positionRect.Width;
            }

            if (positionRect.Y < 0)
            {
                positionRect.Y = 0;
            }
            else if (positionRect.Bottom > containerRect.Height)
            {
                positionRect.Y = containerRect.Height - positionRect.Height;
            }
        }

        private PathGeometry GetClipPathGeometry(PointCollection points, TooltipPosition position)
        {
            PathFigure figure = new PathFigure();

            if (points.Count > 0)
            {
                figure.StartPoint = points[0];

                if (points.Count > 1)
                {
                    double desiredRadius = CornerRadius;
                    LineSegment line = new LineSegment();

                    for (int i = 1; i < (points.Count - 1); i++)
                    {
                        switch (position)
                        {
                            case TooltipPosition.Left:
                                if (i == 2 || i == 3 || i == 4)
                                {
                                    line = new LineSegment(points[i]);
                                    figure.Segments.Add(line);
                                    continue;
                                }
                                break;
                            case TooltipPosition.Auto:
                            case TooltipPosition.Top:
                                if (i == 3 || i == 4 || i == 5)
                                {
                                    line = new LineSegment(points[i]);
                                    figure.Segments.Add(line);
                                    continue;
                                }
                                break;
                            case TooltipPosition.Right:
                                if (i == 4 || i == 5 || i == 6)
                                {
                                    line = new LineSegment(points[i]);
                                    figure.Segments.Add(line);
                                    continue;
                                }
                                break;
                            case TooltipPosition.Bottom:
                                if (i == 1 || i == 2 || i == 3)
                                {
                                    line = new LineSegment(points[i]);
                                    figure.Segments.Add(line);
                                    continue;
                                }
                                break;
                        }

                        Point begin = new Point(points[i].X - points[i - 1].X, points[i].Y - points[i - 1].Y);
                        Point end = new Point(points[i + 1].X - points[i].X, points[i + 1].Y - points[i].Y);
                        double beginLength = Math.Sqrt(begin.X * begin.X + begin.Y * begin.Y);
                        double endLength = Math.Sqrt(end.X * end.X + end.Y * end.Y);
                        double radius = Math.Min(Math.Min(beginLength, endLength) / 2, desiredRadius);

                        // Normalized begin.
                        if (beginLength != 0)
                        {
                            begin.X /= beginLength;
                            begin.Y /= beginLength;
                        }
                        else
                        {
                            begin = new Point();
                        }

                        double beginRadius = beginLength - radius;
                        begin.X *= beginRadius;
                        begin.Y *= beginRadius;
                        line = new LineSegment(points[i - 1].Offset(begin.X, begin.Y));
                        figure.Segments.Add(line);

                        // Normalized end.
                        if (endLength != 0)
                        {
                            end.X /= endLength;
                            end.Y /= endLength;
                        }
                        else
                        {
                            end = new Point();
                        }

                        end.X *= radius;
                        end.Y *= radius;
                        ArcSegment arc = new ArcSegment(points[i].Offset(end.X, end.Y), new Size(radius), 0, SweepDirection.Clockwise, false);
                        figure.Segments.Add(arc);
                    }

                    figure.Segments.Add(new LineSegment(points[points.Count - 1]));
                }
            }

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);
            return geometry;
        }

        private void AppendPath(PathF path, PathFigureCollection figures, Point translatePoint)
        {
            foreach (PathFigure pathFigure in figures)
            {
                for (int i = 0; i < pathFigure.Segments.Count - 1; i++)
                {
                    var pathSegment = pathFigure.Segments[i];
                    if (pathSegment is LineSegment lineSegment)
                    {
                        AddLine(path, lineSegment, translatePoint);
                    }
                    else if (pathSegment is ArcSegment arcSegment)
                    {
                        AddCurve(path, arcSegment, translatePoint, i);
                    }
                }
                path.Close();
            }
        }

        private void AddLine(PathF path, LineSegment lineSegment, Point translatePoint)
        {
            path.LineTo((float)(lineSegment.Point.X - translatePoint.X),
                (float)(lineSegment.Point.Y - translatePoint.Y));
        }

        private void AddCurve(PathF path, ArcSegment arcSegment, Point translatePoint, int index)
        {
            float x = (float)(arcSegment.Point.X - translatePoint.X);
            float y = (float)(arcSegment.Point.Y - translatePoint.Y);
            float desiredRadius = CornerRadius - (CornerRadius * 0.55f);
            if (index == 1)
            {
                path.CurveTo(
                    new PointF(x - desiredRadius, (float)y - CornerRadius),
                    new PointF(x, (float)y - CornerRadius + desiredRadius),
                    new PointF(x, y));
            }
            else if (index == 3 || index == 6)
            {
                path.CurveTo(
                   new PointF(x + CornerRadius, y - desiredRadius),
                   new PointF(x + CornerRadius - desiredRadius, y),
                   new PointF(x, y));
            }
            else if (index == 8 || index == 5)
            {
                path.CurveTo(
                   new PointF(x + desiredRadius, y + CornerRadius),
                   new PointF(x, y + CornerRadius - desiredRadius),
                   new PointF(x, y));
            }
            else if (index == 10)
            {
                path.CurveTo(
                   new PointF(x - CornerRadius, y + desiredRadius),
                   new PointF(x - CornerRadius + desiredRadius, y),
                   new PointF(x, y));
            }
        }

        #endregion
    }

    internal class TooltipFont : ITextElement
    {
        #region Properties

        private Font font;

        public Font Font
        {
            get
            {
                return font;
            }
            set
            {
                if (font != value)
                    font = value;
            }
        }

        private double fontSize = 14;

        public double FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                if (fontSize != value)
                    fontSize = value;
                Font = font.WithSize(fontSize);
            }
        }

        private string fontFamily = string.Empty;

        public string FontFamily
        {
            get
            {
                return fontFamily;
            }
            set
            {
                if (fontFamily != value)
                    fontFamily = value;
                Font = Font.OfSize(fontFamily, fontSize, enableScaling: false).WithAttributes(fontAttributes);
            }
        }

        private FontAttributes fontAttributes = FontAttributes.None;

        public FontAttributes FontAttributes
        {
            get
            {
                return fontAttributes;
            }
            set
            {
                if (fontAttributes != value)
                    fontAttributes = value;
                Font = font.WithAttributes(fontAttributes);
            }
        }

        private Color textColor = Colors.White;

        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                if (textColor != value)
                    textColor = value;
            }
        }

        #endregion

        #region Constructor

        internal TooltipFont(ITextElement element)
        {
            Font = element.Font;
            FontAttributes = element.FontAttributes;
            FontFamily = element.FontFamily;
            FontSize = element.FontSize;
            TextColor = element.TextColor;
        }

        #endregion

        #region Methods

        double ITextElement.FontSizeDefaultValueCreator()
        {
            if (FontSize <= 0)
                return 0.1;

            return FontSize;
        }

        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {
        }

        void ITextElement.OnFontChanged(Font oldValue, Font newValue)
        {
        }

        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {
        }

        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {
        }

        #endregion
    }
}
