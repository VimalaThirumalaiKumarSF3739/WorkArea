// <copyright file="AvatarGroupView.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// AvatarGroupView inherits from the grid, we can add three views in the single <see cref="SfAvatarView"/> and we can customize all the views.
    /// </summary>
    internal class AvatarGroupView : Grid
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarGroupView"/> class.
        /// </summary>
        
        #region Constructor

        internal AvatarGroupView()
        {
            this.PrimaryInitialsLabel.HorizontalTextAlignment = TextAlignment.Center;
            this.PrimaryInitialsLabel.VerticalTextAlignment = TextAlignment.Center;
            this.PrimaryInitialsLabel.HorizontalOptions = LayoutOptions.Center;
            this.PrimaryInitialsLabel.VerticalOptions = LayoutOptions.Center;
            this.SecondaryInitialsLabel.HorizontalTextAlignment = TextAlignment.Center;
            this.SecondaryInitialsLabel.VerticalTextAlignment = TextAlignment.Center;
            this.SecondaryInitialsLabel.HorizontalOptions = LayoutOptions.Center;
            this.SecondaryInitialsLabel.VerticalOptions = LayoutOptions.Center;
            this.TertiaryInitialsLabel.HorizontalTextAlignment = TextAlignment.Center;
            this.TertiaryInitialsLabel.VerticalTextAlignment = TextAlignment.Center;
            this.TertiaryInitialsLabel.HorizontalOptions = LayoutOptions.Center;
            this.TertiaryInitialsLabel.VerticalOptions = LayoutOptions.Center;
            this.PrimaryImage.Aspect = this.SecondaryImage.Aspect = this.TertiaryImage.Aspect = Aspect.AspectFill;
            this.PopulatePrimaryGrid();
            this.PopulateSecondaryGrid();          
            this.RowSpacing = this.SecondaryLayoutGrid.RowSpacing = this.ColumnSpacing = this.SecondaryLayoutGrid.ColumnSpacing = 1;           
        }

        #endregion

        #region Fields

        /// <summary>
        /// Gets or sets tertiaryGrid for loading the third image in the group.
        /// </summary>
        internal Image TertiaryImage { get; set; } = new Image();

        /// <summary>
        /// Gets or sets tertiaryGrid for adding the children as Tertiary image and Tertiary initial label.
        /// </summary>
        internal Grid TertiaryGrid { get; set; } = new Grid();

        /// <summary>
        /// Gets or sets secondaryLayoutGrid for adding the children as both secondary and tertiary grids.
        /// </summary>
        internal Grid SecondaryLayoutGrid { get; set; } = new Grid();

        /// <summary>
        /// Gets or sets primaryInitials label for loading the first text in the group view.
        /// </summary>
        internal FontIconLabel PrimaryInitialsLabel { get; set; } = new FontIconLabel();

        /// <summary>
        /// Gets or sets primaryImage for loading the first image in the group view.
        /// </summary>
        internal Image PrimaryImage { get; set; } = new Image();

        /// <summary>
        /// Gets or sets primaryGrid for adding the children as primary image and primary initial label.
        /// </summary>
        internal Grid PrimaryGrid { get; set; } = new Grid();

        /// <summary>
        /// Gets or sets secondaryInitials label for loading the second text in the group view.
        /// </summary>
        internal FontIconLabel SecondaryInitialsLabel { get; set; } = new FontIconLabel();

        /// <summary>
        /// Gets or sets secondaryImage for loading the second image in the group.
        /// </summary>
        internal Image SecondaryImage { get; set; } = new Image();

        /// <summary>
        /// Gets or sets secondaryGrid for adding the children as secondary image and secondary initial label.
        /// </summary>
        internal Grid SecondaryGrid { get; set; } = new Grid();

        /// <summary>
        /// Gets or sets tertiaryInitialsLabel label for loading the third text in the group view.
        /// </summary>
        internal FontIconLabel TertiaryInitialsLabel { get; set; } = new FontIconLabel();

        #endregion

        #region Internal methods

        /// <summary>
        /// Method performs spacing for the primary, secondary, and tertiary grids.
        /// </summary>
        /// <param name="hasSecondaryElement">Check secondary element.</param>
        /// <param name="hasTertiaryElement">Check the tertiary element.</param>
        internal void ArrageElementsSpacing(bool hasSecondaryElement = false, bool hasTertiaryElement = false)
        {
            this.ColumnDefinitions.Clear();
            this.SecondaryLayoutGrid.RowDefinitions.Clear();
            this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            if (hasSecondaryElement)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            else
            {
                this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0, GridUnitType.Absolute) });
            }

            this.SecondaryLayoutGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            if (hasTertiaryElement)
            {
                this.SecondaryLayoutGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            else
            {
                this.SecondaryLayoutGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Absolute) });
            }
        }

        /// <summary>
        /// Method performs font size and font attributes for all the primary, secondary, and tertiary grids in group view.
        /// </summary>
        /// <param name="referenceLabel">set the reference label.</param>
        internal void SetInitialsFontAttributeValues(Label referenceLabel)
        {
            this.PrimaryInitialsLabel.FontSize = this.SecondaryInitialsLabel.FontSize = this.TertiaryInitialsLabel.FontSize = referenceLabel.FontSize;
            this.PrimaryInitialsLabel.FontAttributes = this.SecondaryInitialsLabel.FontAttributes = this.TertiaryInitialsLabel.FontAttributes = referenceLabel.FontAttributes;
        }

        /// <summary>
        /// Method performs font size and font attributes for the default avatar type of all the primary, secondary, and tertiary grids in group view.
        /// </summary>
        /// <param name="actualFontFamily">string type.</param>
        /// <param name="fontIconFontFamily">fonticon as a string type.</param>
        internal void SetInitialsFontFamily(string actualFontFamily, string fontIconFontFamily)
        {
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {              
                    this.PrimaryInitialsLabel.FontFamily = actualFontFamily;                 
                    this.SecondaryInitialsLabel.FontFamily = actualFontFamily;
                    this.TertiaryInitialsLabel.FontFamily = actualFontFamily;               
            }
            else
            {             
                    this.PrimaryInitialsLabel.FontFamily = actualFontFamily;                           
                    this.SecondaryInitialsLabel.FontFamily = actualFontFamily;                           
                    this.TertiaryInitialsLabel.FontFamily = actualFontFamily;              
            }
        }

        /// <summary>
        /// Set colors method used for setting the color based on the ColorMode in all the primary, secondary, and tertiary grids in the group view.
        /// </summary>
        /// <param name="colorMode">Color mode type.</param>
        internal void SetColors(AvatarColorMode colorMode = AvatarColorMode.Default)
        {
            if (colorMode == AvatarColorMode.DarkBackground)
            {
                if (AvatarViewColorTable.AutomaticColors != null)
                {
                    this.PrimaryGrid.BackgroundColor = AvatarViewColorTable.AutomaticColors[0].DarkColor;
                    this.SecondaryGrid.BackgroundColor = AvatarViewColorTable.AutomaticColors[1].DarkColor;
                    this.TertiaryGrid.BackgroundColor = AvatarViewColorTable.AutomaticColors[2].DarkColor;
                }

                this.PrimaryInitialsLabel.TextColor = AvatarViewColorTable.InitialsLightColor;
                this.SecondaryInitialsLabel.TextColor = AvatarViewColorTable.InitialsLightColor;
                this.TertiaryInitialsLabel.TextColor = AvatarViewColorTable.InitialsLightColor;
            }
            else if (colorMode == AvatarColorMode.LightBackground)
            {
                if (AvatarViewColorTable.AutomaticColors != null)
                {
                    this.PrimaryGrid.BackgroundColor = AvatarViewColorTable.AutomaticColors[0].LightColor;
                    this.SecondaryGrid.BackgroundColor = AvatarViewColorTable.AutomaticColors[1].LightColor;
                    this.TertiaryGrid.BackgroundColor = AvatarViewColorTable.AutomaticColors[2].LightColor;
                }

                this.PrimaryInitialsLabel.TextColor = AvatarViewColorTable.InitialsDarkColor;
                this.SecondaryInitialsLabel.TextColor = AvatarViewColorTable.InitialsDarkColor;
                this.TertiaryInitialsLabel.TextColor = AvatarViewColorTable.InitialsDarkColor;
            }
        }

        /// <summary>
        /// Method performs by adding the primary grid, primary image, primary layout grid, and secondary layout grid into the avatar group view grid.
        /// </summary>
        private void PopulatePrimaryGrid()
        {
            //Todo : https://github.com/dotnet/maui/issues/6986 in Windows 
            this.ArrageElementsSpacing();
            this.Children.Add(this.PrimaryGrid);
            this.Children.Add(this.PrimaryInitialsLabel);
            this.Children.Add(this.PrimaryImage);
            this.Children.Add(this.SecondaryLayoutGrid);      
            Grid.SetColumn(this.SecondaryLayoutGrid, 1);
        }

        /// <summary>
        /// Method performs by adding the secondary grid, secondary image, secondary initial, tertiary grid, tertiary image, and tertiary initial into the avatar group view grid and defines the row for the tertiary grid.
        /// </summary>
        private void PopulateSecondaryGrid()
        {
            this.ArrageElementsSpacing();
            this.SecondaryLayoutGrid.Children.Add(this.SecondaryGrid);
            this.SecondaryLayoutGrid.Children.Add(this.SecondaryInitialsLabel);
            this.SecondaryLayoutGrid.Children.Add(this.SecondaryImage);           
            this.SecondaryLayoutGrid.Children.Add(this.TertiaryGrid);
            this.SecondaryLayoutGrid.Children.Add(this.TertiaryInitialsLabel);
            //this.SecondaryLayoutGrid.Children.Add(this.TertiaryImage);
            this.TertiaryGrid.Children.Add(this.TertiaryImage);
            Grid.SetRow(this.TertiaryGrid, 1);
            Grid.SetRow(this.TertiaryInitialsLabel, 1);
            Grid.SetRow(this.TertiaryImage, 1);           
        }

        #endregion
    }
}