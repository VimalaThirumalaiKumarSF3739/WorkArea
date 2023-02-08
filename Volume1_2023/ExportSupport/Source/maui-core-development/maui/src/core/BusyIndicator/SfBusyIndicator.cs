using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Animations;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core.BusyIndicator;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// <see cref="SfBusyIndicator"/> is a indicator to show a busy status of app loading, data processing etc
    /// </summary>
    /// <example>
    /// The following example show how to initialize the busy indicator.
    /// # [XAML](#tab/tabid-1)
    /// <code><![CDATA[
    /// <sf:SfBusyIndicator IsRunning="True" AnimationType="CircularMaterial" Title="BusyIndicator"/>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
    ///    {
    ///     AnimationType = AnimationType.Cupertino,
    ///     Title = "Cupertino",
    ///     IsRunning = true,
    ///     TextColor = Colors.Blue
    ///     };
    ///  this.Content = busyIndicator;
    /// ]]></code>
    /// ***
    /// </example>
    public class SfBusyIndicator : SfContentView , ITextElement
    {

        #region Fields

        private BusyIndicatorAnimation? busyIndicatorAnimation;

        private IAnimationManager? animationManager;

        private Rect TextRect = new();

        private double titleHeight = 0;

        private AnimationType animation;

        #endregion

        #region Properties

        /// <summary>
        /// Identifies the <see cref="IsRunning"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsRunning"/> bindable property.
        /// </value>
        public static readonly BindableProperty IsRunningProperty =
            BindableProperty.Create(nameof(IsRunning), typeof(bool), typeof(SfBusyIndicator), false, BindingMode.OneWay, null, OnIsRunningPropertyChanged);

        /// <summary>
        /// Gets or sets the value indicating if the indicator is running.
        /// </summary>
        /// <value>
        /// Specifies the indicator is running. The default value is false.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="CircularMaterial" 
        ///                     Title="BusyIndicator"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.CircularMaterial,
        ///    Title = "BusyIndicator",
        ///    IsRunning = true,
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
        public bool IsRunning
        {
            get { return (bool)this.GetValue(IsRunningProperty); }
            set { this.SetValue(IsRunningProperty, value); }
        }

        /// <summary>
        /// OnIsRunning Property Changed method is called when change the Initial value of IsRunning.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnIsRunningPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfBusyIndicator busyIndicator)
            {
                if (busyIndicator != null)
                {
                    if (busyIndicator.busyIndicatorAnimation != null)
                    {
                        busyIndicator.busyIndicatorAnimation.RunAnimation((bool)newValue);
                        busyIndicator.InvalidateDrawable();
                    }
                }
            }
        }


        /// <summary>
        /// Identifies the <see cref="IndicatorColor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IndicatorColor"/> bindable property.
        /// </value>
        public static readonly BindableProperty IndicatorColorProperty =
            BindableProperty.Create(nameof(IndicatorColor), typeof(Color), typeof(SfBusyIndicator), Color.FromArgb("#FF512BD4"), BindingMode.OneWay, null, OnIndicatorColorPropertyChanged);

        /// <summary>
        /// Gets or sets the color of the indicator.
        /// </summary>
        /// <value>
        /// Specifies the color of the indicator. The default value is Colors.Blue.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="CircularMaterial" 
        ///                     Title="BusyIndicator"
        ///                     IndicatorColor="Aqua"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.CircularMaterial,
        ///    Title = "Circular Material",
        ///    IsRunning = true,
        ///    IndicatorColor = Colors.Aqua,
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
        public Color IndicatorColor
        {
            get { return (Color)this.GetValue(IndicatorColorProperty); }
            set { this.SetValue(IndicatorColorProperty, value); }
        }


        /// <summary>
        /// OnIndicator Color Property Changed method is called when change the Initial value of IndicatorColor.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnIndicatorColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfBusyIndicator busyIndicator)
            {
                if (busyIndicator != null)
                {
                    if (busyIndicator.busyIndicatorAnimation != null)
                    {
                        busyIndicator.busyIndicatorAnimation.Color = (Color)newValue;
                        busyIndicator.InvalidateDrawable();
                    }

                }
            }
        }

        /// <summary>
        /// Identifies the <see cref="OverlayFill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="OverlayFill"/> bindable property.
        /// </value>
        public static readonly BindableProperty OverlayFillProperty =
            BindableProperty.Create(nameof(OverlayFill), typeof(Brush), typeof(SfBusyIndicator), null, BindingMode.OneWay, null, OnOverlayFillPropertyChanged);

        /// <summary>
        /// Gets or sets the brush for an overlay background of the indicator. 
        /// </summary>
        /// <value>
        /// Specifies the brush for an overlay background. The default value is null.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="CircularMaterial" 
        ///                     Title="BusyIndicator"
        ///                     IndicatorColor="Aqua"
        ///                     OverlayFill="#ff8000"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.CircularMaterial,
        ///    Title = "Circular Material",
        ///    IsRunning = true,
        ///    IndicatorColor = Colors.Aqua,
        ///    OverlayFill=new SolidColorBrush(Colors.Beige),
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
        public Brush OverlayFill
        {
            get { return (Brush)this.GetValue(OverlayFillProperty); }
            set { this.SetValue(OverlayFillProperty, value); }
        }

        /// <summary>
        /// OnOverlay Fill Property Changed method is called when change the Initial value of OverlayFill.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnOverlayFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfBusyIndicator busyIndicator)
            {
                if (busyIndicator != null)
                {
                    if (busyIndicator.busyIndicatorAnimation != null)
                    {
                        busyIndicator.InvalidateDrawable();
                    }

                }
            }
        }



        /// <summary>
        /// Identifies the <see cref="AnimationType"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AnimationType"/> bindable property.
        /// </value>
        public static readonly BindableProperty AnimationTypeProperty =
            BindableProperty.Create(nameof(AnimationType), typeof(AnimationType), typeof(SfBusyIndicator), AnimationType.CircularMaterial, BindingMode.OneWay, null, OnAnimationTypeChanged);

        /// <summary>
        /// Gets or sets the animation type of the indicator. 
        /// </summary>
        /// <value>
        /// Specifies the animation type of the indicator. The default value is <see cref="AnimationType.CircularMaterial"/>
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="LinearMaterial" 
        ///                     Title="BusyIndicator"
        ///                     IndicatorColor="Aqua"
        ///                     OverlayFill="#ff8000"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.LinearMaterial,
        ///    Title = "BusyIndicator",
        ///    IsRunning = true,
        ///    IndicatorColor = Colors.Aqua,
        ///    OverlayFill=new SolidColorBrush(Colors.Beige),
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
        public AnimationType AnimationType
        {
            get { return (AnimationType)this.GetValue(AnimationTypeProperty); }
            set { this.SetValue(AnimationTypeProperty, value); }
        }

        /// <summary>
        /// OnAnimation Type Changed method is called when change the Initial value of AnimationType.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnAnimationTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfBusyIndicator busyIndicator)
            {
                if (busyIndicator != null)
                {
                    busyIndicator.SetAnimationType((AnimationType)newValue);
                }
            }
        }

        /// <summary>
        /// Identifies the <see cref="Title"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Title"/> bindable property.
        /// </value>
        public static readonly BindableProperty TitleProperty =
            BindableProperty.Create(nameof(Title), typeof(String), typeof(SfBusyIndicator), String.Empty, BindingMode.OneWay, null, OnTitlePropertyChanged);

        /// <summary>
        /// Gets or sets the title of the indicator.
        /// </summary>
        /// <value>
        /// Specifies the title of the indicator. The default value is an empty string.
        /// </value>
        /// <example>
        /// The following example shows how to display the title.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="CircularMaterial" 
        ///                     Title="BusyIndicator"
        ///                     />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.CircularMaterial,
        ///    Title = "BusyIndicator",
        ///    IsRunning = true,
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
        public String Title
        {
            get { return (String)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// OnTitle Property Changed method is called when change the Initial value of Title.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfBusyIndicator busyIndicator)
            {
                if (busyIndicator != null)
                {
                    busyIndicator.InvalidateDrawable();
                }
            }
        }


        /// <summary>
        /// Identifies the <see cref="DurationFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DurationFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty DurationFactorProperty =
            BindableProperty.Create(nameof(DurationFactor), typeof(double), typeof(SfBusyIndicator), 0.5d, BindingMode.OneWay, null, OnDurationFactorPropertyChanged);

        /// <summary>
        /// Gets or sets the duration of the indicator animation.
        /// </summary>
        /// <value>
        /// Specifies the duration of the indicator animation. The default value is 0.5d.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="CircularMaterial" 
        ///                     Title="BusyIndicator"
        ///                     DurationFactor="1"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.CircularMaterial,
        ///    Title = "BusyIndicator",
        ///    IsRunning = true,
        ///    DurationFactor = 1,
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
        public double DurationFactor
        {
            get { return (double)this.GetValue(DurationFactorProperty); }
            set { this.SetValue(DurationFactorProperty, value); }
        }

        /// <summary>
        /// OnDurationFactor Property Changed method is called when change the Initial value of DurationFactor.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnDurationFactorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfBusyIndicator busyIndicator)
            {
                if (busyIndicator != null && busyIndicator.busyIndicatorAnimation!=null)
                {
                    busyIndicator.busyIndicatorAnimation.AnimationDuration = (double)newValue;
                }
            }
        }

        /// <summary>
        /// Identifies the <see cref="TitlePlacement"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TitlePlacement"/> bindable property.
        /// </value>
        public static readonly BindableProperty TitlePlacementProperty =
            BindableProperty.Create(nameof(TitlePlacement), typeof(BusyIndicatorTitlePlacement), typeof(SfBusyIndicator), BusyIndicatorTitlePlacement.Bottom, BindingMode.OneWay, null, OnTitlePlacementPropertyChanged);

        /// <summary>
        /// Gets or sets the place for the title of the indicator.
        /// </summary>
        /// <value>
        /// Specifies the place for the title of the indicator. The default value is <see cref="BusyIndicatorTitlePlacement.Bottom"/>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="CircularMaterial" 
        ///                     Title="BusyIndicator"
        ///                     TitlePlacement="Top"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.CircularMaterial,
        ///    Title = "BusyIndicator",
        ///    IsRunning = true,
        ///    TitlePlacement=BusyIndicatorTitlePlacement.Top,
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
        public BusyIndicatorTitlePlacement TitlePlacement
        {
            get { return (BusyIndicatorTitlePlacement)this.GetValue(TitlePlacementProperty); }
            set { this.SetValue(TitlePlacementProperty, value); }
        }

        /// <summary>
        ///  OnTitlePlacement Property Changed method is called when change the Initial value of TitlePlacement.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnTitlePlacementPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfBusyIndicator busyIndicator)
            {
                if (busyIndicator != null)
                {
                    busyIndicator.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Identifies the <see cref="TitleSpacing"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TitleSpacing"/> bindable property.
        /// </value>
        public static readonly BindableProperty TitleSpacingProperty =
            BindableProperty.Create(nameof(TitleSpacing), typeof(double), typeof(SfBusyIndicator), 10d, BindingMode.OneWay, null, OnTitleSpacingPropertyChanged);

        /// <summary>
        /// Gets or sets the title spacing for give space between title and indicator.
        /// </summary>
        /// <value>
        /// Specifies the title spacing for provide space. The default value is 10d.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="CircularMaterial" 
        ///                     Title="BusyIndicator"
        ///                     TitleSpacing="40"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.CircularMaterial,
        ///    Title = "BusyIndicator",
        ///    IsRunning = true,
        ///    TitleSpacing = 40,
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
        public double TitleSpacing
        {
            get { return (double)this.GetValue(TitleSpacingProperty); }
            set { this.SetValue(TitleSpacingProperty, value); }
        }

        /// <summary>
        /// OnTitleSpacing Property Changed method is called when change the Initial value of TitleSpacing.
        /// </summary>
        ///<param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnTitleSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfBusyIndicator busyIndicator)
            {
                if (busyIndicator != null)
                {
                    busyIndicator.InvalidateDrawable();
                }
            }
        }


        /// <summary>
        /// Identifies the <see cref="SizeFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SizeFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty SizeFactorProperty =
            BindableProperty.Create(nameof(SizeFactor), typeof(double), typeof(SfBusyIndicator), 0.5, BindingMode.OneWay, null, OnSizeFactorPropertyChanged);

        /// <summary>
        /// Gets or sets the size of the indicator.
        /// </summary>
        /// <value>
        /// Specifies the size of the indicator. The default value is 0.5.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="CircularMaterial" 
        ///                     Title="BusyIndicator"
        ///                     SizeFactor="1"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.CircularMaterial,
        ///    Title = "BusyIndicator",
        ///    IsRunning = true,
        ///    SizeFactor = 1,
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
        public double SizeFactor
        {
            get { return (double)this.GetValue(SizeFactorProperty); }
            set { this.SetValue(SizeFactorProperty, value); }
        }

        /// <summary>
        /// OnSizeFactor Property Changed method is called when change the Initial value of TitleSpacing.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnSizeFactorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfBusyIndicator busyIndicator)
            {
                if (busyIndicator != null && busyIndicator.busyIndicatorAnimation!=null)
                {
                    busyIndicator.busyIndicatorAnimation.sizeFactor = (double)newValue;
                }
            }
        }

      

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
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BadgeSettings), Colors.Black, BindingMode.Default, null, OnITextElementPropertyChanged);

        /// <summary>
        /// Gets or sets the font size for the title of the indicator.
        /// </summary>
        /// <value>
        /// Specifies the font size for the title. The default value is 12d.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="CircularMaterial" 
        ///                     Title="BusyIndicator"
        ///                     FontSize="20"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.CircularMaterial,
        ///    Title = "BusyIndicator",
        ///    IsRunning = true,
        ///    FontSize = 20,
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Enumerates values that describe font styles of title.
        /// </summary>
        /// <value>
        /// Specifies the font styles of the title. The default value is None.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="CircularMaterial" 
        ///                     Title="BusyIndicator"
        ///                     FontAttributes="Bold"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.CircularMaterial,
        ///    Title = "BusyIndicator",
        ///    IsRunning = true,
        ///    FontAttributes=FontAttributes.Bold,
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)this.GetValue(FontAttributesProperty); }
            set { this.SetValue(FontAttributesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font family for the title of the indicator.
        /// </summary>
        /// <value>
        /// Specifies the font family for the title of the indicator.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="CircularMaterial" 
        ///                     Title="BusyIndicator"
        ///                     FontFamily="Serif"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.CircularMaterial,
        ///    Title = "BusyIndicator",
        ///    IsRunning = true,
        ///    FontFamily = "Serif",
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
        public string FontFamily
        {
            get { return (string)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color for the title of the indicator.
        /// </summary>
        /// <value>
        /// Specifies the color for the title of the indicator. The default value is Colors.Black.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <sf:SfBusyIndicator IsRunning="True" 
        ///                     AnimationType="CircularMaterial" 
        ///                     Title="BusyIndicator"
        ///                     TextColor="DarkGreen"/>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBusyIndicator busyIndicator = new SfBusyIndicator()
        ///{
        ///    AnimationType = AnimationType.CircularMaterial,
        ///    Title = "BusyIndicator",
        ///    IsRunning = true,
        ///    TextColor=Colors.DarkGreen,
        ///};
        ///this.Content = busyIndicator;
        /// ]]></code>
        /// ***
        /// </example>
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
            return 12d;
        }

        /// <summary>
        /// Invoked when the <see cref="FontAttributesProperty"/> changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {
            this.InvalidateDrawable();
        }



        /// <summary>
        /// Invoked when the <see cref="FontFamilyProperty"/> changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {
            this.InvalidateDrawable();
        }

        /// <summary>
        /// Invoked when the <see cref="FontSizeProperty"/> changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {
            this.InvalidateDrawable();
        }


        private static void OnITextElementPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is SfBusyIndicator busyIndicator)
            {
                if (busyIndicator != null)
                {
                    busyIndicator.InvalidateDrawable();
                }
            }
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfBusyIndicator"/> class.
        /// </summary>
        public SfBusyIndicator()
        {
            this.ValidateLicense();
            this.DrawingOrder = DrawingOrder.AboveContent;
        }

        #endregion

        #region Overrides
        /// <summary>
        /// Override the OnHandlerChanged to set the animation.
        /// </summary>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (Handler != null && Handler.MauiContext != null)
            {
                animationManager = Handler.MauiContext.Services.GetRequiredService<IAnimationManager>();
                this.SetAnimationType(this.AnimationType);
            }
        }

        /// <summary>
        /// Override the ArrangeContent to call CalculateXYPositions from DoubleCircleBusyIndicatorAnimation class.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            if (this.Content != null)
            {
                AbsoluteLayout.SetLayoutBounds(this.Content, bounds);
            }
            if (this.animation == AnimationType.DoubleCircle && this.busyIndicatorAnimation is DoubleCircleBusyIndicatorAnimation doubleCircle)
            {
                doubleCircle.CalculateXYPositions(this);
            }
            return base.ArrangeContent(bounds);
        }

        /// <summary>
        /// On Draw method is used to draw the busy indicator.
        /// </summary>
        /// <param name="canvas">Draw graphical objects on a canvas that's defined as an ICanvas object.</param>
        /// <param name="dirtyRect">It defines the rectangular bounds.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);

            if (IsRunning)
            {
                if (this.OverlayFill != null)
                {
                    canvas.SetFillPaint(this.OverlayFill, dirtyRect);
                    canvas.FillRectangle(dirtyRect);
                }

                if (busyIndicatorAnimation != null)
                {
                    busyIndicatorAnimation.DrawAnimation(this, canvas);
                    this.DrawTitle(canvas);

                }
            }
        }

        #endregion

        #region Methods

        private void DrawTitle(ICanvas canvas)
        {
            if (!String.IsNullOrEmpty(this.Title) && this.busyIndicatorAnimation!=null && this.TitlePlacement != BusyIndicatorTitlePlacement.None)
            {

                this.titleHeight = TextMeasurer.CreateTextMeasurer().MeasureText(this.Title, this).Height;

                this.TextRect.X = 0;
                this.TextRect.Width = this.Width;

                if (this.TitlePlacement == BusyIndicatorTitlePlacement.Bottom)
                    this.TextRect.Y = this.busyIndicatorAnimation.actualRect.Y + this.busyIndicatorAnimation.actualRect.Height + this.TitleSpacing;
                else
                    this.TextRect.Y = this.busyIndicatorAnimation.actualRect.Y - (this.TitleSpacing + this.titleHeight);

                this.TextRect.Height = this.titleHeight;
                canvas.DrawText(this.Title, this.TextRect, HorizontalAlignment.Center, VerticalAlignment.Top, this);
            }
        }

        private void SetAnimationType(AnimationType animationType)
        {
            if (this.animationManager == null)
                return;

            if (animationType == AnimationType.CircularMaterial)
            {
                this.busyIndicatorAnimation = new CircularMaterialBusyIndicatorAnimation(this.animationManager);
            }
            else if (animationType == AnimationType.Cupertino)
            {
                this.busyIndicatorAnimation = new CupertinoBusyIndicatorAnimation(this.animationManager);
            }
            else if(animationType == AnimationType.LinearMaterial)
            {
                this.busyIndicatorAnimation = new LinearMaterialBusyIndicatorAnimation(this.animationManager);
            }
            else if(animationType == AnimationType.SingleCircle)
            {
                this.busyIndicatorAnimation = new SingleCircleBusyIndictorAnimation(this.animationManager);
            }
            else if (animationType == AnimationType.DoubleCircle)
            {
                this.busyIndicatorAnimation = new DoubleCircleBusyIndicatorAnimation(this.animationManager, this);
                this.animation = AnimationType.DoubleCircle;
            }
            if (this.busyIndicatorAnimation != null)
            {
                this.busyIndicatorAnimation.Color = this.IndicatorColor;
                this.busyIndicatorAnimation.RunAnimation(this.IsRunning);
                this.busyIndicatorAnimation.AnimationDuration = this.DurationFactor;
                this.busyIndicatorAnimation.sizeFactor = this.SizeFactor;
                this.InvalidateDrawable();
            }
        }


        internal void OnFontFamilyChanged(string oldValue, string newValue)
        {
            this.InvalidateDrawable();
        }

        internal void OnFontSizeChanged(double oldValue, double newValue)
        {
             this.InvalidateDrawable();
        }

        internal double FontSizeDefaultValueCreator()
        {
            throw new NotImplementedException();
        }

        internal void OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {
            this.InvalidateDrawable();
        }


        /// <summary>
        /// This method hooked whenever the font changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        public void OnFontChanged(Microsoft.Maui.Font oldValue, Microsoft.Maui.Font newValue)
        {
            this.InvalidateDrawable();
        }

        #endregion


    }
}
