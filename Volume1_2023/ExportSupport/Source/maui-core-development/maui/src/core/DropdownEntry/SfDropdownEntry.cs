using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PointerEventArgs = Syncfusion.Maui.Core.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// The SfDropDownEntry class which is base class for DropDownBase class to create SfAutoComplete and SfComboBox control.
    /// </summary>
    public abstract class SfDropdownEntry : SfView, ITouchListener, ITextElement, IKeyboardListener
    {
        #region Fields

        private SfInputView? inputView = new SfInputView();
        private IDropdownRenderer? renderer;
        private RectF clearButtonRectF;
        private RectF dropdownButtonRectF;
        private RectF previousRectBounds;
        private readonly int buttonSize = 32;
        private EffectsRenderer? effectsenderer;
        private SfDropdownView? dropDownView;
        private SfTextInputLayout? textInputLayout;
        private bool isEditableMode = true;
        private bool isOpen = false;
        private Color? dropdownArrowColor = Colors.Black;
        private bool isRTL;
        private Point touchPoint;
        //private bool canShowClearButton = false;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies <see cref="IsDropDownOpen"/> bindable property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsDropDownOpen"/> bindable property.</value>
        public static readonly BindableProperty IsDropDownOpenProperty = BindableProperty.Create(nameof(IsDropDownOpen), typeof(bool), typeof(SfDropdownEntry), false, BindingMode.TwoWay, null, OnIsDropDownOpenPropertyChanged);

        /// <summary>
        /// Identifies <see cref="Placeholder"/> bindable property.
        /// </summary>
        /// <value>The identifier for the <see cref="Placeholder"/> bindable property.</value>
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(SfDropdownEntry), string.Empty, BindingMode.OneWay, null, OnPlaceholderPropertyChanged);

        /// <summary>
        /// Identifies <see cref="PlaceholderColor"/> bindable property.
        /// </summary>
        /// <value>The identifier for the <see cref="PlaceholderColor"/> bindable property.</value>
        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(SfDropdownEntry), Colors.Gray, BindingMode.OneWay, null, OnPlaceholderColorPropertyChanged);

        /// <summary>
        /// Identifies <see cref="ClearButtonIconColor"/> bindable property.
        /// </summary>
        /// <value>The identifier for the <see cref="ClearButtonIconColor"/> bindable property.</value>
        public static readonly BindableProperty ClearButtonIconColorProperty = BindableProperty.Create(nameof(ClearButtonIconColor), typeof(Color), typeof(SfDropdownEntry), Colors.Black, BindingMode.OneWay, null, OnClearButtonIconColorPropertyChanged);

        /// <summary>
        /// Identifies <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <value>The identifier for the <see cref="Stroke"/> bindable property.</value>
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(SfDropdownEntry), GetDefaultStroke(), BindingMode.OneWay, null, OnStrokePropertyChanged);

        /// <summary>
        /// Identifies <see cref="IsClearButtonVisible"/> bindable property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsClearButtonVisible"/> bindable property.</value>
        public static readonly BindableProperty IsClearButtonVisibleProperty =
            BindableProperty.Create("IsClearButtonVisible", typeof(bool), typeof(SfDropdownEntry), true, BindingMode.OneWay, null, OnIsClearButtonVisiblePropertyChanged);

        /// <summary>
        /// Identifies <see cref="IsDropdownIconVisible"/> bindable property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsDropdownIconVisible"/> bindable property.</value>
        internal static readonly BindableProperty IsDropDownIconVisibleProperty =
            BindableProperty.Create("IsDropdownIconVisible", typeof(bool), typeof(SfDropdownEntry), true, BindingMode.OneWay, null, OnIsDropdownButtonVisiblePropertyChanged);

        /// <summary>
        /// Identifies <see cref="IsClearIconVisible"/> bindable property.
        /// </summary>
        /// <value>The identifier for the <see cref="IsClearIconVisible"/> bindable property.</value>
        internal static readonly BindableProperty IsClearIconVisibleProperty =
            BindableProperty.Create("IsClearIconVisible", typeof(bool), typeof(SfDropdownEntry), true, BindingMode.OneWay, null, OnIsClearIconVisiblePropertyChanged);

        /// <summary>
        ///  Identifies <see cref="DropdownContent"/> bindable property.
        /// </summary>
        /// <value>The identifier for the <see cref="DropdownContent"/> bindable property.</value>

        // Using a DependencyProperty as the backing store for DropdownContent.  This enables animation, styling, binding, etc...
        internal static readonly BindableProperty DropdownContentProperty =
            BindableProperty.Create("DropdownContent", typeof(View), typeof(SfDropdownEntry), null, BindingMode.OneWay, null, OnIsDropdownContentPropertyChanged);

        /// <summary>
        /// Identifies <see cref="MaxDropDownHeight"/> bindable property.
        /// </summary>
        /// <value>The identifier for the <see cref="MaxDropDownHeight"/> bindable property.</value>
        public static readonly BindableProperty MaxDropDownHeightProperty =
            BindableProperty.Create(nameof(MaxDropDownHeight), typeof(double), typeof(SfDropdownEntry), 400d, BindingMode.OneWay, null, OnMaxDropDownHeightPropertyChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the clear button should be visible or not.
        /// </summary>
        /// <value>
        /// Specifies the value whether the clear button is visible or not. The default value is false.
        /// </value>
        public bool IsClearButtonVisible
        {
            get { return (bool)GetValue(IsClearButtonVisibleProperty); }
            set { SetValue(IsClearButtonVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the clear button should be visible or not.
        /// </summary>
        /// <value>
        /// Specifies the value whether the clear button is visible or not. The default value is false.
        /// </value>
        internal bool IsClearIconVisible
        {
            get { return (bool)GetValue(IsClearIconVisibleProperty); }
            set { SetValue(IsClearIconVisibleProperty, value); }
        }
        /// <summary>
        /// Gets or sets the text that is displayed in the control until the value is changed
        /// by a user action or some other operation.
        /// </summary>
        /// <value>
        /// The default value is <c>string.Empty</c>.
        /// </value>
        /// <returns>
        /// The text that is displayed in the control when no value is entered.
        /// </returns>
        public string Placeholder
        {
            get { return (string)this.GetValue(PlaceholderProperty); }
            set { this.SetValue(PlaceholderProperty, value); }
        }

        /// <summary>
        /// Gets or sets a color that describes the color of placeholder text.
        /// </summary>
        /// <value>
        /// Specifies the value for place holder color. The default value is Colors.Black.
        /// </value> 
        public Color PlaceholderColor
        {
            get { return (Color)this.GetValue(PlaceholderColorProperty); }
            set { this.SetValue(PlaceholderColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a color that describes the color of clear button.
        /// </summary>
        /// <value>
        /// Specifies the value for clear button. The default value is Colors.Black.
        /// </value> 
        public Color ClearButtonIconColor
        {
            get { return (Color)this.GetValue(ClearButtonIconColorProperty); }
            set { this.SetValue(ClearButtonIconColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum height for a drop-down.
        /// </summary>
        /// <value>
        /// The default value is 400d.
        /// </value>
        public double MaxDropDownHeight
        {
            get { return (double)this.GetValue(MaxDropDownHeightProperty); }
            set { this.SetValue(MaxDropDownHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the drop down is in open or closed state.
        /// </summary>
        /// <value>
        /// The default value is false.
        /// </value>
        /// <returns>True if the drop down is open, otherwise false.</returns>
        public bool IsDropDownOpen
        {
            get { return (bool)this.GetValue(IsDropDownOpenProperty); }
            set { this.SetValue(IsDropDownOpenProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for keyboard listener.
        /// </summary>
        internal IKeyboardListener KeyboardListener { get; set; }

        /// <summary>
        /// Gets or sets a color that describes the color of dropdown button.
        /// </summary>
        /// <value>
        /// Specifies the color value for dropdown button. The default value is Colors.Black.
        /// </value> 
        internal Color? DropDownArrowColor
        {
            get
            {
                return dropdownArrowColor;
            }
            set
            {
                dropdownArrowColor = value;
                if(this.textInputLayout != null && value != null)
                {
                    this.textInputLayout.DropDownButtonColor = value;
                }
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Gets or sets the value to indicate the text changed from append or not.
        /// </summary>
        internal bool IsTextChangedFromAppend { get; set; }

        /// <summary>
        /// Gets or sets the dropdownview.
        /// </summary>
        internal SfDropdownView? DropDownView
        {
            get
            {
                return this.dropDownView;
            }
            set
            {
                this.dropDownView = value;
            }
        }


        /// <summary>
        /// Gets or sets the inputview.
        /// </summary>
        internal SfInputView? InputView
        {
            get
            {
                return this.inputView;
            }
            set
            {
                this.inputView = value;
            }
        }

        /// <summary>
        /// Gets or sets a color that describes the stroke.
        /// </summary>
        /// <value>
        /// Specifies the color value for stroke. The default value is Colors.LightGray.
        /// </value> 
        public Color Stroke
        {
            get { return (Color)this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for editable mode.
        /// </summary>
        internal bool IsEditableMode
        {
            get
            {
                return isEditableMode;
            }
            set
            {
                isEditableMode = value;
                if (this.InputView != null)
                {
                    if (this.IsEditableMode)
                    {
                        this.InputView.IsReadOnly = false;
                        this.InputView.InputTransparent = false;
                    }
                    else
                    {
                        this.InputView.IsReadOnly = true;
                        this.InputView.InputTransparent = true;

                    }
                }

                this.UpdateElementsBounds(this.Bounds);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool IsDropdownIconVisible
        {
            get { return (bool)GetValue(IsDropDownIconVisibleProperty); }
            set { SetValue(IsDropDownIconVisibleProperty, value); }

        }

        /// <summary>
        /// 
        /// </summary>
        internal View? DropdownContent
        {
            get { return (View?)GetValue(DropdownContentProperty); }
            set { SetValue(DropdownContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the parent is text input layout.
        /// </summary>
        private bool IsTextInputLayout
        {
            get;
            set;
        }

        #endregion

        #region Property Changed

        /// <summary>
        /// Invoked whenever the <see cref="IsDropDownOpenProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnIsDropDownOpenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var sfDropdownEntry = bindable as SfDropdownEntry;
            if (sfDropdownEntry?.DropDownView != null)
            {
                if (sfDropdownEntry.IsDropDownOpen && sfDropdownEntry.DropdownContent != null)
                {
                    sfDropdownEntry.ShowDropdown();
                }
                else
                {
                    sfDropdownEntry.HideDropdown();
                }
            }
        }

        /// <summary>
        ///  Invoked whenever the <see cref="IsDropDownIconVisibleProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnIsDropdownButtonVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownEntry sfDropdownEntry)
            {
#if IOS || MACCATALYST
                    if (sfDropdownEntry.dropdownButtonRectF.Width > 0 && sfDropdownEntry.dropdownButtonRectF.Height > 0 || (!sfDropdownEntry.IsDropdownIconVisible && sfDropdownEntry.clearButtonRectF.Width > 0 && sfDropdownEntry.clearButtonRectF.Height > 0)||(sfDropdownEntry.previousRectBounds.Width > 0 && sfDropdownEntry.previousRectBounds.Height>0))
                    {
                        sfDropdownEntry.UpdateElements(sfDropdownEntry.dropdownButtonRectF,sfDropdownEntry.clearButtonRectF,sfDropdownEntry.previousRectBounds);

                    }
                    else
                    {
                        sfDropdownEntry.UpdateElementsBounds(sfDropdownEntry.Bounds);
                    }
#endif
            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="MaxDropDownHeightProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnMaxDropDownHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownEntry sfDropdownEntry && sfDropdownEntry.dropDownView != null)
            {
                sfDropdownEntry.dropDownView.PopupHeight = (double)newValue;
            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="StrokeProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownEntry sfDropdownEntry)
            {
                sfDropdownEntry.InvalidateDrawable();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnIsDropdownContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var sfDropdownEntry = bindable as SfDropdownEntry;

            if (newValue is View content && sfDropdownEntry?.dropDownView != null)
            {
                sfDropdownEntry.dropDownView.UpdatePopupContent(content);
            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="ClearButtonIconColorProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnClearButtonIconColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownEntry sfDropdownEntry)
            {
                if(sfDropdownEntry.textInputLayout!= null && newValue is Color color)
                {
                    sfDropdownEntry.textInputLayout.ClearButtonColor = color;
                }
                sfDropdownEntry.InvalidateDrawable();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnIsClearIconVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownEntry sfDropdownEntry)
            {
#if IOS || MACCATALYST
                    if (sfDropdownEntry.dropdownButtonRectF.Width > 0 && sfDropdownEntry.dropdownButtonRectF.Height > 0 || (!sfDropdownEntry.IsDropdownIconVisible && sfDropdownEntry.clearButtonRectF.Width > 0 && sfDropdownEntry.clearButtonRectF.Height > 0)||(sfDropdownEntry.previousRectBounds.Width > 0 && sfDropdownEntry.previousRectBounds.Height>0))
                    {
                        sfDropdownEntry.UpdateElements(sfDropdownEntry.dropdownButtonRectF,sfDropdownEntry.clearButtonRectF,sfDropdownEntry.previousRectBounds);

                    }
                    else
                    {
                        sfDropdownEntry.UpdateElementsBounds(sfDropdownEntry.Bounds);
                    }
#else
                sfDropdownEntry.UpdateElementsBounds(sfDropdownEntry.Bounds);
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnIsClearButtonVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownEntry sfDropdownEntry)
            {
                sfDropdownEntry.IsClearIconVisible = (bool)newValue;
                sfDropdownEntry.UpdateElementsBounds(sfDropdownEntry.Bounds);
            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="PlaceholderProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnPlaceholderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownEntry sfDropdownEntry && sfDropdownEntry.InputView != null)
            {
                sfDropdownEntry.InputView.Placeholder = (string)newValue;
            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="PlaceholderColorProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnPlaceholderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownEntry sfDropdownEntry && sfDropdownEntry.InputView != null)
            {
                sfDropdownEntry.InputView.PlaceholderColor = (Color)newValue;
            }
        }

        #endregion

        #region ITextElement Interface Implementaion

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

        /// <summary>
        /// Identifies the <see cref="TextColor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextColor"/> bindable property.
        /// </value>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SfDropdownEntry), Colors.Black, BindingMode.Default, null, OnITextElementPropertyChanged);


        /// <summary>
        /// Gets or sets a font size for text.
        /// </summary>
        /// <value>
        /// Specifies the font size value for text.
        /// </value> 
        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a font attributes for text.
        /// </summary>
        /// <value>
        /// Specifies the font attributes value for text.
        /// </value> 
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)this.GetValue(FontAttributesProperty); }
            set { this.SetValue(FontAttributesProperty, value); }
        }


        /// <summary>
        /// Gets or sets a font family for text.
        /// </summary>
        /// <value>
        /// Specifies the font family value for text.
        /// </value> 
        public string FontFamily
        {
            get { return (string)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a text color for text.
        /// </summary>
        /// <value>
        /// Specifies the text color value for text.The default value is Colors.Black.
        /// </value> 
        public Color TextColor
        {
            get { return (Color)this.GetValue(TextColorProperty); }
            set { this.SetValue(TextColorProperty, value); }
        }

        Microsoft.Maui.Font ITextElement.Font => (Microsoft.Maui.Font)this.GetValue(FontElement.FontProperty);

        /// <summary>
        /// Invoked when the <see cref="FontSizeProperty"/> changed.
        /// </summary>
        /// <returns>It returns the font size of the badge text.</returns>
        double ITextElement.FontSizeDefaultValueCreator()
        {
            return 14d;
        }

        /// <summary>
        /// Invoked when the <see cref="FontAttributesProperty"/> changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {
            if (this.InputView != null)
            {
                this.InputView.FontAttributes = newValue;
            }
            this.OnFontPropertiesChanged();
        }



        /// <summary>
        /// Invoked when the <see cref="FontFamilyProperty"/> changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {
            if (this.InputView != null)
            {
                this.InputView.FontFamily = newValue;
            }
            this.OnFontPropertiesChanged();
        }

        /// <summary>
        /// Invoked when the <see cref="FontSizeProperty"/> changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {
            if (this.InputView != null)
            {
                this.InputView.FontSize = newValue;
            }
            this.OnFontPropertiesChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnITextElementPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is SfDropdownEntry sfDropdownEntry && sfDropdownEntry.InputView != null)
            {
                sfDropdownEntry.InputView.TextColor = (Color)newValue;
                sfDropdownEntry.OnFontPropertiesChanged();
            }
        }

        /// <summary>
        /// This method hooked whenever the font changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        public void OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
        {
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfDropdownEntry"/> class.
        /// </summary>
        public SfDropdownEntry()
        {
            this.DrawingOrder = DrawingOrder.AboveContent;          
            this.InitializeElements();
            this.AddTouchListener(this);
            this.InputView?.AddKeyboardListener(this);
            this.AddKeyboardListener(this);
            this.KeyboardListener = this;
            this.SetRendererBasedOnPlatform();
            this.SizeChanged += SfDropdownEntry_SizeChanged;
           
        }
        #endregion

        #region Private Methods

        private void SfDropdownEntry_SizeChanged(object? sender, EventArgs e)
        {
            this.ValidateInputTextLayout();
        }

        /// <summary>
        /// Validate the parent is a TextInputLayout or not.
        /// </summary>
        private void ValidateInputTextLayout()
        {
            Element parent = this.Parent;
            while (parent != null)
            {
                if (parent is SfTextInputLayout textInputLayout)
                {
                    this.textInputLayout = textInputLayout;
                    UpdateTextInputLayoutUI(textInputLayout);
                    this.IsTextInputLayout = true;                 
                    break;
                }

                parent = parent.Parent;
            }
        }

        private void UpdateTextInputLayoutUI(SfTextInputLayout textInputLayout)
        {
            textInputLayout.ShowDropDownButton = this.IsDropdownIconVisible;
            textInputLayout.ShowClearButton = this.IsClearButtonVisible;
        }

        private async void SetInputViewFocus()
        {
            await Task.Delay(50);
            this.InputView?.Focus();
        }

        private static Color GetDefaultStroke()
        {
#if ANDROID
            return Color.FromRgba(100, 100, 100, 255);
#elif WINDOWS
            return Color.FromRgba(141, 141, 141, 255);
#else
            return Colors.LightGray;
#endif
        }
        private void InitializeElements()
        {
            if (this.BackgroundColor == null)
            {
                this.BackgroundColor = Colors.White;
            }
            if (this.dropDownView == null)
            {
                this.dropDownView = new SfDropdownView();
                this.dropDownView.PopupOpened += DropDownView_PopupOpened;
                this.dropDownView.PopupClosed += DropDownView_PopupClosed;
                this.dropDownView.AnchorView = this;
                this.dropDownView.PopupHeight = this.MaxDropDownHeight;
                base.Children.Add(this.dropDownView);
            }

            effectsenderer ??= new EffectsRenderer(this);
            if (this.InputView == null)
            {
                this.InputView = new SfInputView();
            }

            this.InputView.Drawable = this;
            this.InputView.IsTextPredictionEnabled = false;
            this.InputView.IsSpellCheckEnabled = false;
            base.Children.Add(this.InputView);
            this.InputView.HandlerChanged += InputView_HandlerChanged;
        }

        //TODO: Need to enable once clear button visibility issue fixed in ios
        //private void InputView_TextChanged(object? sender, TextChangedEventArgs e)
        //{
        //    if (this.InputView != null && this.IsEditableMode)
        //    {
        //        if (this.InputView.Text.Length > 0)
        //        {
        //            if (!this.canShowClearButton)
        //            {
        //                this.canShowClearButton = true;
        //                this.UpdateElementsBounds(this.Bounds);
        //            }
        //        }
        //        else if (this.canShowClearButton)
        //        {
        //            this.canShowClearButton = false;
        //            this.UpdateElementsBounds(this.Bounds);
        //        }
        //    }
        //}

        //private void InputView_Unfocused(object? sender, FocusEventArgs e)
        //{
        //    if (this.canShowClearButton && this.IsEditableMode)
        //    {
        //        this.canShowClearButton = false;
        //        this.UpdateElementsBounds(this.Bounds);
        //    }
        //}

        //private void InputView_Focused(object? sender, FocusEventArgs e)
        //{
        //    if (this.InputView != null && this.InputView.Text.Length > 0 && !this.canShowClearButton && this.IsEditableMode)
        //    {
        //        this.canShowClearButton = true;
        //        this.UpdateElementsBounds(this.Bounds);
        //    }
        //}

        private void DropDownView_PopupClosed(object? sender, EventArgs e)
        {
            if (this.IsDropDownOpen)
                this.IsDropDownOpen = false;
        }

        private void DropDownView_PopupOpened(object? sender, EventArgs e)
        {
            if (!this.IsDropDownOpen)
                this.IsDropDownOpen = true;
#if WINDOWS
            // In Windows Platform the Popup window width is slightly higher than the editor width,
            // so here update the width once.
            if (!this.IsTextInputLayout && this.DropDownView!= null)
            {
                this.DropDownView.PopupWidth = this.Width;
            }
#endif
            this.SetInputViewFocus();
        }

        private void InputView_HandlerChanged(object? sender, EventArgs e)
        {
            if (this.InputView != null)
            {
                if (this.InputView.Handler != null)
                {
#if ANDROID
                    if (this.InputView.Handler.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText androidEntry)
                    {
                        androidEntry.EditorAction += AndroidEntry_EditorAction;
                    }
#endif
                }
            }
        }

#if ANDROID
        private void AndroidEntry_EditorAction(object? sender, Android.Widget.TextView.EditorActionEventArgs e)
        {
            if(e.ActionId == Android.Views.InputMethods.ImeAction.Next || e.ActionId == Android.Views.InputMethods.ImeAction.Done)
            {
                this.ValidateSelectedItemOnEnter();
                if(IsDropDownOpen)
                {
                    this.IsDropDownOpen = false;
                }
                
                if(this.InputView!= null)
                {
                    this.InputView.Unfocus();
                }
            }
        }

#endif

        private void SetRendererBasedOnPlatform()
        {
#if __ANDROID__
            renderer = new MaterialDropdownEntryRenderer();
#elif WINDOWS
            renderer = new FluentDropdownEntryRenderer();
#else
            renderer = new CupertinoDropdownEntryRenderer();
#endif
        }

#if IOS || MACCATALYST
        internal void UpdateElements(RectF dropDownRectBounds,RectF closeButtonRectBounds,RectF prevBounds)
        {
            int buttonSpace = 0;

            if (IsDropdownIconVisible)
            {
             if(dropDownRectBounds.X > 0 && dropDownRectBounds.Y >0 && dropDownRectBounds.Width>0 && dropDownRectBounds.Height>0)
             {
                dropdownButtonRectF = dropDownRectBounds;

             }
             else
             {
                if (!isRTL)
                {
                   this.dropdownButtonRectF.X = (prevBounds.X + prevBounds.Width) - buttonSize;
                }
                else 
                {
                    this.dropdownButtonRectF.X = 0;
                }

                this.dropdownButtonRectF.Y = prevBounds.Center.Y - (buttonSize / 2);
                this.dropdownButtonRectF.Width = buttonSize;
                this.dropdownButtonRectF.Height = buttonSize;
             }
                buttonSpace = buttonSize;
            }

            if (this.IsClearIconVisible && this.IsEditableMode)
            {
                if (IsDropdownIconVisible)
                {
                    if (!isRTL)
                    {
                        this.clearButtonRectF.X = this.dropdownButtonRectF.X - buttonSize;
                    }
                    else
                    {
                        this.clearButtonRectF.X = this.dropdownButtonRectF.X + buttonSize;
                    }
					
                    buttonSpace = buttonSize * 2;
                }
                else
                {
                    if (!isRTL)
                    {
                       this.clearButtonRectF.X = (float)prevBounds.Width - buttonSize;
                    }
                    else
                    {
                        this.clearButtonRectF.X = 0;
                    }
                    buttonSpace = buttonSize;
                }

               
                  this.clearButtonRectF.Y = (float) prevBounds.Center.Y- (buttonSize / 2);


                this.clearButtonRectF.Width = buttonSize;
                this.clearButtonRectF.Height = buttonSize;


                if (this.IsTextInputLayout)
                {
                    this.clearButtonRectF = new RectF(0, 0, 0, 0);
                    this.dropdownButtonRectF = new RectF(0, 0, 0, 0);
                }
            }

            if (this.InputView != null)
            {
                this.InputView.ButtonSize = buttonSize + 4;
                this.InputView.Margin =  new Thickness(0, 0, 0, 0);

                if (this.InputView.Handler != null && this.InputView.Handler is SfInputViewHandler handler)
                {
                    handler.UpdateButtonSize();
                }
            }

            this.UpdateEffectsRendererBounds();

            if (this.DropdownContent != null && this.DropdownContent.WidthRequest != this.Bounds.Width && !this.IsTextInputLayout)
            {
                this.DropdownContent.WidthRequest = this.Bounds.Width;
            }

            this.InvalidateDrawable();
        }

#endif
       internal void UpdateElementsBounds(RectF bounds)
        {
            this.previousRectBounds = bounds;
            int buttonSpace = 0;

            if (IsDropdownIconVisible)
            {
#if ANDROID
                if (!isRTL)
                {
                  this.dropdownButtonRectF.X = (bounds.X + bounds.Width) - buttonSize-4;
                }
                else 
                {
                    this.dropdownButtonRectF.X = 4;
                }

                this.dropdownButtonRectF.Y = bounds.Center.Y - (buttonSize / 2);
                this.dropdownButtonRectF.Width = buttonSize;
                this.dropdownButtonRectF.Height = buttonSize-2;
#else
                if (!isRTL)
                {
                    this.dropdownButtonRectF.X = (float)((bounds.X + bounds.Width) - buttonSize);
                }
                else
                {
                    this.dropdownButtonRectF.X = 0;

                }
                this.dropdownButtonRectF.Y = bounds.Center.Y - (buttonSize / 2);
                this.dropdownButtonRectF.Width = buttonSize;
                this.dropdownButtonRectF.Height = buttonSize;
#endif
                buttonSpace = buttonSize;
            }

            if (this.IsClearIconVisible && this.IsEditableMode)
            {
                if (IsDropdownIconVisible)
                {
#if ANDROID
                if(!isRTL)
                {
                  this.clearButtonRectF.X = this.dropdownButtonRectF.X - buttonSize + 4;
                }
                else
                {
                  this.clearButtonRectF.X = this.dropdownButtonRectF.X + buttonSize - 4;
                }  
#else
                    if (!isRTL)
                    {
                        this.clearButtonRectF.X = this.dropdownButtonRectF.X - buttonSize;
                    }
                    else
                    {
                        this.clearButtonRectF.X = this.dropdownButtonRectF.X + buttonSize;
                    }

#endif
                    buttonSpace = buttonSize * 2;
                }
                else
                {
                    if (!isRTL)
                    {
                        this.clearButtonRectF.X = (float)(bounds.Width - buttonSize);
                    }
                    else
                    {
                        this.clearButtonRectF.X = 0;
                    }

                    buttonSpace = buttonSize;
                }
               this.clearButtonRectF.Y = bounds.Center.Y - (buttonSize / 2);
#if ANDROID
                
                this.clearButtonRectF.Width = buttonSize -4;
                this.clearButtonRectF.Height = buttonSize -4;
#else
                this.clearButtonRectF.Width = buttonSize;
                this.clearButtonRectF.Height = buttonSize;
#endif
               
                if (this.IsTextInputLayout)
                {
                    this.clearButtonRectF = new RectF(0,0,0,0);
                    this.dropdownButtonRectF = new RectF(0,0,0,0);
                }
            }

            if (this.InputView != null)
            {
                this.InputView.ButtonSize = buttonSize + 4;
#if ANDROID
                this.InputView.Margin = (this.IsTextInputLayout) ? (this.IsEditableMode) ? new Thickness(0, 0, 0, 0) : new Thickness(0, 0, buttonSpace, 0) : new Thickness(10, 0, buttonSpace, 0);
#elif IOS || MACCATALYST
                this.InputView.Margin = new Thickness(0, 0, 0, 0);
                if (this.InputView.Handler != null && this.InputView.Handler is SfInputViewHandler handler)
                {
                    handler.UpdateButtonSize();
                }
#else
                this.InputView.Margin = (this.IsTextInputLayout && this.IsEditableMode) ? new Thickness(0,0,0,0) : new Thickness(0, 0, buttonSpace, 0);
#endif
            }

            this.UpdateEffectsRendererBounds();

            if (this.DropdownContent != null && this.DropdownContent.WidthRequest != bounds.Width && !this.IsTextInputLayout)
            {
                this.DropdownContent.WidthRequest = bounds.Width;
            }
            this.InvalidateDrawable();
        }

        private void UpdateEffectsRendererBounds()
        {
            if (effectsenderer != null)
            {
                effectsenderer.RippleBoundsCollection.Clear();
                effectsenderer.HighlightBoundsCollection.Clear();

                if (this.IsClearIconVisible && this.IsEditableMode)
                {
                    effectsenderer.RippleBoundsCollection.Add(clearButtonRectF);
                    effectsenderer.HighlightBoundsCollection.Add(clearButtonRectF);
                }

                if (this.IsDropdownIconVisible && !IsTextInputLayout)
                {
                    effectsenderer.RippleBoundsCollection.Add(dropdownButtonRectF);
                    effectsenderer.HighlightBoundsCollection.Add(dropdownButtonRectF);
                }
            }
        }

        private void DrawEntryUI(ICanvas canvas, Rect dirtyRect)
        {
            if (this.InputView == null)
                return;

            canvas.Alpha = 1f;
            canvas.FillColor = Colors.Black;
            canvas.StrokeColor = Colors.Black;


            if (this.renderer != null)
            {
                if (this.IsDropdownIconVisible && !IsTextInputLayout)
                {
                    canvas.StrokeColor = this.DropDownArrowColor;
                    canvas.FillColor = this.DropDownArrowColor;
                    renderer.DrawDropDownButton(canvas, this.dropdownButtonRectF);
                }

                if (this.IsClearIconVisible && this.IsEditableMode && !IsTextInputLayout)
                {
                    canvas.StrokeColor = this.ClearButtonIconColor;
                    renderer.DrawClearButton(canvas, this.clearButtonRectF);
                }


                if (this.InputView.FocusedStroke != null && !IsTextInputLayout)
                {
                    renderer.DrawBorder(canvas, dirtyRect, this.InputView.IsFocused, this.Stroke, this.InputView.FocusedStroke);
                }

            }

            if (this.effectsenderer != null)
                this.effectsenderer.DrawEffects(canvas);
        }

#if !MACCATALYST
        /// <summary>
        /// 
        /// </summary>
        bool IKeyboardListener.CanBecomeFirstResponder
        {
            get { return true; }
        }
#endif

#endregion

#region Public Methods

        /// <summary>
        /// Focus method for entry.
        /// </summary>
        public new void Focus()
        {
            if (this.InputView != null)
                this.InputView.Focus();
        }

        /// <summary>
        /// Unfocus method for entry.
        /// </summary>
        public new void Unfocus()
        {
            if (this.InputView != null)
                this.InputView.Unfocus();
        }

        /// <summary>
        /// Key down method for entry.
        /// </summary>
        /// <param name="args">The args.</param>
        public void OnKeyDown(KeyEventArgs args)
        {
            if (this.InputView != null)
            {
                if (this.InputView.IsDeletedButtonPressed)
                {
                    this.InputView.IsDeletedButtonPressed = false;
                }

                if (args.Key == KeyboardKey.Back)
                {
#if ANDROID
                    if (this.IsDropDownOpen)
                    {
                        this.IsDropDownOpen = false;
                    }
#endif
                    this.InputView.IsDeletedButtonPressed = true;
                }
                if (args.Key == KeyboardKey.Delete)
                {
                    this.InputView.IsDeletedButtonPressed = true;

                }
            }

            if (args.Key == KeyboardKey.Enter || args.Key == KeyboardKey.Tab)
            {
                this.ValidateSelectedItemOnEnter();
            }
            else if (args.Key == KeyboardKey.Down)
            {
                this.OnDownButtonPressed();
            }
            else if (args.Key == KeyboardKey.Escape)
            {
                if (this.IsDropDownOpen)
                {
                    this.IsDropDownOpen = false;
                }
            }
        }


        /// <summary>
        /// Key up method for entry.
        /// </summary>
        /// <param name="args">The args.</param>
        public void OnKeyUp(KeyEventArgs args)
        {

        }

#endregion

#region Internal Methods

        /// <summary>
        /// 
        /// </summary>
        internal void ShowDropdown()
        {
            if (this.dropDownView != null && this.DropdownContent != null)
            {
                // Need to remove the code
                this.dropDownView.UpdatePopupContent(this.DropdownContent);
                this.dropDownView.IsOpen = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void HideDropdown()
        {
            if (this.dropDownView != null)
            {
                this.dropDownView.IsOpen = false;
            }
        }

        /// <summary>
        /// Set RTL flow direction method.
        /// </summary>
        internal void SetRTL()
        {
            if (((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
            {
             
               this.isRTL = true;
            }
            else
            {
                this.isRTL = false;
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Arrange content method.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <returns>The size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            this.UpdateElementsBounds(bounds);
            return base.ArrangeContent(bounds);
        }

        /// <summary>
        /// Measure content method.
        /// </summary>
        /// <param name="widthConstraint">The width constraint</param>
        /// <param name="heightConstraint">The height constraint.</param>
        /// <returns>The size.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            var availableWidth = widthConstraint;
            var availableHeight = heightConstraint;

            var measuredWidth = 0d;
            var measuredHeight = 0d;

            var measure = new Size(0, 0);

            if (this.InputView != null)
            {
                measure = this.InputView.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
            }

            if (availableWidth == -1 || availableWidth == double.PositiveInfinity)
            {
                measuredWidth = measure.Width;
            }
            else
            {
                measuredWidth = availableWidth;
            }
            if (heightConstraint == -1 || heightConstraint == double.PositiveInfinity)
            {
                measuredHeight = measure.Height;
            }
            else
            {
                measuredHeight = availableHeight;
            }

            return new Size(measuredWidth, measuredHeight);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="dirtyRect"></param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);
            this.DrawEntryUI(canvas, dirtyRect);
#if WINDOWS
 if(!isRTL)
          this.Clip = new RoundRectangleGeometry(5, dirtyRect);
#elif MACCATALYST || IOS
           this.Clip = new RoundRectangleGeometry(this.IsTextInputLayout ? 0 : 6, dirtyRect);
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            if (this.dropDownView != null)
            {
                this.dropDownView.PopupOpened -= DropDownView_PopupOpened;
                this.dropDownView.PopupClosed -= DropDownView_PopupClosed;
            }
            if (this.InputView != null)
            {
                //this.InputView.Focused -= InputView_Focused;
                //this.InputView.Unfocused -= InputView_Unfocused;
                this.InputView.HandlerChanged -= InputView_HandlerChanged;
                //this.InputView.TextChanged -= InputView_TextChanged;
            }

            if (this.Handler != null)
            {
                if (this.dropDownView != null)
                {
                    this.dropDownView.PopupOpened += DropDownView_PopupOpened;
                    this.dropDownView.PopupClosed += DropDownView_PopupClosed;
                }
                if (this.InputView != null)
                {
                    //this.InputView.Focused += InputView_Focused;
                    //this.InputView.Unfocused += InputView_Unfocused;
                    this.InputView.HandlerChanged += InputView_HandlerChanged;
                    //this.InputView.TextChanged += InputView_TextChanged;
                }
            }
        }

#endregion

#region Interface Methods

        void ITouchListener.OnTouch(PointerEventArgs e)
        {
            if (this.isRTL && DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                touchPoint = e.TouchPoint;
                touchPoint.X = this.Width - touchPoint.X;
            }
            else
            {
                touchPoint = e.TouchPoint;
            }
            if (e.Action == PointerActions.Pressed)
            {
                this.isOpen = this.IsDropDownOpen;
                if (this.clearButtonRectF.Contains(touchPoint) && this.IsClearIconVisible && this.IsEditableMode)
                {
                    this.OnClearButtonTouchDown(e);
                }

                else if (this.dropdownButtonRectF.Contains(touchPoint) && this.IsDropdownIconVisible || !IsEditableMode)
                {
                    this.OnDropdownButtonTouchDown(e);
                }
            }

            if (e.Action == PointerActions.Released)
            {
                if (this.clearButtonRectF.Contains(touchPoint) && this.IsClearIconVisible && this.IsEditableMode)
                {
                    this.OnClearButtonTouchUp(e);
                }

                else if (!this.isOpen && ((this.dropdownButtonRectF.Contains(touchPoint) && this.IsDropdownIconVisible) || !IsEditableMode))
                {
                    this.OnDropdownButtonTouchUp(e);
                }

                this.isOpen = false;
            }
        }

#endregion

#region Virtual Methods

        /// <summary>
        /// Helps to update selected item text to the control used in <see cref="SfDropdownEntry"/>.
        /// </summary>
        internal abstract void ValidateSelectedItemOnEnter();

        /// <summary>
        /// Helps to update selected item text to the control used in <see cref="SfDropdownEntry"/>.
        /// </summary>
        internal abstract void OnDownButtonPressed();

        /// <summary>
        /// 
        /// </summary>
        internal abstract void OnFontPropertiesChanged();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        internal virtual void OnClearButtonTouchDown(PointerEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        internal virtual void OnClearButtonTouchUp(PointerEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        internal virtual void OnDropdownButtonTouchDown(PointerEventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        internal virtual void OnDropdownButtonTouchUp(PointerEventArgs e)
        {
			
#if Android
            if (!this.IsDropDownOpen && !this.DropDownView.IsDropDownCicked)
            {
                this.OnDropdownOpening();
                this.DropDownView.IsDropDownCicked = true;
            }
            else
                this.DropDownView.IsDropDownCicked = false;
#else
            if (!this.IsDropDownOpen)
            {
                this.OnDropdownOpening();
            }
#endif
          
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnDropdownOpening()
        {
            this.ShowDropdown();
        }

#endregion

    }
}
