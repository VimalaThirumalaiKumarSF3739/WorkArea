// <copyright file="Enum.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Specifies the background appearance of <see cref="SfTextInputLayout"/>. Containers improve the discoverability of input view.
    /// </summary>
    public enum ContainerType
    {
        /// <summary>
        /// Draws a border around the layout.
        /// </summary>
        Outlined,

        /// <summary>
        /// Draws a base line.
        /// </summary>
        Filled,

        /// <summary>
        /// Contains no background and compact spacing.
        /// </summary>
        None,
    }

    /// <summary>
    /// Specifies the position of the leading and trailing views in the <see cref="SfTextInputLayout" />.
    /// </summary>
    public enum ViewPosition
    {
        /// <summary>
        /// Position the view inside the layout region.
        /// </summary>
        Inside,

        /// <summary>
        /// Position the view outside the layout region.
        /// </summary>
        Outside,
    }
}
