// <copyright file="InputLayoutBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>


namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Controls.StyleSheets;
    using Microsoft.Maui.Devices;
    using Microsoft.Maui.Graphics;
    using Microsoft.Maui.Platform;
    using Syncfusion.Maui.Core.Internals;
    using Syncfusion.Maui.Graphics.Internals;
    using PointerEventArgs = Syncfusion.Maui.Core.Internals.PointerEventArgs;

    /// <summary>
    /// The text input layout control adds decorative elements such as floating label, icons, and assistive labels on the top of the input views.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <inputLayout:SfTextInputLayout Hint="Hint" HelperText="Helper" ErrorText="Error">
    ///     <Entry />
    /// </inputLayout:SfTextInputLayout>
    /// ]]>
    /// </code>
    /// </example>
    [ContentProperty("Content")]
    public class SfTextInputLayout : SfContentView, ITouchListener
    {

        #region Bindable Properties

        /// <summary>
        /// IsLayoutFocused is used to focus the layout.
        /// </summary>
        private static readonly BindableProperty IsLayoutFocusedProperty =
           BindableProperty.Create(nameof(IsLayoutFocused), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnIsLayoutFocusedChanged);

        /// <summary>
        /// Gets or sets a value that determines the appearance of background and its border.
        /// </summary>
        public static readonly BindableProperty ContainerTypeProperty =
            BindableProperty.Create(nameof(ContainerType), typeof(ContainerType), typeof(SfTextInputLayout), ContainerType.Filled, BindingMode.Default, null, OnContainerTypePropertyChanged);

        /// <summary>
        /// Gets or sets a value for a view to place before input view.
        /// </summary>
        public static readonly BindableProperty LeadingViewProperty =
            BindableProperty.Create(nameof(LeadingView), typeof(View), typeof(SfTextInputLayout), null, BindingMode.Default, null, OnLeadingViewChanged);

        /// <summary>
        /// Gets or sets a value for a view to place after input view.
        /// </summary>
        public static readonly BindableProperty TrailingViewProperty =
            BindableProperty.Create(nameof(TrailingView), typeof(View), typeof(SfTextInputLayout), null, BindingMode.Default, null, OnTrailingViewChanged);

        /// <summary>
        /// Gets or sets a value that indicates whether to display the leading view.
        /// </summary>
        /// <value><c>true</c> if show leading view; otherwise, <c>false</c>.</value>
        public static readonly BindableProperty ShowLeadingViewProperty =
            BindableProperty.Create(nameof(ShowLeadingView), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnShowLeadingViewPropertyChanged);

        /// <summary>
        /// Gets or sets a value that indicates whether to display the trailing view.
        /// </summary>
        /// <value><c>true</c> if show trailing view; otherwise, <c>false</c>.</value>
        public static readonly BindableProperty ShowTrailingViewProperty =
            BindableProperty.Create(nameof(ShowTrailingView), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnShowTrailingViewPropertyChanged);

        /// <summary>
        /// Gets or sets a value that indicates whether to display the hint text.
        /// </summary>
        /// <value><c>true</c> if show hint; otherwise, <c>false</c>.</value>
        public static readonly BindableProperty ShowHintProperty =
            BindableProperty.Create(nameof(ShowHint), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets a value that indicates whether to display the character count when the value is changed.
        /// </summary>
        /// <value><c>true</c> if enable counter; otherwise, <c>false</c>.</value>
        internal static readonly BindableProperty ShowCharCountProperty =
            BindableProperty.Create(nameof(ShowCharCount), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets a value indicating whether to display the helper text and error text. It determines the visibility of the helper text and error text.
        /// </summary>
        /// <value><c>true</c> if show helper text; otherwise, <c>false</c>.</value>
        public static readonly BindableProperty ShowHelperTextProperty =
            BindableProperty.Create(nameof(ShowHelperText), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.TwoWay, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets a value that indicates whether it has validation errors.
        /// </summary>
        /// <value><c>true</c> if has error; otherwise, <c>false</c>.</value>
        public static readonly BindableProperty HasErrorProperty =
            BindableProperty.Create(nameof(HasError), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnHasErrorPropertyChanged);

        /// <summary>
        /// Gets or sets a value that indicates whether to fix the hint label always at the top even when the text is empty.
        /// </summary>
        public static readonly BindableProperty IsHintAlwaysFloatedProperty =
            BindableProperty.Create(nameof(IsHintAlwaysFloated), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnHintAlwaysFloatedPropertyChanged);

        /// <summary>
        /// Gets or sets the border color our base line color based on the container.
        /// </summary>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(SfTextInputLayout), Color.FromRgba("79747E"), BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets the background color of the container.
        /// </summary>
        public static readonly BindableProperty ContainerBackgroundProperty =
            BindableProperty.Create(nameof(ContainerBackground), typeof(Brush), typeof(SfTextInputLayout), new SolidColorBrush(Color.FromRgba("E7E0EC")), BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets a value to customize the customize the corner radius of outline border.
        /// </summary>
        /// <remarks> it is applicable only when set the container type as outlined.</remarks>
        public static readonly BindableProperty OutlineCornerRadiusProperty =
            BindableProperty.Create(nameof(OutlineCornerRadius), typeof(double), typeof(SfTextInputLayout), 3.5d, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets as value to customize the stroke width in focused state, it is applicable for the bottom line and outline border.
        /// when setting the container type as filled and outlined respectively.
        /// </summary>
        public static readonly BindableProperty FocusedStrokeThicknessProperty =
            BindableProperty.Create(nameof(FocusedStrokeThickness), typeof(double), typeof(SfTextInputLayout), 2d, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets as value to customize the stroke width in unfocused state, it is applicable for the bottom line and outline border
        /// when setting the container type as filled and outlined respectively.
        /// </summary>
        public static readonly BindableProperty UnfocusedStrokeThicknessProperty =
            BindableProperty.Create(nameof(UnfocusedStrokeThickness), typeof(double), typeof(SfTextInputLayout), 1d, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets the length of the maximum character value. When the character count reaches the character maximum length, the error color will be applied.
        /// </summary>
        public static readonly BindableProperty CharMaxLengthProperty =
            BindableProperty.Create(nameof(CharMaxLength), typeof(int), typeof(SfTextInputLayout), int.MaxValue, BindingMode.Default, null, OnCharMaxLengthPropertyChanged);

        /// <summary>
        /// Gets or sets a value for hint text in the input view.
        /// </summary>
        public static readonly BindableProperty HintProperty =
            BindableProperty.Create(nameof(Hint), typeof(string), typeof(SfTextInputLayout), string.Empty, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets a value for helper text in the input view. It conveys the additional information about the text in the input view.
        /// </summary>
        public static readonly BindableProperty HelperTextProperty =
            BindableProperty.Create(nameof(HelperText), typeof(string), typeof(SfTextInputLayout), string.Empty, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets the value for validation error. It will be displayed till entering the correct text in the input view.
        /// </summary>
        public static readonly BindableProperty ErrorTextProperty =
            BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(SfTextInputLayout), string.Empty, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets a value indicating whether to allocate space for assistive texts.
        /// assistive labels includes helper text, error text and character counter.
        /// </summary>
        /// <remarks> when this property is set as false, assitive labels will not be shown even they are not enabled.</remarks>
        public static readonly BindableProperty ReserveSpaceForAssistiveLabelsProperty =
         BindableProperty.Create(nameof(ReserveSpaceForAssistiveLabels), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnReserveSpacePropertyChanged);

        /// <summary>
        /// Gets or sets the value that determines whether to place the leading view within the layout.
        /// </summary>
        public static readonly BindableProperty LeadingViewPositionProperty =
            BindableProperty.Create(nameof(LeadingViewPosition), typeof(ViewPosition), typeof(SfTextInputLayout), ViewPosition.Inside, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets a value that determines whether to place the trailing view within the layout.
        /// </summary>
        public static readonly BindableProperty TrailingViewPositionProperty =
            BindableProperty.Create(nameof(TrailingViewPosition), typeof(ViewPosition), typeof(SfTextInputLayout), ViewPosition.Inside, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets desired padding to override default padding for input view.
        /// </summary>
        public static readonly BindableProperty InputViewPaddingProperty =
            BindableProperty.Create(nameof(InputViewPadding), typeof(Thickness), typeof(SfTextInputLayout), new Thickness(-1, -1, -1, -1), BindingMode.Default, null, OnInputViewPaddingPropertyChanged);

        /// <summary>
        /// Gets or sets a value that indicates whether to show the password visibility toggle.
        /// </summary>
        /// <value><c>true</c> if enable password visibility toggle; otherwise, <c>false</c>.</value>
        /// <remarks>This property supports for <see cref="Entry"/> only.</remarks>
        public static readonly BindableProperty EnablePasswordVisibilityToggleProperty =
            BindableProperty.Create(nameof(EnablePasswordVisibilityToggle), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnEnablePasswordTogglePropertyChanged);

        /// <summary>
        /// Gets or sets a value that indicates whether to show the clear button.
        /// </summary>
        /// <value><c>false</c> if disable clear button; otherwise, <c>true</c>.</value>
        /// <remarks>This property supports for SfComboBox only.</remarks>
        internal static readonly BindableProperty ShowClearButtonProperty =
            BindableProperty.Create(nameof(ShowClearButton), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets a value that indicates whether to float the label when it is focused or unfocused.  
        /// </summary>
        public static readonly BindableProperty EnableFloatingProperty =
            BindableProperty.Create(nameof(EnableFloating), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets a value that indicates whether to enable animation for the hint text 
        /// when the input view is focused or unfocused.  
        /// </summary>
        /// <value><c>true</c> if enable hint animation; otherwise, <c>false</c>.</value>
        public static readonly BindableProperty EnableHintAnimationProperty =
            BindableProperty.Create(nameof(EnableHintAnimation), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets a value that indicates whether to show the clear button.
        /// </summary>
        /// <value><c>false</c> if disable clear button; otherwise, <c>true</c>.</value>
        /// <remarks>This property supports for SfComboBox only.</remarks>
        internal static readonly BindableProperty ShowDropDownButtonProperty =
            BindableProperty.Create(nameof(ShowDropDownButton), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnPropertyChanged);


        /// <summary>
        /// Gets or sets the color of the clear button.
        /// </summary>
        internal static readonly BindableProperty ClearButtonColorProperty =
            BindableProperty.Create(nameof(ClearButtonColor), typeof(Color), typeof(SfTextInputLayout), Colors.Grey, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets the color of the clear button.
        /// </summary>
        internal static readonly BindableProperty DropDownButtonColorProperty =
            BindableProperty.Create(nameof(DropDownButtonColor), typeof(Color), typeof(SfTextInputLayout), Colors.Grey, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets the style for hint label.
        /// </summary>
        public static readonly BindableProperty HintLabelStyleProperty =
            BindableProperty.Create(nameof(HintLabelStyle), typeof(LabelStyle), typeof(SfTextInputLayout), null, BindingMode.TwoWay, null, defaultValueCreator: bindale => GetHintLabelStyleDefaultValue(), propertyChanged: OnHintLableStylePropertyChanged);

        /// <summary>
        /// Gets or sets the style for helper label.
        /// </summary>
        public static readonly BindableProperty HelperLabelStyleProperty =
            BindableProperty.Create(nameof(HelperLabelStyle), typeof(LabelStyle), typeof(SfTextInputLayout), null, BindingMode.TwoWay, null, defaultValueCreator: bindale => GetHelperLabelStyleDefaultValue(), propertyChanged: OnHelperLableStylePropertyChanged);

        /// <summary>
        /// Gets or sets the style for error label.
        /// </summary>
        public static readonly BindableProperty ErrorLabelStyleProperty =
            BindableProperty.Create(nameof(ErrorLabelStyle), typeof(LabelStyle), typeof(SfTextInputLayout), null, BindingMode.TwoWay, null, defaultValueCreator: bindale => GetErrorLabelStyleDefaultValue(), propertyChanged: OnErrorLableStylePropertyChanged);

        /// <summary>
        /// Gets or sets the style for counter label.
        /// </summary>
        internal static readonly BindableProperty CounterLabelStyleProperty =
            BindableProperty.Create(nameof(CounterLabelStyle), typeof(LabelStyle), typeof(SfTextInputLayout), null, BindingMode.TwoWay, null, defaultValueCreator: bindale => GetCounterLabelStyleDefaultValue(), propertyChanged: OnCounterLableStylePropertyChanged);

        /// <summary>
        /// Gets a value for current active color based on input view focus state.  
        /// </summary>
        internal static readonly BindablePropertyKey CurrentActiveColorKey =
           BindableProperty.CreateReadOnly(nameof(CurrentActiveColor), typeof(Color), typeof(SfTextInputLayout), Color.FromRgba("79747E"), BindingMode.Default, null, null);

        /// <summary>
        /// Gets or sets a value that indicates whether the hint is floated or not.
        /// </summary>
        /// <value><c>true</c> if hint floated; otherwise, <c>false</c>.</value>
        internal static readonly BindableProperty IsHintFloatedProperty =
            BindableProperty.Create(nameof(IsHintFloated), typeof(bool), typeof(SfTextInputLayout), false, BindingMode.Default, null, OnIsHintFloatedPropertyChanged);

        /// <summary>
        /// Gets a value for current active color based on input view focus state.
        /// </summary>
        public static readonly BindableProperty CurrentActiveColorProperty = CurrentActiveColorKey.BindableProperty;

        /// <summary>
        /// Gets or sets a value that indicates whether users can interact with the control.  
        /// </summary>
        public static new readonly BindableProperty IsEnabledProperty =
            BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(SfTextInputLayout), true, BindingMode.Default, null, OnEnabledPropertyChanged);

        /// <summary>
        /// Returns the default label style for hint label.
        /// </summary>
        /// <returns>The LabelStyle</returns>
        private static LabelStyle GetHintLabelStyleDefaultValue()
        {
            return new LabelStyle() { FontSize = DefaultHintFontSize };
        }

        /// <summary>
        /// Returns the default label style for helper label.
        /// </summary>
        /// <returns>The LabelStyle</returns>
        private static LabelStyle GetHelperLabelStyleDefaultValue()
        {
            return new LabelStyle();
        }

        /// <summary>
        /// Returns the default label style for Error label.
        /// </summary>
        /// <returns>The LabelStyle</returns>
        private static LabelStyle GetErrorLabelStyleDefaultValue()
        {
            return new LabelStyle();
        }

        /// <summary>
        /// Returns the default label style for counter label.
        /// </summary>
        /// <returns>The LabelStyle</returns>
        private static LabelStyle GetCounterLabelStyleDefaultValue()
        {
            return new LabelStyle();
        }

        #endregion

        #region Fields

        /// <summary>
        /// Gets the padding value of the helper text, error text, counter text.
        /// </summary>
        private const double DefaultAssistiveLabelPadding = 4;

        /// <summary>
        /// Gets the height value of the helper text, error text, counter text.
        /// </summary>
        private const double DefaultAssistiveTextHeight = 16;

        /// <summary>
        /// Gets the height value of the hint text.
        /// </summary>
        private const double DefaultHintTextHeight = 24;

        /// <summary>
        /// Gets the top padding of the input view when container type was filled.
        /// </summary>
        private const double FilledTopPadding = 24;

        /// <summary>
        /// Gets the bottom padding of the input view when container type was filled.
        /// </summary>
        private const double FilledBottomPadding = 10;

        /// <summary>
        /// Gets the bottom padding of the input view when container type was none.
        /// </summary>
        private const double NoneBottomPadding = 5;

        /// <summary>
        /// Gets the top padding of the input view when container type was none.
        /// </summary>
        private const double NoneTopPadding = 32;

        /// <summary>
        /// Gets the top and bottom padding of the input view when container type was outlined.
        /// </summary>
        private const double OutlinedPadding = 16;

        /// <summary>
        /// Gets the left padding of the input view when container type was outlined and filled.
        /// </summary>
        private const double EdgePadding = 16;

        /// <summary>
        /// Gets the right padding of the input view when container type was outlined and filled.
        /// </summary>
        private const double RightPadding = 16;

        /// <summary>
        /// Gets the value of floated hint starting x position when container type is outlined.
        /// </summary>
        private const float StartX = 12;

        /// <summary>
        /// Gets the font size of the hint text when layout in unfocused state.
        /// </summary>
        private const float DefaultHintFontSize = 16;

        /// <summary>
        /// Gets the font size of the floated hint text.
        /// </summary>
        private const float FloatedHintFontSize = 12;

        /// <summary>
        /// Gets the duration of the animation in milliseconds.
        /// </summary>
        private const double DefaultAnimationDuration = 167;

        /// <summary>
        /// Gets the size of the clear button and password toggle visibilityS icon.
        /// </summary>
        private const int IconSize = 32;

        /// <summary>
        /// Gets or sets a value of right side space for counter text.
        /// </summary>
        private const float CounterTextPadding = 12;

        /// <summary>
        /// Gets or sets the string value of counter text.
        /// </summary>
        private string counterText = "0";

        /// <summary>
        /// Gets or sets a value indicating the hint was animating.
        /// </summary>
        private bool isAnimating = false;

        /// <summary>
        /// Gets or sets a value indicating the content is editable or not.
        /// </summary>
        private bool isNonEditableContent = false;

        /// <summary>
        /// Gets or sets a value indicating the font size of hint during animation.
        /// </summary>
        private double animatingFontSize;

        /// <summary>
        /// Gets or sets the left padding value of leading view.
        /// </summary>
        private double leadingViewLeftPadding = 8;

        /// <summary>
        /// Gets or sets the right padding value of leading view.
        /// </summary>
        private double leadingViewRightPadding = 12;

        /// <summary>
        /// Gets or sets the left padding value of trailing view.
        /// </summary>
        private double trailingViewLeftPadding = 12;

        /// <summary>
        /// Gets or sets the right padding value of trailing view.
        /// </summary>
        private double trailingViewRightPadding = 8;

        /// <summary>
        /// Gets or sets the leading or trailing view default width.
        /// </summary>
        private double defaultLeadAndTrailViewWidth = 24;

        /// <summary>
        /// Gets or sets a value of leading view width.
        /// </summary>
        private double leadViewWidth = 0;

        /// <summary>
        /// Gets or sets a value of trailing view width.
        /// </summary>
        private double trailViewWidth = 0;

        /// <summary>
        /// Gets or sets the left and right padding value for password visibility toggle icon.
        /// </summary>
        private float passwordToggleIconEdgePadding = 8;

        /// <summary>
        /// Gets or sets the top and bottom padding value for password visibility toggle icon in collapsed state.
        /// </summary>
        private float passwordToggleCollapsedTopPadding = 9;

        /// <summary>
        /// Gets or sets the top and bottom padding value for password visibility toggle icon in visible state.
        /// </summary>
        private float passwordToggleVisibleTopPadding = 10;

        /// <summary>
        /// Gets the path data value for password toggle visible icon.
        /// </summary>
        private string toggleVisibleIconPath = "M8,3.3000005C9.2070007,3.3000005 10.181999,4.2819988 10.181999,5.5000006 10.181999,6.7179991 9.2070007,7.6999975 8,7.6999975 6.7929993,7.6999975 5.8180008,6.7179991 5.8180008,5.5000006 5.8180008,4.2819988 6.7929993,3.3000005 8,3.3000005z M8,1.8329997C5.993,1.8329997 4.3639984,3.475999 4.3639984,5.5000006 4.3639984,7.5249983 5.993,9.1669999 8,9.1669999 10.007,9.1669999 11.636002,7.5249983 11.636002,5.5000006 11.636002,3.475999 10.007,1.8329997 8,1.8329997z M8,0C11.636002,-1.1919138E-07 14.742001,2.2800001 16,5.5000006 14.742001,8.7199975 11.636002,11 8,11 4.3639984,11 1.2579994,8.7199975 0,5.5000006 1.2579994,2.2800001 4.3639984,-1.1919138E-07 8,0z";

        /// <summary>
        /// Gets the path data value for password toggle collapsed icon.
        /// </summary>
        private string toggleCollapsedIconPath = "M4.7510004,4.9479995C4.5109997,5.4359995 4.3660002,5.9739994 4.3660002,6.5489993 4.3660002,8.5569992 5.9949999,10.186999 8.0030003,10.186999 8.5780001,10.186999 9.1160002,10.040999 9.6040001,9.8009992L8.4760003,8.6729991C8.3239999,8.7099991 8.1630001,8.7319992 8.0030003,8.7319992 6.796,8.7319992 5.8210001,7.7569993 5.8210001,6.5489993 5.8210001,6.3889993 5.8429999,6.2289994 5.8790002,6.0759995z M8.0110002,4.3729997C9.2189999,4.3729995,10.194,5.3479995,10.194,6.5559993L10.179,6.6719995 7.8870001,4.3809996z M7.9960003,1.092C11.634,1.092 14.741,3.3549995 16,6.5489993 15.469,7.9019992 14.603,9.0879991 13.504,10.004999L11.38,7.8799992C11.547,7.4659992 11.641,7.0219994 11.641,6.5489993 11.641,4.5399996 10.012,2.9099996 8.0030003,2.9099996 7.5310001,2.9099996 7.0869999,3.0049996 6.6719999,3.1729996L5.1000004,1.6009998C6.0030003,1.2739997,6.9770002,1.092,7.9960003,1.092z M1.6520004,0L14.552,12.900999 13.628,13.823998 11.496,11.699999 11.19,11.394999C10.208,11.786999 9.131,12.005999 8.0030003,12.005999 4.3660002,12.005999 1.2589998,9.7429991 0,6.5489993 0.56700039,5.1089995 1.5130005,3.8569996 2.7209997,2.9179997L2.3870001,2.5829997 0.72700024,0.92399979z";

        /// <summary>
        /// Gets or sets a value indicating the input password text is visible.
        /// </summary>
        private bool isPasswordTextVisible = false;

        /// <summary>
        /// Gets or sets a value indicating the hint was animating from down to up.
        /// </summary>
        private bool isHintDownToUp = true;

        /// <summary>
        /// Gets or sets the start value for custom animation.
        /// </summary>
        private double translateStart = 0;

        /// <summary>
        /// Gets or sets the end value for custom animation.
        /// </summary>
        private double translateEnd = 1;

        /// <summary>
        /// Gets or sets the start value for scaling animation.
        /// </summary>
        double fontSizeStart = DefaultHintFontSize;

        /// <summary>
        /// Gets or sets the end value for scaling animation.
        /// </summary>
        double fontSizeEnd = FloatedHintFontSize;

        /// <summary>
        /// Gets or sets the animating font value for scaling custom animation.
        /// </summary>
        float fontsize = DefaultHintFontSize;

        private EffectsRenderer effectsenderer;

        private PathBuilder pathBuilder = new();

        private RectF primaryIconRectF = new();

        private RectF secondaryIconRectF = new();

        private RectF outlineRectF = new();

        private RectF hintRect = new();

        private RectF backgroundRectF = new();

        private RectF clipRect = new();

        private RectF viewBounds = new();

        private PointF startPoint = new();

        private PointF endPoint = new();

        private RectF helperTextRect = new();

        private RectF errorTextRect = new();

        private RectF counterTextRect = new();

        private LabelStyle internalHintLabelStyle = new();

        private LabelStyle internalHelperLabelStyle = new();

        private LabelStyle internalErrorLabelStyle = new();

        private LabelStyle internalCounterLabelStyle = new();

        private IDropdownRenderer? tilRenderer;

        private SfDropdownEntry? dropdownEntry;

        private Rect? oldBounds;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfTextInputLayout"/> class.
        /// </summary>
        public SfTextInputLayout()
        {
            this.ValidateLicense(true);
            this.DrawingOrder = DrawingOrder.BelowContent;
            this.HintFontSize = DefaultHintFontSize;
            this.SetRendererBasedOnPlatform();
            this.AddTouchListener(this);
            this.effectsenderer = new EffectsRenderer(this);
            this.PropertyChanged += SfTextInputLayout_PropertyChanged;
            this.AddDefaultVSM();
            this.UpdateViewBounds();
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to display the hint text.
        /// </summary>
        /// <value><c>true</c> if show hint; otherwise, <c>false</c>.</value>
        public bool ShowHint
        {
            get { return (bool)this.GetValue(ShowHintProperty); }
            set { this.SetValue(ShowHintProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display the character count when the value is changed.
        /// </summary>
        /// <value><c>true</c> if enable counter; otherwise, <c>false</c>.</value>
        internal bool ShowCharCount
        {
            get { return (bool)this.GetValue(ShowCharCountProperty); }
            set { this.SetValue(ShowCharCountProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display the helper text and error text. It determines the visibility of the helper text and error text.
        /// </summary>
        /// <value><c>true</c> if show helper text; otherwise, <c>false</c>.</value>
        public bool ShowHelperText
        {
            get => (bool)this.GetValue(ShowHelperTextProperty);
            set => this.SetValue(ShowHelperTextProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether it has validation errors. It determines the visibility of the error text.
        /// </summary>
        /// <value><c>true</c> if has error; otherwise, <c>false</c>.</value>
        public bool HasError
        {
            get => (bool)this.GetValue(HasErrorProperty);
            set => this.SetValue(HasErrorProperty, value);
        }

        /// <summary>
        /// Gets or sets the border color or base line color based on container.
        /// </summary>
        public Color Stroke
        {
            get => (Color)this.GetValue(StrokeProperty);
            set => this.SetValue(StrokeProperty, value);
        }

        /// <summary>
        /// Gets or sets the Hint text font size.
        /// </summary>
        internal float HintFontSize { get; set; }

        /// <summary>
        /// Gets or sets the background color of the container.
        /// </summary>
        public Brush ContainerBackground
        {
            get => (Brush)this.GetValue(ContainerBackgroundProperty);
            set => this.SetValue(ContainerBackgroundProperty, value);
        }

        /// <summary>
        /// Gets or sets a value to customize the corner radius of outline border.
        /// </summary>
        /// <remarks>It is applicable only when set the container type as outlined.</remarks>
        /// <value>The default value is 4.</value>
        public double OutlineCornerRadius
        {
            get => (double)this.GetValue(OutlineCornerRadiusProperty);
            set => this.SetValue(OutlineCornerRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets as value to customize the stroke width in focused state, it is applicable for the bottom line and outline border
        /// when setting the container type as filled and outlined respectively.
        /// </summary>
        /// <value>The default value is 2.</value>
        public double FocusedStrokeThickness
        {
            get => (double)this.GetValue(FocusedStrokeThicknessProperty);
            set => this.SetValue(FocusedStrokeThicknessProperty, value);
        }

        /// <summary>
        /// Gets or sets as value to customize the stroke width in unfocused state, it is applicable for the bottom line and outline border
        /// when setting the container type as filled and outlined respectively.
        /// </summary>
        /// <value>The default value is 1.</value>
        public double UnfocusedStrokeThickness
        {
            get => (double)this.GetValue(UnfocusedStrokeThicknessProperty);
            set => this.SetValue(UnfocusedStrokeThicknessProperty, value);
        }

        /// <summary>
        /// Gets or sets the length of the maximum character value. When the character count reaches the character maximum length, the error color will be applied.
        /// </summary>
        public int CharMaxLength
        {
            get => (int)this.GetValue(CharMaxLengthProperty);
            set => this.SetValue(CharMaxLengthProperty, value);
        }

        /// <summary>
        /// Gets or sets a value for a view to place before input view.
        /// </summary>
        public View LeadingView
        {
            get => (View)this.GetValue(LeadingViewProperty);
            set => this.SetValue(LeadingViewProperty, value);
        }

        /// <summary>
        /// Gets or sets a value for a view to place after input view.
        /// </summary>
        public View TrailingView
        {
            get => (View)this.GetValue(TrailingViewProperty);
            set => this.SetValue(TrailingViewProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show or hide the leading view.
        /// </summary>
        /// <value><c>true</c> to show leading view; <c>false</c> to hide.</value>
        public bool ShowLeadingView
        {
            get => (bool)this.GetValue(ShowLeadingViewProperty);
            set => this.SetValue(ShowLeadingViewProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show or hide the trailing view.
        /// </summary>
        /// <value><c>true</c> to show trailing view; <c>false</c> to hide.</value>
        public bool ShowTrailingView
        {
            get => (bool)this.GetValue(ShowTrailingViewProperty);
            set => this.SetValue(ShowTrailingViewProperty, value);
        }

        /// <summary>
        /// Gets or sets the value that determines whether to place the leading view within the layout.
        /// </summary>
        public ViewPosition LeadingViewPosition
        {
            get => (ViewPosition)this.GetValue(LeadingViewPositionProperty);
            set => this.SetValue(LeadingViewPositionProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that determines whether to place the trailing view within the layout.
        /// </summary>
        public ViewPosition TrailingViewPosition
        {
            get => (ViewPosition)this.GetValue(TrailingViewPositionProperty);
            set => this.SetValue(TrailingViewPositionProperty, value);
        }

        /// <summary>
        /// Gets or sets desired padding to override default padding for input view.
        /// </summary>
        public Thickness InputViewPadding
        {
            get { return (Thickness)GetValue(InputViewPaddingProperty); }
            set { SetValue(InputViewPaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this control is focused.
        /// </summary>
        private bool IsLayoutFocused
        {
            get => (bool)this.GetValue(IsLayoutFocusedProperty);
            set => this.SetValue(IsLayoutFocusedProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to fix the hint label always at the top even when the text is empty.
        /// </summary>
        public bool IsHintAlwaysFloated
        {
            get => (bool)this.GetValue(IsHintAlwaysFloatedProperty);
            set => this.SetValue(IsHintAlwaysFloatedProperty, value);
        }

        /// <summary>
        /// Gets or sets a value for hint text in the input view.
        /// </summary>
        public string Hint
        {
            get => (string)this.GetValue(HintProperty);
            set => this.SetValue(HintProperty, value);
        }

        /// <summary>
        /// Gets or sets a value for helper text in the input view. It conveys the additional information about the text in the input view.
        /// </summary>
        public string HelperText
        {
            get => (string)this.GetValue(HelperTextProperty);
            set => this.SetValue(HelperTextProperty, value);
        }

        /// <summary>
        /// Gets or sets the value for validation error. Error messages are displayed below the input line, replacing helper text until fixed.
        /// </summary>
        public string ErrorText
        {
            get => (string)this.GetValue(ErrorTextProperty);
            set => this.SetValue(ErrorTextProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allocate space for assistive labels.
        /// Assistive labels includes helper text, error text and character counter.
        /// </summary>
        /// <remarks>When this property is set as false, space will be allocated based on the values of helper text, error text and character count labels.</remarks>
        public bool ReserveSpaceForAssistiveLabels
        {
            get => (bool)this.GetValue(ReserveSpaceForAssistiveLabelsProperty);
            set => this.SetValue(ReserveSpaceForAssistiveLabelsProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that determines the appearance of background and its border.
        /// </summary>
        public ContainerType ContainerType
        {
            get => (ContainerType)this.GetValue(ContainerTypeProperty);
            set => this.SetValue(ContainerTypeProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the password visibility toggle.
        /// </summary>
        /// <value><c>true</c> if enable password visibility toggle; otherwise, <c>false</c>.</value>
        /// <remarks>This property supports for <see cref="Entry"/> only.</remarks>
        public bool EnablePasswordVisibilityToggle
        {
            get => (bool)this.GetValue(EnablePasswordVisibilityToggleProperty);
            set => this.SetValue(EnablePasswordVisibilityToggleProperty, value);
        }

        /// <summary>
        /// Gets a value for current active color based on input view focus state.  
        /// </summary>
        public Color CurrentActiveColor
        {
            get { return (Color)GetValue(CurrentActiveColorProperty); }
            internal set { SetValue(CurrentActiveColorKey, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether users can interact with the control.  
        /// </summary>
        public new bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the DropDown Icon.
        /// </summary>
        /// <value><c>true</c> if DropDown button is visible; otherwise, <c>false</c>.</value>
        /// <remarks>This property supports for DropDown controls only.</remarks>
        internal bool ShowDropDownButton
        {
            get => (bool)this.GetValue(ShowDropDownButtonProperty);
            set => this.SetValue(ShowDropDownButtonProperty, value);
        }

        /// <summary>
        /// Gets or sets the color of the clear button.
        /// </summary>
        internal Color DropDownButtonColor
        {
            get => (Color)this.GetValue(DropDownButtonColorProperty);
            set => this.SetValue(DropDownButtonColorProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the clear button.
        /// </summary>
        /// <value><c>false</c> if disable clear button; otherwise, <c>true</c>.</value>
        /// <remarks>This property supports for SfCombobox and SfAutoComplete only.</remarks>
        internal bool ShowClearButton
        {
            get => (bool)this.GetValue(ShowClearButtonProperty);
            set => this.SetValue(ShowClearButtonProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to float the label when it is focused or unfocused.  
        /// </summary>
        public bool EnableFloating
        {
            get => (bool)this.GetValue(EnableFloatingProperty);
            set => this.SetValue(EnableFloatingProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enable animation for the hint text 
        /// when the input view is focused or unfocused.  
        /// </summary>
        /// <value><c>true</c> if enable hint animation; otherwise, <c>false</c>.</value>
        public bool EnableHintAnimation
        {
            get => (bool)this.GetValue(EnableHintAnimationProperty);
            set => this.SetValue(EnableHintAnimationProperty, value);
        }


        /// <summary>
        /// Gets or sets the color of the clear button.
        /// </summary>
        internal Color ClearButtonColor
        {
            get => (Color)this.GetValue(ClearButtonColorProperty);
            set => this.SetValue(ClearButtonColorProperty, value);
        }

        /// <summary>
        /// Gets or sets the style for hint label.
        /// </summary>
        public LabelStyle HintLabelStyle
        {
            get { return (LabelStyle)GetValue(HintLabelStyleProperty); }
            set { SetValue(HintLabelStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for helper label.
        /// </summary>
        public LabelStyle HelperLabelStyle
        {
            get { return (LabelStyle)GetValue(HelperLabelStyleProperty); }
            set { SetValue(HelperLabelStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for error label.
        /// </summary>
        public LabelStyle ErrorLabelStyle
        {
            get { return (LabelStyle)GetValue(ErrorLabelStyleProperty); }
            set { SetValue(ErrorLabelStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for counter label.
        /// </summary>
        internal LabelStyle CounterLabelStyle
        {
            get { return (LabelStyle)GetValue(CounterLabelStyleProperty); }
            set { SetValue(CounterLabelStyleProperty, value); }
        }

        /// <summary>
        /// Gets the value of input text of the text input layout.
        /// </summary>
        public string? Text { get; private set; }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets a value indicating whether the background mode is outline.
        /// </summary>
        internal bool IsOutlined
        {
            get { return this.ContainerType == ContainerType.Outlined; }
        }

        /// <summary>
        /// Gets a value indicating whether the background mode is none.
        /// </summary>
        internal bool IsNone
        {
            get { return this.ContainerType == ContainerType.None; }
        }

        /// <summary>
        /// Gets a value indicating whether the background mode is filled.
        /// </summary>
        internal bool IsFilled
        {
            get { return this.ContainerType == ContainerType.Filled; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the hint was floated.
        /// </summary>
        /// <remarks>This property is used to update the control UI based of hint state.</remarks>
        private bool IsHintFloated
        {
            get { return (bool)GetValue(IsHintFloatedProperty); }
            set { SetValue(IsHintFloatedProperty, value); }
        }

        private Color DisabledColor { get { return Color.FromUint(0x42000000); } }

        /// <summary>
        /// Gets a value indicating the size of hint.
        /// </summary>
        internal SizeF HintSize
        {
            get
            {
                if (string.IsNullOrEmpty(this.Hint) || this.HintLabelStyle == null)
                {
                    return new Size(0, DefaultHintTextHeight);
                }

                this.internalHintLabelStyle.FontSize = this.HintFontSize;
                var size = this.Hint.Measure(internalHintLabelStyle);
                size.Height = DefaultHintTextHeight;
                size.Width += DefaultAssistiveLabelPadding + DefaultAssistiveLabelPadding;
                return size;
            }
        }

        /// <summary>
        /// Gets a value indicating the size of floated hint.
        /// </summary>
        internal SizeF FloatedHintSize
        {
            get
            {
                if (string.IsNullOrEmpty(this.Hint) || this.HintLabelStyle == null)
                {
                    return new Size(0, DefaultAssistiveTextHeight);
                }

                this.internalHintLabelStyle.FontSize = FloatedHintFontSize;
                var size = this.Hint.Measure(this.internalHintLabelStyle);
                size.Height = DefaultAssistiveTextHeight;
                size.Width += DefaultAssistiveLabelPadding + DefaultAssistiveLabelPadding;
                return size;
            }
        }

        /// <summary>
        /// Gets a value indicating the size of assistive text.
        /// </summary>
        internal SizeF CounterTextSize
        {
            get
            {
                if (string.IsNullOrEmpty(this.counterText) || this.CounterLabelStyle == null)
                {
                    return GetLabelSize(new Size(0, DefaultAssistiveTextHeight));
                }

                var size = this.counterText.Measure(this.internalCounterLabelStyle);
                size.Height = DefaultAssistiveTextHeight;
                size.Width += DefaultAssistiveLabelPadding;
                return GetLabelSize(size);
            }
        }

        /// <summary>
        /// Gets a value indicating the size of helper text.
        /// </summary>
        internal SizeF HelperTextSize
        {
            get
            {
                if (string.IsNullOrEmpty(this.HelperText) || this.HelperLabelStyle == null)
                {
                    return GetLabelSize(new Size(0, DefaultAssistiveTextHeight));
                }

                var size = this.HelperText.Measure(this.internalHelperLabelStyle);
                size.Height = GetNumberOfLines(size.Width) * DefaultAssistiveTextHeight;
                return GetLabelSize(size);
            }
        }

        /// <summary>
        /// Gets a value indicating the size of Error text.
        /// </summary>
        internal SizeF ErrorTextSize
        {
            get
            {
                if (string.IsNullOrEmpty(this.ErrorText) || this.ErrorLabelStyle == null)
                {
                    return GetLabelSize(new Size(0, DefaultAssistiveTextHeight));
                }

                var size = this.ErrorText.Measure(this.internalErrorLabelStyle);
                size.Height = GetNumberOfLines(size.Width) * DefaultAssistiveTextHeight;
                return GetLabelSize(size);
            }
        }

        /// <summary>
        /// Gets the base line max height.
        /// </summary>
        internal double BaseLineMaxHeight => Math.Max(this.FocusedStrokeThickness, this.UnfocusedStrokeThickness);

        /// <summary>
        /// Gets a value indicating the top padding of the input view.
        /// </summary>
        private double TopPadding
        {
            get { return this.IsOutlined ? OutlinedPadding + (DefaultAssistiveTextHeight / 2) : this.IsFilled ? FilledTopPadding : NoneTopPadding; }
        }

        /// <summary>
        /// Gets a value indicating the bottom padding of the input view.
        /// </summary>
        private double BottomPadding
        {
            get
            {
                return (this.IsFilled ? FilledBottomPadding
                    : this.IsOutlined ? OutlinedPadding : NoneBottomPadding) + (ReserveSpaceForAssistiveLabels ? TotalAssistiveTextHeight() + DefaultAssistiveLabelPadding : 0);
            }
        }

        /// <summary>
        /// Gets a value indicating the edge padding of the input view.
        /// </summary>
        private double LeftPadding
        {
            get
            {
                return this.IsNone ? 0 : EdgePadding;
            }
        }

        private double AssistiveLabelPadding
        {
            get
            {
                return this.ReserveSpaceForAssistiveLabels ? DefaultAssistiveLabelPadding + BaseLineMaxHeight / 2 : BaseLineMaxHeight / 2;
            }
        }

        private PathF ToggleIconPath
        {
            get
            {
                return this.isPasswordTextVisible ? this.pathBuilder.BuildPath(this.toggleVisibleIconPath) : this.pathBuilder.BuildPath(this.toggleCollapsedIconPath);
            }
        }

        private bool isRTL
        {
            get { return ((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft; }
        }

        private bool IsDropDownIconVisible
        {
            get { return (this.ShowDropDownButton && this.Content is SfDropdownEntry); }
        }

        private bool IsClearIconVisible
        {
            get { return (this.ShowClearButton && this.IsLayoutFocused && !this.isNonEditableContent && !string.IsNullOrEmpty(this.Text)); }
        }

        private bool IsPassowordToggleIconVisible
        {
            get { return ((this.IsLayoutFocused || this.IsHintFloated) && this.EnablePasswordVisibilityToggle && this.Content is Entry); }
        }
        #endregion

        #region Interface Implementation

        /// <summary>
        /// This method invoke when touch the control.
        /// </summary>
        /// <param name="e">PointerEventArgs.</param>
        async void ITouchListener.OnTouch(PointerEventArgs e)
        {
            // In Windows touch point doesn't change based on RTL 
            // Need to remove the workaround for above issue once the issue fixed in Windows platfrom.
            Point touchPoint;
            if (this.isRTL && DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                touchPoint = e.TouchPoint;
                touchPoint.X = this.Width - touchPoint.X;
            }
            else
            {
                touchPoint = e.TouchPoint;
            }

            if (e.Action == PointerActions.Released)
            {
                if ((this.EnablePasswordVisibilityToggle || this.ShowDropDownButton) && this.primaryIconRectF.Contains(touchPoint))
                {
                    this.ToggleIcon();
                    return;
                }

                if (this.IsClearIconVisible && this.secondaryIconRectF.Contains(touchPoint) && this.IsLayoutFocused)
                {
                    this.ClearText();
                    return;
                }

                // The control does not focus when touch the outside of the content region
                // so here we call focus mannually.
                if (this.Content != null && !this.Content.IsFocused)
                {
                    if ((IsOutlined && outlineRectF.Contains(touchPoint)) || ((IsFilled || IsNone) && backgroundRectF.Contains(touchPoint)))
                    {
                        if (this.isNonEditableContent && this.Content is SfDropdownEntry dropdownEntry)
                        {
                            dropdownEntry.IsDropDownOpen = true;
                            return;
                        }
                        await Task.Delay(1);
                        if (this.Content is InputView inputView)
                        {
                            inputView.Focus();
                        }
                        else if (this.Content is SfDropdownEntry dropdownentry && dropdownentry.InputView != null)
                        {
                            dropdownentry.InputView.Focus();
                        }
                    }
                }
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// This method set focus to the control.
        /// </summary>
        public new void Focus()
        {
            if (this.Content is InputView inputView)
            {
                inputView.Focus();
            }

            if (this.Content is SfDropdownEntry dropDownEntry)
            {
                dropDownEntry.Focus();
            }
        }

        /// <summary>
        /// This method set unfocus to the control.
        /// </summary>
        public new void Unfocus()
        {
            if (this.Content is InputView inputView)
            {
                inputView.Unfocus();
            }

            if (this.Content is SfDropdownEntry dropDownEntry)
            {
                dropDownEntry.Unfocus();
            }
        }

        /// <summary>
        /// To get desired left padding.
        /// </summary>
        /// <returns>double value</returns>
        internal double GetLeftPadding()
        {
            return InputViewPadding.Left < 0 ? this.LeftPadding : InputViewPadding.Left;
        }

        /// <summary>
        /// To get desired top padding.
        /// </summary>
        /// <returns>double value</returns>
        internal double GetTopPadding()
        {
            double topPadding = this.TopPadding;

            // In widows entry has default padding value 6 so we reduce the value.
            if (DeviceInfo.Platform == DevicePlatform.WinUI && !(Content is Editor))
            {
                topPadding -= 6;
            }
            return InputViewPadding.Top < 0 ? topPadding : InputViewPadding.Top + GetDefaultTopPadding();
        }

        /// <summary>
        /// To get desired right padding.
        /// </summary>
        /// <returns>double value</returns>
        internal double GetRightPadding()
        {
            return InputViewPadding.Right < 0 ? (this.isNonEditableContent) ? this.BaseLineMaxHeight : RightPadding : InputViewPadding.Right;
        }

        /// <summary>
        /// To get desired bottom padding.
        /// </summary>
        /// <returns>double value</returns>
        internal double GetBottomPadding()
        {
            double bottomPadding = this.BottomPadding;

            // In widows entry has default padding value 8 so we reduce the value.
            if (DeviceInfo.Platform == DevicePlatform.WinUI && !(Content is Editor))
            {
                bottomPadding -= 10;
            }
            return InputViewPadding.Bottom < 0 ? bottomPadding : InputViewPadding.Bottom + GetDefaultBottomPadding();
        }

        private double GetDefaultTopPadding()
        {
            return (this.IsFilled ? DefaultAssistiveTextHeight : DefaultAssistiveTextHeight / 2);
        }

        private double GetDefaultBottomPadding()
        {
            return (this.ReserveSpaceForAssistiveLabels ? DefaultAssistiveLabelPadding + TotalAssistiveTextHeight() :  0);
        }

        private double TotalAssistiveTextHeight()
        {
            return (this.ErrorTextSize.Height > this.HelperTextSize.Height ? this.ErrorTextSize.Height : this.HelperTextSize.Height);
        }

        /// <summary>
        /// Gets the totol number of lines required for given helper and error text.
        /// </summary>
        /// <param name="totalWidth"></param>
        /// <returns>Number of Lines</returns>
        private int GetNumberOfLines(double totalWidth)
        {
            int number = 1;

            number += (int)(totalWidth / ((this.GetAssistiveTextWidth() == 1) ? totalWidth : this.GetAssistiveTextWidth()));

            return number;

        }

        private void UpdateIconVisibility()
        {
            if (this.Content is InputView)
            {
                this.ShowClearButton = false;
                this.ShowDropDownButton = false;
            }
        }

        #endregion

        #region Override Methods

        private void UpdateContentMargin(View view)
        {
            if (view != null)
            {
                this.Content.Margin = new Thickness()
                {
                    Top = this.GetTopPadding(),
                    Bottom = this.GetBottomPadding(),
                    Left = this.GetLeftPadding(),
                    Right = this.GetRightPadding()
                };
            }
        }

        /// <inheritdoc/>
        protected override void OnContentChanged(object oldValue, object newValue)
        {

            if (newValue != null && newValue is InputView || (newValue != null && newValue is SfDropdownEntry dropdownControl && dropdownControl.InputView is InputView))
            {

                var inputView = newValue as InputView;
                if (newValue is SfDropdownEntry dropdownEntry)
                {
                    this.dropdownEntry = dropdownEntry;
                    this.isNonEditableContent = !dropdownEntry.IsEditableMode;
                    inputView = this.dropdownEntry.InputView;
                }
                if (inputView != null)
                {
                    if (!this.isNonEditableContent)
                    {
                        inputView.Focused += this.InputViewFocused;
                        inputView.Unfocused += this.InputViewUnfocused;
                    }

                    inputView.BackgroundColor = Colors.Transparent;
                    inputView.TextChanged += this.InputViewTextChanged;
                    inputView.HandlerChanged += this.InputView_HandlerChanged;
                    if (!string.IsNullOrEmpty(inputView.Text))
                    {
                        IsHintFloated = true;
                        isHintDownToUp = false;
                        this.Text = inputView.Text;
                    }
                }
                if (newValue is Entry entry && this.EnablePasswordVisibilityToggle)
                {
                    entry.IsPassword = true;
                    this.isPasswordTextVisible = false;
                }

                if (this.dropdownEntry != null)
                {
                    this.dropdownEntry.BackgroundColor = Colors.Transparent;
                    this.ClearButtonColor = this.dropdownEntry.ClearButtonIconColor;
                    this.ShowClearButton = this.dropdownEntry.IsClearButtonVisible;
                    if (this.dropdownEntry.DropDownArrowColor != null)
                    {
                        this.DropDownButtonColor = this.dropdownEntry.DropDownArrowColor;
                    }
                    if (this.dropdownEntry.DropDownView != null)
                    {
                        this.dropdownEntry.DropDownView.AnchorView = this;
                        this.dropdownEntry.DropDownView.PopupOpened += DropDownView_PopupOpened;
                        this.dropdownEntry.DropDownView.PopupClosed += DropDownView_PopupClosed;
                    }
                }
            }
            else if (newValue != null)
            {
                this.IsHintAlwaysFloated = true;
            }

            if (newValue is View view)
            {
                this.UpdateContentMargin(view);
            }

            //For placeholder overlap issue here handled the opacity value for controls.
            //If opacity was less than 0.01, ios no longer receives touch events.
            if (newValue is InputView entryEditorContent)
            {
                entryEditorContent.Opacity = this.IsHintFloated ? 1 : (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst || DeviceInfo.Platform == DevicePlatform.macOS) ? 0.02 : 0;
            }

            if (newValue is SfDropdownEntry dropdownContent && dropdownContent.InputView != null)
            {
                //Due to NonEditable comboBox has only label so here we alter the opacity to entire dropdown content in Android platform.
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    dropdownContent.Opacity = this.IsHintFloated ? 1 : 0;
                }
                else
                {
                    dropdownContent.InputView.Opacity = this.IsHintFloated ? 1 : (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst || DeviceInfo.Platform == DevicePlatform.macOS) ? 0.02 : 0;
                }
            }

            this.UpdateIconVisibility();
            this.UpdateViewBounds();
            base.OnContentChanged(oldValue, newValue!);

            if (!this.IsEnabled)
            {
                this.OnEnabledPropertyChanged(this.IsEnabled);
            }

        }

        /// <inheritdoc/>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            base.MeasureContent(widthConstraint, heightConstraint);
            var availableWidth = widthConstraint;
            var availableHeight = heightConstraint;

            var measuredWidth = 0d;
            var measuredHeight = 0d;

            var measure = new Size(0, 0);

            if (this.Content != null)
            {
                measure = this.Content.Measure(widthConstraint, heightConstraint, MeasureFlags.IncludeMargins);
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

        /// <inheritdoc/>
        protected override Size ArrangeContent(Rect bounds)
        {
            if (oldBounds == null)
            {
                oldBounds = bounds;
                this.UpdateViewBounds();
            }
            else if (oldBounds?.Width != bounds.Width ||
                  oldBounds?.Height != bounds.Height ||
                  oldBounds?.X != bounds.X ||
                  oldBounds?.Y != bounds.Y)
            {
                this.UpdateViewBounds();
            }

            return base.ArrangeContent(bounds);
        }

        /// <inheritdoc/>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);
            canvas.SaveState();
            canvas.Antialias = true;
            this.UpdateIconRectF();
            this.DrawBorder(canvas, dirtyRect);
            this.DrawHintText(canvas, dirtyRect);
            this.DrawAssistiveText(canvas, dirtyRect);
            this.DrawClearIcon(canvas, dirtyRect);
            this.DrawPasswordToggleIcon(canvas, dirtyRect);
            this.DrawDropDownIcon(canvas, dirtyRect);
            this.UpdatePopupPositions();
            this.effectsenderer.DrawEffects(canvas);
            this.UpdateContentMargin(this.Content);
            this.UpdateContentPosition();
            canvas.ResetState();
        }

        private void UpdatePopupPositions()
        {
            if (this.dropdownEntry?.DropDownView != null)
            {
                this.dropdownEntry.DropDownView.PopupX = GetPopupX();
                this.dropdownEntry.DropDownView.PopupY = GetPopupY();
                this.dropdownEntry.DropDownView.PopupWidth = GetPopupWidth();
            }
        }

        /// <inheritdoc/>
        protected override void ChangeVisualState()
        {
            base.ChangeVisualState();
            string stateName = this.HasError ? "Error" : this.IsLayoutFocused ? "Focused" : "Normal";
            VisualStateManager.GoToState(this, stateName);
            if (!this.HasError)
            {
                this.CurrentActiveColor = this.IsEnabled ? this.Stroke : this.DisabledColor;
            }
        }

        /// <inheritdoc/>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            this.UnhookEvents();

            if (this.Handler != null)
            {
                this.HookEvents();
            }
        }

        #endregion

        #region OnPropertyChanged Methods

        /// <summary>
        /// Invoked whenever the <see cref="LeadingViewProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnLeadingViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SfTextInputLayout)?.AddView(oldValue, newValue);
            (bindable as SfTextInputLayout)?.InvalidateDrawable();
        }

        /// <summary>
        /// Invoked whenever the <see cref="TrailingViewProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnTrailingViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SfTextInputLayout)?.AddView(oldValue, newValue);
            (bindable as SfTextInputLayout)?.InvalidateDrawable();
        }

        /// <summary>
        /// Invoked whenever the <see cref="IsHintAlwaysFloatedProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnHintAlwaysFloatedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfTextInputLayout inputLayout && newValue is bool value)
            {
                if (!value && !string.IsNullOrEmpty(inputLayout.Text))
                {
                    inputLayout.IsHintFloated = true;
                    inputLayout.isHintDownToUp = !inputLayout.IsHintFloated;
                    inputLayout.InvalidateDrawable();
                    return;
                }

                inputLayout.IsHintFloated = value;
                inputLayout.isHintDownToUp = !inputLayout.IsHintFloated;
                inputLayout.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="ShowLeadingViewProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnShowLeadingViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue is bool value)
            {
                (bindable as SfTextInputLayout)?.UpdateLeadingViewVisibility(value);
                (bindable as SfTextInputLayout)?.UpdateViewBounds();
            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="ShowTrailingViewProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnShowTrailingViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue is bool value)
            {
                (bindable as SfTextInputLayout)?.UpdateTrailingViewVisibility(value);
                (bindable as SfTextInputLayout)?.UpdateViewBounds();
            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="IsLayoutFocusedProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnIsLayoutFocusedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SfTextInputLayout)?.ChangeVisualState();
            (bindable as SfTextInputLayout)?.StartAnimation();

        }

        /// <summary>
        /// Invoked whenever the <see cref="HasErrorProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnHasErrorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SfTextInputLayout)?.ChangeVisualState();
            (bindable as SfTextInputLayout)?.InvalidateDrawable();
        }

        /// <summary>
        /// Invoked whenever the <see cref="CharMaxLengthProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnCharMaxLengthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfTextInputLayout inputLayout && newValue is int value)
            {
                inputLayout.counterText = $"0/{value}";
                inputLayout.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="EnablePasswordVisibilityToggleProperty"/> is set.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnEnablePasswordTogglePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfTextInputLayout inputLayout && inputLayout.Content is Entry entry && newValue is bool value)
            {
                entry.IsPassword = value;
                inputLayout.isPasswordTextVisible = false;
                inputLayout.UpdateViewBounds();
            }
        }

        private static void OnIsHintFloatedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfTextInputLayout inputLayout)
            {
                //// If opacity was less than 0.01, ios and maccatalyst no longer receives touch events.
                if (newValue is bool value && inputLayout.Content is InputView)
                {
                    inputLayout.Content.Opacity = value ? 1 : (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst || DeviceInfo.Platform == DevicePlatform.macOS) ? 0.02 : 0;
                }
                if (newValue is bool value1 && inputLayout.Content is SfDropdownEntry dropdownContent && dropdownContent.InputView != null)
                {
                    //Due to NonEditable comboBox has only label so here we alter the opacity to entire dropdown content in Android platform.
                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        dropdownContent.Opacity = value1 ? 1 : 0;
                    }
                    else
                    {
                        dropdownContent.InputView.Opacity = value1 ? 1 : (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.MacCatalyst || DeviceInfo.Platform == DevicePlatform.macOS) ? 0.02 : 0;
                    }
                }
            }
        }

        private static void OnInputViewPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SfTextInputLayout)?.UpdateContentMargin((bindable as SfTextInputLayout)?.Content!);
            (bindable as SfTextInputLayout)?.UpdateViewBounds();
        }

        /// <summary>
        /// Raised when the <see cref="IsEnabled"/> property was changed.
        /// </summary>
        /// <param name="bindable">object</param>
        /// <param name="oldValue">object</param>
        /// <param name="newValue">object</param>
        private static void OnEnabledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SfTextInputLayout)?.OnEnabledPropertyChanged((bool)newValue);
        }

        private static void OnHintLableStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfTextInputLayout inputLayout)
            {
                if (oldValue is LabelStyle oldLabelStyle)
                {
                    oldLabelStyle.PropertyChanged -= inputLayout.HintLabelStyle_PropertyChanged;
                }
                if (newValue is LabelStyle newLabelStyle)
                {
                    inputLayout.internalHintLabelStyle.TextColor = newLabelStyle.TextColor;
                    inputLayout.internalHintLabelStyle.FontFamily = newLabelStyle.FontFamily;
                    inputLayout.internalHintLabelStyle.FontAttributes = newLabelStyle.FontAttributes;
                    inputLayout.HintFontSize = (float)(newLabelStyle.FontSize < 12d ? FloatedHintFontSize : newLabelStyle.FontSize);
                    newLabelStyle.PropertyChanged += inputLayout.HintLabelStyle_PropertyChanged;
                    inputLayout.UpdateViewBounds();
                }
            }
        }

        private static void OnHelperLableStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfTextInputLayout inputLayout)
            {
                if (oldValue is LabelStyle oldLabelStyle)
                {
                    oldLabelStyle.PropertyChanged -= inputLayout.HelperLabelStyle_PropertyChanged;
                }
                if (newValue is LabelStyle newLabelStyle)
                {
                    inputLayout.internalHelperLabelStyle.TextColor = newLabelStyle.TextColor;
                    inputLayout.internalHelperLabelStyle.FontFamily = newLabelStyle.FontFamily;
                    inputLayout.internalHelperLabelStyle.FontAttributes = newLabelStyle.FontAttributes;
                    inputLayout.internalHelperLabelStyle.FontSize = newLabelStyle.FontSize;
                    newLabelStyle.PropertyChanged += inputLayout.HelperLabelStyle_PropertyChanged;
                    inputLayout.UpdateViewBounds();
                }
            }
        }

        private static void OnErrorLableStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfTextInputLayout inputLayout)
            {
                if (oldValue is LabelStyle oldLabelStyle)
                {
                    oldLabelStyle.PropertyChanged -= inputLayout.ErrorLabelStyle_PropertyChanged;
                }
                if (newValue is LabelStyle newLabelStyle)
                {
                    inputLayout.internalErrorLabelStyle.TextColor = newLabelStyle.TextColor;
                    inputLayout.internalErrorLabelStyle.FontFamily = newLabelStyle.FontFamily;
                    inputLayout.internalErrorLabelStyle.FontAttributes = newLabelStyle.FontAttributes;
                    inputLayout.internalErrorLabelStyle.FontSize = newLabelStyle.FontSize;
                    newLabelStyle.PropertyChanged += inputLayout.ErrorLabelStyle_PropertyChanged;
                    inputLayout.UpdateViewBounds();
                }
            }
        }

        private static void OnCounterLableStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfTextInputLayout inputLayout)
            {
                if (oldValue is LabelStyle oldLabelStyle)
                {
                    oldLabelStyle.PropertyChanged -= inputLayout.CounterLabelStyle_PropertyChanged;
                }
                if (newValue is LabelStyle newLabelStyle)
                {
                    inputLayout.internalCounterLabelStyle.TextColor = newLabelStyle.TextColor;
                    inputLayout.internalCounterLabelStyle.FontFamily = newLabelStyle.FontFamily;
                    inputLayout.internalCounterLabelStyle.FontAttributes = newLabelStyle.FontAttributes;
                    inputLayout.internalCounterLabelStyle.FontSize = newLabelStyle.FontSize;
                    newLabelStyle.PropertyChanged += inputLayout.CounterLabelStyle_PropertyChanged;
                    inputLayout.UpdateViewBounds();
                }
            }
        }

        private static void OnReserveSpacePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfTextInputLayout inputLayout && inputLayout.Content != null)
            {
                inputLayout.UpdateContentMargin(inputLayout.Content);             
            }
        }

        private static void OnContainerTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfTextInputLayout inputLayout && inputLayout.Content != null)
            {
                inputLayout.UpdateContentMargin(inputLayout.Content);
                inputLayout.UpdateViewBounds();
            }
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SfTextInputLayout)?.UpdateViewBounds();
        }
        #endregion

        #region Private Methods

        private void SfTextInputLayout_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FlowDirection) || e.PropertyName == nameof(Height))
            {
                this.UpdateViewBounds();
            }
        }

        private void HookLabelStylePropertyChanged()
        {
            if (this.HelperLabelStyle != null)
            {
                this.HelperLabelStyle.PropertyChanged += HelperLabelStyle_PropertyChanged;
            }
            if (this.ErrorLabelStyle != null)
            {
                this.ErrorLabelStyle.PropertyChanged += ErrorLabelStyle_PropertyChanged;
            }
            if (this.HintLabelStyle != null)
            {
                this.HintLabelStyle.PropertyChanged += HintLabelStyle_PropertyChanged;
            }
            if (this.CounterLabelStyle != null)
            {
                this.CounterLabelStyle.PropertyChanged += CounterLabelStyle_PropertyChanged;
            }
        }

        private void UnHookLabelStylePropertyChanged()
        {
            if (this.HelperLabelStyle != null)
            {
                this.HelperLabelStyle.PropertyChanged -= HelperLabelStyle_PropertyChanged;
            }
            if (this.ErrorLabelStyle != null)
            {
                this.ErrorLabelStyle.PropertyChanged -= ErrorLabelStyle_PropertyChanged;
            }
            if (this.HintLabelStyle != null)
            {
                this.HintLabelStyle.PropertyChanged -= HintLabelStyle_PropertyChanged;
            }
            if (this.CounterLabelStyle != null)
            {
                this.CounterLabelStyle.PropertyChanged -= CounterLabelStyle_PropertyChanged;
            }
        }

        private void HintLabelStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is LabelStyle labelStyle && !string.IsNullOrEmpty(e.PropertyName))
            {
                this.HintFontSize = (float)(labelStyle.FontSize < 12d ? FloatedHintFontSize : labelStyle.FontSize);
                MatchLabelStyleProperty(internalHintLabelStyle, labelStyle, e.PropertyName);
                this.InvalidateDrawable();
            }
        }

        private void HelperLabelStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is LabelStyle labelStyle && !string.IsNullOrEmpty(e.PropertyName))
            {
                MatchLabelStyleProperty(internalHelperLabelStyle, labelStyle, e.PropertyName);
                this.InvalidateDrawable();
            }
        }

        private void ErrorLabelStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is LabelStyle labelStyle && !string.IsNullOrEmpty(e.PropertyName))
            {
                MatchLabelStyleProperty(internalErrorLabelStyle, labelStyle, e.PropertyName);
                this.InvalidateDrawable();
            }
        }

        private void CounterLabelStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is LabelStyle labelStyle && !string.IsNullOrEmpty(e.PropertyName))
            {
                MatchLabelStyleProperty(internalCounterLabelStyle, labelStyle, e.PropertyName);
                this.InvalidateDrawable();
            }
        }

        private void MatchLabelStyleProperty(LabelStyle internalLabelStyle, LabelStyle labelStyle, string propertyName)
        {
            if (propertyName == "FontAttributes")
                internalLabelStyle.FontAttributes = labelStyle.FontAttributes;
            if (propertyName == "TextColor")
                internalLabelStyle.TextColor = labelStyle.TextColor;
            if (propertyName == "FontSize")
                internalLabelStyle.FontSize = labelStyle.FontSize;
            if (propertyName == "FontFamily")
                internalLabelStyle.FontFamily = labelStyle.FontFamily;
        }

        private void SetRendererBasedOnPlatform()
        {
#if __ANDROID__
            this.tilRenderer = new MaterialDropdownEntryRenderer();
#elif WINDOWS
            this.tilRenderer = new FluentDropdownEntryRenderer();
#else
            this.tilRenderer = new CupertinoDropdownEntryRenderer();
#endif
        }

        /// <summary>
        /// Raised when the <see cref="IsEnabled"/> property was changed.
        /// </summary>
        /// <param name="isEnabled">Boolean</param>
        private void OnEnabledPropertyChanged(bool isEnabled)
        {
            base.IsEnabled = isEnabled;

            if (this.Content != null)
            {
                this.Content.IsEnabled = isEnabled;
            }
            this.InvalidateDrawable();
        }

        /// <summary>
        /// This method update the InputView, TrailView and LeadView position.
        /// </summary>
        private void UpdateViewBounds()
        {
            this.UpdateLeadingViewPosition();
            this.UpdateTrailingViewPosition();
            this.UpdateContentPosition();
            this.InvalidateDrawable();
        }

        private void DropDownView_PopupOpened(object? sender, EventArgs e)
        {
            // Issue: In Windows popup window width is not updated with the AnchorView width.
            // Due to SfDropDownEntry class set the widthrequest value in UpdateElementBounds method.
            // Here workaround for above issue.
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                dropdownEntry?.DropdownContent?.SetValue(WidthRequestProperty, this.GetPopupWidth());
            }

            // In NonEditable ComboBox is automatically focused when popup is open,
            // so we set the focus manually. 
            if (isNonEditableContent)
            {
                IsLayoutFocused = true;
            }
        }

        private void DropDownView_PopupClosed(object? sender, EventArgs e)
        {
            // In NonEditable ComboBox is automatically focused when popup is open,
            // so we set the unfocus manually. 
            if (isNonEditableContent)
            {
                IsLayoutFocused = false;
            }
        }

        private int GetPopupX()
        {
            int point = this.IsOutlined ? (int)this.outlineRectF.X : (int)this.startPoint.X;
            if (this.isRTL)
            {
                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    point = (int)(this.Width - point - GetPopupWidth());
                }
                else
                {
                    point = -point;
                }
            }
            return point;
        }

        private int GetPopupY()
        {
            return this.ReserveSpaceForAssistiveLabels ? (int)-((this.ErrorTextSize.Height > this.HelperTextSize.Height ? this.ErrorTextSize.Height : this.HelperTextSize.Height) + DefaultAssistiveLabelPadding - this.BaseLineMaxHeight) : 0;
        }

        private double GetPopupWidth()
        {
            int width = 0;
            if (this.IsOutlined)
                width = (int)this.outlineRectF.Width;
            else
                width = (int)(this.endPoint.X - this.startPoint.X);
            return width;
        }

        /// <summary>
        /// This method return size based on ReserveSpaceForAssistiveLabels boolean property.
        /// </summary>
        /// <param name="size"></param>
        /// <returns>Size</returns>
        private SizeF GetLabelSize(SizeF size)
        {
            if (this.ReserveSpaceForAssistiveLabels)
            {
                return size;
            }
            else
            {
                size.Height = 0;
                size.Width = 0;
                return size;
            }
        }

        /// <summary>
        /// This method raised when input view OnHandlerChanged event has been occur.
        /// </summary>
        /// <param name="sender">Entry.</param>
        /// <param name="e">EventArgs.</param>
        private void InputView_HandlerChanged(object? sender, EventArgs e)
        {

#if ANDROID
           if((sender as InputView)?.Handler != null && (sender as InputView)?.Handler?.PlatformView  is AndroidX.AppCompat.Widget.AppCompatEditText androidEntry)
           {
               androidEntry.SetBackgroundColor(Android.Graphics.Color.Transparent); 
               androidEntry.SetPadding(0,0,0,0);
           }
#endif
#if WINDOWS
            if ((sender as InputView)?.Handler != null && (sender as InputView)?.Handler?.PlatformView is Microsoft.UI.Xaml.Controls.TextBox windowEntry)
            {
                windowEntry.BorderThickness = new Microsoft.UI.Xaml.Thickness(0); 
                windowEntry.Padding = new Microsoft.UI.Xaml.Thickness(0);
                windowEntry.Resources["TextControlBorderThemeThicknessFocused"] = windowEntry.BorderThickness;
            }    
#endif
#if IOS || MACCATALYST
            if ((sender as Entry)?.Handler != null && (sender as Entry)?.Handler?.PlatformView is UIKit.UITextField iOSEntry)
            {
                iOSEntry.BackgroundColor = UIKit.UIColor.Clear;
                iOSEntry.BorderStyle = UIKit.UITextBorderStyle.None;
                iOSEntry.Layer.BorderWidth = 0f;
                iOSEntry.Layer.BorderColor = UIKit.UIColor.Clear.CGColor;
                iOSEntry.LeftViewMode = UIKit.UITextFieldViewMode.Never;
            }

            if ((sender as Editor)?.Handler != null && (sender as Editor)?.Handler?.PlatformView is UIKit.UITextView iOSEditor)
            {
                iOSEditor.BackgroundColor = UIKit.UIColor.Clear;
                iOSEditor.Layer.BorderWidth = 0f;
                iOSEditor.Layer.BorderColor = UIKit.UIColor.Clear.CGColor;
                iOSEditor.TextContainer.LineFragmentPadding = 0f;
            }
#endif
        }

        /// <summary>
        /// This method raised when input view of this control has been focused.
        /// </summary>
        /// <param name="sender">Input View.</param>
        /// <param name="e">FocusEventArgs.</param>
        private void InputViewFocused(object? sender, FocusEventArgs e)
        {
            this.IsLayoutFocused = true;
        }

        /// <summary>
        /// This method raised when input view of this control has been unfocused.
        /// </summary>
        /// <param name="sender">Input Layout Base.</param>
        /// <param name="e">Focus Event args. </param>
        private void InputViewUnfocused(object? sender, FocusEventArgs e)
        {
            this.IsLayoutFocused = false;
        }

        /// <summary>
        /// This method raised when input view text has been changed.
        /// </summary>
        /// <param name="sender">InputView.</param>
        /// <param name="e">TextChangedEventArgs.</param>
        private void InputViewTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is InputView || sender is SfInputView)
            {
                this.Text = e.NewTextValue;

                if (string.IsNullOrEmpty(this.Text) && !IsLayoutFocused)
                {
                    if (!this.IsHintAlwaysFloated)
                    {
                        this.IsHintFloated = false;
                        this.isHintDownToUp = true;
                        this.InvalidateDrawable();
                    }
                }
                else if (!string.IsNullOrEmpty(this.Text) && !this.IsHintFloated)
                {
                    IsHintFloated = true;
                    isHintDownToUp = false;
                    this.InvalidateDrawable();
                }

                // Clear icon can't draw when isClearIconVisible property updated based on text.
                // So here call the InvalidateDrawable to draw the clear icon.
                if (this.Text?.Length == 1 || this.Text?.Length == 0)
                {
                    this.InvalidateDrawable();
                }

                //Call this method after bouncing issue has beed fixed and implement the ShowCharCount API.
                //this.UpdateCounterLabelText();
            }

            // In Windows platfrom editors autosize is not working so here we manually call the measure method.
            if (DeviceInfo.Platform == DevicePlatform.WinUI && this.Content is Editor)
            {
                this.InvalidateMeasure();
            }

        }

        /// <summary>
        /// This method used to clear the text in input view.
        /// </summary>
        private void ClearText()
        {
            if (this.Content is InputView inputView)
            {
                inputView.Text = string.Empty;
            }
            else if (this.Content is SfDropdownEntry dropDownEntry)
            {
                if (dropDownEntry.InputView != null)
                    dropDownEntry.InputView.Text = string.Empty;
            }
        }


        /// <summary>
        /// This method sets the default stroke value based on control states.
        /// </summary>
        private void AddDefaultVSM()
        {
            VisualStateGroupList visualStateGroupList = new VisualStateGroupList() { };

            VisualStateGroup visualStateGroup = new VisualStateGroup();

            visualStateGroup.Name = "CommonStates";

            VisualState focusedState = new VisualState() { Name = "Focused" };
            Setter focusedStrokeSetter = new Setter() { Property = StrokeProperty, Value = Color.FromArgb("#6750A4") };
            focusedState.Setters.Add(focusedStrokeSetter);

            VisualState errorState = new VisualState() { Name = "Error" };
            Setter errorStrokeSetter = new Setter() { Property = StrokeProperty, Value = Color.FromArgb("#B3261E") };
            errorState.Setters.Add(errorStrokeSetter);

            VisualState normalState = new VisualState() { Name = "Normal" };
            Setter normalStrokeSetter = new Setter() { Property = StrokeProperty, Value = Color.FromArgb("#79747E") };
            normalState.Setters.Add(normalStrokeSetter);

            visualStateGroup.States.Add(normalState);
            visualStateGroup.States.Add(focusedState);
            visualStateGroup.States.Add(errorState);
            visualStateGroupList.Add(visualStateGroup);

            if (!this.HasVisualStateGroups())
            {
                Setter mainSetter = new Setter() { Property = VisualStateManager.VisualStateGroupsProperty, Value = visualStateGroupList };

                var style = new Style(typeof(SfTextInputLayout));

                style.Setters.Add(mainSetter);

                Resources = new ResourceDictionary() { style };

                this.Style = style;
            }
        }

        /// <summary>
        /// This method unhook the all the hooked event of the entry.
        /// </summary>
        private void UnhookEvents()
        {
            if (this.Content != null && this.Content is InputView inputView)
            {
                inputView.Focused -= this.InputViewFocused;
                inputView.Unfocused -= this.InputViewUnfocused;
                inputView.TextChanged -= this.InputViewTextChanged;
                inputView.HandlerChanged -= this.InputView_HandlerChanged;
            }
            if (this.Content != null && this.Content is SfDropdownEntry dropdownEntry)
            {
                if (dropdownEntry.InputView is InputView inputview)
                {
                    if (!isNonEditableContent)
                    {
                        inputview.Focused -= this.InputViewFocused;
                        inputview.Unfocused -= this.InputViewUnfocused;
                    }
                    inputview.TextChanged -= this.InputViewTextChanged;
                    inputview.HandlerChanged -= this.InputView_HandlerChanged;
                }
                if (dropdownEntry.DropDownView is SfDropdownView dropdownView)
                {
                    dropdownView.PopupOpened -= this.DropDownView_PopupOpened;
                    dropdownView.PopupClosed -= this.DropDownView_PopupClosed;
                }
            }
            this.PropertyChanged -= SfTextInputLayout_PropertyChanged;
            this.UnHookLabelStylePropertyChanged();
        }

        private void HookEvents()
        {
            if (this.Content != null && this.Content is InputView inputView)
            {
                inputView.Focused += this.InputViewFocused;
                inputView.Unfocused += this.InputViewUnfocused;
                inputView.TextChanged += this.InputViewTextChanged;
                inputView.HandlerChanged += this.InputView_HandlerChanged;
            }
            if (this.Content != null && this.Content is SfDropdownEntry dropdownEntry)
            {
                if (dropdownEntry.InputView is InputView inputview)
                {
                    if (!isNonEditableContent)
                    {
                        inputview.Focused += this.InputViewFocused;
                        inputview.Unfocused += this.InputViewUnfocused;
                    }
                    inputview.TextChanged += this.InputViewTextChanged;
                    inputview.HandlerChanged += this.InputView_HandlerChanged;
                }
                if (dropdownEntry.DropDownView is SfDropdownView dropdownView)
                {
                    dropdownView.PopupOpened += this.DropDownView_PopupOpened;
                    dropdownView.PopupClosed += this.DropDownView_PopupClosed;
                }
            }
            this.PropertyChanged += SfTextInputLayout_PropertyChanged;
            this.HookLabelStylePropertyChanged();
        }

        /// <summary>
        /// This method changed the password toggle visibility icon and drop down window visibility into collapesed icon and viceversa.
        /// </summary>
        private void ToggleIcon()
        {
#if WINDOWS
            if(this.Content is SfDropdownEntry dropdownEntry)
            {
                if(!this.isNonEditableContent)
                {
                    if (this.IsLayoutFocused && !dropdownEntry.IsDropDownOpen)
                    {
                        dropdownEntry.IsDropDownOpen = false;
                    }
                    else
                    {
                        dropdownEntry.IsDropDownOpen = true;
                    }
                }
            }
#else
            if (this.Content is SfDropdownEntry dropdownEntry && !this.isNonEditableContent)
            {
                dropdownEntry.IsDropDownOpen = !dropdownEntry.IsDropDownOpen;
            }

#endif         
            if (this.Content is Entry entry)
            {
                this.isPasswordTextVisible = !this.isPasswordTextVisible;
                entry.IsPassword = !this.isPasswordTextVisible;
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// This method perform adding a new view and removing the old view from the control.
        /// </summary>
        /// <param name="oldValue">New View.</param>
        /// <param name="newValue">Old View.</param>
        private void AddView(object oldValue, object newValue)
        {
            var oldView = (View)oldValue;
            if (oldView != null && this.Contains(oldView))
            {
                this.Remove(oldView);
            }

            var newView = (View)newValue;
            if (newView != null)
            {
                this.Add(newView);
            }

            this.UpdateLeadingViewVisibility(this.ShowLeadingView);
            this.UpdateTrailingViewVisibility(this.ShowTrailingView);
        }

        /// <summary>
        /// This method update the Content RectF.
        /// </summary>
        private void UpdateContentPosition()
        {
            this.UpdateLeadViewWidthForContent();
            this.UpdateTrailViewWidthForContent();

            if (this.Content != null)
            {
                this.viewBounds.X = (int)this.leadViewWidth;
                this.viewBounds.Y = 0;
                this.viewBounds.Width = (int)(this.Width - this.leadViewWidth - this.trailViewWidth);
                this.viewBounds.Height = (int)this.Height;

                if (((!this.isNonEditableContent && this.ShowClearButton) || this.ShowDropDownButton || this.EnablePasswordVisibilityToggle) && !this.isNonEditableContent)
                {
                    this.viewBounds.Width -= (float)(((this.ShowDropDownButton || this.EnablePasswordVisibilityToggle) && (!this.isNonEditableContent && this.ShowClearButton)) ? ((IconSize * 2) - RightPadding + DefaultAssistiveLabelPadding) : (IconSize - RightPadding + DefaultAssistiveLabelPadding));
                }

                AbsoluteLayout.SetLayoutBounds(this.Content, this.viewBounds);
            }
        }

        /// <summary>
        /// This method update the LeadingView RectF.
        /// </summary>
        private void UpdateLeadingViewPosition()
        {
            if (this.ShowLeadingView && this.LeadingView != null)
            {
                this.viewBounds.X = (float)(this.leadingViewLeftPadding + ((this.IsOutlined && LeadingViewPosition == ViewPosition.Inside) ? this.BaseLineMaxHeight : 0));
                this.viewBounds.Y = (float)(this.IsOutlined ? this.BaseLineMaxHeight : 0);
                this.viewBounds.Width = (float)(this.LeadingView.WidthRequest == -1 ? (this.LeadingView.Width == -1 || this.LeadingView.Width == this.Width) ? this.defaultLeadAndTrailViewWidth : this.LeadingView.Width : this.LeadingView.WidthRequest);
                this.viewBounds.Height = (float)(this.Height - this.AssistiveLabelPadding - TotalAssistiveTextHeight());

                if (this.IsOutlined || this.IsFilled)
                {
                    this.LeadingView.VerticalOptions = LayoutOptions.Center;
                }

                if (this.IsNone)
                {
                    this.viewBounds.Height = (float)(this.viewBounds.Height - this.BaseLineMaxHeight - NoneBottomPadding);
                    this.LeadingView.VerticalOptions = LayoutOptions.End;
                }

                AbsoluteLayout.SetLayoutBounds(this.LeadingView, this.viewBounds);
            }
        }

        /// <summary>
        /// This method update the TrailingView RectF.
        /// </summary>
        private void UpdateTrailingViewPosition()
        {
            if (this.ShowTrailingView && this.TrailingView != null)
            {
                this.viewBounds.Width = (float)(this.TrailingView.WidthRequest == -1 ? (this.TrailingView.Width == -1 || this.TrailingView.Width == this.Width) ? this.defaultLeadAndTrailViewWidth : this.TrailingView.Width : this.TrailingView.WidthRequest);
                this.viewBounds.X = (float)(this.Width - this.viewBounds.Width - this.trailingViewRightPadding);
                this.viewBounds.Y = (float)(this.IsOutlined ? this.BaseLineMaxHeight : 0);
                this.viewBounds.Height = (float)(this.Height - this.AssistiveLabelPadding - TotalAssistiveTextHeight());

                if (this.IsOutlined || this.IsFilled)
                {
                    this.TrailingView.VerticalOptions = LayoutOptions.Center;
                }

                if (this.IsNone)
                {
                    this.viewBounds.Height = (float)(this.viewBounds.Height - this.BaseLineMaxHeight - NoneBottomPadding);
                    this.TrailingView.VerticalOptions = LayoutOptions.End;
                }

                AbsoluteLayout.SetLayoutBounds(this.TrailingView, this.viewBounds);
            }
        }

        /// <summary>
        /// This method changes the LeadingView visibility based on boolean parameter.
        /// </summary>
        /// <param name="showLeadingView">Boolean.</param>
        private void UpdateLeadingViewVisibility(bool showLeadingView)
        {
            if (this.LeadingView != null)
            {
                this.LeadingView.IsVisible = showLeadingView;
            }
        }

        /// <summary>
        /// This method changes the TrailingView visibility based on boolean parameter.
        /// </summary>
        /// <param name="showTrailingView">Boolean.</param>
        private void UpdateTrailingViewVisibility(bool showTrailingView)
        {
            if (this.TrailingView != null)
            {
                this.TrailingView.IsVisible = showTrailingView;
            }
        }

        /// <summary>
        /// This method updates the LeadingView width with their padding values.
        /// </summary>
        private void UpdateLeadViewWidthForContent()
        {
            this.leadViewWidth = 0;

            if (this.ShowLeadingView && this.LeadingView != null)
            {
                this.leadViewWidth = ((this.LeadingView.Width == -1 || this.LeadingView.Width == this.Width) ? this.defaultLeadAndTrailViewWidth : this.LeadingView.Width) + (this.LeadingViewPosition == ViewPosition.Outside ? this.leadingViewLeftPadding + this.leadingViewRightPadding : this.IsNone ? this.leadingViewLeftPadding + this.leadingViewRightPadding : this.leadingViewLeftPadding);
            }
        }

        /// <summary>
        /// This method updates the TrailingView width with their padding values.
        /// </summary>
        private void UpdateTrailViewWidthForContent()
        {
            this.trailViewWidth = 0;

            if (this.ShowTrailingView && this.TrailingView != null)
            {
                this.trailViewWidth = ((this.TrailingView.Width == -1 || this.TrailingView.Width == this.Width) ? this.defaultLeadAndTrailViewWidth : this.TrailingView.Width) + (TrailingViewPosition == ViewPosition.Outside ? this.trailingViewLeftPadding + this.trailingViewRightPadding : this.trailingViewLeftPadding);
            }
        }

        /// <summary>
        /// This method update the leading view width only in leading view position is outside.
        /// </summary>
        private void UpdateLeadViewWidthForBorder()
        {
            this.leadViewWidth = 0;
            if (this.ShowLeadingView && this.LeadingView != null && this.LeadingViewPosition == ViewPosition.Outside)
            {
                this.leadViewWidth = this.LeadingView.Width + this.leadingViewLeftPadding + this.leadingViewRightPadding;
            }
        }

        /// <summary>
        /// This method update the leading view width only in leading view position is outside.
        /// </summary>
        private void UpdateTrailViewWidthForBorder()
        {
            this.trailViewWidth = 0;
            if (this.ShowTrailingView && this.TrailingView != null && this.TrailingViewPosition == ViewPosition.Outside)
            {
                this.trailViewWidth = this.TrailingView.Width + this.trailingViewLeftPadding + this.trailingViewRightPadding;
            }
        }

        /// <summary>
        /// This method updates the effects tilRenderer rectF.
        /// </summary>
        private void UpdateEffectsRendererBounds()
        {
            if (this.effectsenderer != null)
            {
                this.effectsenderer.RippleBoundsCollection.Clear();
                this.effectsenderer.HighlightBoundsCollection.Clear();

                if (this.IsClearIconVisible && this.IsLayoutFocused)
                {
                    this.effectsenderer.RippleBoundsCollection.Add(this.secondaryIconRectF);
                    this.effectsenderer.HighlightBoundsCollection.Add(this.secondaryIconRectF);
                }

                if (this.IsPassowordToggleIconVisible || this.IsDropDownIconVisible)
                {
                    this.effectsenderer.RippleBoundsCollection.Add(this.primaryIconRectF);
                    this.effectsenderer.HighlightBoundsCollection.Add(this.primaryIconRectF);
                }
            }
        }

        /// <summary>
        /// This method update the counter text string value.
        /// </summary>
        private void UpdateCounterLabelText()
        {
            if (this.ShowCharCount)
            {
                var textLength = string.IsNullOrEmpty(this.Text) ? 0 : this.Text.Length;
                this.counterText = this.CharMaxLength == int.MaxValue ? $"{textLength}" : $"{textLength}/{this.CharMaxLength}";
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// This method updates the hint text position in none type container.
        /// </summary>
        private void UpdateNoneContainerHintPosition()
        {
            if (this.IsHintFloated)
            {
                this.hintRect.X = 0;
                this.hintRect.Y = (float)DefaultAssistiveLabelPadding;
                this.hintRect.Width = this.FloatedHintSize.Width;
                this.hintRect.Height = this.FloatedHintSize.Height;
            }
            else
            {
                this.hintRect.X = 0;
                this.hintRect.Y = (int)(this.Height - this.HintSize.Height - this.BaseLineMaxHeight - this.AssistiveLabelPadding - (this.ErrorTextSize.Height > this.HelperTextSize.Height ? this.ErrorTextSize.Height : this.HelperTextSize.Height));
                this.hintRect.Width = this.HintSize.Width;
                this.hintRect.Height = this.HintSize.Height;
            }
        }

        /// <summary>
        /// This method updates the hint text position in filled type container.
        /// </summary>
        private void UpdateFilledContainerHintPosition()
        {
            if (this.IsHintFloated)
            {
                this.hintRect.X = (float)((StartX) + DefaultAssistiveLabelPadding);
                this.hintRect.Y = (float)(DefaultAssistiveLabelPadding * 2);
                this.hintRect.Width = this.FloatedHintSize.Width;
                this.hintRect.Height = this.FloatedHintSize.Height;
            }
            else
            {
                this.hintRect.X = (float)(StartX + DefaultAssistiveLabelPadding);
                this.hintRect.Y = (float)((this.Height - TotalAssistiveTextHeight() - AssistiveLabelPadding) / 2) - (HintSize.Height / 2);
                this.hintRect.Width = this.HintSize.Width;
                this.hintRect.Height = this.HintSize.Height;
            }
        }

        /// <summary>
        /// This method updates the hint text position in outlined type container.
        /// </summary>
        private void UpdateOutlinedContainerHintPosition()
        {
            if (this.IsHintFloated)
            {
                this.hintRect.X = (int)(StartX);
                this.hintRect.Y = 0;
                this.hintRect.Width = (int)this.FloatedHintSize.Width;
                this.hintRect.Height = (int)this.FloatedHintSize.Height;
            }
            else
            {
                this.hintRect.X = (float)(StartX);
                this.hintRect.Y = outlineRectF.Center.Y - (this.HintSize.Height / 2);
                this.hintRect.Width = this.HintSize.Width;
                this.hintRect.Height = this.HintSize.Height;
            }
        }

        /// <summary>
        /// This method updates the starting point of the hint text need.
        /// </summary>
        private void UpdateHintPosition()
        {
            if (this.isAnimating)
            {
                if (IsOutlined)
                {
                    hintRect.X = (float)(StartX + DefaultAssistiveLabelPadding);
                    if (this.ShowLeadingView && this.LeadingView != null)
                    {
                        this.leadViewWidth = this.LeadingView.Width + (this.LeadingViewPosition == ViewPosition.Outside ? this.leadingViewLeftPadding + this.leadingViewRightPadding : this.IsNone ? this.leadingViewLeftPadding + this.leadingViewRightPadding : this.leadingViewLeftPadding);
                        this.hintRect.X += (float)this.leadViewWidth;
                    }
                    if (this.isRTL)
                    {
                        this.hintRect.X = (float)(this.Width - this.hintRect.X - this.hintRect.Width);
                    }
                }
                return;
            }

            if (this.IsNone)
            {
                this.UpdateNoneContainerHintPosition();
            }

            if (this.IsFilled)
            {
                this.UpdateFilledContainerHintPosition();
            }

            if (this.IsOutlined)
            {
                this.UpdateOutlinedContainerHintPosition();
            }

            if (this.ShowLeadingView && this.LeadingView != null)
            {
                this.leadViewWidth = this.LeadingView.Width + (this.LeadingViewPosition == ViewPosition.Outside ? this.leadingViewLeftPadding + this.leadingViewRightPadding : this.IsNone ? this.leadingViewLeftPadding + this.leadingViewRightPadding : this.leadingViewLeftPadding);
                this.hintRect.X += (float)this.leadViewWidth;
            }

            if (this.isRTL)
            {
                this.hintRect.X = (float)(this.Width - this.hintRect.X - this.hintRect.Width);
            }

        }

        /// <summary>
        /// This method updates the starting point of the assistive text.
        /// </summary>
        private void UpdateHelperTextPosition()
        {
            this.UpdateLeadViewWidthForContent();
            this.UpdateTrailViewWidthForBorder();
            var startPadding = this.IsNone ? 0 : StartX + DefaultAssistiveLabelPadding;
            this.helperTextRect.X = (int)(startPadding + this.leadViewWidth);
            this.helperTextRect.Y = (int)(this.Height - TotalAssistiveTextHeight() - BaseLineMaxHeight / 2);
            this.helperTextRect.Width = (int)(this.Width - startPadding - CounterTextPadding - DefaultAssistiveLabelPadding - ((this.ShowCharCount) ? CounterTextSize.Width + CounterTextPadding : 0) - this.trailViewWidth - this.leadViewWidth);
            this.helperTextRect.Height = this.HelperTextSize.Height;

            if (this.isRTL)
            {
                this.helperTextRect.X = (int)(this.Width - this.helperTextRect.X - this.helperTextRect.Width);
            }

        }

        private double GetAssistiveTextWidth()
        {
            this.UpdateLeadViewWidthForContent();
            this.UpdateTrailViewWidthForBorder();

            if (this.Width >= 0)
            {
                return this.Width - (this.IsNone ? 0 : StartX + DefaultAssistiveLabelPadding) - CounterTextPadding - DefaultAssistiveLabelPadding - ((this.ShowCharCount) ? CounterTextSize.Width + CounterTextPadding : 0) - this.trailViewWidth - this.leadViewWidth;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// This method updates the starting point of the assistive text.
        /// </summary>
        private void UpdateErrorTextPosition()
        {
            this.UpdateLeadViewWidthForContent();
            this.UpdateTrailViewWidthForBorder();
            var startPadding = this.IsNone ? 0 : StartX + DefaultAssistiveLabelPadding;
            this.errorTextRect.X = (int)(startPadding + this.leadViewWidth);
            this.errorTextRect.Y = (int)((this.Height) - TotalAssistiveTextHeight() - BaseLineMaxHeight / 2);
            this.errorTextRect.Width = (int)(this.Width - startPadding - CounterTextPadding - DefaultAssistiveLabelPadding - ((this.ShowCharCount) ? CounterTextSize.Width + CounterTextPadding : 0) - this.trailViewWidth - this.leadViewWidth);
            this.errorTextRect.Height = (int)ErrorTextSize.Height;

            if (this.isRTL)
            {
                this.errorTextRect.X = (int)(this.Width - this.errorTextRect.X - this.errorTextRect.Width);
            }

        }

        /// <summary>
        /// This method updates the starting point of the counter.
        /// </summary>
        private void UpdateCounterTextPosition()
        {
            this.UpdateTrailViewWidthForBorder();
            this.counterTextRect.X = (int)(this.Width - this.CounterTextSize.Width - CounterTextPadding - this.trailViewWidth);
            this.counterTextRect.Y = (int)(this.Height - CounterTextSize.Height);
            this.counterTextRect.Width = (int)CounterTextSize.Width;
            this.counterTextRect.Height = (int)CounterTextSize.Height;

            if (this.isRTL)
            {
                this.counterTextRect.X = (float)(this.Width - this.counterTextRect.X - this.counterTextRect.Width);
            }
        }

        /// <summary>
        /// This method updates the start point and end point of the base line in filled and none type container.
        /// </summary>
        private void UpdateBaseLinePoints()
        {
            this.UpdateLeadViewWidthForBorder();
            this.UpdateTrailViewWidthForBorder();
            this.startPoint.X = (float)this.leadViewWidth;
            this.startPoint.Y = (float)(this.Height - TotalAssistiveTextHeight() - AssistiveLabelPadding);
            this.endPoint.X = (float)(this.Width - this.trailViewWidth);
            this.endPoint.Y = (float)(this.Height - TotalAssistiveTextHeight() - AssistiveLabelPadding);
        }

        /// <summary>
        /// This method updates the rectF of the border line in outlined type container.
        /// </summary>
        private void UpdateOutlineRectF()
        {
            this.UpdateLeadViewWidthForBorder();
            this.UpdateTrailViewWidthForBorder();
            this.outlineRectF.X = (float)(this.BaseLineMaxHeight + this.leadViewWidth);
            this.outlineRectF.Y = (float)((this.BaseLineMaxHeight > this.FloatedHintSize.Height / 2) ? this.BaseLineMaxHeight : this.FloatedHintSize.Height / 2);
            this.outlineRectF.Width = (float)((this.Width - (this.BaseLineMaxHeight * 2)) - this.leadViewWidth - this.trailViewWidth);
            this.outlineRectF.Height = (float)(this.Height - this.outlineRectF.Y - TotalAssistiveTextHeight() - AssistiveLabelPadding);
        }


        /// <summary>
        /// This method updates the rectF of the background color in filled type container.
        /// </summary>
        private void UpdateBackgroundRectF()
        {
            this.UpdateLeadViewWidthForBorder();
            this.UpdateTrailViewWidthForBorder();
            this.backgroundRectF.X = (float)this.leadViewWidth;
            this.backgroundRectF.Y = 0;
            this.backgroundRectF.Width = (float)(this.Width - this.leadViewWidth - this.trailViewWidth);
            this.backgroundRectF.Height = (float)(this.Height - TotalAssistiveTextHeight() - this.AssistiveLabelPadding);
        }

        /// <summary>
        /// This method updates the rectF of the floated hint text space in outlined type container.
        /// </summary>
        private void CalculateClipRect()
        {
            if (!string.IsNullOrEmpty(this.Hint) && this.EnableFloating)
            {
                this.clipRect.X = (float)(this.outlineRectF.X + StartX - this.BaseLineMaxHeight);
                this.clipRect.Y = 0;
                this.clipRect.Width = (float)(this.FloatedHintSize.Width);
                this.clipRect.Height = this.FloatedHintSize.Height;
                if (this.ShowLeadingView && this.LeadingView != null && this.LeadingViewPosition == ViewPosition.Inside)
                {
                    this.clipRect.X = (float)(this.clipRect.X + this.LeadingView.Width + this.leadingViewLeftPadding);
                }
            }
            else
            {
                this.clipRect = new Rect(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// This method updates the primaryIconRectF and secondaryIconRectF.
        /// </summary>
        private void UpdateIconRectF()
        {
            this.UpdateOutlineRectF();
            this.UpdateBackgroundRectF();
            this.UpdateTrailViewWidthForContent();
            this.UpdatePrimaryIconRectF();
            this.UpdateSecondaryIconRectF();
            this.UpdateEffectsRendererBounds();
        }

        /// <summary>
        /// This method calculate the password toggle icon rectF position.
        /// </summary>
        private void UpdatePrimaryIconRectF()
        {
            this.primaryIconRectF.X = (float)(this.Width - this.trailViewWidth - (this.IsOutlined ? this.BaseLineMaxHeight * 2 : this.BaseLineMaxHeight) - IconSize);
            if (this.IsNone)
            {
                this.primaryIconRectF.Y = (float)((this.Content.Y + (this.Content.Height / 2)) - (IconSize / 2));
            }
            else if (this.IsOutlined)
            {
                this.primaryIconRectF.Y = (float)((this.outlineRectF.Center.Y) - (IconSize / 2));
            }
            else
            {
                this.primaryIconRectF.Y = (float)(((this.Height - this.AssistiveLabelPadding - TotalAssistiveTextHeight()) / 2) - (IconSize / 2));
            }

            this.primaryIconRectF.Width = (IsPassowordToggleIconVisible || IsDropDownIconVisible) ? IconSize : 0;
            this.primaryIconRectF.Height = (IsPassowordToggleIconVisible || IsDropDownIconVisible) ? IconSize : 0;

            if (this.isRTL)
            {
                this.primaryIconRectF.X = (float)(this.Width - this.primaryIconRectF.X - this.primaryIconRectF.Width);
            }

        }

        /// <summary>
        /// This method calculate the clear icon rectF position.
        /// </summary>
        private void UpdateSecondaryIconRectF()
        {
            this.secondaryIconRectF.X = (primaryIconRectF.Width != 0) ? this.primaryIconRectF.X - IconSize : this.primaryIconRectF.X;
            this.secondaryIconRectF.Y = this.primaryIconRectF.Y;
            this.secondaryIconRectF.Width = IconSize;
            this.secondaryIconRectF.Height = IconSize;

            // Need to check clear icon size with UI team and remove this code.
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                this.secondaryIconRectF.X += 2;
                this.secondaryIconRectF.Y += 2;
                this.secondaryIconRectF.Width = IconSize - 4;
                this.secondaryIconRectF.Height = IconSize - 4;
            }

            if (this.isRTL)
            {
                this.secondaryIconRectF.X -= (float)(this.secondaryIconRectF.Width);
            }

        }

        /// <summary>
        /// Updates the hint color of UI elements based on state.
        /// </summary>
        private void UpdateHintColor()
        {
            this.internalHintLabelStyle.TextColor = this.IsEnabled ? (IsLayoutFocused || this.HasError) ? this.Stroke : this.HintLabelStyle.TextColor : this.DisabledColor;
        }

        /// <summary>
        /// Updates the helper color of UI elements based on state.
        /// </summary>
        private void UpdateHelperTextColor()
        {
            this.internalHelperLabelStyle.TextColor = this.IsEnabled ? this.HelperLabelStyle.TextColor : this.DisabledColor;
        }

        /// <summary>
        /// Updates the error color of UI elements based on state.
        /// </summary>
        private void UpdateErrorTextColor()
        {
            this.internalErrorLabelStyle.TextColor = this.IsEnabled ? this.ErrorLabelStyle.TextColor.Equals(Color.FromRgba(0, 0, 0, 0.87)) ? this.Stroke : this.ErrorLabelStyle.TextColor : this.DisabledColor;
        }

        /// <summary>
        /// Updates the counter color of UI elements based on state.
        /// </summary>
        private void UpdateCounterTextColor()
        {
            this.internalCounterLabelStyle.TextColor = this.IsEnabled ? this.HasError ? this.ErrorLabelStyle.TextColor.Equals(Color.FromRgba(0, 0, 0, 0.87)) ? this.Stroke : this.ErrorLabelStyle.TextColor : this.CounterLabelStyle.TextColor : this.DisabledColor;
        }

        #endregion

        #region Drawing Methods
        private void DrawBorder(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();
            if (this.isRTL)
            {
                canvas.Translate((float)this.Width, 0);
                canvas.Scale(-1, 1);
            }
            canvas.StrokeSize = (float)(this.IsLayoutFocused ? this.FocusedStrokeThickness : this.UnfocusedStrokeThickness);
            canvas.StrokeColor = this.IsEnabled ? this.Stroke : this.DisabledColor;
            if (!this.IsOutlined)
            {
                this.UpdateBaseLinePoints();
                if (this.IsFilled)
                {
                    canvas.SaveState();
                    if (this.ContainerBackground is SolidColorBrush backgroundColor)
                        canvas.FillColor = backgroundColor.Color;
                    this.UpdateBackgroundRectF();
                    canvas.FillRoundedRectangle(this.backgroundRectF, this.OutlineCornerRadius, this.OutlineCornerRadius, 0, 0);
                    canvas.RestoreState();
                }

                canvas.DrawLine(this.startPoint.X, this.startPoint.Y, this.endPoint.X, this.endPoint.Y);
            }
            else
            {
                this.UpdateOutlineRectF();

                if (((this.IsLayoutFocused && !string.IsNullOrEmpty(this.Hint)) || this.IsHintFloated) && this.ShowHint)
                {
                    this.CalculateClipRect();
                    canvas.SubtractFromClip(this.clipRect);
                }

                if (this.ContainerBackground is SolidColorBrush backgroundColor)
                    canvas.FillColor = backgroundColor.Color;
                canvas.FillRoundedRectangle(this.outlineRectF, this.OutlineCornerRadius);
                canvas.DrawRoundedRectangle(this.outlineRectF, this.OutlineCornerRadius);
            }

            canvas.RestoreState();
        }

        private void DrawHintText(ICanvas canvas, RectF dirtyRect)
        {
            if ((IsHintFloated && !this.EnableFloating) || (!IsHintFloated && !string.IsNullOrEmpty(this.Text)))
            {
                return;
            }

            if (this.ShowHint && !string.IsNullOrEmpty(this.Hint) && this.HintLabelStyle != null)
            {
                canvas.SaveState();
                this.UpdateOutlineRectF();
                this.UpdateHintPosition();
                this.UpdateHintColor();

                this.internalHintLabelStyle.FontSize = this.isAnimating ? (float)this.animatingFontSize : this.IsHintFloated ? FloatedHintFontSize : this.HintLabelStyle.FontSize;

                HorizontalAlignment horizontalAlignment = (IsNone || isAnimating || IsFilled) ? isRTL ? HorizontalAlignment.Right : HorizontalAlignment.Left : HorizontalAlignment.Center;
                VerticalAlignment verticalAlignment = VerticalAlignment.Center;

                canvas.DrawText(this.Hint, hintRect, horizontalAlignment, verticalAlignment, internalHintLabelStyle);
                canvas.RestoreState();
            }
        }

        private void DrawAssistiveText(ICanvas canvas, RectF dirtyRect)
        {
            if (this.HasError)
            {
                this.DrawErrorText(canvas, dirtyRect);
            }
            else
            {
                this.DrawHelperText(canvas, dirtyRect);
            }

            this.DrawCounterText(canvas, dirtyRect);
        }

        private void DrawHelperText(ICanvas canvas, RectF dirtyRect)
        {
            if (this.ShowHelperText && !string.IsNullOrEmpty(this.HelperText) && this.HelperLabelStyle != null)
            {
                canvas.SaveState();
                this.UpdateHelperTextPosition();
                this.UpdateHelperTextColor();

                canvas.DrawText(this.HelperText, helperTextRect, this.isRTL ? HorizontalAlignment.Right : HorizontalAlignment.Left, VerticalAlignment.Top, internalHelperLabelStyle);

                canvas.RestoreState();
            }
        }

        private void DrawErrorText(ICanvas canvas, RectF dirtyRect)
        {
            if (this.ShowHelperText && !string.IsNullOrEmpty(this.ErrorText) && this.ErrorLabelStyle != null)
            {
                canvas.SaveState();
                this.UpdateErrorTextPosition();
                this.UpdateErrorTextColor();

                canvas.DrawText(this.ErrorText, errorTextRect, this.isRTL ? HorizontalAlignment.Right : HorizontalAlignment.Left, VerticalAlignment.Top, internalErrorLabelStyle);

                canvas.RestoreState();
            }
        }

        private void DrawCounterText(ICanvas canvas, RectF dirtyRect)
        {
            if (this.ShowCharCount && !string.IsNullOrEmpty(this.counterText) && this.CounterLabelStyle != null)
            {
                canvas.SaveState();
                this.UpdateCounterTextPosition();
                this.UpdateCounterTextColor();

                canvas.DrawText(this.counterText, counterTextRect, this.isRTL ? HorizontalAlignment.Right : HorizontalAlignment.Left, VerticalAlignment.Top, internalCounterLabelStyle);

                canvas.RestoreState();
            }
        }

        private void DrawClearIcon(ICanvas canvas, RectF dirtyRect)
        {
            if (this.IsClearIconVisible)
            {
                canvas.SaveState();
                canvas.StrokeColor = this.ClearButtonColor;
                canvas.StrokeSize = 1.5f;
                this.tilRenderer?.DrawClearButton(canvas, secondaryIconRectF);
                canvas.RestoreState();
            }
        }

        private void DrawPasswordToggleIcon(ICanvas canvas, RectF dirtyRect)
        {
            if (IsPassowordToggleIconVisible)
            {
                canvas.SaveState();

                canvas.FillColor = this.Stroke;
                canvas.Translate(this.primaryIconRectF.X + this.passwordToggleIconEdgePadding, this.primaryIconRectF.Y + (this.isPasswordTextVisible ? this.passwordToggleVisibleTopPadding : this.passwordToggleCollapsedTopPadding));

                // Using Path data value for toggle icon it seems smaller in Android platform when compared to Xamarin.Forms UI
                // so fix the above issue by scaling the canvas.
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    canvas.Scale(1.2f, 1.2f);
                }

                canvas.FillPath(this.ToggleIconPath);
                canvas.RestoreState();
            }
        }

        /// <summary>
        /// Draw dropdown button method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="rectF">The rect.</param>
        public void DrawDropDownIcon(ICanvas canvas, RectF rectF)
        {
            if (IsDropDownIconVisible)
            {
                canvas.SaveState();
                canvas.FillColor = this.DropDownButtonColor;
                canvas.StrokeColor = this.DropDownButtonColor;
                canvas.StrokeSize = 2;
                this.tilRenderer?.DrawDropDownButton(canvas, primaryIconRectF);
                canvas.RestoreState();
            }

        }
        #endregion

        #region Animation Methods

        private void UpdateSizeStartAndEndValue()
        {
            this.fontSizeStart = this.IsHintFloated ? FloatedHintFontSize : this.HintFontSize;
            this.fontSizeEnd = this.IsHintFloated ? this.HintFontSize : FloatedHintFontSize;
        }

        private void UpdateStartAndEndValue()
        {
            if (this.IsNone && (this.Width > 0 && this.Height > 0))
            {
                if (this.IsHintFloated)
                {
                    this.translateStart = DefaultAssistiveLabelPadding;
                    this.translateEnd = this.Height - this.HintSize.Height - this.BaseLineMaxHeight - this.AssistiveLabelPadding - TotalAssistiveTextHeight() - DefaultAssistiveLabelPadding;
                }
                else
                {
                    this.translateStart = this.Height - this.HintSize.Height - this.BaseLineMaxHeight - this.AssistiveLabelPadding - TotalAssistiveTextHeight() - DefaultAssistiveLabelPadding;
                    this.translateEnd = DefaultAssistiveLabelPadding;
                }
            }

            if (this.IsOutlined && (this.Width > 0 && this.Height > 0))
            {
                if (this.IsHintFloated)
                {
                    this.translateStart = 0;
                    this.translateEnd = outlineRectF.Center.Y - (this.HintSize.Height / 2);
                }
                else
                {
                    this.translateStart = outlineRectF.Center.Y - (this.HintSize.Height / 2);
                    this.translateEnd = 0;
                }
            }

            if (this.IsFilled && (this.Width > 0 && this.Height > 0))
            {
                if (this.IsHintFloated)
                {
                    this.translateStart = DefaultAssistiveLabelPadding * 2;
                    this.translateEnd = ((this.Height - TotalAssistiveTextHeight() - this.AssistiveLabelPadding) / 2) - (HintSize.Height / 2);
                }
                else
                {
                    this.translateStart = ((this.Height - TotalAssistiveTextHeight() - this.AssistiveLabelPadding) / 2) - (HintSize.Height / 2);
                    this.translateEnd = DefaultAssistiveLabelPadding * 2;
                }
            }
        }

        private void StartAnimation()
        {
            if (this.IsHintFloated && this.IsLayoutFocused)
            {
                this.InvalidateDrawable();
                return;
            }

            if (string.IsNullOrEmpty(this.Text) && !this.IsLayoutFocused && !EnableFloating)
            {
                this.IsHintFloated = false;
                this.isHintDownToUp = true;
                this.InvalidateDrawable();
                return;
            }

            if (!string.IsNullOrEmpty(this.Text) || this.IsHintAlwaysFloated || !this.EnableHintAnimation)
            {
                this.IsHintFloated = true;
                this.isHintDownToUp = false;
                if (!this.EnableHintAnimation && !IsLayoutFocused && string.IsNullOrEmpty(this.Text))
                {
                    this.IsHintFloated = false;
                    this.isHintDownToUp = true;
                }
                this.InvalidateDrawable();
                return;
            }

            this.animatingFontSize = this.IsHintFloated ? FloatedHintFontSize : HintFontSize;
            this.UpdateStartAndEndValue();
            this.UpdateSizeStartAndEndValue();
            this.IsHintFloated = !this.IsHintFloated;

            // Text is disapper when the rect size is not compatible with text size, so here calculate the rect again.
            if (!this.IsHintFloated)
            {
                this.UpdateHintPosition();
            }

            this.fontsize = (float)this.fontSizeStart;
            var scalingAnimation = new Animation(this.OnScalingAnimationUpdate, this.fontSizeStart, this.fontSizeEnd, Easing.Linear);

            var translateAnimation = new Animation(this.OnTranslateAnimationUpdate, this.translateStart, this.translateEnd, Easing.SinIn);

            translateAnimation.WithConcurrent(scalingAnimation, 0, 1);

            translateAnimation.Commit(this, "showAnimator", rate: 7, length: (uint)DefaultAnimationDuration, finished: this.OnTranslateAnimationEnded, repeat: () => false);

        }

        private void OnScalingAnimationUpdate(double value)
        {
            this.fontsize = (float)value;
        }

        private void OnTranslateAnimationUpdate(double value)
        {
            this.isAnimating = true;
            this.hintRect.Y = (float)value;
            this.animatingFontSize = (float)this.fontsize;
            this.InvalidateDrawable();
        }

        private void OnTranslateAnimationEnded(double value, bool isCompleted)
        {
            this.isAnimating = false;
            this.isHintDownToUp = !this.isHintDownToUp;
        }
        #endregion
    }
}

