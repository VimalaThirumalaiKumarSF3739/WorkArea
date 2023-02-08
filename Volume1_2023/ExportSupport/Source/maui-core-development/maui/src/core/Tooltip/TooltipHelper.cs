﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Animation = Microsoft.Maui.Animations.Animation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Font = Microsoft.Maui.Font;
using Microsoft.Maui.Animations;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// This class represents as a helper class for rendering the tooltip with text using canvas of drawable view.
    /// </summary>
    internal class TooltipHelper : ITextElement
    {
        #region Fields

        readonly Action callback;
        internal float noseHeight = 5f;
        internal float noseWidth = 6f;
        internal float noseOffset = 2f;
        float cornerRadius = 4f;
        Point nosePoint;
        double animationValue = 1;
        TooltipPosition actualPosition;
        IAnimationManager? animationManager = null;
        Animation? durationAnimation;

        #endregion

        #region Properties

        #region Internal properties

        internal Rect TooltipRect { get; set; }
        internal Rect RoundedRect { get; set; }
        internal Thickness ContentViewMargin { get; set; }
        internal Size ContentSize { get; set; }
        internal TooltipPosition PriorityPosition { get; set; } = TooltipPosition.Top;
        internal List<TooltipPosition> PriorityPositionList { get; set; } = new List<TooltipPosition>() { TooltipPosition.Bottom, TooltipPosition.Left, TooltipPosition.Right };
        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the text for tooltip.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or set the font for tooltip.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the font size for the tooltip.
        /// </summary>
        public double FontSize { get; set; } = 14;
       
        /// <summary>
        /// Gets or sets the font family for the tooltip.
        /// </summary>
        public string FontFamily { get; set; } = "TimesNewRoman";

        /// <summary>
        /// Gets or sets the font attributes for the tooltip.
        /// </summary>
        public FontAttributes FontAttributes { get; set; } = FontAttributes.None;
        
        /// <summary>
        /// Gets or sets the position for the tooltip.
        /// </summary>
        public TooltipPosition Position { get; set; } = TooltipPosition.Auto;

        /// <summary>
        /// Gets or sets the draw type of tooltip UI.
        /// </summary>
        public TooltipDrawType DrawType { get; set; } = TooltipDrawType.Default;

        /// <summary>
        /// Gets or sets the padding of tooltip.
        /// </summary>
        public Thickness Padding { get; set; } = new Thickness(5);

        /// <summary>
        /// Gets or sets the tooltip visible duration.
        /// </summary>
        public int Duration { get; set; } = 2;

        /// <summary>
        /// Gets or sets the background for the tooltip.
        /// </summary>
        public Brush Background { get; set; } = Brush.Black;

        /// <summary>
        /// Gets or sets the stroke for the tooltip.
        /// </summary>
        public Brush Stroke { get; set; }  = Brush.Transparent;

        /// <summary>
        /// Gets or sets the text color for the tooltip.
        /// </summary>
        public Color TextColor { get; set; } = Colors.White;

        /// <summary>
        /// Gets or sets the strokewidth for the tooltip.
        /// </summary>
        public float StrokeWidth { get; set; } = 0f;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new instance of the <see cref="TooltipHelper"/> class.
        /// </summary>
        /// <param name="action"></param>
        public TooltipHelper(Action action)
        {
            callback = action;
        }

        #endregion

        #region Methods

        #region Public methods

        /// <summary>
        /// Shows the tooltip based on target and container rectangle.
        /// </summary>
        /// <param name="containerRect"></param>
        /// <param name="targetRect"></param>
        /// <param name="animated"></param>
        public void Show(Rect containerRect, Rect targetRect, bool animated)
        {
            if (containerRect.IsEmpty || (string.IsNullOrEmpty(Text) && ContentSize.IsZero)) return;

            var x = containerRect.X;
            var y = containerRect.Y;
            var width = containerRect.Width;
            var height = containerRect.Height;

            if (targetRect.X > x + width || targetRect.Y > y + height) return;

            bool isContentView = !ContentSize.IsZero;
            Size size = isContentView ? ContentSize : Text.Measure(this);
            Size contentSize = new Size(size.Width + Padding.Left + Padding.Right + StrokeWidth, size.Height + Padding.Top + Padding.Bottom + StrokeWidth);
            Rect tooltipRect = GetTooltipRect(Position, containerRect, targetRect, contentSize);
            actualPosition = GetAcutalTooltipPosition(containerRect, targetRect, ref tooltipRect, contentSize, Position, noseHeight, noseOffset);
            GetNosePoint(actualPosition, targetRect, tooltipRect, contentSize, isContentView);

            if (!isContentView)
            {
                TooltipRect = GetDrawRect(tooltipRect, actualPosition, noseHeight, noseOffset);
                
                //TODO: Need to implement the animation support.
                //if (animated)
                //{
                //    CommitToolTipAnimation(0, 1);
                //}
                //else
                //{
                    Invalidate();
                    Finished();
                //}
            }
            else
            {
                TooltipRect = tooltipRect;
                RoundedRect = GetRoundedRect();
            }
        }

        /// <summary>
        /// Hides the tooltip.
        /// </summary>
        /// <param name="animated"></param>
        public void Hide(bool animated)
        {
            TooltipRect = Rect.Zero;
            Invalidate();
        }

        /// <summary>
        /// Draws the tooltip.
        /// </summary>
        /// <param name="canvas"></param>
        public void Draw(ICanvas canvas)
        {
            OnDraw(canvas);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Draw the toolip based on the draw type.
        /// </summary>
        /// <param name="canvas"></param>
        protected virtual void OnDraw(ICanvas canvas)
        {
            if ((string.IsNullOrEmpty(Text) && ContentSize.IsZero) || TooltipRect.IsEmpty) return;

            switch (DrawType)
            {
                case TooltipDrawType.Default:
                    DrawDefaultTooltip(canvas);
                    break;
                case TooltipDrawType.Paddle:
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Private methods

        void Invalidate()
        {
            callback.Invoke();
        }

        #region Animation methods

        void SetAnimationManager()
        {
            if (Application.Current != null && animationManager == null)
            {
                var handler = Application.Current.Handler;
                if (handler != null && handler.MauiContext != null)
                    animationManager = handler.MauiContext.Services.GetRequiredService<IAnimationManager>();
            }
        }

        void UpdateToolTipAnimation(double value)
        {
            animationValue = value;
            Invalidate();
        }

        void CommitToolTipAnimation(double start, double end)
        {
            SetAnimationManager();
            if (animationManager != null)
            {
                Animation animation = new Animation(UpdateToolTipAnimation, finished: Finished);
                Animation cubicAnimation = new Animation(UpdateToolTipAnimation, start, end, Easing.CubicInOut);
                animation.Add(0, 1, cubicAnimation);
                animation.Commit(animationManager);
            }
        }

        void Finished()
        {
            SetAnimationManager();
            if (animationManager != null)
            {
                durationAnimation = null;
                durationAnimation = new Animation(Animate, start:0, duration:Duration, Easing.Linear, finished: AutoHide);
                durationAnimation.Commit(animationManager);
            }
        }

        void Animate(double value)
        {

        }

        void AutoHide()
        {
            if (durationAnimation != null && durationAnimation.HasFinished)
                Hide(false);
        }
      
        Rect GetAnimationRect()
        {
            var rect = TooltipRect;
            var x = TooltipRect.X;
            var y = TooltipRect.Y;
            var width = TooltipRect.Width;
            var height = TooltipRect.Height;
            var value = animationValue;
            
            var tooltipPosition = actualPosition == TooltipPosition.Auto ? PriorityPosition : actualPosition;

            switch (tooltipPosition)
            {
                case TooltipPosition.Top:
                    rect = new Rect(x + (width / 2 * (1 - value)), y + (height * (1 - value)), width * value, height * value);
                    break;
                case TooltipPosition.Left:
                    rect = new Rect(x + (width * (1 - value)), y + (height / 2 * (1 - value)), width * value, height * value);
                    break;
                case TooltipPosition.Right:
                    rect = new Rect(x, y + (height / 2 * (1 - value)), width * value, height * value);
                    break;
                case TooltipPosition.Bottom:
                    rect = new Rect(x + (width / 2 * (1 - value)), y, width * value, height * value);
                    break;
                default:
                    break;
            }
            return rect;
        }

        #endregion

        #region Calculation methods

        Rect GetTooltipRect(TooltipPosition position, Rect containerRect, Rect targetRect, Size contentSize)
        {
            double xPos = 0, yPos = 0;
            double width = contentSize.Width;
            double height = contentSize.Height;
            var tooltipPosition = position == TooltipPosition.Auto ? PriorityPosition : position;
            switch (tooltipPosition)
            {
                case TooltipPosition.Top:
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

        void EdgedDetection(ref Rect positionRect, Rect containerRect)
        {
            if (Position == TooltipPosition.Auto || Position == TooltipPosition.Top || Position == TooltipPosition.Bottom)
            {
                if (positionRect.X < 0)
                {
                    positionRect.X = 0 + StrokeWidth/2;
                }
                else if (positionRect.Right > containerRect.Width)
                {
                    positionRect.X = containerRect.Width - positionRect.Width - StrokeWidth/2;
                }
            }

            if (Position == TooltipPosition.Auto)
            {
                if (positionRect.Y < 0)
                {
                    positionRect.Y = 0;
                }
                else if (positionRect.Bottom > containerRect.Height)
                {
                    positionRect.Y = containerRect.Height - positionRect.Height;
                }
            }
        }

        TooltipPosition GetAcutalTooltipPosition(Rect containerRect, Rect targetRect, ref Rect tooltipRect, Size contentSize, TooltipPosition tooltipPosition, float noseHeight, float noseOffset)
        {
            TooltipPosition actualPosition = tooltipPosition;

            if (actualPosition == TooltipPosition.Auto && !IsTargetRectIntersectsWith(tooltipRect, targetRect))
            {
                List<TooltipPosition> list = PriorityPositionList;

                foreach (TooltipPosition position in list)
                {
                    var newFrame = GetTooltipRect(position, containerRect, targetRect, contentSize);

                    if (IsTargetRectIntersectsWith(newFrame, targetRect))
                    {
                        tooltipRect = newFrame;
                        actualPosition = position;
                        break;
                    }
                }
            }

            return actualPosition == TooltipPosition.Auto ? PriorityPosition : actualPosition;
        }

        Rect GetDrawRect(Rect tooltipRect, TooltipPosition actualPosition, float noseHeight, float noseOffset)
        {
            var offset = noseOffset + noseHeight;

            switch (actualPosition)
            {
                case TooltipPosition.Top:
                    tooltipRect.Y -= StrokeWidth;
                    tooltipRect.Height = tooltipRect.Height - offset;
                    break;

                case TooltipPosition.Bottom:
                    tooltipRect.Y += offset + StrokeWidth;
                    tooltipRect.Height = tooltipRect.Height - offset;
                    break;

                case TooltipPosition.Right:
                    tooltipRect.X += offset + StrokeWidth;
                    tooltipRect.Width = tooltipRect.Width - offset;
                    break;

                case TooltipPosition.Left:
                    tooltipRect.X -= StrokeWidth;
                    tooltipRect.Width = tooltipRect.Width - offset;
                    break;
            }

            return tooltipRect;
        }

        Rect GetRoundedRect()
        {
            var roundedRect = Rect.Zero;
            var offset = noseOffset + noseHeight;
            var width = TooltipRect.Width;
            var height = TooltipRect.Height;
            var margin = Padding;

            switch (actualPosition)
            {
                case TooltipPosition.Top:
                    margin.Bottom += offset;
                    roundedRect = new Rect(new Point(0, 0), new Size(width, height - offset));
                    break;

                case TooltipPosition.Bottom:
                    margin.Top += offset;
                    roundedRect = new Rect(new Point(0, offset), new Size(width, height - offset));
                    break;

                case TooltipPosition.Right:
                    margin.Left += offset;
                    roundedRect = new Rect(new Point(offset, 0), new Size(width - offset, height));
                    break;

                case TooltipPosition.Left:
                    margin.Right += offset;
                    roundedRect = new Rect(new Point(0, 0), new Size(width - offset, height));
                    break;
            }

            ContentViewMargin = margin;
            return roundedRect;
        }
        
        bool IsTargetRectIntersectsWith(Rect tooltipRect, Rect targetRect)
        {
            return !tooltipRect.IntersectsWith(targetRect);
        }

        void GetNosePoint(TooltipPosition position, Rect targetRect, Rect tooltipRect, Size contentSize, bool isContentView)
        {
            double noseOrigin = 0d;

            if (!isContentView)
            {
                switch (position)
                {
                    case TooltipPosition.Bottom:
                    case TooltipPosition.Top:
                        if (tooltipRect.Width < targetRect.Width)
                        {
                            noseOrigin = tooltipRect.X + contentSize.Width / 2;
                        }
                        else
                        {
                            noseOrigin = Math.Abs(targetRect.X + targetRect.Width / 2);
                        }

                        nosePoint = new Point(noseOrigin, (position == TooltipPosition.Auto || position == TooltipPosition.Top) ? targetRect.Y - noseOffset - StrokeWidth : tooltipRect.Y + noseOffset + StrokeWidth);
                        break;

                    case TooltipPosition.Left:
                    case TooltipPosition.Right:
                        if (tooltipRect.Height < targetRect.Height)
                        {
                            noseOrigin = tooltipRect.Y + contentSize.Height / 2;
                        }
                        else
                        {
                            noseOrigin = Math.Abs(targetRect.Y + targetRect.Height / 2);
                        }

                        nosePoint = new Point(position == TooltipPosition.Right ? tooltipRect.X + noseOffset + StrokeWidth: tooltipRect.X + tooltipRect.Width - noseOffset - StrokeWidth, noseOrigin);
                        break;
                }
            }
            else
            {
                switch (position)
                {
                    case TooltipPosition.Bottom:
                    case TooltipPosition.Top:
                        if (tooltipRect.Width < targetRect.Width)
                        {
                            noseOrigin = contentSize.Width / 2;
                        }
                        else
                        {
                            noseOrigin = Math.Abs(tooltipRect.X - targetRect.X) + targetRect.Width / 2;
                        }

                        nosePoint = new Point(noseOrigin, (position == TooltipPosition.Auto || position == TooltipPosition.Top) ? tooltipRect.Height - noseOffset : noseOffset);
                        break;

                    case TooltipPosition.Left:
                    case TooltipPosition.Right:
                        if (tooltipRect.Height < targetRect.Height)
                        {
                            noseOrigin = contentSize.Height / 2;
                        }
                        else
                        {
                            noseOrigin = Math.Abs(tooltipRect.Y - targetRect.Y) + targetRect.Height / 2;
                        }

                        nosePoint = new Point(position == TooltipPosition.Right ? noseOffset : tooltipRect.Width - noseOffset, noseOrigin);
                        break;
                }
            }
        }

        #endregion

        #region Draw methods
        
        PointCollection GetPointCollection(Rect roundedRect)
        {
            PointCollection points = new PointCollection();
            var drawRadius = cornerRadius * (1 - 0.552228474);
            var rect = roundedRect;
            var x = rect.X;
            var y = rect.Y;
            var width = rect.Width;
            var height = rect.Height;
            switch (actualPosition)
            {
                case TooltipPosition.Top:
                    points.Add(new Point(x + cornerRadius, y));
                    points.Add(new Point(x + width - cornerRadius, y));
                    points.Add(new Point(x + width - drawRadius, y)); //c1
                    points.Add(new Point(x + width, y + drawRadius)); //c1
                    points.Add(new Point(x + width, y + cornerRadius));
                    points.Add(new Point(x + width, y + height - cornerRadius));
                    points.Add(new Point(x + width, y + height - drawRadius));//c2
                    points.Add(new Point(x + width - drawRadius, y + height));//c2
                    points.Add(new Point(x + width - cornerRadius, y + height));
                    points.Add(new Point(nosePoint.X + noseWidth, nosePoint.Y - noseHeight));
                    points.Add(nosePoint);
                    points.Add(new Point(nosePoint.X - noseWidth, nosePoint.Y - noseHeight));
                    points.Add(new Point(x + cornerRadius, y + height));
                    points.Add(new Point(x + drawRadius, y + height));//c3
                    points.Add(new Point(x, y + height - drawRadius));//c3
                    points.Add(new Point(x, y + height - cornerRadius));
                    points.Add(new Point(x, y + cornerRadius));
                    points.Add(new Point(x, y + drawRadius));//c4
                    points.Add(new Point(x + drawRadius, y));//c4
                    points.Add(new Point(x + cornerRadius, y));
                    break;

                case TooltipPosition.Bottom:
                    points.Add(new Point(x + width - cornerRadius, y + height));
                    points.Add(new Point(x + cornerRadius, y + height));
                    points.Add(new Point(x + drawRadius, y + height));//c1
                    points.Add(new Point(x, y + height - drawRadius));//c1
                    points.Add(new Point(x, y + height - cornerRadius));
                    points.Add(new Point(x, y + cornerRadius));
                    points.Add(new Point(x, y + drawRadius));//c2
                    points.Add(new Point(x + drawRadius, y));//c2
                    points.Add(new Point(x + cornerRadius, y));
                    points.Add(new Point(nosePoint.X - noseWidth, nosePoint.Y + noseHeight));
                    points.Add(nosePoint);
                    points.Add(new Point(nosePoint.X + noseWidth, nosePoint.Y + noseHeight));
                    points.Add(new Point(x + width - cornerRadius, y));
                    points.Add(new Point(x + width - drawRadius, y));//c3
                    points.Add(new Point(x + width, y + drawRadius));//c3
                    points.Add(new Point(x + width, y + cornerRadius));
                    points.Add(new Point(x + width, y + height - cornerRadius));
                    points.Add(new Point(x + width, y + height - drawRadius));//c4
                    points.Add(new Point(x + width - drawRadius, y + height));//c4
                    points.Add(new Point(x + width - cornerRadius, y + height));
                    break;

                case TooltipPosition.Right:
                    points.Add(new Point(x + width, y + cornerRadius));
                    points.Add(new Point(x + width, y + height - cornerRadius));
                    points.Add(new Point(x + width, y + height - drawRadius));//c1
                    points.Add(new Point(x + width - drawRadius, y + height));//c1
                    points.Add(new Point(x + width - cornerRadius, y + height));
                    points.Add(new Point(x + cornerRadius, y + height));
                    points.Add(new Point(x + drawRadius, y + height));//c2
                    points.Add(new Point(x, y + height - drawRadius));//c2
                    points.Add(new Point(x, y + height - cornerRadius));
                    points.Add(new Point(nosePoint.X + noseHeight, nosePoint.Y + noseWidth));
                    points.Add(nosePoint);
                    points.Add(new Point(nosePoint.X + noseHeight, nosePoint.Y - noseWidth));
                    points.Add(new Point(x, y + cornerRadius));
                    points.Add(new Point(x, y + drawRadius));//c3
                    points.Add(new Point(x + drawRadius, y));//c3
                    points.Add(new Point(x + cornerRadius, y));
                    points.Add(new Point(x + width - cornerRadius, y));
                    points.Add(new Point(x + width - drawRadius, y));//c4
                    points.Add(new Point(x + width, y + drawRadius));//c4
                    points.Add(new Point(x + width, y + cornerRadius));
                    break;

                case TooltipPosition.Left:
                    points.Add(new Point(x, y + height - cornerRadius));
                    points.Add(new Point(x, y + cornerRadius));
                    points.Add(new Point(x, y + drawRadius));//c1
                    points.Add(new Point(x + drawRadius, y));//c1
                    points.Add(new Point(x + cornerRadius, y));
                    points.Add(new Point(x + width - cornerRadius, y));
                    points.Add(new Point(x + width - drawRadius, y));//c2
                    points.Add(new Point(x + width, y + drawRadius));//c2
                    points.Add(new Point(x + width, y + cornerRadius));
                    points.Add(new Point(nosePoint.X - noseHeight, nosePoint.Y + noseWidth));
                    points.Add(nosePoint);
                    points.Add(new Point(nosePoint.X - noseHeight, nosePoint.Y - noseWidth));
                    points.Add(new Point(x + width, y + height - cornerRadius));
                    points.Add(new Point(x + width, y + height - drawRadius));//c3
                    points.Add(new Point(x + width - drawRadius, y + height));//c3
                    points.Add(new Point(x + width - cornerRadius, y + height));
                    points.Add(new Point(x + cornerRadius, y + height));
                    points.Add(new Point(x + drawRadius, y + height));//c4
                    points.Add(new Point(x, y + height - drawRadius));//c4
                    points.Add(new Point(x, y + height - cornerRadius));
                    break;
            }

            return points;
        }

        PathF GetDrawPath(PointCollection points)
        {
            var path = new PathF();

            if (points != null)
            {
                path.MoveTo(points[0]);
                path.LineTo(points[1]);
                path.CurveTo(points[2], points[3], points[4]);
                path.LineTo(points[5]);
                path.CurveTo(points[6], points[7], points[8]);
                path.LineTo(points[9]);
                path.LineTo(points[10]);
                path.LineTo(points[11]);
                path.LineTo(points[12]);
                path.CurveTo(points[13], points[14], points[15]);
                path.LineTo(points[16]);
                path.CurveTo(points[17], points[18], points[19]);
            }

            path.Close();
            return path;
        }

        void DrawDefaultTooltip(ICanvas canvas)
        {
            var rect = animationValue >= 1 ? TooltipRect : GetAnimationRect();
            var points = GetPointCollection(ContentSize.IsZero ? rect : RoundedRect);
            var path = GetDrawPath(points);

            canvas.SaveState();
            canvas.StrokeColor = ((Paint)Stroke).ToColor();
            canvas.StrokeSize = StrokeWidth;
            canvas.FillColor = ((Paint)Background).ToColor();
            canvas.FillPath(path);
            canvas.DrawPath(path);

            if (!string.IsNullOrEmpty(Text))
            {
                if (animationValue < 1)
                {
                    TooltipFont iTextElement = new TooltipFont(this);
                    double fontSize = iTextElement.FontSize * animationValue;
                    iTextElement.FontSize = fontSize < 1 ? 1 : fontSize;
                    canvas.DrawText(Text, rect, HorizontalAlignment.Center, VerticalAlignment.Center, iTextElement);
                }
                else
                {
                    canvas.DrawText(Text, rect, HorizontalAlignment.Center, VerticalAlignment.Center, this);
                }
            }

            canvas.RestoreState();
        }

        #endregion

        #region Property methods

        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {
        }

        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {
        }

        double ITextElement.FontSizeDefaultValueCreator()
        {
            throw new NotImplementedException();
        }

        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {
        }

        void ITextElement.OnFontChanged(Font oldValue, Font newValue)
        {
        }

        #endregion

        #endregion

        #endregion
    }
}
