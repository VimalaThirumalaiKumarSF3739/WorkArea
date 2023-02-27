using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.ComponentModel;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class LegendItem : INotifyPropertyChanged, ILegendItem
    {
        #region Fields

        private int index;
        private object? item = null;
        private string text = string.Empty;
        private string fontFamily = string.Empty;
        private FontAttributes fontAttributes = FontAttributes.None;
        private Brush iconBrush = new SolidColorBrush(Colors.Transparent);
        private Color textColor = Colors.Black;
        private ShapeType iconType = ShapeType.Rectangle;
        private double iconHeight = 12;
        private double iconWidth = 12;
        private float fontSize = 12;
        private Thickness textMargin = new Thickness(0);
        private bool isToggled = false;
        private bool isIconVisible = true;
        private Brush disableBrush = new SolidColorBrush(Color.FromRgba(0, 0, 0, 0.38));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the corresponding icon color for legend item.
        /// </summary>
        public Brush IconBrush
        {
            get { return iconBrush; }

            set
            {
                if (iconBrush == value)
                {
                    return;
                }

                iconBrush = value;
                OnPropertyChanged(nameof(IconBrush));
            }
        }

        /// <summary>
        /// Gets or sets the height of the legend icon.
        /// </summary>
        /// <value>This property takes double value.</value>
        public double IconHeight
        {
            get { return iconHeight; }
            set
            {
                if (iconHeight == value)
                {
                    return;
                }

                iconHeight = value;
                OnPropertyChanged(nameof(IconHeight));
            }
        }

        /// <summary>
        /// Gets or sets the width of the legend icon.
        /// </summary>
        /// <value>This property takes double value.</value>
        public double IconWidth
        {
            get { return iconWidth; }
            set
            {
                if (iconWidth == value)
                {
                    return;
                }

                iconWidth = value;
                OnPropertyChanged(nameof(IconWidth));
            }
        }

        /// <summary>
        /// Gets the corresponding index for legend item.
        /// </summary>
        public int Index
        {
            get { return index; }

            internal set
            {
                if (index == value)
                {
                    return;
                }

                index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display the legend icon.
        /// </summary>
        public bool IsIconVisible
        {
            get { return isIconVisible; }

            set
            {
                if (isIconVisible == value)
                {
                    return;
                }

                isIconVisible = value;
                OnPropertyChanged(nameof(IsIconVisible));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the legend item is toggled or not.
        /// </summary>
        public bool IsToggled
        {
            get { return isToggled; }

            set
            {
                if (isToggled == value)
                {
                    return;
                }

                isToggled = value;
                OnPropertyChanged(nameof(IsToggled));
            }
        }

        /// <summary>
        /// Gets the corresponding data point for series.
        /// </summary>
        public object? Item
        {
            get { return item; }

            internal set
            {
                if (item == value)
                {
                    return;
                }

                item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        /// <summary>
        /// Gets or sets the corresponding label for legend item.
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                if (text == value)
                {
                    return;
                }

                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        /// <summary>
        /// Gets or sets the font attribute type for the legend item label.
        /// </summary>
        public FontAttributes FontAttributes
        {
            get { return fontAttributes; }
            internal set
            {
                if (fontAttributes == value)
                {
                    return;
                }

                fontAttributes = value;
                OnPropertyChanged(nameof(FontAttributes));
            }
        }

        /// <summary>
        /// Gets or sets the font family name for the legend item label.
        /// </summary>
        public string FontFamily
        {
            get { return fontFamily; }
            internal set
            {
                if (fontFamily == value)
                {
                    return;
                }

                fontFamily = value;
                OnPropertyChanged(nameof(FontFamily));
            }
        }

        /// <summary>
        /// Gets or sets the font size for the legend label text. 
        /// </summary>
        public float FontSize
        {
            get { return fontSize; }
            internal set
            {
                if (fontSize == value)
                {
                    return;
                }

                fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }

        /// <summary>
        /// Gets or sets the icon type in legend.
        /// </summary>
        public ShapeType IconType
        {
            get { return iconType; }
            internal set
            {
                if (iconType == value)
                {
                    return;
                }

                iconType = value;
                OnPropertyChanged(nameof(IconType));
            }
        }

        /// <summary>
        /// Gets or sets the corresponding text color for legend item.
        /// </summary>
        public Color TextColor
        {
            get { return textColor; }
            internal set
            {
                if (textColor == value)
                {
                    return;
                }

                textColor = value;
                OnPropertyChanged(nameof(TextColor));
            }
        }

        /// <summary>
        /// Gets or sets the margin of the legend text.
        /// </summary>
        public Thickness TextMargin
        {
            get { return textMargin; }            
            internal set
            {
                if (textMargin == value)
                {
                    return;
                }

                textMargin = value;
                OnPropertyChanged(nameof(TextMargin));
            }
        }

        #endregion

        #region Internal Properties

        internal Brush DisableBrush
        {
            get { return disableBrush; }

            set
            {
                if (disableBrush == value)
                {
                    return;
                }

                disableBrush = value;
                OnPropertyChanged(nameof(DisableBrush));
            }
        }

        #endregion

        #region Private Properties

        Brush ILegendItem.DisableBrush { get => DisableBrush; set => DisableBrush = value; }
        FontAttributes ILegendItem.FontAttributes { get => FontAttributes; set => FontAttributes = value; }
        string ILegendItem.FontFamily { get => FontFamily; set => FontFamily = value; }
        float ILegendItem.FontSize { get => FontSize; set => FontSize = value; }
        ShapeType ILegendItem.IconType { get => IconType; set => IconType = value; }
        Color ILegendItem.TextColor { get => TextColor; set => TextColor = value; }
        Thickness ILegendItem.TextMargin { get => TextMargin; set => TextMargin = value; }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendItem"/> class.
        /// </summary>
        public LegendItem()
        {
        }

        #endregion

        #region event

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Methods

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
