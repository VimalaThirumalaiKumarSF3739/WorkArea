using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Animations;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using Animation = Microsoft.Maui.Animations.Animation;
using ControlAnimation = Microsoft.Maui.Controls.Animation;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// This class represents a content view to show tooltip in absolute layout.
    /// </summary>
    internal class SfTooltip : ContentView
    {
        #region Fields

        private readonly Grid parentView;
        private readonly TooltipDrawableView drawableView;
        private readonly ContentView contentView;
        private readonly TooltipHelper tooltipHelper;
        private bool isDisappeared = false;
        bool isTooltipActivate = false;
        
        const string durationAnimation = "DurationAnimation";
        #endregion

        #region Properties

        private View? content;

        /// <summary>
        /// Gets or sets the content for tooltip.
        /// </summary>
        public new View? Content
        {
            get
            {
                return content;
            }

            set
            {
                if (content != value)
                {
                    content = value;
                    OnContentChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the position of tooltip.
        /// </summary>
        public TooltipPosition Position { get; set; } = TooltipPosition.Auto;

        /// <summary>
        /// Gets or sets the duration of the tooltip in seconds.
        /// </summary>
        public double Duration { get; set; } = 2;

        /// <summary>
        /// Gets or sets the background for tooltip. This is bindable property.
        /// </summary>
        public static readonly new BindableProperty BackgroundProperty =
            BindableProperty.Create(nameof(Background), typeof(Brush), typeof(SfTooltip), new SolidColorBrush(Colors.Black),
                                    propertyChanged: OnBackgroundPropertyChanged);
        /// <summary>
        /// Gets or sets the background for tooltip.
        /// </summary>
        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        #endregion

        #region Events

        /// <summary>
        /// It represents the tooltip closed event handler. This tooltip closed event is hooked when tooltip is disappear from the visibility.
        /// </summary>
        public event EventHandler<TooltipClosedEventArgs>? TooltipClosed;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new instance of the <see cref="SfTooltip"/> class.
        /// </summary>
        public SfTooltip()
        {
            parentView = new Grid();
            drawableView = new TooltipDrawableView(this);
            contentView = new ContentView();
            tooltipHelper = new TooltipHelper(drawableView.InvalidateDrawable);
            parentView.Add(drawableView);
            parentView.Add(contentView);

            base.Content = parentView;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shows the tooltip based on target and container rectangle.
        /// </summary>
        /// <param name="containerRect"></param>
        /// <param name="targetRect"></param>
        /// <param name="animated"></param>
        public void Show(Rect containerRect, Rect targetRect, bool animated)
        {
            if (containerRect.IsEmpty || targetRect.IsEmpty || Content == null) return;

            var x = containerRect.X;
            var y = containerRect.Y;
            var width = containerRect.Width;
            var height = containerRect.Height;

            if (targetRect.X > x + width || targetRect.Y > y + height) return;

            tooltipHelper.Position = Position;
            tooltipHelper.Duration = Duration;
            tooltipHelper.Background = Background;

            if (isTooltipActivate)
            {
                isDisappeared = false;
            }

            if (this.Opacity == 0f)
                this.Opacity = 1f;

            Content.VerticalOptions = LayoutOptions.Start;
            Content.HorizontalOptions = LayoutOptions.Start;
            tooltipHelper.ContentSize = Content.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins).Request;
            tooltipHelper.Show(containerRect, targetRect, false);
            SetContentMargin(tooltipHelper.ContentViewMargin);
            AbsoluteLayout.SetLayoutBounds(this, tooltipHelper.TooltipRect);
            drawableView.InvalidateDrawable();

            isTooltipActivate = true;

            if (animated)
            {
                ShowAnimation();
            }
            else
            {
                AutoHide();
            }
        }

        IAnimationManager? animationManager = null;

        /// <summary>
        /// Hides the tooltip.
        /// </summary>
        /// <param name="animated"></param>
        public void Hide(bool animated)
        {
            this.AbortAnimation(durationAnimation);
            this.Opacity = 0f;
            AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 0, 1, 1));
            isTooltipActivate = false;
            TooltipClosed?.Invoke(this, new TooltipClosedEventArgs() { IsCompleted = isDisappeared });
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        internal void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Draw(canvas);
        }

        void Draw(ICanvas canvas)
        {
            if (tooltipHelper.RoundedRect == Rect.Zero) return;
            tooltipHelper.Draw(canvas);
        }

        void ShowAnimation()
        {
            SetAnimationManager();
            if (animationManager != null)
            {
                var animation = new Animation(UpdateToolTipAnimation, start: 0, 0.25, Easing.SpringOut, AutoHide);
                animation.Commit(animationManager);
            }
        }

        void SetAnimationManager()
        {
            if (Application.Current != null && animationManager == null)
            {
                var handler = Application.Current.Handler;
                if (handler != null && handler.MauiContext != null)
                    animationManager = handler.MauiContext.Services.GetRequiredService<IAnimationManager>();
            }
        }
        
        private void UpdateToolTipAnimation(double value)
        {
            Scale = value;
        }

        void AutoHide()
        {
            this.AbortAnimation(durationAnimation);
            var duration = tooltipHelper.Duration;

            if (double.IsFinite(duration) && duration > 0)
            {
                ControlAnimation animation = new ControlAnimation();
                animation.Commit(this, durationAnimation, 16, (uint)(tooltipHelper.Duration * 1000), Easing.Linear, Hide, () => false);
            }
        }

        void Hide(double value, bool isCompleted)
        {
            isDisappeared = !isCompleted;

            if (!isCompleted)
                Hide(false);
        }

        #region ContentView Methods

        /// <summary>
        /// Invoked when binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (Content != null)
            {
                SetInheritedBindingContext(Content, BindingContext);
            }
        }


        private static void OnBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        /// <summary>
        /// Updated margin for <see cref="ContentView"/>.
        /// </summary>
        /// <param name="thickness"></param>
        void SetContentMargin(Thickness thickness)
        {
            if (contentView != null)
            {
                contentView.Margin = thickness;
            }
        }

        void OnContentChanged()
        {
            if (Content != null)
            {
                SetInheritedBindingContext(Content, BindingContext);
            }

            contentView.Content = Content;
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// This class represents a drawable view used draw the tooltip using native drawing options. 
    /// </summary>
    internal class TooltipDrawableView : SfDrawableView
    {
        SfTooltip tooltip;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            tooltip.Draw(canvas, dirtyRect);
        }

        internal TooltipDrawableView(SfTooltip sfTooltip)
        {
            tooltip = sfTooltip;
        }
    }
}
