// <copyright file="BadgeSettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Platform;
    using Microsoft.Maui.Controls.Shapes;
    using Microsoft.Maui.Devices;
    using Microsoft.Maui.Graphics;
    using Syncfusion.Maui.Graphics.Internals;
    using Font = Microsoft.Maui.Font;

    /// <summary>
    /// Represents the badge settings class.
    /// </summary>
    public class BadgeSettings : Element, ITextElement
    {
        #region Bindable properties

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
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BadgeSettings), Colors.White, BindingMode.Default, null, OnTextColorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Background"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Background"/> bindable property.
        /// </value>
        public static readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(nameof(Background), typeof(Brush), typeof(BadgeSettings), new SolidColorBrush(Colors.Transparent), BindingMode.Default, null, OnBackgroundPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StrokeThickness"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeThicknessProperty =
            BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(BadgeSettings), 0d, BindingMode.Default, null, OnStrokeThicknessPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CornerRadius"/> bindable property.
        /// </value>
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(BadgeSettings), new CornerRadius(25), BindingMode.Default, null, null, OnCornerRadiusPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Stroke"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(BadgeSettings), Colors.Transparent, BindingMode.Default, null, OnStrokePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Position"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Position"/> bindable property.
        /// </value>
        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position), typeof(BadgePosition), typeof(BadgeSettings), BadgePosition.TopRight, BindingMode.Default, null, null, OnBadgePositionPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Offset"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Offset"/> bindable property.
        /// </value>
        public static readonly BindableProperty OffsetProperty =
            BindableProperty.Create(nameof(Offset), typeof(Point), typeof(BadgeSettings), new Point(0, 0), BindingMode.Default, null, null, OnOffsetPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Animation"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Animation"/> bindable property.
        /// </value>
        public static readonly BindableProperty AnimationProperty =
            BindableProperty.Create(nameof(Animation), typeof(BadgeAnimation), typeof(BadgeSettings), BadgeAnimation.Scale, BindingMode.Default, null, null, OnBadgeAnimationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Type"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Type"/> bindable property.
        /// </value>
        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create(nameof(Type), typeof(BadgeType), typeof(BadgeSettings), BadgeType.Primary, BindingMode.Default, null, null, OnBadgeTypePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TextPadding"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextPadding"/> bindable property.
        /// </value>
        public static readonly BindableProperty TextPaddingProperty =
            BindableProperty.Create(nameof(TextPadding), typeof(Thickness), typeof(BadgeSettings), GetTextPadding(), BindingMode.Default, null, null, OnTextPaddingPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AutoHide"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AutoHide"/> bindable property.
        /// </value>
        public static readonly BindableProperty AutoHideProperty =
            BindableProperty.Create(nameof(AutoHide), typeof(bool), typeof(BadgeSettings), false, BindingMode.Default, null, null, OnAutoHidePropertyChanged);

        /// <summary>        
        /// Identifies the <see cref="BadgeAlignment"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BadgeAlignment"/> bindable property.
        /// </value>
        public static readonly BindableProperty BadgeAlignmentProperty =
            BindableProperty.Create(nameof(BadgeAlignment), typeof(BadgeAlignment), typeof(BadgeSettings), BadgeAlignment.Center, BindingMode.Default, null, null, OnBadgeAlignmentPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Icon"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Icon"/> bindable property.
        /// </value>
        // TODO: Need to fix the font icon issue in library or need to draw the icons.
        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(nameof(Icon), typeof(BadgeIcon), typeof(BadgeSettings), BadgeIcon.None, BindingMode.Default, null, null, OnBadgeIconPropertyChanged);

        #endregion

        #region Fields

        /// <summary>
        /// Specifies the badge view.
        /// </summary>
        private SfBadgeView? badgeView;

        #endregion

        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the <see cref="BadgeSettings" /> class.
        /// </summary>
        public BadgeSettings()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background for the badge. This property will be set only if BadgeType is set to BadgeType.None.
        /// </summary>
        /// <value>
        /// Specifies the background color of the badge. The default value is Colors.Transparent.
        /// </value>
        /// <example>
        /// The following example shows how to apply the background color for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Background="Green"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     Background = Colors.Green     
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public Brush Background
        {
            get { return (Brush)this.GetValue(BackgroundProperty); }
            set { this.SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke thickness for the badge. The stroke thickness will be visible only if the stroke is not set to transparent.
        /// </summary>
        /// <value>
        /// Specifies the stroke thickness of the badge. The default value is 0d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the stroke thickness for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings StrokeThickness="2"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     StrokeThickness = 2     
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public double StrokeThickness
        {
            get { return (double)this.GetValue(StrokeThicknessProperty); }
            set { this.SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the corner radius of the badge.
        /// </summary>
        /// <value>
        /// Specifies the corner radius of the badge. The default value is CornerRadius(25).
        /// </value>
        /// <example>
        /// The following example shows how to apply the corner radius for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings CornerRadius="5"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     CornerRadius = new CornerRadius(5, 5, 5, 5)   
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)this.GetValue(CornerRadiusProperty); }
            set { this.SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke for the badge.
        /// </summary>
        /// <value>
        /// Specifies the stroke of the badge. The default value is Colors.Transparent.
        /// </value>
        /// <example>
        /// The following example shows how to apply the stroke for a badge.
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Stroke="Orange"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     Stroke = Colors.Orange   
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public Color Stroke
        {
            get { return (Color)this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the position where the badge will be shown relative to the Content.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Core.BadgePosition"/> enumeration that specifies the position of the badge relative to the content. The default value is <see cref="Syncfusion.Maui.Core.BadgePosition.TopRight"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the position for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Position="BottomRight"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     Position = BadgePosition.BottomRight
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public BadgePosition Position
        {
            get { return (BadgePosition)this.GetValue(PositionProperty); }
            set { this.SetValue(PositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for offset.
        /// </summary>
        /// <value>
        /// Specifies the offset of the badge. The default value is Point(2, 2).
        /// </value>
        /// <example>
        /// The following example shows how to apply the offset for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Offset="10,10"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     Offset = new Point(10, 10)
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public Point Offset
        {
            get { return (Point)this.GetValue(OffsetProperty); }
            set { this.SetValue(OffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the animation that should be used when the badge is shown. 
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Core.BadgeAnimation"/> enumeration that specifies the animation of the badge when the badge is shown. The default value is <see cref="Syncfusion.Maui.Core.BadgeAnimation.Scale"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the animation for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Animation="None"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     Animation = BadgeAnimation.None
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public BadgeAnimation Animation
        {
            get { return (BadgeAnimation)this.GetValue(AnimationProperty); }
            set { this.SetValue(AnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the type of the badge. BadgeSettings.BackgroundColor is not applied when this property set other than BadgeIcon.None.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Core.BadgeType"/> enumeration that specifies the type of the badge. The default value is <see cref="Syncfusion.Maui.Core.BadgeType.Primary"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the type for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Type="Success"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     Type = BadgeType.Success
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public BadgeType Type
        {
            get { return (BadgeType)this.GetValue(TypeProperty); }
            set { this.SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the padding around the text of the badge.
        /// </summary>
        /// <value>
        /// Specifies the text padding around the text of the badge. The default value is Thickness(3) for UWP and Thickness(5) for other platforms.
        /// </value>
        /// <example>
        /// The following example shows how to apply the text padding for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings TextPadding="10"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     TextPadding = new Thickness(10, 10, 10, 10)
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public Thickness TextPadding
        {
            get { return (Thickness)this.GetValue(TextPaddingProperty); }
            set { this.SetValue(TextPaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the badge text is AutoHide or not.
        /// </summary>
        /// <value>
        /// Specifies the value for auto-hide of the badge when the value is zero or empty. The default value is false.
        /// </value>
        /// <example>
        /// The following example shows how to apply the auto hide for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings AutoHide="True"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     AutoHide = true
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public bool AutoHide
        {
            get { return (bool)this.GetValue(AutoHideProperty); }
            set { this.SetValue(AutoHideProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for badge alignment.
        /// </summary>
        /// <value>
        /// Specifies the value for alignment of the badge. The default value is <see cref="Syncfusion.Maui.Core.BadgeAlignment.Center"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the alignment for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary" 
        ///                 WidthRequest="120"  
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings BadgeAlignment="Start"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     BadgeAlignment = BadgeAlignment.Start
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public BadgeAlignment BadgeAlignment
        {
            get { return (BadgeAlignment)GetValue(BadgeAlignmentProperty); }
            set { this.SetValue(BadgeAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size of the text to be displayed in badge over the control.
        /// </summary>
        /// <value>
        /// Specifies the font size of the badge text. The default value is -1d.
        /// </value>
        /// <example>
        /// The following example shows how to apply the font size for the badge text.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings FontSize="32"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     FontSize = 32
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
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
        /// Gets or sets the font attributes of the text to be displayed in badge over the control.
        /// </summary>
        /// <value>
        /// One of the <see cref="Microsoft.Maui.Controls.FontAttributes"/> enumeration that specifies the font attributes of badge text. The default mode is <see cref="Microsoft.Maui.Controls.FontAttributes.None"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the font attributes for the badge text.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings FontAttributes="Bold"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     FontAttributes = FontAttributes.Bold
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)this.GetValue(FontAttributesProperty); }
            set { this.SetValue(FontAttributesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font family of the text to be displayed in badge over the control. The property will be set only if BadgeIcon is set to BadgeIcon.None.
        /// </summary>
        /// <value>
        /// Specifies the font family of the badge text. The default value is an empty string.
        /// </value>
        /// <example>
        /// The following example shows how to apply the font family for the badge text.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings FontFamily="OpenSansRegular"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     FontFamily = "OpenSansRegular"
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public string FontFamily
        {
            get { return (string)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color of the text to be displayed in badge over the control.
        /// </summary>
        /// <value>
        /// Specifies the text color of the badge text. The default value is Colors.White.
        /// </value>
        /// <example>
        /// The following example shows how to apply the text color for the badge text.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings TextColor="Red"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     TextColor = Colors.Red
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        public Color TextColor
        {
            get { return (Color)this.GetValue(TextColorProperty); }
            set { this.SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets the font of the badge text.
        /// </summary>
        Font ITextElement.Font => (Font)this.GetValue(FontElement.FontProperty);

        /// <summary>
        /// Gets or sets the Icon to be displayed in the badge. BadgeText and BadgeSettings.FontFamily is not applied when this property is set other than BadgeIcon.None.
        /// </summary>
        /// <value>
        /// One of the <see cref="Syncfusion.Maui.Core.BadgeIcon"/> enumeration that specifies the icon of the badge. The default value is <see cref="Syncfusion.Maui.Core.BadgeIcon.None"/>.
        /// </value>
        /// <example>
        /// The following example shows how to apply the icon for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Icon="Add"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     Icon = BadgeIcon.Add
        /// };
        ///
        /// Button button = new Button();
        /// button.Text = "Primary";
        /// button.WidthRequest = 120;
        /// button.HeightRequest = 60;
        ///
        /// sfBadgeView.Content = button;
        /// ]]></code>
        /// ***
        /// </example>
        // TODO: Need to fix the font icon issue in library or need to draw the icons.
        public BadgeIcon Icon
        {
            get { return (BadgeIcon)this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for badge view.
        /// </summary>
        internal SfBadgeView? BadgeView
        {
            get
            {
                return this.badgeView;
            }

            set
            {
                this.badgeView = value;
                this.ApplySettingstoUpdatedBadgeView();
            }
        }

        #endregion

        #region Public methods

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
            this.UpdateFontAttributes(newValue);
        }

        /// <summary>
        /// Invoked when the <see cref="Font"/> property changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontChanged(Font oldValue, Font newValue)
        {
        }

        /// <summary>
        /// Invoked when the <see cref="FontFamilyProperty"/> changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {
            this.UpdateFontFamily(newValue);
        }

        /// <summary>
        /// Invoked when the <see cref="FontSizeProperty"/> changed.
        /// </summary>
        /// <param name="oldValue">oldValue.</param>
        /// <param name="newValue">newValue.</param>
        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {
            this.UpdateFontSize(newValue);
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// To update the badge offset.
        /// </summary>
        /// <param name="position">The badge position.</param>
        /// <param name="badgeView">The badge view.</param>
        /// <param name="offset">The offset.</param>
        internal static void UpdateOffset(BadgePosition position, SfBadgeView badgeView, Point offset)
        {
            if (badgeView != null && badgeView.BadgeLabelView != null)
            {
                if (position == BadgePosition.Top || position == BadgePosition.Left || position == BadgePosition.TopLeft)
                {
                    badgeView.BadgeLabelView.Margin = new Thickness(offset.X, offset.Y, 0, 0);
                }
                else if (position == BadgePosition.Right || position == BadgePosition.TopRight)
                {
                    badgeView.BadgeLabelView.Margin = new Thickness(0, offset.Y, -offset.X, 0);
                }
                else if (position == BadgePosition.Bottom || position == BadgePosition.BottomLeft)
                {
                    badgeView.BadgeLabelView.Margin = new Thickness(offset.X, 0, 0, -offset.Y);
                }
                else if (position == BadgePosition.BottomRight)
                {
                    badgeView.BadgeLabelView.Margin = new Thickness(0, 0, -offset.X, -offset.Y);
                }

                badgeView?.InvalidateDrawable();


            }
        }

        /// <summary>
        /// Method used to update the badge settings to the updated badge view.
        /// </summary>
        internal void ApplySettingstoUpdatedBadgeView()
        {
            if (this.BadgeView != null)
            {
                OnBackgroundPropertyChanged(this, null, this.Background);
                OnBadgeAnimationPropertyChanged(this, null, this.Animation);
                OnBadgeIconPropertyChanged(this, null, this.Icon);
                OnBadgePositionPropertyChanged(this, null, this.Position);
                OnBadgeTypePropertyChanged(this, null, this.Type);
                OnCornerRadiusPropertyChanged(this, null, this.CornerRadius);
                this.UpdateFontAttributes(this.FontAttributes);
                this.UpdateFontFamily(this.FontFamily);
                this.UpdateFontSize(this.FontSize);
                OnOffsetPropertyChanged(this, null, this.Offset);
                OnStrokePropertyChanged(this, null, this.Stroke);
                OnStrokeThicknessPropertyChanged(this, null, this.StrokeThickness);
                OnTextColorPropertyChanged(this, null, this.TextColor);
                OnTextPaddingPropertyChanged(this, null, this.TextPadding);
                OnAutoHidePropertyChanged(this, null, this.AutoHide);
                OnBadgeAlignmentPropertyChanged(this, null, this.BadgeAlignment);

                if (this.badgeView != null)
                {
                    this.badgeView.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// To get the color for badge view based on badge type.
        /// </summary>
        /// <param name="value">The badge type value.</param>
        /// <returns>It returns the background color of the badge.</returns>
        internal Brush GetBadgeBackground(BadgeType value)
        {
            Brush badgeColor = new SolidColorBrush(Colors.Transparent);

            switch (value)
            {
                case BadgeType.Error:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(220, 53, 69, 255));
                    }

                    break;

                case BadgeType.Dark:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(52, 58, 64, 255));
                    }

                    break;

                case BadgeType.Info:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(23, 162, 184, 255));
                    }

                    break;

                case BadgeType.Light:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(248, 249, 250, 255));
                    }

                    break;

                case BadgeType.Primary:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(0, 123, 255, 255));
                    }

                    break;

                case BadgeType.Secondary:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(108, 117, 125, 255));
                    }

                    break;

                case BadgeType.Success:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(40, 167, 69, 255));
                    }

                    break;

                case BadgeType.Warning:
                    {
                        badgeColor = new SolidColorBrush(Color.FromRgba(255, 193, 7, 255));
                    }

                    break;
                case BadgeType.None:
                    {
                        badgeColor = this.Background;
                    }

                    break;
            }

            return badgeColor;
        }

        /// <summary>
        /// To align the badge text based on the badge position.
        /// </summary>
        /// <param name="badgeView">The badge view.</param>
        /// <param name="position">The badge position.</param>
        internal void AlignBadgeText(SfBadgeView badgeView, BadgePosition position)
        {
            bool isRTL = ((badgeView as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft;
            if (badgeView.BadgeLabelView != null)
            {
                switch (position)
                {
                    case BadgePosition.Left:
                        badgeView.BadgeLabelView.XPosition = isRTL ? badgeView.Width : 0;
                        badgeView.BadgeLabelView.YPosition = badgeView.Height / 2;
                        break;
                    case BadgePosition.Right:
                        badgeView.BadgeLabelView.XPosition = isRTL ? 0 : badgeView.Width;
                        badgeView.BadgeLabelView.YPosition = badgeView.Height / 2;
                        break;
                    case BadgePosition.Top:
                        badgeView.BadgeLabelView.XPosition = badgeView.Width / 2;
                        badgeView.BadgeLabelView.YPosition = 0;
                        break;
                    case BadgePosition.Bottom:
                        badgeView.BadgeLabelView.XPosition = badgeView.Width / 2;
                        badgeView.BadgeLabelView.YPosition = badgeView.Height;
                        break;
                    case BadgePosition.TopLeft:
                        badgeView.BadgeLabelView.XPosition = isRTL ? badgeView.Width : 0;
                        badgeView.BadgeLabelView.YPosition = 0;
                        break;
                    case BadgePosition.TopRight:
                        badgeView.BadgeLabelView.XPosition = isRTL ? 0 : badgeView.Width;
                        badgeView.BadgeLabelView.YPosition = 0;
                        break;
                    case BadgePosition.BottomLeft:
                        badgeView.BadgeLabelView.XPosition = isRTL ? badgeView.Width : 0;
                        badgeView.BadgeLabelView.YPosition = badgeView.Height;
                        break;
                    case BadgePosition.BottomRight:
                        badgeView.BadgeLabelView.XPosition = isRTL ? 0 : badgeView.Width;
                        badgeView.BadgeLabelView.YPosition = badgeView.Height;
                        break;
                }
            }
        }

        /// <summary>
        /// To align the badge based on the badge position.
        /// </summary>
        /// <param name="badgeView">The badge view.</param>
        /// <param name="position">The badge position.</param>

        internal void AlignBadge(SfBadgeView badgeView, BadgePosition position)
        {
            AlignBadgeText(badgeView, position);
            badgeView.InvalidateDrawable();
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Invoked whenever the Parent of an element is set.
        /// </summary>
        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (this.Parent != null && this.Parent is SfBadgeView)
            {
                this.BadgeView = this.Parent as SfBadgeView;
            }
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Invoked when the <see cref="TextColorProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnTextColorPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.TextColor = (Color)newValue;
                settings.BadgeView.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="BackgroundProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBackgroundPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.BadgeBackground = (Brush)newValue;
                settings.BadgeView.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="StrokeProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnStrokePropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.Stroke = (Color)newValue;
                settings.BadgeView.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="StrokeThicknessProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnStrokeThicknessPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.StrokeThickness = (double)newValue;
                settings.BadgeView.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="TextPaddingProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnTextPaddingPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.TextPadding = (Thickness)newValue;
                settings.BadgeView.BadgeLabelView.CalculateBadgeBounds();
                settings.BadgeView.InvalidateDrawable();

            }
        }

        /// <summary>
        /// Invoked when the <see cref="OffsetProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnOffsetPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                var point = (Point)newValue;
                UpdateOffset(settings.Position, settings.BadgeView, point);
            }
        }

        /// <summary>
        /// Invoked when the <see cref="CornerRadiusProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnCornerRadiusPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.CornerRadius = (CornerRadius)newValue;
                settings.BadgeView.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="PositionProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgePositionPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                var position = (BadgePosition)newValue;
                settings.BadgeView.BadgeLabelView.BadgePosition = (BadgePosition)newValue;
                settings.AlignBadge(settings.BadgeView, position);
            }
        }

        /// <summary>
        /// Invoked when the <see cref="TypeProperty"/> is set for badge control.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgeTypePropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null && (BadgeType)newValue != BadgeType.None)
            {
                settings.BadgeView.BadgeLabelView.BadgeBackground = settings.GetBadgeBackground((BadgeType)newValue);
                settings.BadgeView.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="AnimationProperty"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgeAnimationPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                if ((BadgeAnimation)newValue == BadgeAnimation.Scale)
                {
                    settings.BadgeView.BadgeLabelView.AnimationEnabled = true;
                }
                else
                {
                    settings.BadgeView.BadgeLabelView.AnimationEnabled = false;
                }
            }
        }

        /// <summary>
        /// Invoked when the <see cref="IconProperty"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgeIconPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.BadgeIcon = (BadgeIcon)newValue;
                settings.BadgeView.BadgeLabelView.CalculateBadgeBounds();
                settings.BadgeView.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Invoked when the <see cref="AutoHideProperty"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnAutoHidePropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                settings.BadgeView.BadgeLabelView.AutoHide = (bool)newValue;
            }
        }

        /// <summary>
        /// Invoked when the <see cref="BadgeAlignmentProperty"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgeAlignmentPropertyChanged(BindableObject bindable, object? oldValue, object newValue)
        {
            if (bindable is BadgeSettings settings && settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                BadgeAlignment alignment = (BadgeAlignment)newValue;
                settings.UpdateBadgeAlignment(settings, alignment);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Get default value of text padding.
        /// </summary>
        /// <returns>returns default value for badge text padding.</returns>
        private static Thickness GetTextPadding()
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                return new Thickness(3);
            }
            else
            {
                return new Thickness(5);
            }
        }

        private void UpdateFontAttributes(FontAttributes newValue)
        {
            if (this.BadgeView != null && this.BadgeView.BadgeLabelView != null)
            {
                this.BadgeView.BadgeLabelView.FontAttributes = newValue;
                this.BadgeView.InvalidateDrawable();
                this.BadgeView.BadgeLabelView.CalculateBadgeBounds();
            }
        }

        private void UpdateFontFamily(string newValue)
        {
            if (this.BadgeView != null && this.BadgeView.BadgeLabelView != null)
            {
                this.BadgeView.BadgeLabelView.FontFamily = newValue;
                this.BadgeView.InvalidateDrawable();
                this.BadgeView.BadgeLabelView.CalculateBadgeBounds();
            }
        }

        private void UpdateFontSize(double newValue)
        {
            if (this.BadgeView != null && this.BadgeView.BadgeLabelView != null)
            {
                this.BadgeView.BadgeLabelView.FontSize = (float)newValue;
                this.BadgeView.InvalidateDrawable();
                this.BadgeView.BadgeLabelView.CalculateBadgeBounds();
            }
        }

        /// <summary>
        /// To update the badge alignment.
        /// </summary>
        /// <param name="settings">The badge settings.</param>
        /// <param name="alignment">The alignment of the badge.</param>
        internal void UpdateBadgeAlignment(BadgeSettings settings, BadgeAlignment alignment)
        {
            if (settings.BadgeView != null && settings.BadgeView.BadgeLabelView != null)
            {
                double controlWidth = settings.BadgeView.WidthRequest >= 0 ? settings.BadgeView.WidthRequest : 0;
                double controlHeight = settings.BadgeView.HeightRequest >= 0 ? settings.BadgeView.HeightRequest : 0;
                double badgeWidth = settings.BadgeView.BadgeLabelView.WidthRequest >= 0 ? settings.BadgeView.BadgeLabelView.WidthRequest : 0;
                double badgeHeight = settings.BadgeView.BadgeLabelView.HeightRequest >= 0 ? settings.BadgeView.BadgeLabelView.HeightRequest : 0;

                if (alignment == BadgeAlignment.Center)
                {
                    if (settings.BadgeView.Content?.WidthRequest < 0 && settings.BadgeView.Content?.HeightRequest < 0)
                    {
                        // To update the content size of the badge view according to the position. 
                        if (settings.Position == BadgePosition.Left || settings.Position == BadgePosition.Right)
                        {
                            UpdateContentSize(settings.BadgeView, badgeWidth / 2, 0);
                        }
                        else if (settings.Position == BadgePosition.Top || settings.Position == BadgePosition.Bottom)
                        {
                            UpdateContentSize(settings.BadgeView, 0, badgeHeight / 2);
                        }
                        else
                        {
                            UpdateContentSize(settings.BadgeView, badgeWidth / 2, badgeHeight / 2);
                        }
                    }

                    else if (settings.BadgeView.WidthRequest < 0 && settings.BadgeView.HeightRequest < 0 && settings.BadgeView.Content != null)
                    {
                        // To update the badge view control size according to the position.
                        if (settings.Position == BadgePosition.Left || settings.Position == BadgePosition.Right)
                        {
                            UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest + badgeWidth / 2, settings.BadgeView.Content.HeightRequest);

                        }
                        else if (settings.Position == BadgePosition.Top || settings.Position == BadgePosition.Bottom)
                        {
                            UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest, settings.BadgeView.Content.HeightRequest + badgeHeight / 2);
                        }
                        else
                        {
                            UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest + badgeWidth / 2, settings.BadgeView.Content.HeightRequest + badgeHeight / 2);
                        }
                    }
                }
                else if (alignment == BadgeAlignment.Start)
                {
                    if (settings.BadgeView.Content?.WidthRequest < 0 && settings.BadgeView.Content?.HeightRequest < 0)
                    {
                        // To update the content size of the badge view according to the position.
                        if (settings.Position == BadgePosition.Left || settings.Position == BadgePosition.Right)
                        {
                            UpdateContentSize(settings.BadgeView, badgeWidth, 0);
                        }
                        else if (settings.Position == BadgePosition.Top || settings.Position == BadgePosition.Bottom)
                        {
                            UpdateContentSize(settings.BadgeView, 0, badgeHeight);
                        }
                        else
                        {
                            UpdateContentSize(settings.BadgeView, badgeWidth, badgeHeight);
                        }
                    }

                    if (settings.BadgeView.Content?.WidthRequest > 0 && settings.BadgeView.Content?.HeightRequest > 0)
                    {
                        // To update the badge view control size according to the position.
                        if (settings.Position == BadgePosition.Left || settings.Position == BadgePosition.Right)
                        {
                            UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest + badgeWidth, settings.BadgeView.Content.HeightRequest);
                        }
                        else if (settings.Position == BadgePosition.Top || settings.Position == BadgePosition.Bottom)
                        {
                            UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest, settings.BadgeView.Content.HeightRequest + badgeHeight);
                        }
                        else
                        {
                            UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest + badgeWidth, settings.BadgeView.Content.HeightRequest + badgeHeight);
                        }
                    }

                }
                else
                {
                    if (settings.BadgeView.Content !=null && controlWidth > 0)
                    {
                        settings.BadgeView.Content.WidthRequest = controlWidth;
                    }

                    if (settings.BadgeView.Content != null && controlHeight > 0)
                    {
                        settings.BadgeView.Content.HeightRequest = controlHeight;
                    }

                    if (settings.BadgeView.Content?.WidthRequest > 0 && settings.BadgeView.Content?.HeightRequest > 0)
                    {
                        UpdateControlSize(settings.BadgeView, settings.BadgeView.Content.WidthRequest, settings.BadgeView.Content.HeightRequest);
                    }
                }
            }
        }

        /// <summary>
        /// To update the content size of the badge view.
        /// </summary>
        /// <param name="badgeView">The badge view.</param>
        /// <param name="badgeWidth">The width of the badge.</param>
        /// <param name="badgeHeight">The height of the badge.</param>
        private static void UpdateContentSize(SfBadgeView badgeView, double badgeWidth, double badgeHeight)
        {
            double controlWidth = badgeView.WidthRequest;
            double controlHeight = badgeView.HeightRequest;

            if (badgeView.Content != null && controlWidth - badgeWidth > 0)
            {
                badgeView.Content.WidthRequest = controlWidth - badgeWidth;
            }
            else if (badgeView.Content != null && badgeView.Content.WidthRequest - badgeWidth > 0)
            {
                badgeView.Content.WidthRequest -= badgeWidth;
            }
            if (badgeView.Content != null && controlHeight - badgeHeight > 0)
            {
                badgeView.Content.HeightRequest = controlHeight - badgeHeight;
            }
            else if (badgeView.Content != null && badgeView.Content.HeightRequest - badgeHeight > 0)
            {
                badgeView.Content.HeightRequest -= badgeHeight;
            }

            badgeView.BadgeSettings?.UpdateContentLayout(badgeWidth, badgeHeight);
        }

        private void UpdateContentLayout(double badgeWidth, double badgeHeight)
        {
            if (BadgeView?.Content != null)
            {
                if (Position == BadgePosition.Left || Position == BadgePosition.BottomLeft)
                {
                    AbsoluteLayout.SetLayoutBounds(BadgeView.Content, new Rect(badgeWidth, 0, BadgeView.Content.WidthRequest, BadgeView.Content.HeightRequest));
                }
                else if (Position == BadgePosition.Top || Position == BadgePosition.TopRight)
                {
                    AbsoluteLayout.SetLayoutBounds(BadgeView.Content, new Rect(0, badgeHeight, BadgeView.Content.WidthRequest, BadgeView.Content.HeightRequest));
                }
                else if (Position == BadgePosition.Right || Position == BadgePosition.Bottom || Position == BadgePosition.BottomRight)
                {
                    AbsoluteLayout.SetLayoutBounds(BadgeView.Content, new Rect(0, 0, BadgeView.Content.WidthRequest, BadgeView.Content.HeightRequest));
                }
                else if (Position == BadgePosition.TopLeft)
                {
                    AbsoluteLayout.SetLayoutBounds(BadgeView.Content, new Rect(badgeWidth, badgeHeight, BadgeView.Content.WidthRequest, BadgeView.Content.HeightRequest));
                }
            }
        }

        /// <summary>
        /// To update the badge view control size.
        /// </summary>
        /// <param name="badgeView">The badge view.</param>
        /// <param name="width">The width of the badge view.</param>
        /// <param name="height">The height of the badge view.</param>
        private static void UpdateControlSize(SfBadgeView badgeView, double width, double height)
        {
            double controlWidth = badgeView.WidthRequest;
            double controlHeight = badgeView.HeightRequest;
            if (controlWidth < 0 && width > 0)
            {
                badgeView.WidthRequest = width;
            }

            if (controlHeight < 0 && height > 0)
            {
                badgeView.HeightRequest = height;
            }
        }

        #endregion
    }
}