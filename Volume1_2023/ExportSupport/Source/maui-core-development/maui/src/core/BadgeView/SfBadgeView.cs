// <copyright file="SfBadgeView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;

    /// <summary>
    /// The .NET MAUI Badge View control allows you to show a notification badge with a text over any controls. Adding a control in a Badge View control, you can show a notification badge value over the control.
    /// </summary>
    /// <example>
    /// The following examples show how to initialize the badge view.
    /// # [XAML](#tab/tabid-1)
    /// <code><![CDATA[
    /// <badge:SfBadgeView HorizontalOptions="Center"
    ///                    VerticalOptions="Center"
    ///                    BadgeText="20">
    ///
    ///     <badge:SfBadgeView.Content>
    ///         <Button Text="Primary"
    ///                 WidthRequest="120"
    ///                 HeightRequest="60"/>
    ///     </badge:SfBadgeView.Content>
    ///
    /// </badge:SfBadgeView>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfBadgeView sfBadgeView = new SfBadgeView();
    /// sfBadgeView.HorizontalOptions = LayoutOptions.Center;
    /// sfBadgeView.VerticalOptions = LayoutOptions.Center;
    /// sfBadgeView.BadgeText = "20";
    ///
    /// Button button = new Button();
    /// button.Text = "Primary";
    /// button.WidthRequest = 120;
    /// button.HeightRequest = 60;
    ///
    /// sfBadgeView.Content = button;
    /// this.Content = sfBadgeView;
    /// ]]></code>
    /// ***
    /// </example>
    [DesignTimeVisible(true)]
    [ContentProperty(nameof(Content))]
    public class SfBadgeView : SfContentView
    {
        #region Bindable properties
		
		 /// <summary>        
        /// Identifies the <see cref="ScreenReaderText"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ScreenReaderText"/> bindable property.
        /// </value>
        public static readonly BindableProperty ScreenReaderTextProperty =
            BindableProperty.Create(nameof(ScreenReaderText), typeof(string), typeof(SfBadgeView), string.Empty, BindingMode.OneWay, null, OnScreenReaderTextPropertyChanged);

        /// <summary>
        /// Identifies the content bindable property.
        /// </summary>
        /// <value>
        /// The identifier for content bindable property.
        /// </value>
        public static new readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(SfBadgeView), null, BindingMode.OneWay, null, OnContentPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="BadgeText"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BadgeText"/> bindable property.
        /// </value>
        public static readonly BindableProperty BadgeTextProperty =
            BindableProperty.Create(nameof(BadgeText), typeof(string), typeof(SfBadgeView), string.Empty, BindingMode.OneWay, null, OnBadgeTextPropertyChanged);


        /// <summary>
        /// Identifies the <see cref="BadgeSettings"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BadgeSettings"/> bindable property.
        /// </value>
        public static readonly BindableProperty BadgeSettingsProperty =
            BindableProperty.Create(nameof(BadgeSettings), typeof(BadgeSettings), typeof(SfBadgeView), null, BindingMode.OneWay, null, OnBadgeSettingsPropertyChanged, null, defaultValueCreator: GetBadgeSettingsDefaultValue);

        #endregion

        #region Fields

        private BadgeLabelView? badgeLabelView;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfBadgeView"/> class.
        /// </summary>
        public SfBadgeView()
        {
            this.ValidateLicense(true);
            InitializeBadgeLayer();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the screen reader text for the badge.
        /// </summary>
        /// <value>
        /// Specifies the screen reader text of the badge. The default value is an empty string.
        /// </value>
        /// <example>
        /// The following example shows how to apply the screen reader text for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView ScreenReaderText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.ScreenReaderText = "20";
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
        public string ScreenReaderText
        {
            get { return (string)this.GetValue(ScreenReaderTextProperty); }
            set { this.SetValue(ScreenReaderTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text for the badge. The property will be set only if BadgeIcon is set to BadgeIcon.None.
        /// </summary>
        /// <value>
        /// Specifies the badge text of the badge. The default value is an empty string.
        /// </value>
        /// <example>
        /// The following example shows how to apply the badge text for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
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
        public string BadgeText
        {
            get { return (string)this.GetValue(BadgeTextProperty); }
            set { this.SetValue(BadgeTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for badge settings.
        /// </summary>
        /// <value>
        /// Specifies the badge settings of the badge. The default value is null.
        /// </value>
        /// <example>
        /// The following example shows how to apply the badge settings for a badge.
        /// # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <badge:SfBadgeView BadgeText="20">
        ///     <badge:SfBadgeView.Content>
        ///         <Button Text="Primary"
        ///                 WidthRequest="120"
        ///                 HeightRequest="60"/>
        ///     </badge:SfBadgeView.Content>
        ///     <badge:SfBadgeView.BadgeSettings>
        ///         <badge:BadgeSettings Stroke="Orange"
        ///                              StrokeThickness="2"/>
        ///     </badge:SfBadgeView.BadgeSettings>
        /// </badge:SfBadgeView>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfBadgeView sfBadgeView = new SfBadgeView();
        /// sfBadgeView.BadgeText = "20";
        /// sfBadgeView.BadgeSettings = new BadgeSettings 
        /// {
        ///     Stroke = Colors.Orange,
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
        public BadgeSettings? BadgeSettings
        {
            get { return (BadgeSettings)this.GetValue(BadgeSettingsProperty); }
            set { this.SetValue(BadgeSettingsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that defines the badge label view.
        /// </summary>
        internal BadgeLabelView? BadgeLabelView
        {
            get { return this.badgeLabelView; }
            set { this.badgeLabelView = value; }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method used to initialize the badge layer.
        /// </summary>
        internal void InitializeBadgeLayer()
        {
            this.BadgeLabelView ??= new BadgeLabelView(this);
			
            this.BadgeLabelView.ScreenReaderText = this.ScreenReaderText;
            this.BadgeLabelView.Text = this.BadgeText;
            this.DrawingOrder = DrawingOrder.AboveContent;
            base.Content = this.Content;
            if (this.BadgeSettings == null)
            {
                this.BadgeSettings = new BadgeSettings();
            }
            else
            {
                this.BadgeSettings.ApplySettingstoUpdatedBadgeView();
            }

            this.BadgeLabelView.TextElement = this.BadgeSettings;
        }

        /// <summary>
        /// Initialize Forms BadgeSettings.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <returns>The default value of BadgeSettings.</returns>
        private static object? GetBadgeSettingsDefaultValue(BindableObject bindable)
        {
            return new BadgeSettings { BadgeView = bindable as SfBadgeView };
        }

        #endregion

        #region Override methods

        /// <summary>
        /// OnPropertyChanged method
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            //Implicit Style being applied before calling the constructor, Below is the workaround
            if (propertyName == "ImplicitStyle")
            {
                this.InitializeBadgeLayer();
            }
            base.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Called when the binding context is changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (this.BadgeSettings != null)
            {
                SfBadgeView.SetInheritedBindingContext(this.BadgeSettings, this.BindingContext);
            }
        }

        /// <summary>
        /// Draw method.
        /// </summary>
        /// <param name="canvas">The Canvas.</param>
        /// <param name="dirtyRect">The rectangle.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);
           this.BadgeSettings?.AlignBadgeText(this, this.BadgeSettings.Position);
           badgeLabelView?.DrawBadge(canvas);

        }

        /// <summary>
        /// ArrangeContent method.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <returns>It returns the size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            if (this.BadgeSettings != null)
            {
                this.BadgeSettings.AlignBadge(this, this.BadgeSettings.Position);
            }
            return base.ArrangeContent(bounds);
        }

        #endregion

        #region Property changed
	
        /// <summary>
        /// Invoked whenever the <see cref="ContentProperty"/> is set for badge view.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfBadgeView badgeView)
            {
                badgeView.Content = (View)newValue;
                badgeView.InitializeBadgeLayer();
            }
        }


        /// <summary>
        /// Invoked whenever the <see cref="ScreenReaderTextProperty"/> is set for badge view.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnScreenReaderTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfBadgeView badgeView)
            {
                if (badgeView.BadgeLabelView != null)
                {
                    badgeView.BadgeLabelView.ScreenReaderText = (string)newValue;
                }
            }
        }


        /// <summary>
        /// Invoked whenever the <see cref="BadgeTextProperty"/> is set for badge view.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgeTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfBadgeView badgeView && badgeView.BadgeLabelView != null)
            {
                badgeView.BadgeLabelView.Text = (string)newValue;
                badgeView.BadgeLabelView.CalculateBadgeBounds();
                badgeView.InvalidateDrawable();

            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="BadgeSettingsProperty"/> is set for badge view.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnBadgeSettingsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                if (oldValue is BadgeSettings previousSetting)
                {
                    previousSetting.BindingContext = null;
                    previousSetting.Parent = null;
                }
            }

            if (newValue != null)
            {
                if (newValue is BadgeSettings currentSetting && bindable is SfBadgeView badgeView)
                {
                    currentSetting.Parent = badgeView;
                    if (badgeView.BadgeLabelView != null)
                    {
                        badgeView.BadgeLabelView.TextElement = currentSetting;
                    }

                    SetInheritedBindingContext(badgeView.BadgeSettings, badgeView.BindingContext);
                }
            }
        }

        #endregion
    }
}
