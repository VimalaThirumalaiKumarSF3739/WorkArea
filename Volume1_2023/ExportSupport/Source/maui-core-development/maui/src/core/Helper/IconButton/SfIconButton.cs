namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;
    using Syncfusion.Maui.Core.Internals;
    using Syncfusion.Maui.Graphics.Internals;
    using PointerEventArgs = Syncfusion.Maui.Core.Internals.PointerEventArgs;

    /// <summary>
    /// Represents a class which contains information of button icons.
    /// </summary>
    internal class SfIconButton : Grid, ITouchListener
    {
        #region Fields

        /// <summary>
        /// Used to trigger whenever the tap gesture tap event triggered.
        /// </summary>
        internal Action<string>? Clicked;

        /// <summary>
        /// The show touch effect.
        /// </summary>
        private bool showTouchEffect;

        /// <summary>
        /// Holds that the view is visible or not.
        /// </summary>
        private bool visibility;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfIconButton"/> class.
        /// </summary>
        /// <param name="child">The child view.</param>
        /// <param name="showTouchEffect">The show touch effect.</param>
        internal SfIconButton(View child, bool showTouchEffect = true)
        {
            this.showTouchEffect = showTouchEffect;
            this.EffectsView = new SfEffectsView();
            this.Add(this.EffectsView);
            this.EffectsView.Content = child;
            this.EffectsView.ShouldIgnoreTouches = true;
            this.AddTouchListener(this);
            this.visibility = true;
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets a value indicating whether is show touch effect or not.
        /// </summary>
        internal bool ShowTouchEffect
        {
            get
            {
                return this.showTouchEffect;
            }

            set
            {
                if (value == this.showTouchEffect)
                {
                    return;
                }

                this.showTouchEffect = value;
            }
        }

        /// <summary>
        /// Gets or sets the effective view.
        /// </summary>
        internal SfEffectsView EffectsView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the view is visible or not.
        /// TODO: IsVisible property breaks in 6.0.400 release.
        /// Issue link -https://github.com/dotnet/maui/issues/7507
        /// -https://github.com/dotnet/maui/issues/8044
        /// -https://github.com/dotnet/maui/issues/7482
        /// </summary>
        internal bool Visibility
        {
            get
            {
                return this.visibility;
            }

            set
            {
#if !__MACCATALYST__
                this.IsVisible = value;
#endif
                this.visibility = value;
            }
        }

        #endregion

        #region Public method

        /// <summary>
        /// Method invokes on touch interaction.
        /// </summary>
        /// <param name="e">The touch event args.</param>
        void ITouchListener.OnTouch(PointerEventArgs e)
        {
            if (!this.showTouchEffect)
            {
                return;
            }

            if (e.Action == PointerActions.Pressed)
            {
                this.EffectsView.ApplyEffects();
            }
            else if (e.Action == PointerActions.Released)
            {
                this.EffectsView.Reset();
                var sfIconView = this.EffectsView.Content as SfIconView;
                if (sfIconView != null)
                {
                    this.Clicked?.Invoke(sfIconView.Text);
                }
#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
                //// Show effect will false when we reach min or max date view and
                //// the view is enabled because it was disabled when loading busy indicator.
                if (this.showTouchEffect && this.IsEnabled)
                {
                    //// The hovering color is not maintained when you press and release the mouse pointer in navigation arrows.
                    this.EffectsView.Background = new SolidColorBrush(Colors.Black.WithAlpha(0.04f));
                }
                else
                {
                    this.EffectsView.Background = Brush.Transparent;
                }
#endif
            }
#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
            else if (e.Action == PointerActions.Entered)
            {
                this.EffectsView.ApplyEffects(SfEffects.Highlight);
            }
            else if (e.Action == PointerActions.Exited)
            {
                this.EffectsView.Reset();
                this.EffectsView.Background = Brush.Transparent;
            }
#endif
            else if (e.Action == PointerActions.Cancelled)
            {
                this.EffectsView.Reset();
                this.EffectsView.Background = Brush.Transparent;
            }
        }

        #endregion

        #region Internal method

        /// <summary>
        /// Method to update icon style.
        /// </summary>
        /// <param name="textStyle"> The text style value.</param>
        internal void UpdateStyle(ITextElement textStyle)
        {
            var iconView = this.EffectsView.Content as SfIconView;
            if (iconView != null)
            {
                iconView.UpdateStyle(textStyle);
            }
        }

        /// <summary>
        /// Method to update icon color.
        /// </summary>
        /// <param name="iconColor"> The icon color.</param>
        internal void UpdateIconColor(Color iconColor)
        {
            var iconView = this.EffectsView.Content as SfIconView;
            if (iconView != null)
            {
                iconView.UpdateIconColor(iconColor);
            }
        }

        #endregion
    }
}