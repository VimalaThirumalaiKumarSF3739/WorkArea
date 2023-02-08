// <copyright file="SfAvatarView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Devices;
using Syncfusion.Maui.Core.Helper;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// SfAvatarView is a graphical representation of the user image. You can customize it by adding image, background color, icon, text etc.
    /// </summary>
    /// <example>
    /// The following examples show how to initialize the <see cref="SfAvatarView"/>
    /// # [XAML](#tab/tabid-1)
    /// <code><![CDATA[
    ///  <sfavatar:SfAvatarView AvatarName="Alex"
    ///                         VerticalOptions="Center"
    ///                         HorizontalOptions="Center"   
    ///                         HeightRequest="50"
    ///                         CornerRadius="25"
    ///                         WidthRequest="50" />
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///  SfAvatarView avatar = new SfAvatarView();
    ///  avatar.AvatarName = "Alex";
    ///	 avatar.VerticalOptions = LayoutOptions.Center;
    ///  avatar.HorizontalOptions = LayoutOptions.Center;
    ///	 avatar.WidthRequest = 50;
    ///	 avatar.HeightRequest = 50;
    ///	 avatar.CornerRadius = 25;
    ///  this.Content = avatar;
    /// ]]></code>
    /// ***
    /// </example>

    [DesignTimeVisible(true)]
    public class SfAvatarView : SfView
    {
        #region Bindable properties
        /// <summary>
        /// Identifies the <see cref="InitialsColor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="InitialsColor"/> bindable property.
        /// </value>
        public static readonly BindableProperty InitialsColorProperty =
            BindableProperty.Create(nameof(InitialsColor), typeof(Color), typeof(SfAvatarView), Colors.Black, BindingMode.OneWay, null, OnInitialsColorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="FontSize"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontSize"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(double), typeof(SfAvatarView), 18d, BindingMode.OneWay, null, OnFontSizePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="FontFamily"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontFamily"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnFontFamilyPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="FontAttributes"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontAttributes"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnFontAttributesPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ImageSource"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ImageSource"/> bindable property.
        /// </value>
        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnImageSourcePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ContentType"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ContentType"/> bindable property.
        /// </value>
        public static readonly BindableProperty ContentTypeProperty =
            BindableProperty.Create(nameof(ContentType), typeof(ContentType), typeof(SfAvatarView), ContentType.Initials, BindingMode.OneWay, null, OnContentTypePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="InitialsType"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="InitialsType"/> bindable property.
        /// </value>
        public static readonly BindableProperty InitialsTypeProperty =
            BindableProperty.Create(nameof(InitialsType), typeof(InitialsType), typeof(SfAvatarView), InitialsType.SingleCharacter, BindingMode.OneWay, null, OnInitialsTypePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AvatarCharacter"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AvatarCharacter"/> bindable property.
        /// </value>
        public static readonly BindableProperty AvatarCharacterProperty =
            BindableProperty.Create(nameof(AvatarCharacter), typeof(AvatarCharacter), typeof(SfAvatarView), AvatarCharacter.Avatar1, BindingMode.OneWay, null, OnAvatarCharacterPropertyChanged);


        /// <summary>
        /// Identifies the <see cref="HeightRequest"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HeightRequest"/> bindable property.
        /// </value>
        public static new readonly BindableProperty HeightRequestProperty =
            BindableProperty.Create(nameof(HeightRequest), typeof(double), typeof(SfAvatarView), 0d, BindingMode.OneWay, null, OnHeightRequestPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="GroupSource"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="GroupSource"/> bindable property.
        /// </value>
        public static readonly BindableProperty GroupSourceProperty =
            BindableProperty.Create(nameof(GroupSource), typeof(System.Collections.IEnumerable), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnGroupSourcePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="BackgroundColorMemberPath"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BackgroundColorMemberPath"/> bindable property.
        /// </value>
        public static readonly BindableProperty BackgroundColorMemberPathProperty =
            BindableProperty.Create(nameof(BackgroundColorMemberPath), typeof(string), typeof(SfAvatarView), string.Empty, BindingMode.OneWay, null, OnBackgroundColorMemberPathPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="InitialsColorMemberPath"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="InitialsColorMemberPath"/> bindable property.
        /// </value>
        public static readonly BindableProperty InitialsColorMemberPathProperty =
            BindableProperty.Create(nameof(InitialsColorMemberPath), typeof(string), typeof(SfAvatarView), string.Empty, BindingMode.OneWay, null, OnInitialsColorMemberPathPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AvatarName"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AvatarName"/> bindable property.
        /// </value>
        public static readonly BindableProperty AvatarNameProperty =
            BindableProperty.Create(nameof(AvatarName), typeof(string), typeof(SfAvatarView), string.Empty, BindingMode.OneWay, null, OnNamePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AvatarSize"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AvatarSize"/> bindable property.
        /// </value>
        public static readonly BindableProperty AvatarSizeProperty =
            BindableProperty.Create(nameof(AvatarSize), typeof(AvatarSize), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnAvatarStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="BackgroundColor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BackgroundColor"/> bindable property.
        /// </value>
        public static new readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnBackgroundColorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Background"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Background"/> bindable property.
        /// </value>
        public static new readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(nameof(Background), typeof(Brush), typeof(SfAvatarView), null, BindingMode.OneWay, null, OnBackgroundPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AvatarShape"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AvatarShape"/> bindable property.
        /// </value>
        public static readonly BindableProperty AvatarShapeProperty =
            BindableProperty.Create(nameof(AvatarShape), typeof(AvatarShape), typeof(SfAvatarView), AvatarShape.Custom, BindingMode.OneWay, null, OnAvatarStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="WidthRequest"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="WidthRequest"/> bindable property.
        /// </value>
        public static new readonly BindableProperty WidthRequestProperty =
            BindableProperty.Create(nameof(WidthRequest), typeof(double), typeof(SfAvatarView), 0d, BindingMode.OneWay, null, OnWidthRequestPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CornerRadius"/> bindable property.
        /// </value>
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(SfAvatarView), new CornerRadius(0), BindingMode.OneWay, null, OnCornerRadiusPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ImageSourceMemberPath"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ImageSourceMemberPath"/> bindable property.
        /// </value>
        public static readonly BindableProperty ImageSourceMemberPathProperty =
            BindableProperty.Create(nameof(ImageSourceMemberPath), typeof(string), typeof(SfAvatarView), string.Empty, BindingMode.OneWay, null, OnImageSourceMemberPathPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AvatarColorMode"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AvatarColorMode"/> bindable property.
        /// </value>
        public static readonly BindableProperty AvatarColorModeProperty =
            BindableProperty.Create(nameof(AvatarColorMode), typeof(AvatarColorMode), typeof(SfAvatarView), AvatarColorMode.Default, BindingMode.OneWay, null, OnColorModePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="InitialsMemberPath"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="InitialsMemberPath"/> bindable property.
        /// </value>
        public static readonly BindableProperty InitialsMemberPathProperty =
            BindableProperty.Create(nameof(InitialsMemberPath), typeof(string), typeof(SfAvatarView), string.Empty, BindingMode.OneWay, null, OnInitialsMemberPathPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StrokeThickness"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeThicknessProperty =
            BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(SfAvatarView), 2d, BindingMode.OneWay, null, OnStrokeThicknessPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Stroke"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(SfAvatarView), new SolidColorBrush(Colors.Black), BindingMode.OneWay, null, OnStrokePropertyChanged);

        #endregion

        #region Fields

        /// <summary>
        /// IsGroupSourceEmpty bool for checking the condition for setting the font icon and text color in group view.
        /// </summary>
        private bool isGroupSourceEmpty = true;

        /// <summary>
        /// The Image Location.
        /// </summary>
        private readonly string imageLocation = "Syncfusion.Maui.Core.AvatarView.VectorImages.";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfAvatarView"/> class.
        /// </summary>
        public SfAvatarView()
        {
            this.ValidateLicense(true);
            this.InitializeElements();
            this.DisplayCurrentAvatarElement();
            this.SetInitialsBasedOnInitialsType(this.AvatarName, this.InitialsLabel);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of <see cref="StrokeThickness"/> of the control.This property can be used to customize the thickness of stroke. 
        /// </summary>
        /// <value> Specifies the stroke thickness.The default value is 2d. </value>
        /// <example>
        /// The following example shows how to apply the stroke thickness for a <see cref="SfAvatarView"/>.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  AvatarName="Alex"
        ///                       StrokeThickness="10"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
		///  avatar.StrokeThickness = 10;
		///  avatar.AvatarName = "Alex";
		///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
		///  avatar.WidthRequest = 150;
		///  avatar.HeightRequest = 150;
		///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { this.SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of <see cref="Stroke"/> of the control.This property can be used to change the color of stroke.
        /// </summary>
        /// <value>Specifies the stroke.The default value is <see cref="Colors.Black"/>.</value>
        /// <example>
        /// The following example shows how to apply the stroke for a <see cref="SfAvatarView"/>.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  AvatarName="Alex"
        ///                       Stroke="Red"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.Stroke = Colors.Red;
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value of the AvatarCharacter, which is shown only when the avatar type is AvatarCharacter. They have vector images in the avatarcharacter types.
        /// </summary>
        public AvatarCharacter AvatarCharacter
        {
            get { return (AvatarCharacter)this.GetValue(AvatarCharacterProperty); }
            set { this.SetValue(AvatarCharacterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="FontAttributes"/> for the initials when the ContentType is Initials.
        /// </summary>
        /// <value>Specifies the font attributes.The default value is null.</value>
        /// <example>
        /// The following example shows how to apply the FontAttributes.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  AvatarName="Alex"
        ///                       FontAttributes="Bold"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.FontAttributes = FontAttributes.Bold;
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)this.GetValue(FontAttributesProperty); }
            set { this.SetValue(FontAttributesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="FontFamily"/> for the initials when the ContentType is Initials.
        /// </summary>
        /// <value>Specifies the font family.The default value is null.</value>
        /// <example>
        /// The following example shows how to apply the fontfamily.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  AvatarName="Alex"
        ///                       FontFamily="OpenSansRegular"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.FontFamily="OpenSansRegular";
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public string FontFamily
        {
            get { return (string)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color which defines the color for the initials string.
        /// </summary>
        /// <value>Specifies the Initial Color.The default value is <see cref="Colors.Black"/> </value>
        /// <example>
        /// The following example shows how to apply the initial color for <see cref="SfAvatarView"/>.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  AvatarName="Alex"
        ///                       InitialsColor="Red"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.InitialsColor = Colors.Red;
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public Color InitialsColor
        {
            get { return (Color)this.GetValue(InitialsColorProperty); }
            set { this.SetValue(InitialsColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of the font for the initials string.
        /// </summary>
        /// <value>Specifies the font size.The default value is 18d.</value>
        /// <example>
        /// The following example shows how to apply the font size.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  AvatarName="Alex"
        ///                       FontSize="18" 
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.FontSize = 18;
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        //[TypeConverter(typeof(FontSizeConverter))]

        public double FontSize
        {
            get { return (double)this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ImageSource for displaying a custom image when ContentType is set to Custom. 
        /// </summary>
        /// <value> Specifies the custom ImageSource.The default value is null.</value>
        /// <example>
        /// The following example shows how to apply the image.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  ContentType="Custom"
        ///                       ImageSource="dotnet_bot.png"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.ContentType = ContentType.Custom;
        ///  avatar.ImageSource = "dotnet_bot.png";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public ImageSource ImageSource
        {
            get { return (ImageSource)this.GetValue(ImageSourceProperty); }
            set { this.SetValue(ImageSourceProperty, value); }
        }


        /// <summary>
        /// Gets or sets the value for the Avatar type. The types are initials, group, custom, and avatarcharacter.
        /// </summary>
        /// <value>Specifies the content type.</value>
        /// <example>
        /// The following example shows how to apply the content type for a <see cref="SfAvatarView"/>.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  ContentType="Custom"
        ///                       ImageSource="dotnet_bot.png"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.ContentType = ContentType.Custom;
        ///  avatar.ImageSource = "dotnet_bot.png";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public ContentType ContentType
        {
            get { return (ContentType)this.GetValue(ContentTypeProperty); }
            set { this.SetValue(ContentTypeProperty, value); }
        }


        /// <summary>
        /// Gets or sets the value for the avatar name, which displays the text in the <see cref="SfAvatarView"/>.
        /// </summary>
        /// <value>Specifies the avatar name.The default value is string.Empty.</value>
        /// <example>
        /// The following example shows how to apply the avatar name.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView 
        ///                       AvatarName="Alex"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public string AvatarName
        {
            get { return (string)this.GetValue(AvatarNameProperty); }
            set { this.SetValue(AvatarNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the HeightRequest of the control.
        /// </summary>
        /// <value>Specifies the height request for <see cref="SfAvatarView"/>.The default value is 0d. </value>
        /// <example>
        /// The following example shows how to apply the height request.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView 
        ///                       AvatarName="Alex"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public new double HeightRequest
        {
            get { return (double)this.GetValue(HeightRequestProperty); }
            set { this.SetValue(HeightRequestProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the initial type. 
        /// </summary>
        /// <value>Specifies the initial type for <see cref="SfAvatarView"/>.The default value is <see cref="InitialsType.SingleCharacter"/>. </value>
        /// <example>
        /// The following example shows how to apply the initial type.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  InitialsType="SingleCharacter"
        ///                       AvatarName="Alex"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.InitialsType = InitialsType.SingleCharacter;
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public InitialsType InitialsType
        {
            get { return (InitialsType)this.GetValue(InitialsTypeProperty); }
            set { this.SetValue(InitialsTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the background color.
        /// </summary>
        /// <value>Specifies the background color for <see cref="SfAvatarView"/>. The default value is null.</value>
        /// <example>
        /// The following example shows how to apply the background color.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  BackgroundColor="Aqua" 
        ///                       AvatarName="Alex"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.BackgroundColor = Colors.Red;
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public new Color BackgroundColor
        {
            get { return (Color)this.GetValue(BackgroundColorProperty); }
            set { this.SetValue(BackgroundColorProperty, value); }
        }


        /// <summary>
        /// Gets or sets the value for the background.
        /// </summary>
        /// <value>Specifies the background for <see cref="SfAvatarView"/>. The default value is null. </value>
        /// <example>
        /// The following example shows how to apply the background.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  Background="Aqua" 
        ///                       AvatarName="Alex"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.Background = Colors.Red;
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public new Brush Background
        {
            get { return (Brush)this.GetValue(BackgroundProperty); }
            set { this.SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the GroupSource used for setting more than one image/text, and this is shown only when avatar type is set to Group.
        /// </summary>
        /// <value>Specifies the GroupSource for <see cref="SfAvatarView"/>. The default value is null.</value>
        /// <example>
        /// The following example shows how to apply the group source.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  
        ///                       ContentType="Group" 
        ///                       GroupSource="{Binding CollectionImage}"  
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  EmployeeViewMdoel emp = new EmployeeViewMdoel()
        ///  avatar.GroupSource = emp.CollectionImage;
        ///  avatar.ContentType = ContentType.Group;
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public System.Collections.IEnumerable GroupSource
        {
            get { return (System.Collections.IEnumerable)this.GetValue(GroupSourceProperty); }
            set { this.SetValue(GroupSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the BackgroundColorMemberPath that shows the background color value when avatar type is set to Group.
        /// </summary>
        /// <value>Specifies the BackgroundColorMemberPath. The default value is string.Empty.</value>
        /// <example>
        /// The following example shows how to apply the backgroundcolor member path.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  ContentType="Group" 
        ///                       GroupSource="{Binding CollectionImage}" 
        ///                       BackgroundColorMemberPath="Colors"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  EmployeeViewMdoel emp = new EmployeeViewMdoel()
        ///  avatar.GroupSource = emp.CollectionImage;
        ///  avatar.BackgroundColorMemberPath = "Colors";
        ///  avatar.ContentType = ContentType.Group;
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public string BackgroundColorMemberPath
        {
            get { return (string)this.GetValue(BackgroundColorMemberPathProperty); }
            set { this.SetValue(BackgroundColorMemberPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the ImageSourceMemberPath that shows the image value when avatar type is set to Group.
        /// </summary>
        /// <value>Specifies the ImageSourceMemberPath for <see cref="SfAvatarView"/>.The default value is string.Empty.</value>
        /// <example>
        /// The following example shows how to apply the imagesource member path.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  ContentType="Group" 
        ///                       GroupSource="{Binding CollectionImage}" 
        ///                       ImageSourceMemberPath="ImageSource"  
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  EmployeeViewMdoel emp = new EmployeeViewMdoel()
        ///  avatar.GroupSource = emp.CollectionImage;
        ///  avatar.ImageSourceMemberPath = "ImageSource";
        ///  avatar.ContentType = ContentType.Group;
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public string ImageSourceMemberPath
        {
            get { return (string)this.GetValue(ImageSourceMemberPathProperty); }
            set { this.SetValue(ImageSourceMemberPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for corner radius.
        /// </summary>
        /// <value>Specifies the corner radius.The default value is cornerradius(0).</value>
        /// <example>
        /// The following example shows how to apply the corner radius.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  AvatarName = "Alex"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)this.GetValue(CornerRadiusProperty); }
            set { this.SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the ColorMode, which consists of three types such as default, dark color, and light color. 
        /// </summary>
        /// <value>Specifies the avatar color mode for <see cref="SfAvatarView"/>.The default value is <see cref="AvatarColorMode.Default"/>. </value>
        /// <example>
        /// The following example shows how to apply the avatar color mode in <see cref="SfAvatarView"/>.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  AvatarName = "Alex"
        ///                       AvatarColorMode="DarkBackground"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.AvatarName = "Alex";
        ///  avatar.AvatarColorMode = AvatarColorMode.DarkBackground;
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public AvatarColorMode AvatarColorMode
        {
            get { return (AvatarColorMode)this.GetValue(AvatarColorModeProperty); }
            set { this.SetValue(AvatarColorModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the InitialsColorMemberPath that shows the initials color value when avatar type is set to Group.
        /// </summary>
        /// <value>Specifies the avatar color mode.The default value is string.Empty</value>
        /// <example>
        /// The following example shows how to apply the avatar color mode.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  AvatarName = "Alex"
        ///                       InitialsColorMemberPath="Colors" 
        ///                       AvatarColorMode="DarkBackground"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.AvatarName = "Alex";
        ///  avatar.InitialsColorMemberPath="Colors" 
        ///  avatar.AvatarColorMode = AvatarColorMode.DarkBackground;
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public string InitialsColorMemberPath
        {
            get { return (string)this.GetValue(InitialsColorMemberPathProperty); }
            set { this.SetValue(InitialsColorMemberPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the WidthRequest of the control.
        /// </summary>
        /// <value> Specifies the width request. The default value is 0d.</value>
        /// <example>
        /// The following example shows how to apply the width request.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView 
        ///                       AvatarName="Alex"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example>
        public new double WidthRequest
        {
            get { return (double)this.GetValue(WidthRequestProperty); }
            set { this.SetValue(WidthRequestProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the InitialsMemberPath that shows the initials value when avatar type is set to Group.
        /// </summary>
        /// <value>Specifies the initialmemberpath.The default value is string.Empty.</value>
        /// <example>
        /// The following example shows how to apply the initial member path.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  ContentType="Group"
        ///                       InitialsMemberPath="Name"
        ///                       GroupSource="{Binding PeopleCollection}"
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  AvatarViewModel avm = new AvatarViewModel();
        ///  avatar.ContentType = ContentType.Group;
        ///  avatar.InitialsMemberPath = "Name";
        ///  avatar.GroupSource = avm.PeopleCollection;
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example> 
        public string InitialsMemberPath
        {
            get { return (string)this.GetValue(InitialsMemberPathProperty); }
            set { this.SetValue(InitialsMemberPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the AvatarShape which is used for changing the shapes of the avatar views.
        /// </summary>
        /// <value>Specifies the avatar shape. The default value is <see cref="AvatarShape.Custom"/>.</value>
        /// <example>
        /// The following example shows how to apply the avatar shape.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  AvatarName="Alex"
        ///                       AvatarShape="Square" 
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.AvatarShape = AvatarShape.Square;
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example> 
        public AvatarShape AvatarShape
        {
            get { return (AvatarShape)this.GetValue(AvatarShapeProperty); }
            set { this.SetValue(AvatarShapeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the AvatarSize which is used for changing the size of the avatar views.
        /// </summary>
        /// <value>Specifies the avatar size.The default value is null.</value>
        /// <example>
        /// The following example shows how to apply the avatar size.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <avatar:SfAvatarView  AvatarName="Alex"
        ///                       AvatarSize="Medium" 
        ///                       AvatarShape="Square" 
        ///                       VerticalOptions="Center"
        ///                       HorizontalOptions="Center"          
        ///                       WidthRequest="150"
        ///                       HeightRequest="150"
        ///                       CornerRadius="75" />
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfAvatarView avatar = new SfAvatarView();
        ///  avatar.AvatarShape = AvatarShape.Square;
        ///  avatar.AvatarSize =AvatarSize.Medium;
        ///  avatar.AvatarName = "Alex";
        ///  avatar.VerticalOptions = LayoutOptions.Center;
        ///  avatar.HorizontalOptions = LayoutOptions.Center;
        ///  avatar.WidthRequest = 150;
        ///  avatar.HeightRequest = 150;
        ///  avatar.CornerRadius = 75;
        ///  this.Content = avatar;
        /// ]]></code>
        /// ***
        /// </example> 
        public AvatarSize AvatarSize
        {
            get { return (AvatarSize)this.GetValue(AvatarSizeProperty); }
            set { this.SetValue(AvatarSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Border View that holds all the Content inside the border.
        /// </summary>
        internal SfBorder? Border;

        /// <summary>
        /// Gets or sets the layout grid that hold all the views, grids, and images.
        /// </summary>
        internal Grid? LayoutGrid;

        /// <summary>
        /// Gets or sets the Avatar image for getting the default images in the Avatar character avatar types.
        /// </summary>
        internal Image? AvatarImage { get; set; }

        /// <summary>
        /// Gets or sets the Custom image for adding image using the image source property.
        /// </summary>
        internal Image? CustomImage { get; set; }

        /// <summary>
        /// Gets or sets the initials label for adding the text and change the font size and font attributes.
        /// </summary>
        internal FontIconLabel InitialsLabel { get; set; } = new FontIconLabel();

        /// <summary>
        /// Gets or sets the GroupView for arranging more than one text or image in the view. Maximum three views can be arranged based on GroupSource count.
        /// </summary>
        internal AvatarGroupView? GroupView { get; set; }

        /// <summary>
        /// Gets or sets FontIconFontFamily for setting the font icon.
        /// </summary>
        internal string FontIconFontFamily { get; set; } = string.Empty;

        #endregion

        #region PropertyChanged

        /// <summary>
        /// OnDefaultAvatarPropertyChanged method sets the deault value when chanigng dynamically.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnAvatarCharacterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.SetAvatarCharacter(avatarView.AvatarCharacter);
            }
        }


        /// <summary>
        /// OnInitials Color Property Changed  method reflect when change the Initials color.
        /// </summary>
        /// <param name="bindable">The data binding object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnInitialsColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.SetColors();
            }
        }

        /// <summary>
        /// OnStroke property changed  method reflect when change the Stroke.
        /// </summary>
        /// <param name="bindable">The data binding object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnStrokePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView && avatarView.Border != null)
            {
                avatarView.Border.Stroke = avatarView.Stroke;
            }
        }

        /// <summary>
        /// OnStrokeThickness property changed  method reflect when change the StrokeThickness.
        /// </summary>
        /// <param name="bindable">The data binding object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnStrokeThicknessPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView && avatarView.Border != null)
            {
                avatarView.Border.StrokeThickness = avatarView.StrokeThickness;
            }
        }


        /// <summary>
        /// OnFontSize properties changed method reflect when change the font.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnFontSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.ApplyAvatarStyleSetting();
                if (avatarView.GroupView != null)
                {
                    avatarView.GroupView.SetInitialsFontAttributeValues(avatarView.InitialsLabel);
                }
            }
        }

        /// <summary>
        /// OnFontFamily properties changed method reflect when change the font family.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnFontFamilyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.InitialsLabel.FontFamily = avatarView.FontFamily;

                if (avatarView.GroupView != null)
                {
                    avatarView.GroupView.SetInitialsFontFamily(avatarView.FontFamily, avatarView.FontIconFontFamily);
                }
            }
        }

        /// <summary>
        /// OnFontAttributes properties changed method reflect when change the font family.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnFontAttributesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.InitialsLabel.FontAttributes = avatarView.FontAttributes;
                if (avatarView.GroupView != null)
                {
                    avatarView.GroupView.SetInitialsFontAttributeValues(avatarView.InitialsLabel);
                }
            }
        }

        /// <summary>
        /// Set the image when using the custom view.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.InitializeCustomImage();
                if (avatarView.CustomImage != null)
                {
                    avatarView.CustomImage.Source = avatarView.ImageSource;
                }

                avatarView.DisplayCurrentAvatarElement();
            }
        }

        /// <summary>
        /// Content properties changed method reflect when change the content type.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnContentTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.DisplayCurrentAvatarElement();
            }
        }

        /// <summary>
        /// OnInitialsType property changed method reflect when change the initials type.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnInitialsTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.SetInitialsBasedOnInitialsType(avatarView.AvatarName, avatarView.InitialsLabel);
            }
        }


        /// <summary>
        /// OnName property changed method reflect when changing the name property.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.SetInitialsBasedOnInitialsType(avatarView.AvatarName, avatarView.InitialsLabel);
                avatarView.DisplayCurrentAvatarElement();
            }
        }

        /// <summary>
        /// OnAvatarStylePropertyChanged method reflect when changing the AvatarStyle property.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnAvatarStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.ApplyAvatarStyleSetting();
            }
        }

        /// <summary>
        /// OnHeightRequestPropertyChanged method reflect when changing height property.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnHeightRequestPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.ApplyAvatarStyleSetting();
            }
        }

        /// <summary>
        /// OnColorMode PropertyChanged method reflect when changing colormode property.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnColorModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.SetColors();
            }
        }

        /// <summary>
        /// OnBackgroundColorPropertyChanged method reflect when changing background color property.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnBackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.SetColors();
            }
        }

        /// <summary>
        /// OnBackgroundPropertyChanged method reflect when changing background property.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.SetColors();
            }
        }

        /// <summary>
        /// OnCornerRadiusPropertyChanged method reflect when changing corner radius property.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.ApplyAvatarStyleSetting();
            }
        }

        /// <summary>
        /// OnWidthRequestPropertyChanged method reflect when changing width request property.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnWidthRequestPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.ApplyAvatarStyleSetting();
            }
        }

        /// <summary>
        /// OnGroupSourcePropertyChanged method reflect when changing GroupSource property.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnGroupSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.InitializeGroupView();
                avatarView.HookCollectionChangedEvent();
                avatarView.DisplayCurrentAvatarElement();
                avatarView.UpdateGroupViewValues();
            }
        }

        /// <summary>
        /// OnInitialsMemberPathPropertyChanged method reflect when changing InitialsMemberPath property.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnInitialsMemberPathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.UpdateGroupViewValues();
            }
        }

        /// <summary>
        /// OnImageSourceMemberPathPropertyChanged method reflect when changing ImageSourceMemberPathProperty.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnImageSourceMemberPathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.UpdateGroupViewValues();
            }
        }

        /// <summary>
        /// OnBackgroundColorMemberPathPropertyChanged method reflect when changing BackgroundColorMemberPathProperty.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnBackgroundColorMemberPathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.UpdateGroupViewValues();
            }
        }

        /// <summary>
        /// OnInitialsColorMemberPathPropertyChanged method reflect when changing InitialsColorMemberPathProperty.
        /// </summary>
        /// <param name="bindable">the data binding object.</param>
        /// <param name="oldValue">the old value.</param>
        /// <param name="newValue">the new value.</param>
        private static void OnInitialsColorMemberPathPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfAvatarView avatarView)
            {
                avatarView.UpdateGroupViewValues();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method performs initialization for all the grids, images, and views added in layout grid and set base as layout grid.
        /// </summary>
        private void InitializeElements()
        {
            this.InitialsLabel.HorizontalTextAlignment = TextAlignment.Center;
            this.InitialsLabel.VerticalTextAlignment = TextAlignment.Center;
            this.InitialsLabel.HorizontalOptions = LayoutOptions.Fill;
            this.InitialsLabel.VerticalOptions = LayoutOptions.Fill;
            this.InitialsLabel.FontSize = this.FontSize;
            this.InitialsLabel.FontAttributes = this.FontAttributes;
            this.InitialsLabel.FontFamily = this.FontFamily;
            this.InitialsLabel.TextColor = this.InitialsColor;

            this.LayoutGrid = new Grid()
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
            };

            this.LayoutGrid.Children.Add(this.InitialsLabel);
            
            this.Border = new SfBorder()
            {
                Content = this.LayoutGrid,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Stroke = this.Stroke,
                StrokeThickness = this.StrokeThickness,
            };
	    
            this.Add(Border);

            base.Background = this.Background;
            base.BackgroundColor = this.BackgroundColor;
            base.HeightRequest = this.HeightRequest;
            base.WidthRequest = this.WidthRequest;
            this.UpdateSizeofParent(base.WidthRequest, base.HeightRequest);
        }

        /// <summary>
        /// Initialize group view method.
        /// </summary>
        private void InitializeGroupView()
        {
            if (this.GroupView == null)
            {
                this.GroupView = new AvatarGroupView();
                this.GroupView.SetInitialsFontAttributeValues(this.InitialsLabel);
                this.GroupView.SetInitialsFontFamily(this.FontFamily, this.FontIconFontFamily);
                this.GroupView.SetColors(this.AvatarColorMode);
                this.LayoutGrid?.Children.Insert(this.LayoutGrid.Children.IndexOf(this.InitialsLabel), this.GroupView);
                this.UpdateSizeofParent(base.WidthRequest, base.HeightRequest);
            }
        }

        /// <summary>
        /// Initialize custom image method.
        /// </summary>
        private void InitializeCustomImage()
        {
            if (this.CustomImage == null)
            {
                this.CustomImage = new Image
                {
                    Aspect = Aspect.AspectFill
                };
                this.LayoutGrid?.Children.Insert(this.LayoutGrid.Children.IndexOf(this.InitialsLabel), this.CustomImage);
                this.UpdateSizeofParent(base.WidthRequest, base.HeightRequest);
            }
        }

        private void InitializeAvatarImage()
        {
            if (this.AvatarImage == null)
            {
                this.AvatarImage = new Image
                {
                    Aspect = Aspect.AspectFill
                };
                this.LayoutGrid?.Children.Insert(this.LayoutGrid.Children.IndexOf(this.InitialsLabel), this.AvatarImage);
                this.UpdateSizeofParent(base.WidthRequest, base.HeightRequest);
            }
        }

        private void UpdateSizeofParent(double width, double height)
        {
            if (width > 0 && height > 0)
            {
                var thickness = this.StrokeThickness * 2;
                this.InitialsLabel.WidthRequest = width - thickness;
                this.InitialsLabel.HeightRequest = height - thickness;

                if (this.GroupView != null)
                {
                    this.GroupView.WidthRequest = width - thickness;
                    this.GroupView.HeightRequest = height - thickness;
                }

                if (this.CustomImage != null)
                {
                    this.CustomImage.WidthRequest = width - thickness;
                    this.CustomImage.HeightRequest = height - thickness;
                }

                if (this.AvatarImage != null)
                {
                    this.AvatarImage.WidthRequest = width - thickness;
                    this.AvatarImage.HeightRequest = height - thickness;
                }
            }
        }

        /// <summary>
        /// Initially, set IsVisible to false for all the views and set true based on the avatar type for the corresponding views.
        /// </summary>
        private void DisplayCurrentAvatarElement()
        {
            this.InitialsLabel.AvatarContentType = this.ContentType.ToString();
            this.CollaspeAllAvatarView();

            if (this.ContentType == ContentType.Initials)
            {
                this.InitialsLabel.IsVisible = true;
            }
            else if (this.ContentType == ContentType.Custom)
            {
                if (this.CustomImage != null)
                {
                    this.CustomImage.IsVisible = true;
                }
            }
            else if (this.ContentType == ContentType.AvatarCharacter)
            {
                this.SetAvatarCharacter(this.AvatarCharacter);
            }
            else if (ContentType == ContentType.Group)
            {
                if (this.GroupView != null)
                {
                    this.GroupView.IsVisible = true;
                }
            }
            else
            {
                this.SetAvatarCharacter(this.AvatarCharacter);
                var avatarCharacterTypeAvatarCollection = Enum.GetValues(typeof(AvatarCharacter));

                if (this.GroupSource != null)
                {
                    this.CollaspeAllAvatarView();
                    if (this.GroupView != null)
                    {
                        this.GroupView.IsVisible = true;
                    }
                }
                else if (this.ImageSource != null && !string.IsNullOrEmpty(this.ImageSource.ToString()))
                {
                    this.CollaspeAllAvatarView();
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        foreach (var item in avatarCharacterTypeAvatarCollection)
                        {
                            var items = "File: " + item + ".png";
                            if (items.ToString(CultureInfo.InvariantCulture) == this.ImageSource.ToString() && this.CustomImage != null)
                            {
                                this.CustomImage.Source = ImageSource.FromResource((this.imageLocation + item + AvatarViewStaticText.AvatarCharacterFileTypeText).ToString(CultureInfo.InvariantCulture), typeof(SfAvatarView));
                            }
                        }
                    }

                    if (this.CustomImage != null)
                    {
                        this.CustomImage.IsVisible = true;
                    }
                }
                else
                {
                    this.CollaspeAllAvatarView();
                    this.UpdateAvatarImageVisibility();
                }
            }
        }

        /// <summary>
        /// Method which sets all the view and layout as false state.
        /// </summary>
        private void CollaspeAllAvatarView()
        {
            if (this.GroupView != null)
            {
                this.GroupView.IsVisible = false;
            }

            if (this.CustomImage != null)
            {
                this.CustomImage.IsVisible = false;
            }

            if (this.AvatarImage != null)
            {
                this.AvatarImage.IsVisible = false;
            }

            this.InitialsLabel.IsVisible = false;
        }

        /// <summary>
        /// Method for setting the avatar image source for the avatar character  type.
        /// </summary>
        /// <param name="avatarCharacterType">Avatar character type.</param>
        private void SetAvatarCharacter(AvatarCharacter avatarCharacterType)
        {
            this.InitializeAvatarImage();
            if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
            {
                if (this.AvatarImage != null)
                {
                    this.AvatarImage.Source = ImageSource.FromResource((this.imageLocation + avatarCharacterType + AvatarViewStaticText.AvatarCharacterFileTypeText).ToString(CultureInfo.InvariantCulture), typeof(SfAvatarView));

                }
            }

            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                if (this.AvatarImage != null)
                {
                    this.AvatarImage.Source = ImageSource.FromResource((this.imageLocation + avatarCharacterType + AvatarViewStaticText.AvatarCharacterFileTypeText).ToString(CultureInfo.InvariantCulture), typeof(SfAvatarView));
                }

            }

            this.UpdateAvatarImageVisibility();
        }

        /// <summary>
        /// Update the image visibility based in content type
        /// </summary>
        private void UpdateAvatarImageVisibility()
        {
            if (this.ContentType == ContentType.Default && this.AvatarImage != null && this.InitialsLabel != null && !string.IsNullOrEmpty(this.AvatarName))
            {
                this.AvatarImage.IsVisible = false;
                this.InitialsLabel.IsVisible = true;
            }
            else if (this.AvatarImage != null)
            {
                this.AvatarImage.IsVisible = true;
            }
        }

        /// <summary>
        /// Method performed for the group view and set for all the group source value in the corresponding primary, secondary, and tertiary grids.
        /// </summary>
        private void UpdateGroupViewValues()
        {
            if (this.GroupSource != null)
            {
                this.isGroupSourceEmpty = true;
                int initialCount = 0;

                foreach (var item in this.GroupSource)
                {
                    this.isGroupSourceEmpty = false;

                    if (this.GroupView != null)
                    {
                        if (initialCount == 0)
                        {
                            this.SetGroupElementValue(this.GroupView.PrimaryInitialsLabel, this.GroupView.PrimaryGrid, this.GroupView.PrimaryImage, this.GetPropertyValue(this.InitialsMemberPath, item), this.GetPropertyValue(this.ImageSourceMemberPath, item), this.GetPropertyValue(this.BackgroundColorMemberPath, item), this.GetPropertyValue(this.InitialsColorMemberPath, item));
                        }
                        else if (initialCount == 1)
                        {
                            this.GroupView.ArrageElementsSpacing(true);
                            this.SetGroupElementValue(this.GroupView.SecondaryInitialsLabel, this.GroupView.SecondaryGrid, this.GroupView.SecondaryImage, this.GetPropertyValue(this.InitialsMemberPath, item), this.GetPropertyValue(this.ImageSourceMemberPath, item), this.GetPropertyValue(this.BackgroundColorMemberPath, item), this.GetPropertyValue(this.InitialsColorMemberPath, item));
                        }
                        else if (initialCount == 2)
                        {
                            this.GroupView.ArrageElementsSpacing(true, true);
                            this.SetGroupElementValue(this.GroupView.TertiaryInitialsLabel, this.GroupView.TertiaryGrid, this.GroupView.TertiaryImage, this.GetPropertyValue(this.InitialsMemberPath, item), this.GetPropertyValue(this.ImageSourceMemberPath, item), this.GetPropertyValue(this.BackgroundColorMemberPath, item), this.GetPropertyValue(this.InitialsColorMemberPath, item));
                        }
                    }

                    if (++initialCount > 2)
                    {
                        break;
                    }
                }

                if (this.GroupView != null)
                {
                    if (this.isGroupSourceEmpty)
                    {
                        this.GroupView.ArrageElementsSpacing(false, false);
                        this.GroupView.PrimaryImage.Source = string.Empty;

                    }
                    else
                    {
                        this.GroupView.SetInitialsFontFamily(this.FontFamily, this.FontIconFontFamily);
                    }
                }
            }
        }

        /// <summary>
        /// Method used for the group view and set the text, image, and color added in the group view.
        /// </summary>
        /// <param name="groupLabel">Group label as label type.</param>
        /// <param name="groupGrid">Group Grid as grid type.</param>
        /// <param name="groupImage">Group image as image Type.</param>
        /// <param name="initialsValue">Initial value as object type.</param>
        /// <param name="imageValue">Image value as object type.</param>
        /// <param name="backgroundColorValue">Color as object type.</param>
        /// <param name="textColorValue">Text color value as object type.</param>
        private void SetGroupElementValue(Label groupLabel, Grid groupGrid, Image groupImage, object? initialsValue, object? imageValue, object? backgroundColorValue, object? textColorValue)
        {
            if (initialsValue != null)
            {
                groupImage.Source = string.Empty;
                this.SetInitialsBasedOnInitialsType(initialsValue.ToString(), groupLabel);
            }

            if (imageValue != null)
            {
                var avatarCharacterTypeAvatarCollection = Enum.GetValues(typeof(AvatarCharacter));

                bool isCustomImage = true;

                foreach (var item in avatarCharacterTypeAvatarCollection)
                {
                    if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        var items = item.ToString() + ".png";

                        if (items == imageValue.ToString())
                        {
                            groupLabel.Text = string.Empty;
                            groupImage.Source = ImageSource.FromResource((this.imageLocation + item + AvatarViewStaticText.AvatarCharacterFileTypeText).ToString(CultureInfo.InvariantCulture), typeof(SfAvatarView));
                            isCustomImage = false;

                        }
                    }
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        var items = item.ToString() + ".png";
                        if (items == imageValue.ToString())
                        {
                            groupLabel.Text = string.Empty;
                            groupImage.Source = ImageSource.FromResource((this.imageLocation + item + AvatarViewStaticText.AvatarCharacterFileTypeText).ToString(CultureInfo.InvariantCulture), typeof(SfAvatarView));
                            isCustomImage = false;

                        }
                    }
                }

                if (isCustomImage)
                {
                    if (imageValue is ImageSource)
                    {
                        groupImage.Source = imageValue as ImageSource;
                    }
                    else
                    {
                        groupImage.Source = imageValue.ToString();

                    }
                }
            }

            if (this.AvatarColorMode == AvatarColorMode.Default)
            {
                if (backgroundColorValue != null && backgroundColorValue is Color)
                {
                    groupGrid.BackgroundColor = (Color)backgroundColorValue;
                }

                if (textColorValue != null && textColorValue is Color)
                {
                    groupLabel.TextColor = (Color)textColorValue;
                }
            }

            groupLabel.FontFamily = this.FontFamily;
        }


        /// <summary>
        /// Method used for performing two-way binding for all the APIs.
        /// </summary>
        /// <param name="propertyName">String type.</param>
        /// <param name="item">object type.</param>
        /// <returns>Returns a value.</returns>
        private object? GetPropertyValue(string propertyName, object item)
        {
            if (string.IsNullOrEmpty(propertyName) || item == null)
            {
                return null;
            }

            object? propertyValue = null;
            PropertyInfo? propertyInfo = item.GetType().GetProperty(propertyName);

            if (propertyInfo != null)
            {
                propertyValue = propertyInfo.GetValue(item);
            }

            return propertyValue;
        }

        /// <summary>
        /// Method hooks and unhooks the collection change event for the group source collections.
        /// </summary>
        private void HookCollectionChangedEvent()
        {
            if (this.GroupSource is INotifyCollectionChanged groupSource)
            {
                groupSource.CollectionChanged -= this.SfAvatarView_CollectionChanged;
                groupSource.CollectionChanged += this.SfAvatarView_CollectionChanged;
            }
        }

        /// <summary>
        /// Set Ccolor is used for setting the color based on the ColorMode.
        /// </summary>
        private void SetColors()
        {
            if (AvatarViewColorTable.AutomaticColors == null)
            {
                AvatarViewColorTable.GenerateAutomaticBackgroundColors();
            }

            if (this.AvatarColorMode != AvatarColorMode.Default && Border != null)
            {
                if (this.AvatarColorMode == AvatarColorMode.DarkBackground)
                {
                    if (AvatarViewColorTable.AutomaticColors != null )
                    {
                        Border.BackgroundColor = AvatarViewColorTable.AutomaticColors[AvatarViewColorTable.CurrentBackgroundColorIndex].DarkColor;
                    }

                    this.InitialsLabel.TextColor = AvatarViewColorTable.InitialsLightColor;
                }
                else if (this.AvatarColorMode == AvatarColorMode.LightBackground)
                {
                    if (AvatarViewColorTable.AutomaticColors != null)
                    {
                        Border.BackgroundColor = AvatarViewColorTable.AutomaticColors[AvatarViewColorTable.CurrentBackgroundColorIndex].LightColor;
                    }

                    this.InitialsLabel.TextColor = AvatarViewColorTable.InitialsDarkColor;
                }

                if (AvatarViewColorTable.AutomaticColors != null && ++AvatarViewColorTable.CurrentBackgroundColorIndex >= AvatarViewColorTable.AutomaticColors.Count)
                {
                    AvatarViewColorTable.CurrentBackgroundColorIndex = 0;
                }
            }
            else if(Border != null)
            {
                if (Background != null)
                    Border.Background = this.Background;
                else
                {
                    Border.BackgroundColor = this.BackgroundColor;
                }
                this.InitialsLabel.TextColor = this.InitialsColor;
            }

            if (this.GroupView != null)
            {
                this.GroupView.SetColors(this.AvatarColorMode);
            }
        }

        /// <summary>
        /// Method performs the default AvatarStyle  type and set sizing for the avatar view.
        /// </summary>
        private void ApplyAvatarStyleSetting()
        {
            if (this.AvatarShape == AvatarShape.Custom)
            {
                this.SetAvatarSizing(this.WidthRequest, this.HeightRequest, this.CornerRadius, this.FontSize);
            }
            else
            {
                this.ApplyConstantAvatarStyleSetting();
            }
        }

        /// <summary>
        /// Method used for calling the avatar sizing based on the avatar stlyes types.
        /// </summary>
        private void ApplyConstantAvatarStyleSetting()
        {
            if (this.AvatarSize == AvatarSize.ExtraLarge && this.AvatarShape == AvatarShape.Circle)
            {
                this.SetAvatarSizing(AvatarViewSizeTable.ExtraLargeSize, AvatarViewSizeTable.ExtraLargeFontSize);

            }
            else if (this.AvatarSize == AvatarSize.Large && this.AvatarShape == AvatarShape.Circle)
            {
                this.SetAvatarSizing(AvatarViewSizeTable.LargeSize, AvatarViewSizeTable.LargeFontSize);

            }
            else if (this.AvatarSize == AvatarSize.Medium && this.AvatarShape == AvatarShape.Circle)
            {
                this.SetAvatarSizing(AvatarViewSizeTable.MediumSize, AvatarViewSizeTable.MediumFontSize);

            }
            else if (this.AvatarSize == AvatarSize.Small && this.AvatarShape == AvatarShape.Circle)
            {
                this.SetAvatarSizing(AvatarViewSizeTable.SmallSize, AvatarViewSizeTable.SmallFontSize);

            }
            else if (this.AvatarSize == AvatarSize.ExtraSmall && this.AvatarShape == AvatarShape.Circle)
            {
                this.SetAvatarSizing(AvatarViewSizeTable.ExtraSmallSize, AvatarViewSizeTable.ExtraSmallFontSize);

            }
            else if (this.AvatarSize == AvatarSize.ExtraLarge && this.AvatarShape == AvatarShape.Square)
            {
                this.SetAvatarSizing(AvatarViewSizeTable.ExtraLargeSize, AvatarViewSizeTable.ExtraLargeFontSize, false);
            }
            else if (this.AvatarSize == AvatarSize.Large && this.AvatarShape == AvatarShape.Square)
            {
                this.SetAvatarSizing(AvatarViewSizeTable.LargeSize, AvatarViewSizeTable.LargeFontSize, false);
            }
            else if (this.AvatarSize == AvatarSize.Medium && this.AvatarShape == AvatarShape.Square)
            {
                this.SetAvatarSizing(AvatarViewSizeTable.MediumSize, AvatarViewSizeTable.MediumFontSize, false);
            }
            else if (this.AvatarSize == AvatarSize.Small && this.AvatarShape == AvatarShape.Square)
            {
                this.SetAvatarSizing(AvatarViewSizeTable.SmallSize, AvatarViewSizeTable.SmallFontSize, false);
            }
            else if (this.AvatarSize == AvatarSize.ExtraSmall && this.AvatarShape == AvatarShape.Square)
            {
                this.SetAvatarSizing(AvatarViewSizeTable.ExtraSmallSize, AvatarViewSizeTable.ExtraSmallFontSize, false);
            }
        }

        /// <summary>
        /// Method calls from the VisualSetting and adds the corner radius based on the IsCircleType property.
        /// </summary>
        /// <param name="avatarSizeRequest">double type.</param>
        /// <param name="initialFontSize">Inital as double type.</param>
        /// <param name="isCircleType">boolean type.</param>
        private void SetAvatarSizing(double avatarSizeRequest, double initialFontSize, bool isCircleType = true)
        {
            CornerRadius avatarCornerRadius;
            if (isCircleType)
            {
                avatarCornerRadius = avatarSizeRequest / 2;
            }
            else
            {
                avatarCornerRadius = AvatarViewSizeTable.SquareAvatarStyleCornerRadius;
            }

            this.SetAvatarSizing(avatarSizeRequest, avatarSizeRequest, avatarCornerRadius, initialFontSize);
        }

        /// <summary>
        /// Method for setting the height, width and Corner Radius for the main view.
        /// </summary>
        /// <param name="avatarWidthRequest">Width value.</param>
        /// <param name="avatarHeightRequest">Height value.</param>
        /// <param name="avatarCornerRadius">Avatar Corner radius value.</param>
        /// <param name="initialFontSize">Inital Font Size value.</param>
        private void SetAvatarSizing(double avatarWidthRequest, double avatarHeightRequest, CornerRadius avatarCornerRadius, double initialFontSize)
        {
            base.HeightRequest = avatarHeightRequest;
            base.WidthRequest = avatarWidthRequest;
            this.UpdateSizeofParent(avatarHeightRequest, avatarHeightRequest);
            //Todo : https://github.com/dotnet/maui/issues/6805 in Android

            if (this.Border != null)
            {
                this.Border.StrokeShape = new RoundRectangle { CornerRadius = avatarCornerRadius };
            }

            if (this.InitialsLabel != null)
            {
                this.InitialsLabel.FontSize = initialFontSize;
            }

            if (this.GroupView != null && this.InitialsLabel != null)
            {
                this.GroupView.SetInitialsFontAttributeValues(this.InitialsLabel);
            }
        }

        /// <summary>
        /// Method used for the adding the text in the element label for Iinitials avatar type and setting the text for both Single and Ddouble characters for InitialsType.
        /// </summary>
        /// <param name="initialsValue">string type.</param>
        /// <param name="elementLabel">label type.</param>
        private void SetInitialsBasedOnInitialsType(string? initialsValue, Label elementLabel)
        {
            if (initialsValue == null)
            {
                return;
            }

            if (initialsValue.Length <= 0)
            {
                return;
            }
            else
            {
                elementLabel.FontFamily = this.FontFamily;
            }

            if (this.InitialsType == InitialsType.SingleCharacter)
            {
                foreach (var item in initialsValue)
                {
                    if (item.ToString(CultureInfo.InvariantCulture) != AvatarViewStaticText.SpaceText)
                    {
                        elementLabel.Text = item.ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture);
                        return;
                    }
                }

            }
            else
            {
                elementLabel.Text = this.GetValidatedInitials(initialsValue, elementLabel).ToUpper(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Method for the double character type, which separates the word and makes the two character.
        /// </summary>
        /// <param name="initialsValue">get the inital value.</param>
        /// <param name="labelElement">label type.</param>
        /// <returns>Returns a value.</returns>
        private string GetValidatedInitials(string initialsValue, Label labelElement)
        {
            if (initialsValue.Contains(AvatarViewStaticText.SpaceText))
            {
                var wordCollection = initialsValue.Split(' ');

                if (wordCollection.Length > 1)
                {
                    string validatedInitial = string.Empty;
                    string firstWord = string.Empty;

                    foreach (var item in wordCollection)
                    {
                        if (item.Length > 0)
                        {
                            if (validatedInitial.Length == 0)
                            {
                                validatedInitial = item[0].ToString(new NumberFormatInfo());
                            }
                            else if (validatedInitial.Length == 1)
                            {
                                validatedInitial += item[0].ToString(new NumberFormatInfo());
                            }
                            else
                            {
                                validatedInitial = firstWord[0].ToString(new NumberFormatInfo());
                                validatedInitial += item[0].ToString(new NumberFormatInfo());
                            }

                            if (string.IsNullOrEmpty(firstWord))
                            {
                                firstWord = item;
                            }
                        }
                    }

                    if (validatedInitial.Length == 2)
                    {
                        return validatedInitial;
                    }

                    return this.GetSingleWordInitial(firstWord, labelElement);
                }
                else if (wordCollection.Length == 1)
                {
                    return this.GetSingleWordInitial(wordCollection[0], labelElement);
                }
            }

            return this.GetSingleWordInitial(initialsValue, labelElement);
        }

        /// <summary>
        /// Method which performs calculation for the double character type and add returns the word.
        /// </summary>
        /// <param name="word">string type.</param>
        /// <param name="labelElement">The Label Element.</param>
        /// <returns>Returns a value.</returns>
        private string GetSingleWordInitial(string word, Label labelElement)
        {

            if (word.Length <= 1)
            {
                return word + word;
            }

            return word[0].ToString(CultureInfo.InvariantCulture) + word[word.Length - 1].ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Method calls the updateGroupview values used only for the group type.
        /// </summary>
        /// <param name="sender">Object type.</param>
        /// <param name="e">Collection changed type.</param>
        private void SfAvatarView_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            this.UpdateGroupViewValues();
        }

        #endregion
    }

}
