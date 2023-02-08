using System;
using System.Collections.Generic;
using System.Text;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// AvatarShape provides direct shapes for <see cref="SfAvatarView"/>.
    /// </summary>
    public enum AvatarShape
    {
        /// <summary>
        /// Defines the Square shape.
        /// </summary>
        Square,

        /// <summary>
        /// Defines the circle shape.
        /// </summary>
        Circle,

        /// <summary>
        /// Custom enum displays the custom visual type.
        /// </summary>
        Custom,
    }

    /// <summary>
    /// AvatarSize describes the different sizes for <see cref="SfAvatarView"/>.
    /// </summary>
    public enum AvatarSize
    {
        /// <summary>
        /// Sets the size of the avatar to 64d.
        /// </summary>
        ExtraLarge,

        /// <summary>
        /// Sets the size of the avatar to 48d.
        /// </summary>
        Large,

        /// <summary>
        /// Sets the size of the avatar to 40d.
        /// </summary>
        Medium,

        /// <summary>
        /// Sets the size of the avatar to 32d.
        /// </summary>
        Small,

        /// <summary>
        /// Sets the size of the avatar to 24d.
        /// </summary>
        ExtraSmall,
    }

    /// <summary>
    /// Type of the avatar to be displayed.
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// AvatarCharacter enum contains default font icon and image.
        /// </summary>
        AvatarCharacter,

        /// <summary>
        ///  Displays the initials as the avatar and allows single and double characters.
        /// </summary>
        Initials,
      
        /// <summary>
        /// Displays the custom image as the avatar.
        /// </summary>
        Custom,

        /// <summary>
        /// Displays a group view as the avatar with the maximum of three views.
        /// </summary>
        Group,

        /// <summary>
        /// Displays the initial type as the avatar if set.
        /// </summary>
        Default,

    }

    /// <summary>
    /// Displays a single character or double character in the avatar view when the ContentType is set as Initials.
    /// </summary>
    public enum InitialsType
    {
        /// <summary>
        /// Displays only one character in the <see cref="SfAvatarView"/>
        /// </summary>
        SingleCharacter,

        /// <summary>
        /// Displays two characters in the <see cref="SfAvatarView"/>
        /// </summary>
        DoubleCharacter,
    }

    /// <summary>
    /// Determines the background color of the avatar view based on the text color.
    /// </summary>
    public enum AvatarColorMode
    {
        /// <summary>
        /// Contains the default color for the text and background color.
        /// </summary>
        Default,

        /// <summary>
        /// Sets the background color as a light color.
        /// </summary>
        LightBackground,

        /// <summary>
        /// Sets the background color as dark color.
        /// </summary>
        DarkBackground,
    }

    /// <summary>
    /// Avatar Character enum contains the default image and the default font icon in the SfAvatarView, which is used only for AvatarCharacter type.
    /// </summary>
    public enum AvatarCharacter
    {
        /// <summary>
        /// Avatar1 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar1,

        /// <summary>
        /// Avatar2 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar2,

        /// <summary>
        /// Avatar3 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar3,

        /// <summary>
        /// Avatar4 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar4,

        /// <summary>
        /// Avatar5 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar5,

        /// <summary>
        /// Avatar6 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar6,

        /// <summary>
        /// Avatar7 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar7,

        /// <summary>
        /// Avatar8 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar8,

        /// <summary>
        /// Avatar9 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar9,

        /// <summary>
        /// Avatar10 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar10,

        /// <summary>
        /// Avatar11 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar11,

        /// <summary>
        /// Avatar12 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar12,

        /// <summary>
        /// Avatar13 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar13,

        /// <summary>
        /// Avatar14 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar14,

        /// <summary>
        /// Avatar15 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar15,

        /// <summary>
        /// Avatar16 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar16,

        /// <summary>
        /// Avatar17 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar17,

        /// <summary>
        /// Avatar18 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar18,

        /// <summary>
        /// Avatar19 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar19,

        /// <summary>
        /// Avatar20 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar20,

        /// <summary>
        /// Avatar21 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar21,

        /// <summary>
        /// Avatar22 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar22,

        /// <summary>
        /// Avatar23 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar23,

        /// <summary>
        /// Avatar24 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar24,

        /// <summary>
        /// Avatar25 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar25,

        /// <summary>
        /// Avatar26 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar26,

        /// <summary>
        /// Avatar27 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar27,

        /// <summary>
        /// Avatar28 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar28,

        /// <summary>
        /// Avatar29 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar29,

        /// <summary>
        /// Avatar30 enum of the AvatarCharacter contains the default PNG image.
        /// </summary>
        Avatar30,
    }


    /// <summary>
    /// AvatarView static text.
    /// </summary>
    internal static class AvatarViewStaticText
    {
        /// <summary>
        /// Used for adding the Avatar Character images.
        /// </summary>
        internal const string AvatarCharacterFileTypeText = ".png";

        /// <summary>
        /// Space text value.
        /// </summary>
        internal static readonly string SpaceText = string.Empty;
    }


}