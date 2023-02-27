// <copyright file="BadgeLabelView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;
    using Syncfusion.Maui.Graphics.Internals;
    using CanvasExtensions = Syncfusion.Maui.Graphics.Internals.CanvasExtensions;

    /// <summary>
    /// Represents the BadgeLabelView class.
    /// </summary>
    internal class BadgeLabelView
    {
        #region Fields

        private bool animationEnabled;

        private ITextElement? textElement;

        private View? content;

        private BadgePosition badgePosition = BadgePosition.TopRight;

        private BadgeIcon badgeIcon = BadgeIcon.None;

        private double xPos, yPos;

        private Color stroke = Colors.Transparent;

        private double strokeThickness;

        private Brush badgeBackground = new SolidColorBrush(Colors.Transparent);

        private Color textColor = Colors.Transparent;

        private string text = string.Empty;
		
		private string screenReaderText = String.Empty;

        private Thickness textPadding;

        private CornerRadius cornerRadius;

        private float fontSize = 12;

        private string fontFamily = string.Empty;

        private FontAttributes fontAttributes = FontAttributes.None;

        private bool autoHide;

        private Size textSize;

        private double sizeRatio = 1;

        private readonly SfBadgeView badgeView;

        private RectF badgeSize;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeLabelView"/> class.
        /// </summary>
        public BadgeLabelView(SfBadgeView badgeView)
        {
            this.badgeView = badgeView;
        }

        #endregion

        #region Properties

        internal double WidthRequest { get; set; }

        internal double HeightRequest { get; set; }

        internal Thickness Margin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the animation is enabled or not.
        /// </summary>
        internal bool AnimationEnabled
        {
            get { return this.animationEnabled; }
            set { this.animationEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the value that defines the text element.
        /// </summary>
        internal ITextElement? TextElement
        {
            get { return this.textElement; }
            set { this.textElement = value; }
        }

        /// <summary>
        /// Gets or sets the value that defines the content.
        /// </summary>
        internal View? Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        /// <summary>
        /// Gets or sets the value that defines the badge icon.
        /// </summary>
        internal BadgeIcon BadgeIcon
        {
            get
            {
                return this.badgeIcon;
            }

            set
            {
                this.badgeIcon = value;
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the badge position.
        /// </summary>
        internal BadgePosition BadgePosition
        {
            get
            {
                return this.badgePosition;
            }
            set
            {
                this.badgePosition = value;
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the x position.
        /// </summary>
        internal double XPosition
        {
            get
            {
                return xPos;
            }
            set
            {
                this.xPos = value;
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the y position.
        /// </summary>
        internal double YPosition
        {
            get
            {
                return yPos;
            }
            set
            {
                this.yPos = value;
            }
        }
        /// <summary>
        /// Gets or sets the value that defines the stroke thickness.
        /// </summary>
        internal double StrokeThickness
        {
            get
            {
                return this.strokeThickness;
            }

            set
            {
                this.strokeThickness = value;
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the stroke color.
        /// </summary>
        internal Color Stroke
        {
            get
            {
                return this.stroke;
            }

            set
            {
                this.stroke = value;
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the background color of the badge.
        /// </summary>
        internal Brush BadgeBackground
        {
            get
            {
                return this.badgeBackground;
            }

            set
            {
                this.badgeBackground = value;
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the text color.
        /// </summary>
        internal Color TextColor
        {
            get
            {
                return this.textColor;
            }

            set
            {
                this.textColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the badge text.
        /// </summary>
        internal string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
                this.CalculateBadgeBounds();
                this.ShowBadgeBasedOnText(this.text, this.AutoHide);
                this.UpdateSemanticProperties();
            }
        }


        /// <summary>
        /// Gets or sets the value for screen reader text.
        /// </summary>
        internal string ScreenReaderText
        {
            get
            {
                return this.screenReaderText;
            }
            set
            {
                this.screenReaderText = value;
                this.UpdateSemanticProperties();
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the text padding.
        /// </summary>
        internal Thickness TextPadding
        {
            get
            {
                return this.textPadding;
            }

            set
            {
                this.textPadding = value;
                this.CalculateBadgeBounds();
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the corner radius.
        /// </summary>
        internal CornerRadius CornerRadius
        {
            get
            {
                return this.cornerRadius;
            }

            set
            {
                this.cornerRadius = value;
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the font size.
        /// </summary>
        internal float FontSize
        {
            get
            {
                return this.fontSize;
            }

            set
            {
                this.fontSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the font family.
        /// </summary>
        internal string FontFamily
        {
            get
            {
                return this.fontFamily;
            }

            set
            {
                this.fontFamily = value;
            }
        }

        /// <summary>
        /// Gets or sets the value that defines the font attributes.
        /// </summary>
        internal FontAttributes FontAttributes
        {
            get
            {
                return this.fontAttributes;
            }

            set
            {
                this.fontAttributes = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the badge text is AutoHide or not.
        /// </summary>
        internal bool AutoHide
        {
            get
            {
                return this.autoHide;
            }

            set
            {
                this.autoHide = value;
                this.ShowBadgeBasedOnText(this.Text, this.autoHide);
            }
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// To get the start point of the text.
        /// </summary>
        /// <param name="viewBounds">viewBounds.</param>
        /// <returns>It returns the start point of the text.</returns>
        internal PointF GetTextStartPoint(Rect viewBounds)
        {
            PointF startPoint = default(PointF);
            startPoint.X = (float)(viewBounds.Center.X - (this.textSize.Width / 2));
            startPoint.Y = (float)(viewBounds.Center.Y - (this.textSize.Height / 2));
            if (!string.IsNullOrEmpty(this.Text)) 
            { 
#if __ANDROID__
                startPoint.Y = (float)(viewBounds.Center.Y + (this.textSize.Height / 2.5));
#elif __IOS__ || __MACCATALYST__
                startPoint.Y = (float)(viewBounds.Center.Y - (this.textSize.Height / 2));
#endif
            }

            return startPoint;
        }

        /// <summary>
        /// To calculate the badge bounds.
        /// </summary>
        /// <returns>It returns the badge bounds.</returns>
        internal void CalculateBadgeBounds()
        {
            RectF rect = default(RectF);

            if (!string.IsNullOrEmpty(this.Text))
            {
                var textMeasurer = TextMeasurer.CreateTextMeasurer();
                this.textSize = textMeasurer.MeasureText(this.Text, this.TextElement!);
            }
            else if (this.BadgeIcon != BadgeIcon.None && this.BadgeIcon != BadgeIcon.Dot)
            {
                this.textSize = new Size(this.GetProccessedFontSize(), this.GetProccessedFontSize());
            }

            rect.Width = (float)(this.textSize.Width + this.TextPadding.Left + this.TextPadding.Right);
            rect.Height = (float)(this.textSize.Height + this.TextPadding.Top + this.TextPadding.Bottom);

            if (!string.IsNullOrEmpty(this.Text))
            {
                if (this.Text.Length == 1 && this.TextPadding.Left == this.TextPadding.Right &&
                    this.TextPadding.Top == this.TextPadding.Bottom &&
                    this.TextPadding.Left == this.TextPadding.Top)
                {
                    if (rect.Width > rect.Height)
                    {
                        rect.Height = rect.Width;
                    }
                    else
                    {
                        rect.Width = rect.Height;
                    }
                }
                else if (this.Text.Length > 1)
                {
                    // Added some default padding for two or more digits.
                    rect.Width += 10;
                }
            }
            else
            {
                rect.Width = (this.badgeIcon == BadgeIcon.Dot) ? 10 : (this.badgeIcon == BadgeIcon.None) ? 0 : rect.Width;
                rect.Height = (this.badgeIcon == BadgeIcon.Dot) ? 10 : (this.badgeIcon == BadgeIcon.None) ? 0 : rect.Height;
            }

            this.badgeSize = rect;
            this.WidthRequest = rect.Width;
            this.HeightRequest = rect.Height;
        }

        private RectF GetScaledRect()
        {
            RectF rect = new()
            {
                Width = (float)(this.badgeSize.Width * sizeRatio),
                Height = (float)(this.badgeSize.Height * sizeRatio)
            };
            rect.X = (float)(this.WidthRequest - rect.Width) / 2;
            rect.Y = (float)(this.HeightRequest - rect.Height) / 2;
            return rect;
        }

        /// <summary>
        /// Method used to show the badge.
        /// </summary>
        internal void Show()
        {
            if (this.AnimationEnabled)
            {
                this.StartScaleAnimation();
            }
        }

        /// <summary>
        /// Method used to hide the badge.
        /// </summary>
        internal void Hide()
        {
            if (this.AnimationEnabled)
            {
                this.StartScaleAnimation();
            }
        }

        /// <summary>
        /// Method used to hide the badge text when the value is empty or zero.
        /// </summary>
        /// <param name="value">value.</param>
        /// <param name="canHide">canHide.</param>
        internal void ShowBadgeBasedOnText(string value, bool canHide)
        {
            if (string.IsNullOrEmpty(value) || value == "0")
            {
                if (canHide)
                {
                    // TODO: Need to enable once below issue resolved.
                    // TODO: 'Unable to find IAnimationManager for 'Syncfusion.Maui.Core.BadgeLabelView'. (Parameter 'animatable')' exception occuring when parent not set. Need to check on next Preview (Preview 11) and remove this try catch once its fixed.
                    // this.Hide();
                    //this.IsVisible = false;
                }
                else
                {
                    this.Show();
                }
            }
            else
            {
                this.Show();
            }
        }

      

        /// <summary>
        /// Method used to draw the badge.
        /// </summary>
        /// <param name="canvas">canvas.</param>
       
        internal void DrawBadge(ICanvas canvas)
        {
            var currentBounds = this.GetScaledRect();

            var fillBounds = this.CalculatePosition();
            fillBounds = CalculateMargin(fillBounds);
            var backgroundBounds = default(RectF);
            backgroundBounds.X = (float)(fillBounds.X + (this.StrokeThickness / 2));
            backgroundBounds.Y = (float)(fillBounds.Y + (this.StrokeThickness / 2));
            backgroundBounds.Right = (float)(fillBounds.Right - (this.StrokeThickness / 2));
            backgroundBounds.Bottom = (float)(fillBounds.Bottom - (this.StrokeThickness / 2));
            canvas.SetFillPaint(badgeBackground, fillBounds);
            canvas.FillRoundedRectangle(fillBounds, this.CornerRadius.TopLeft, this.CornerRadius.TopRight, this.CornerRadius.BottomLeft, this.CornerRadius.BottomRight);
            canvas.StrokeSize = (float)this.StrokeThickness;
            canvas.StrokeColor = this.Stroke;
            if (this.StrokeThickness > 0)
            {
                canvas.DrawRoundedRectangle(fillBounds, this.CornerRadius.TopLeft, this.CornerRadius.TopRight, this.CornerRadius.BottomLeft, this.CornerRadius.BottomRight);
            }

            PointF startPoint = this.GetTextStartPoint(fillBounds);
            if (!string.IsNullOrEmpty(this.text))
            {
                canvas.DrawText(this.text, startPoint.X, startPoint.Y, this.TextElement!);
            }
            else
            {
                var rect = new RectF(startPoint.X, startPoint.Y, this.GetScaledFontSize(), this.GetScaledFontSize());
                canvas.StrokeColor = this.TextColor;
                canvas.StrokeSize = 1.5f;
                canvas.SetFillPaint(new SolidColorBrush(this.TextColor), rect);
                GetBadgeIcon(canvas, rect, this.BadgeIcon);
            }
        }

        #endregion

        #region Private methods
        /// <summary>
        /// To get the badge icon.
        /// </summary>
        private static void GetBadgeIcon(ICanvas canvas, RectF rect, BadgeIcon value)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midHeight = y + (rect.Height / 2);

            switch (value)
            {
                case BadgeIcon.Add:
                    rect.X += rect.Width / 12;
                    rect.Y += rect.Height / 12;
                    rect.Width -= rect.Width / 6;
                    rect.Height -= rect.Height / 6;
                    CanvasExtensions.DrawPlus(canvas, rect, false, rect.Width + 4);
                    break;

                case BadgeIcon.Available:
                    CanvasExtensions.DrawTick(canvas, rect);
                    break;

                case BadgeIcon.Away:
                    CanvasExtensions.DrawAwaySymbol(canvas, rect);
                    break;

                case BadgeIcon.Busy:
                    canvas.DrawLine(new PointF(x, midHeight), new PointF(width, midHeight));
                    break;

                case BadgeIcon.Delete:
                    rect.X += rect.Width / 6;
                    rect.Y += rect.Height / 6;
                    rect.Width -= rect.Width / 3;
                    rect.Height -= rect.Height / 3;
                    CanvasExtensions.DrawCross(canvas, rect, false, rect.Width);
                    break;

                case BadgeIcon.Prohibit1:
                    canvas.DrawLine(new PointF(width - 1, y + 1), new PointF(x + 1, height - 1));
                    break;

                case BadgeIcon.Prohibit2:
                    canvas.DrawLine(new PointF(x + 1, y + 1), new PointF(width - 1, height - 1));
                    break;                
            }
        }


        /// <summary>
        /// Calculate margin
        /// </summary>
        /// <param name="fillBounds">Fill Bounds.</param>
        /// <returns> The rectangle</returns>
        private RectF CalculateMargin(RectF fillBounds)
        {
            if (BadgePosition == BadgePosition.Top)
            {
                fillBounds.X = (float)(fillBounds.X + Margin.Left / 2);
                fillBounds.Y = (float)(fillBounds.Y + Margin.Top);
            }
            else if (BadgePosition == BadgePosition.Left)
            {
                fillBounds.X = (float)(fillBounds.X + Margin.Left);
                fillBounds.Y = (float)(fillBounds.Y + Margin.Top / 2);
            }
            else if (BadgePosition == BadgePosition.TopLeft)
            {
                fillBounds.X = (float)(fillBounds.X + Margin.Left);
                fillBounds.Y = (float)(fillBounds.Y + Margin.Top);
            }
            else if (BadgePosition == BadgePosition.Right)
            {
                fillBounds.X = (float)(fillBounds.X - Margin.Right);
                fillBounds.Y = (float)(fillBounds.Y + Margin.Top / 2);
            }
            else if (BadgePosition == BadgePosition.TopRight)
            {
                fillBounds.X = (float)(fillBounds.X - Margin.Right);
                fillBounds.Y = (float)(fillBounds.Y + Margin.Top);
            }
            else if (BadgePosition == BadgePosition.Bottom)
            {
                fillBounds.X = (float)(fillBounds.X + Margin.Left / 2);
                fillBounds.Y = (float)(fillBounds.Y - Margin.Bottom);
            }
            else if (BadgePosition == BadgePosition.BottomLeft)
            {
                fillBounds.X = (float)(fillBounds.X + Margin.Left);
                fillBounds.Y = (float)(fillBounds.Y - Margin.Bottom);
            }
            else if (BadgePosition == BadgePosition.BottomRight)
            {
                fillBounds.X = (float)(fillBounds.X - Margin.Right);
                fillBounds.Y = (float)(fillBounds.Y - Margin.Bottom);
            }
            return fillBounds;
        }

        /// <summary>
        /// Calculate position.
        /// </summary>
        /// <returns> The rectangle</returns>
        private RectF CalculatePosition()
        {
            var currentBounds = this.GetScaledRect();
            bool isRTL = ((badgeView as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft;
            var fillBounds = default(RectF);
            if (BadgePosition == BadgePosition.BottomLeft)
            {
                fillBounds.X = isRTL? (float)(XPosition - this.badgeSize.Width + currentBounds.X - (this.StrokeThickness / 2)):(float)(XPosition + currentBounds.X - (this.StrokeThickness / 2));
                fillBounds.Y = (float)(YPosition - this.badgeSize.Width + currentBounds.Y + (this.StrokeThickness / 2));
            }
            else if (BadgePosition == BadgePosition.BottomRight)
            {
                fillBounds.X = isRTL? (float)(XPosition + currentBounds.X - (this.StrokeThickness / 2)):(float)(XPosition - this.badgeSize.Width + currentBounds.X - (this.StrokeThickness / 2));
                fillBounds.Y = (float)(YPosition - this.badgeSize.Width + currentBounds.Y + (this.StrokeThickness / 2));

            }
            else if (BadgePosition == BadgePosition.TopRight)
            {
                fillBounds.X = isRTL? (float)(XPosition + currentBounds.X - (this.StrokeThickness / 2)) :(float)(XPosition - this.badgeSize.Width + currentBounds.X - (this.StrokeThickness / 2));
                fillBounds.Y = (float)(YPosition + currentBounds.Y + (this.StrokeThickness / 2));
            }
            else if (BadgePosition == BadgePosition.Right)
            {
                fillBounds.X = isRTL? (float)(XPosition + currentBounds.X - (this.StrokeThickness / 2)):(float)(XPosition - this.badgeSize.Width + currentBounds.X - (this.StrokeThickness / 2));
                fillBounds.Y = (float)(YPosition - currentBounds.Height / 2 - (this.StrokeThickness / 2));
            }
            else if (BadgePosition == BadgePosition.Left)
            {
                fillBounds.X = isRTL ? (float)(XPosition - this.badgeSize.Width + currentBounds.X - (this.StrokeThickness / 2)) : (float)(XPosition + currentBounds.X - (this.StrokeThickness / 2));
                fillBounds.Y = (float)(YPosition - currentBounds.Height / 2 - (this.StrokeThickness / 2));
            }
            else if (BadgePosition == BadgePosition.Bottom)
            {
                fillBounds.X = (float)(XPosition - currentBounds.Width / 2 - (this.StrokeThickness / 2));
                fillBounds.Y = (float)(YPosition - this.badgeSize.Width + currentBounds.Y + (this.StrokeThickness / 2));
            }
            else if (BadgePosition == BadgePosition.Top)
            {
                fillBounds.X = (float)(XPosition - currentBounds.Width / 2 - (this.StrokeThickness / 2));
                fillBounds.Y = (float)(YPosition + currentBounds.Y + (this.StrokeThickness / 2));
            }
            else if (BadgePosition == BadgePosition.TopLeft)
            {
                fillBounds.X = isRTL? (float)(XPosition - this.badgeSize.Width + currentBounds.X - (this.StrokeThickness / 2)):(float)(XPosition + currentBounds.X - (this.StrokeThickness / 2));
                fillBounds.Y = (float)(YPosition + currentBounds.Y + (this.StrokeThickness / 2));
            }

            fillBounds.Width = (float)(currentBounds.Width);
            fillBounds.Height = (float)(currentBounds.Height);
            return fillBounds;

        }

        private void UpdateSemanticProperties()
        {
            if (String.IsNullOrEmpty(this.ScreenReaderText))
            {
                //SemanticProperties.SetDescription(this, this.Text);
            } 
            else
            {
                //SemanticProperties.SetDescription(this, this.ScreenReaderText);
            }
        }

        private float GetProccessedFontSize()
        {
            var textSize = this.FontSize;

            if (textSize <= 1)
            {
                textSize = 1;
            }

            return textSize;
        }
		
        private float GetScaledFontSize()
        {
            var textSize = (float)(this.FontSize * this.sizeRatio);

            if (textSize <= 1)
            {
                textSize = 1;
            }

            return textSize;
        }

        private void StartScaleAnimation()
        {
            // TODO: 'Unable to find IAnimationManager for 'Syncfusion.Maui.Core.BadgeLabelView'. (Parameter 'animatable')' exception occuring when parent not set. Need to check on next Preview (Preview 11) and remove this try catch once its fixed.
            try
            {
                if ((this.badgeView.WidthRequest > 0 && this.badgeView.HeightRequest > 0) || (this.badgeView.Height > 0 && this.badgeView.Width > 0))
                {
                    var fadeOutAnimation = new Animation(OnShowHideAnimationUpdate, 0, 1);
                    fadeOutAnimation.Commit(this.badgeView, "showAnimator", length: (uint)250, easing: Easing.Linear,
                        finished: OnShowHideAnimationEnded, repeat: () => false);
                }
            }
            catch (ArgumentException)
            {
            }
        }

        private void OnShowHideAnimationUpdate(double value)
        {
            this.sizeRatio = value;
            this.badgeView.InvalidateDrawable();
        }

        private void OnShowHideAnimationEnded(double value, bool isCompleted)
        {
            if (value == 0 && isCompleted)
            {
                this.sizeRatio = value;
            }
        }

        #endregion
    }
}
