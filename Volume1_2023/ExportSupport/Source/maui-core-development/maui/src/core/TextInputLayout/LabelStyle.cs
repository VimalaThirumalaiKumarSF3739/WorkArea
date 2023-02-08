using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
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
    public class LabelStyle : BindableObject, ITextElement
    {
        #region Bindable properties

        /// <summary>
        /// Gets or sets the text color to SfTextInputLayout label.
        /// </summary>
        public static readonly BindableProperty TextColorProperty =
         BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(LabelStyle), Color.FromRgba(0, 0, 0, 0.87));

        /// <summary>
        /// Gets or sets the size of the font for label.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

        /// <summary>
        /// Gets or sets the font family to which the font for the label belongs.
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

        /// <summary>
        /// Gets or sets a value that indicates whether the font for the label is bold, italic, or neither.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the size of the font for label.
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font family to which the font for the label belongs.
        /// </summary>
        public string FontFamily
        {
            get { return (string)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the font for the label is bold, italic, or neither.
        /// </summary>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)this.GetValue(FontAttributesProperty); }
            set { this.SetValue(FontAttributesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text color of SfTextInputLayout controls label.
        /// <remarks> 
        /// This property is used to customize helper label, hint label and counter label text color.For customizing error label text color, use Stroke property value in error state.
        /// </remarks> 
        /// </summary>
        public Color TextColor
        {
            get { return (Color)this.GetValue(TextColorProperty); }
            set { this.SetValue(TextColorProperty, value); }
        }

        Microsoft.Maui.Font ITextElement.Font => (Microsoft.Maui.Font)GetValue(FontElement.FontProperty);

        #endregion

        #region Methods

        double ITextElement.FontSizeDefaultValueCreator()
        {
            return 12d;
        }

        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {

        }

        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {

        }

        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {

        }

        void ITextElement.OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
        {

        }
        #endregion
    }
}
