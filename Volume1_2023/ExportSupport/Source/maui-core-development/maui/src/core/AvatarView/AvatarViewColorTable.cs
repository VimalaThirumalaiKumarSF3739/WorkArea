// <copyright file="AvatarViewColorTable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// AvatarViewColorTable class contains the color which can be used for setting the <see cref="SfAvatarView"/> .
    /// </summary>
    internal static class AvatarViewColorTable
    {
        #region Fields

        /// <summary>
        /// Gets or sets current Background color index.
        /// </summary>
        internal static int CurrentBackgroundColorIndex { get; set; } = 0;

        /// <summary>
        /// Gets or sets initals Light color value.
        /// </summary>
        internal static Color InitialsLightColor { get; set; } = Color.FromArgb("#FFFFFF");

        /// <summary>
        /// Gets or sets initals dark color value.
        /// </summary>
        internal static Color InitialsDarkColor { get; set; } = Color.FromArgb("#000000");

        /// <summary>
        /// Gets or sets automatic color collection value.
        /// </summary>
        internal static List<AvatarViewAutomaticColor>? AutomaticColors { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Method performs for setting all the default colors for the light and dark colors used for background color in the group view.
        /// </summary>
        internal static void GenerateAutomaticBackgroundColors()
        {
            AutomaticColors = new List<AvatarViewAutomaticColor>();
            AutomaticColors.Add(new AvatarViewAutomaticColor() { LightColor = Color.FromArgb("#90DDFE"), DarkColor = Color.FromArgb("#976F0C") });
            AutomaticColors.Add(new AvatarViewAutomaticColor() { LightColor = Color.FromArgb("#9FCC69"), DarkColor = Color.FromArgb("#740A1C") });
            AutomaticColors.Add(new AvatarViewAutomaticColor() { LightColor = Color.FromArgb("#FCCE65"), DarkColor = Color.FromArgb("#5C2E91") });
            AutomaticColors.Add(new AvatarViewAutomaticColor() { LightColor = Color.FromArgb("#FE9B90"), DarkColor = Color.FromArgb("#004E8C") });
            AutomaticColors.Add(new AvatarViewAutomaticColor() { LightColor = Color.FromArgb("#9AA8F5"), DarkColor = Color.FromArgb("#B73EAA") });
            AutomaticColors.Add(new AvatarViewAutomaticColor() { LightColor = Color.FromArgb("#F5EF9A"), DarkColor = Color.FromArgb("#69797E") });
            AutomaticColors.Add(new AvatarViewAutomaticColor() { LightColor = Color.FromArgb("#FBBC93"), DarkColor = Color.FromArgb("#068387") });
            AutomaticColors.Add(new AvatarViewAutomaticColor() { LightColor = Color.FromArgb("#D7E99C"), DarkColor = Color.FromArgb("#498204") });
            AutomaticColors.Add(new AvatarViewAutomaticColor() { LightColor = Color.FromArgb("#E79AF5"), DarkColor = Color.FromArgb("#4F6BED") });
            AutomaticColors.Add(new AvatarViewAutomaticColor() { LightColor = Color.FromArgb("#9FEFC5"), DarkColor = Color.FromArgb("#CA500F") });
        }

        #endregion
    }
}