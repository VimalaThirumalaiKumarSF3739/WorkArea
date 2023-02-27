
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Maui.Graphics.Color;

#if WINDOWS
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using WColor = Windows.UI.Color;
using GradientBrush = Microsoft.UI.Xaml.Media.GradientBrush;
#elif ANDROID
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Android.Content;
using Android.OS;
using Android.Util;
#endif


namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// The SfInputLabel is a label component which is used to add input view for combobox non editable mode.
    /// </summary>
    internal class SfInputLabel : Label
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SfInputLabel"/> class.
        /// </summary>
        public SfInputLabel()
        {
            this.Style = new Style(typeof(SfInputLabel));
            this.TextColor = Colors.Black;
            this.FontAutoScalingEnabled = false;
        }
    }

    /// <summary>
    /// The SfInputView is a entry component which is used to create input view entry for combobox editable mode and autocomplete.
    /// </summary>
    internal class SfInputView : Entry
    {
        #region Fields

        private  IDrawable? drawable;
        private bool isDeleteButtonPressed = false;

#if MACCATALYST || IOS
        private Color? focusedColor = Color.FromArgb("#8EBDFF");
#else
       private Color? focusedColor = Colors.Gray;
#endif

#endregion

#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfInputView"/> class.
        /// </summary>
        public SfInputView() 
        {
            Iniitalize();
        }

        private void Iniitalize()
        {
            this.Style = new Style(typeof(SfInputView));
            this.BackgroundColor = Colors.Transparent;
            this.TextColor = Colors.Black;
            this.FontSize = 14d;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to indicates delete button pressed or not.
        /// </summary>
        internal bool IsDeletedButtonPressed
        {
            get
            {
                return isDeleteButtonPressed;
            }
            set
            {
                isDeleteButtonPressed = value;
            }
        }

        /// <summary>
        /// Gets or sets the value for focused stroke.
        /// </summary>
        internal Color? FocusedStroke
        {
            get
            {
                return focusedColor;
            }
            set
            {
                focusedColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the drawable value.
        /// </summary>
        internal IDrawable? Drawable 
        {
            get { return drawable; }
            set { drawable = value; }
        }


        /// <summary>
        /// Gets or sets the button size value.
        /// </summary>
        internal double ButtonSize { get; set; }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Select text method.
        /// </summary>
        /// <param name="start">The start value.</param>
        internal void SelectText(int start)
        {
            if (this.Handler == null)
                return;
#if WINDOWS
            if (this.Handler.PlatformView is Microsoft.Maui.Platform.MauiPasswordTextBox windowEntry)
            {
                windowEntry.Select(start, (windowEntry.Text.Length - start));
            }

#elif ANDROID
            if (this.Handler.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText androidEntry)
            {
                if (androidEntry.Text != null)
                    androidEntry.SetSelection(start, androidEntry.Text.Length);
            }

#elif MACCATALYST || IOS
            if (this.Handler.PlatformView is Microsoft.Maui.Platform.MauiTextField macEntry)
            {
                if (macEntry.Text != null)
                {
                    Foundation.NSRange range = new((nint)start, (nint)(macEntry.Text.Length - start));
                    UIKit.UITextPosition startPosition = macEntry.GetPosition(macEntry.BeginningOfDocument, range.Location);
                    UIKit.UITextPosition end = macEntry.GetPosition(startPosition, range.Length);
                    macEntry.SelectedTextRange = macEntry.GetTextRange(startPosition, end);
                }
            }
#endif

        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Handler changed method.
        /// </summary>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
#if WINDOWS
            if (this.Handler != null && this.Handler.PlatformView is Microsoft.UI.Xaml.Controls.TextBox textbox)
            {
                textbox.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                textbox.Resources["TextControlBorderThemeThicknessFocused"] = textbox.BorderThickness;

                WColor? color;
                var borderBrush = textbox.Resources["TextControlBorderBrushFocused"];

                if(borderBrush is GradientBrush brush)
                {
                    if ( brush!= null && brush.GradientStops != null)
                    {
                        color = brush.GradientStops.FirstOrDefault()?.Color;
                        if(color != null)
                        this.FocusedStroke = new Color(color.Value.R,color.Value.G,color.Value.B,color.Value.A);
                    }
                }
            }
#elif ANDROID
            if (this.Handler != null && this.Handler.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText textbox)
            {
                textbox.SetBackgroundColor(Android.Graphics.Color.Transparent);
                Android.Graphics.Color color = new(GetThemeAccentColor(textbox.Context!));
                this.FocusedStroke = new Color(color.R, color.G, color.B, color.A);
            }
#endif

            this.Focused -= SfEntry_Focused;
            this.Unfocused -= SfEntry_Unfocused;

            if (this.Handler != null)
            {
                this.Focused += SfEntry_Focused;
                this.Unfocused += SfEntry_Unfocused;
            }
        }

        #endregion

        #region Events
        /// <summary>
        /// It invoked when entry get unfocused.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The focus event args.</param>
        private void SfEntry_Unfocused(object? sender, FocusEventArgs e)
        {
            if (this.drawable is SfView sfView)
                sfView.InvalidateDrawable();
        }

        /// <summary>
        /// It invoked when the entry get focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The focus event args.</param>
        private void SfEntry_Focused(object? sender, FocusEventArgs e)
        {
            if (this.drawable is SfView sfView)
                sfView.InvalidateDrawable();
            if (!string.IsNullOrEmpty(this.Text) && !this.IsReadOnly)
            {
                this.SelectText(0);
            }
        }

        #endregion

#if ANDROID
        /// <summary>
        /// Get theme accent color method.
        /// </summary>
        /// <param name="context">The context.</param>        
        private static int GetThemeAccentColor(Context context)
        {
            int colorAttr;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                colorAttr = Android.Resource.Attribute.ColorAccent;
            }
            else if (context.Resources != null)
            {
                //Get colorAccent defined for AppCompat
                colorAttr = context.Resources.GetIdentifier("colorAccent", "attr", context.PackageName);
            }
            else
                colorAttr = Android.Resource.Color.BackgroundDark;

            TypedValue outValue = new();
            context.Theme?.ResolveAttribute(colorAttr, outValue, true);
            return outValue.Data;
        }
#endif
    }
}
