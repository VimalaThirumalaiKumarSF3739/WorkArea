// <copyright file="AvatarViewAutomaticColor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// AvatarView Automatic color class defines the Background color property as light or dark.
    /// </summary>
    internal class AvatarViewAutomaticColor
    {
        #region Fields

        /// <summary>
        /// Gets or sets the value for the Light color.
        /// </summary>
        internal Color? LightColor { get; set; }

        /// <summary>
        /// Gets or sets the value for Dark color.
        /// </summary>
        internal Color? DarkColor { get; set; }

        #endregion
    }
}