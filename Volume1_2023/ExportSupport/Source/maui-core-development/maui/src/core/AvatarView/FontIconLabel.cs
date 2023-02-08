// <copyright file="FontIconLabel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Label for adding the font icon file in renderer.
    /// </summary>
    internal class FontIconLabel : Label
    {
        #region Fields

        /// <summary>
        /// The avatar content type.
        /// </summary>
        internal string? AvatarContentType { get; set; }

        #endregion

        public FontIconLabel()
        {
            this.Style = new Style(typeof(Label));
        }
    }
}
