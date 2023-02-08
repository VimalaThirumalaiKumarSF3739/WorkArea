namespace Syncfusion.Maui.Core
{
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;
    using Syncfusion.Maui.Graphics.Internals;
    using System;

    /// <summary>
    /// Specifies the type of sficons.
    /// </summary>
    internal enum SfIcon
    {
        /// <summary>
        /// Specifies the forward icon.
        /// </summary>
        Forward,

        /// <summary>
        /// Specifies the backward icon.
        /// </summary>
        Backward,

        /// <summary>
        /// Specifies the downward icon.
        /// </summary>
        Downward,

        /// <summary>
        /// Specifies the upward icon.
        /// </summary>
        Upward,

        /// <summary>
        /// Specifies the today icon.
        /// </summary>
        Today,

        /// <summary>
        /// Specifies the option icon.
        /// </summary>
        Option,

        /// <summary>
        /// Specifies the view button.
        /// </summary>
        Button,

        /// <summary>
        /// Specifies the combobox view.
        /// </summary>
        ComboBox,

        /// <summary>
        /// Specifies the today button.
        /// </summary>
        TodayButton,

        /// <summary>
        /// Specifies the divider view.
        /// </summary>
        Divider,

        /// <summary>
        /// Specifies the week number text.
        /// </summary>
        WeekNumber,
    }

    /// <summary>
    /// Represents a class which contains information of icons view.
    /// </summary>
    internal class SfIconView : SfDrawableView
    {
        #region Fields

        /// <summary>
        /// This bool is used to identify whether the combobox dropdown is open or not.
        /// </summary>
        private bool isOpen;

        /// <summary>
        /// The icon size.
        /// </summary>
        private double iconSize => this.textStyle.FontSize / 1.5;

        /// <summary>
        /// The border color.
        /// </summary>
        private Color borderColor;

        /// <summary>
        /// The today highlight color.
        /// </summary>
        private Color highlightColor;

        /// <summary>
        /// The icon Text.
        /// </summary>
        private string text;

        /// <summary>
        /// Holds that the view is visible or not.
        /// </summary>
        private bool visibility;

        /// <summary>
        /// Icon text style value.
        /// </summary>
        private TextStyle textStyle;

        /// <summary>
        /// Defines the icon text is selected or not.
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Defines the flow direction is RTL or not.
        /// </summary>
        private bool isRTL;

        /// <summary>
        /// Used to trim week number text while SfIcon type is Week number.
        /// It is applicable for SfScheduler.
        /// </summary>
        private Func<double, string, string>? getTrimWeekNumberText;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfIconView"/> class.
        /// </summary>
        /// <param name="icon">The sficon.</param>
        /// <param name="textStyle">The header text style.</param>
        /// <param name="text">The icon text.</param>
        /// <param name="borderColor">Border color for the button.</param>
        /// <param name="highlightColor">Highlight color to highlight the selected icon.</param>
        /// <param name="isRTL">Defines the icon flow direction is RTL.</param>
        /// <param name="isSelected">Defined the icon is selected or not.</param>
        /// <param name="getTrimWeekNumberText">Used to trim the week number text based on available width and height.</param>
        internal SfIconView(SfIcon icon, ITextElement textStyle, string text, Color borderColor, Color highlightColor, bool isSelected = false, bool isRTL = false, Func<double, string, string>? getTrimWeekNumberText = null)
        {
            this.Icon = icon;
            this.textStyle = new TextStyle()
            {
                TextColor = textStyle.TextColor,
                FontSize = textStyle.FontSize,
                FontAttributes = textStyle.FontAttributes,
                FontFamily = textStyle.FontFamily,
            };
            this.text = text;
            this.isRTL = isRTL;
            this.isSelected = isSelected;
            this.borderColor = borderColor;
            this.highlightColor = highlightColor;
            this.getTrimWeekNumberText = getTrimWeekNumberText;
            this.visibility = true;
        }

        #endregion

        #region Property

        internal TextStyle TextStyle
        {
            get
            {
                return this.textStyle;
            }

            set
            {
                if (this.textStyle.FontSize == value.FontSize && this.textStyle.TextColor == value.TextColor && this.textStyle.FontAttributes == value.FontAttributes && this.textStyle.FontFamily == value.FontFamily)
                {
                    return;
                }

                this.textStyle = value;
                this.InvalidateDrawable();
            }
        }

        internal Color BorderColor
        {
            get
            {
                return this.borderColor;
            }

            set
            {
                if (this.borderColor == value)
                {
                    return;
                }

                this.borderColor = value;
                this.InvalidateDrawable();
            }
        }

        internal Color HighlightColor
        {
            get
            {
                return this.highlightColor;
            }

            set
            {
                if (this.highlightColor == value)
                {
                    return;
                }

                this.highlightColor = value;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the sficon text.
        /// </summary>
        internal string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.text == value)
                {
                    return;
                }

                this.text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the combobox dropdown is open or not.
        /// </summary>
        internal bool IsOpen
        {
            get
            {
                return this.isOpen;
            }

            set
            {
                if (this.isOpen == value)
                {
                    return;
                }

                this.isOpen = value;
            }
        }

        internal bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                if (this.isSelected == value)
                {
                    return;
                }

                this.isSelected = value;
                this.InvalidateDrawable();
            }
        }

        internal bool IsRTL
        {
            get
            {
                return this.isRTL;
            }

            set
            {
                if (this.isRTL == value)
                {
                    return;
                }

                this.isRTL = value;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the view is visible or not.
        /// TODO: IsVisible property breaks in 6.0.400 release.
        /// Issue link -https://github.com/dotnet/maui/issues/7507
        /// -https://github.com/dotnet/maui/issues/8044
        /// -https://github.com/dotnet/maui/issues/7482
        /// </summary>
        internal bool Visibility
        {
            get
            {
                return this.visibility;
            }

            set
            {
#if !__MACCATALYST__
                this.IsVisible = value;
#endif
                this.visibility = value;
            }
        }

        /// <summary>
        /// Gets or sets the sficon.
        /// </summary>
        internal SfIcon Icon { get; set; }

        #endregion

        #region Internal method

        /// <summary>
        /// Method to update icon style.
        /// </summary>
        /// <param name="textStyle">The text style value.</param>
        internal void UpdateStyle(ITextElement textStyle)
        {
            this.TextStyle = new TextStyle()
            {
                TextColor = textStyle.TextColor,
                FontSize = textStyle.FontSize,
                FontAttributes = textStyle.FontAttributes,
                FontFamily = textStyle.FontFamily,
            };
        }

        /// <summary>
        /// Method to update icon color.
        /// </summary>
        /// <param name="iconColor">The icon color.</param>
        internal void UpdateIconColor(Color iconColor)
        {
            this.TextStyle = new TextStyle()
            {
                TextColor = iconColor,
                FontSize = textStyle.FontSize,
                FontAttributes = textStyle.FontAttributes,
                FontFamily = textStyle.FontFamily,
            };
        }

        #endregion

        #region Override method

        /// <summary>
        /// Method to draw the sficon.
        /// </summary>
        /// <param name="canvas">The draw canvas.</param>
        /// <param name="dirtyRect">The rectangle.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
#if __MACCATALYST__
            //// TODO: IsVisibility property breaks in 6.0.400 release.
            if (!this.Visibility)
            {
                return;
            }

#endif
            canvas.SaveState();

            canvas.StrokeSize = 1.5f;
            canvas.FillColor = this.textStyle.TextColor;
            canvas.StrokeColor = this.textStyle.TextColor;
            float width = dirtyRect.Width;
            float height = dirtyRect.Height;
            switch (this.Icon)
            {
                case SfIcon.Forward:
                    {
                        this.DrawForward(width, height, canvas);
                        break;
                    }

                case SfIcon.Backward:
                    {
                        this.DrawBackward(width, height, canvas);
                        break;
                    }

                case SfIcon.Downward:
                    {
                        this.DrawDownward(width, height, canvas);
                        break;
                    }

                case SfIcon.Upward:
                    {
                        this.DrawUpward(width, height, canvas);
                        break;
                    }

                case SfIcon.Today:
                    {
                        this.DrawToday(width, height, canvas);
                        break;
                    }

                case SfIcon.Option:
                    {
                        this.DrawOption(width, height, canvas);
                        break;
                    }

#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
                case SfIcon.Button:
                    {
                        this.DrawTilesButton(width, height, canvas);
                        break;
                    }
#endif
                case SfIcon.ComboBox:
                    {
                        this.DrawComboBox(width, height, canvas);
                        break;
                    }

                case SfIcon.TodayButton:
                    {
                        this.DrawTodayButton(width, height, canvas);
                        break;
                    }

                case SfIcon.Divider:
                    {
                        this.DrawDivider(width, height, canvas);
                        break;
                    }

                case SfIcon.WeekNumber:
                    {
                        this.DrawWeekNumber(width, height, canvas);
                        break;
                    }
            }

            canvas.RestoreState();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to draw the forward icon.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        private void DrawForward(float width, float height, ICanvas canvas)
        {
            float arrowHeight = (float)this.iconSize;
            float arrowWidth = arrowHeight / 2;
            float centerPosition = height / 2;
            float leftPosition = (width / 2) - (arrowWidth / 2);
            float topPosition = (height / 2) - (arrowHeight / 2);
            float rightPosition = (width / 2) + (arrowWidth / 2);
            float bottomPosition = (height / 2) + (arrowHeight / 2);
            PathF path = new PathF();
            path.MoveTo(leftPosition, topPosition);
            path.LineTo(rightPosition, centerPosition);
            path.LineTo(leftPosition, bottomPosition);
            canvas.DrawPath(path);
        }

        /// <summary>
        /// Method to draw the backward icon.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        private void DrawBackward(float width, float height, ICanvas canvas)
        {
            float arrowHeight = (float)this.iconSize;
            float arrowWidth = arrowHeight / 2;
            float centerPosition = height / 2;
            float leftPosition = (width / 2) - (arrowWidth / 2);
            float topPosition = (height / 2) - (arrowHeight / 2);
            float rightPosition = (width / 2) + (arrowWidth / 2);
            float bottomPosition = (height / 2) + (arrowHeight / 2);
            PathF path = new PathF();
            path.MoveTo(rightPosition, topPosition);
            path.LineTo(leftPosition, centerPosition);
            path.LineTo(rightPosition, bottomPosition);
            canvas.DrawPath(path);
        }

        /// <summary>
        /// Method to draw the downward icon.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        private void DrawDownward(float width, float height, ICanvas canvas)
        {
            float arrowWidth = (float)this.iconSize;
            float arrowHeight = arrowWidth / 2;
            float centerPosition = width / 2;
            float leftPosition = (width / 2) - (arrowWidth / 2);
            float topPosition = (height / 2) - (arrowHeight / 2);
            float rightPosition = (width / 2) + (arrowWidth / 2);
            float bottomPosition = (height / 2) + (arrowHeight / 2);
            PathF path = new PathF();
            path.MoveTo(leftPosition, topPosition);
            path.LineTo(centerPosition, bottomPosition);
            path.LineTo(rightPosition, topPosition);
            canvas.DrawPath(path);
        }

        /// <summary>
        /// Method to draw the upward icon.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        private void DrawUpward(float width, float height, ICanvas canvas)
        {
            float arrowWidth = (float)this.iconSize;
            float arrowHeight = arrowWidth / 2;
            float centerPosition = width / 2;
            float leftPosition = (width / 2) - (arrowWidth / 2);
            float topPosition = (height / 2) - (arrowHeight / 2);
            float rightPosition = (width / 2) + (arrowWidth / 2);
            float bottomPosition = (height / 2) + (arrowHeight / 2);
            PathF path = new PathF();
            path.MoveTo(leftPosition, bottomPosition);
            path.LineTo(centerPosition, topPosition);
            path.LineTo(rightPosition, bottomPosition);
            canvas.DrawPath(path);
        }

        /// <summary>
        /// Method to draw the today icon.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        private void DrawToday(float width, float height, ICanvas canvas)
        {
            float iconSize = (float)this.iconSize;
            float secondaryRectSize = iconSize / 5;
            float leftPosition = (width / 2) - (iconSize / 2);
            float rightPosition = leftPosition + iconSize;
            float topPosition = (height / 2) - (iconSize / 2);
            canvas.DrawRoundedRectangle(leftPosition, topPosition, iconSize, iconSize, 1);
            canvas.DrawLine(leftPosition, topPosition + secondaryRectSize, rightPosition, topPosition + secondaryRectSize);
            canvas.DrawLine(leftPosition + secondaryRectSize, topPosition, leftPosition + secondaryRectSize, topPosition - secondaryRectSize);
            canvas.DrawLine(rightPosition - secondaryRectSize, topPosition, rightPosition - secondaryRectSize, topPosition - secondaryRectSize);
            canvas.FillRectangle(leftPosition + secondaryRectSize, topPosition + (2 * secondaryRectSize), secondaryRectSize, secondaryRectSize);
        }

        /// <summary>
        /// Method to draw the option icon.
        /// </summary>
        /// <param name="width">The icon width.</param>
        /// <param name="height">The icon height.</param>
        /// <param name="canvas">The draw canvas.</param>
        private void DrawOption(float width, float height, ICanvas canvas)
        {
            float totalHeight = (float)this.iconSize;
            float radius = totalHeight / 8;
            float centerYPosition = height / 2;
            float centerXPosition = width / 2;
            float topPosition = (height / 2) - (totalHeight / 2);
            float bottomPosition = (height / 2) + (totalHeight / 2);
            canvas.FillCircle(centerXPosition, topPosition + radius, radius);
            canvas.FillCircle(centerXPosition, centerYPosition, radius);
            canvas.FillCircle(centerXPosition, bottomPosition - radius, radius);
        }

        /// <summary>
        /// Method to draw the responsive UI today button.
        /// </summary>
        /// <param name="width">The button width.</param>
        /// <param name="height">The button height.</param>
        /// <param name="canvas">The draw canvas.</param>
        private void DrawTodayButton(float width, float height, ICanvas canvas)
        {
            this.DrawTilesButton(width, height, canvas);
        }

        /// <summary>
        /// Method to draw the divider view.
        /// </summary>
        /// <param name="width">The button width.</param>
        /// <param name="height">The button height.</param>
        /// <param name="canvas">The draw canvas.</param>
        private void DrawDivider(float width, float height, ICanvas canvas)
        {
            canvas.StrokeSize = 1;
            canvas.SaveState();
            Color strokeColor = this.GetCellBorderColor();
            canvas.StrokeColor = strokeColor;
            canvas.FillColor = strokeColor;
            float startPosition = 2;
            canvas.DrawLine(width / 2, startPosition, width / 2, height - startPosition);
            canvas.RestoreState();
        }

        /// <summary>
        /// Get the stroke color based on cell border color.
        /// </summary>
        /// <returns>Return cell border color value.</returns>
        private Color GetCellBorderColor()
        {
            if (this.borderColor != Colors.Transparent)
            {
                return borderColor;
            }

            return Colors.LightGray;
        }

        /// <summary>
        /// Method to draw the combobox button.
        /// </summary>
        /// <param name="width">The button width.</param>
        /// <param name="height">The button height.</param>
        /// <param name="canvas">The draw canvas.</param>
        private void DrawComboBox(float width, float height, ICanvas canvas)
        {
            var padding = 5;
            var margin = 2 * padding;
            var dropDownIConWidth = 10;
            canvas.StrokeSize = 1;

            float startPosition = 1;
            canvas.SaveState();
            TextStyle iconTextStyle = new TextStyle()
            {
                FontSize = this.textStyle.FontSize,
                TextColor = this.isSelected ? highlightColor : this.textStyle.TextColor,
                FontAttributes = this.textStyle.FontAttributes,
                FontFamily = this.textStyle.FontFamily
            };
            //// To show the borderline the start position will be 2, the height and width is adjusted to show the border.
            this.DrawText(canvas, this.Text, iconTextStyle, new RectF(margin, 0, width - 2, height - 2), HorizontalAlignment.Left, VerticalAlignment.Center);
            canvas.RestoreState();

            canvas.SaveState();
            canvas.StrokeColor = iconTextStyle.TextColor;
            canvas.FillColor = iconTextStyle.TextColor;
            //// To show the borderline the start position will be 2, the width and height is adjusted to show the border. The corner radius is 5
            canvas.DrawRoundedRectangle(startPosition, startPosition, width - 2, height - 2, 5);
            canvas.RestoreState();
            canvas.StrokeColor = this.textStyle.TextColor.WithAlpha(0.5f);
            canvas.FillColor = this.textStyle.TextColor.WithAlpha(0.5f);

            if (!this.IsOpen)
            {
                canvas.DrawInverseTriangle(new RectF(width - (2 * margin), (height - 4) / 2, dropDownIConWidth, dropDownIConWidth / 2), false);
            }
            else
            {
                canvas.DrawTriangle(new RectF(width - (2 * margin), (height - 4) / 2, dropDownIConWidth, dropDownIConWidth / 2), false);
            }
        }

        private void DrawText(ICanvas canvas, string text, ITextElement textStyle, Rect rect, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, VerticalAlignment verticalAlignment = VerticalAlignment.Top)
        {
            if (rect.Height <= 0 || rect.Width <= 0)
            {
                return;
            }

            canvas.DrawText(text, rect, horizontalAlignment, verticalAlignment, textStyle);
        }

        /// <summary>
        /// Method to draw the tiles button.
        /// </summary>
        /// <param name="width">The button width.</param>
        /// <param name="height">The button height.</param>
        /// <param name="canvas">The draw canvas.</param>
        private void DrawTilesButton(float width, float height, ICanvas canvas)
        {
            canvas.StrokeSize = 1;
            Color textColor, strokeColor;

            if (this.isSelected)
            {
                textColor = this.highlightColor;
                strokeColor = textColor;
            }
            else
            {
                textColor = this.textStyle.TextColor;
                strokeColor = this.GetCellBorderColor();
            }

            float startPosition = 1;
            canvas.SaveState();
            TextStyle iconTextStyle = new TextStyle() { FontSize = this.textStyle.FontSize, TextColor = textColor, FontAttributes = this.textStyle.FontAttributes, FontFamily = this.textStyle.FontFamily };
            //// To show the borderline the start position will be 2, the height is adjusted to show the border.
            this.DrawText(canvas, this.Text, iconTextStyle, new RectF(startPosition, startPosition, width - 2, height - 2), HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.RestoreState();

            canvas.SaveState();
            canvas.StrokeColor = strokeColor;
            canvas.FillColor = strokeColor;
            //// To show the borderline the start position will be 2, the width and height is adjusted to show the border. The corner radius is 5
            canvas.DrawRoundedRectangle(startPosition, startPosition, width - 2, height - 2, 5);
            canvas.RestoreState();
        }

        /// <summary>
        /// Method to draw the week number text.
        /// </summary>
        /// <param name="width">The button width.</param>
        /// <param name="height">The button height.</param>
        /// <param name="canvas">The draw canvas.</param>
        private void DrawWeekNumber(float width, float height, ICanvas canvas)
        {
            string text = this.Text;
            if (this.getTrimWeekNumberText != null)
            {
                text = this.getTrimWeekNumberText(width, text);
            }

#if WINDOWS
            //// TODO: horizontal and vertical alignments are not center aligned with draw text method. Hence used pading values explicitly.
            HorizontalAlignment alignment = this.isRTL ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            double startPosition = this.isRTL ? 0 : 5;
            this.DrawText(canvas, text, textStyle, new Rect(startPosition, 1, width - 5, height - 1), alignment, VerticalAlignment.Top);
#else
            this.DrawText(canvas, text, textStyle, new RectF(0, 0, width, height), HorizontalAlignment.Center, VerticalAlignment.Center);
#endif
        }

        #endregion
    }

    internal class TextStyle : BindableObject, ITextElement
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="TextColor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextColor"/> bindable property.
        /// </value>
        public static readonly BindableProperty TextColorProperty =
           BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TextStyle), Colors.Black);

        /// <summary>
        /// Identifies the <see cref="FontSize"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontSize"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

        /// <summary>
        /// Identifies the <see cref="FontFamily"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontFamily"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

        /// <summary>
        /// Identifies the <see cref="FontAttributes"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontAttributes"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text color for the icon text.
        /// </summary>
        public Color TextColor
        {
            get { return (Color)this.GetValue(TextColorProperty); }
            set { this.SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the double value that represents size of the icon text.
        /// </summary>
        public double FontSize
        {
            get { return (double)this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the string, that represents font family of the icon text.
        /// </summary>
        public string FontFamily
        {
            get { return (string)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the FontAttributes of the icon text.
        /// </summary>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)this.GetValue(FontAttributesProperty); }
            set { this.SetValue(FontAttributesProperty, value); }
        }

        /// <summary>
        /// Gets the font of the icon text.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "We require this.")]
        Microsoft.Maui.Font ITextElement.Font => (Microsoft.Maui.Font)this.GetValue(FontElement.FontProperty);

        #endregion

        #region Methods

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Justification = "We require this.")]
        double ITextElement.FontSizeDefaultValueCreator()
        {
            return 12d;
        }

        /// <inheritdoc/>
        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {
        }

        /// <inheritdoc/>
        void ITextElement.OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
        {
        }

        /// <inheritdoc/>
        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {
        }

        /// <inheritdoc/>
        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {
        }

        #endregion
    }
}