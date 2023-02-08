// <copyright file="RippleEffectLayer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Maui;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Devices;
    using Microsoft.Maui.Graphics;
    using Syncfusion.Maui.Graphics.Internals;

    /// <summary>
    /// Represents the RippleEffectLayer class.
    /// </summary>
    internal class RippleEffectLayer 
    {
        #region Fields

        private const float RippleTransparencyFactor = 0.12f;
        private float rippleDiameter;
        private readonly string rippleAnimatorName = "RippleAnimator";
        private readonly string fadeOutName = "RippleFadeOut";
        private Point touchPoint;
        private double animationAreaLength;
        private float alphaValue;
        private bool fadeOutRipple;
        private Brush rippleColor = new SolidColorBrush(Colors.Black);
        private double rippleAnimationDuration;
        private readonly float minAnimationDuration = 1f;
        private bool removeRippleAnimation;
        private readonly IDrawable drawable;
        private readonly IAnimatable animation;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RippleEffectLayer"/> class.
        /// </summary>
        /// <param name="rippleColor">The ripple color</param>
        /// <param name="rippleDuration">The ripple duration</param>
        /// <param name="drawable">drawable</param>
        /// <param name="animate">animate</param>
        public RippleEffectLayer(Brush rippleColor, double rippleDuration,IDrawable drawable,IAnimatable animate)
        {
            this.rippleColor = rippleColor;
            this.rippleAnimationDuration = rippleDuration;
            this.drawable = drawable;
            this.animation = animate;
            this.alphaValue = RippleTransparencyFactor;
        }

        #endregion

        #region Properties

        internal double Width { get; set; }

        internal double Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to clear the animation.
        /// </summary>
        internal bool CanRemoveRippleAnimation
        {
            get { return this.removeRippleAnimation; }
            set { this.removeRippleAnimation = value; }
        }

        /// <summary>
        /// Gets the ripple fade in and fade out animation duration in milliseconds.
        /// </summary>
        private float RippleFadeInOutAnimationDuration
        {
            get
            {
                return (float)((this.rippleAnimationDuration < this.minAnimationDuration
                    ? this.minAnimationDuration : this.rippleAnimationDuration) / 4);
            }
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="dirtyRect"></param>
        internal void DrawRipple(ICanvas canvas, RectF dirtyRect)
        {
            if (this.rippleColor != null)
            {
                canvas.Alpha = this.alphaValue;
                DrawRipple(canvas, dirtyRect, this.rippleColor, false);
            }
        }


       /// <summary>
       /// 
       /// </summary>
       /// <param name="canvas"></param>
       /// <param name="dirtyRect"></param>
       /// <param name="color"></param>
       /// <param name="clipBounds"></param>
        internal void DrawRipple(ICanvas canvas, RectF dirtyRect,Brush color, bool clipBounds = false)
        {
            if (this.rippleColor != null)
            {
                canvas.SetFillPaint(color, dirtyRect);
                if (clipBounds)
                    this.ExpandRippleEllipse(canvas, dirtyRect);
                else
                    this.ExpandRippleEllipse(canvas);
            }
        }

        /// <summary>
        /// Start ripple animation method.
        /// </summary>
        /// <param name="point">The touch point.</param>
        /// <param name="rippleColor">The ripple color.</param>
        /// <param name="rippleAnimationDuration">The ripple aniamtion duration.</param>
        /// <param name="initialRippleFactor">The initial ripple factor value.</param>
        /// <param name="fadeoutRipple">The fadeout ripple property.</param>
        /// <param name="canRepeat">The can repeat value.</param>
        internal void StartRippleAnimation(Point point, Brush rippleColor, double rippleAnimationDuration, float initialRippleFactor, bool fadeoutRipple, bool canRepeat = false)
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI && ((drawable as IVisualElementController)?.EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
            {
                this.touchPoint = new Point(this.Width- point.X, point.Y);
            }
            else
            {
                this.touchPoint = point;
            }
            
            this.rippleColor = rippleColor;
            this.rippleAnimationDuration = rippleAnimationDuration;
            this.fadeOutRipple = fadeoutRipple;
            this.alphaValue = RippleTransparencyFactor;
            double initialRippleRadius = this.GetRippleRadiusFromFactor(initialRippleFactor);
            this.animationAreaLength = this.GetFinalRadius(point);

            var rippleRadiusAnimation = new Animation(this.OnRippleAnimationUpdate, initialRippleRadius, this.animationAreaLength);
            rippleRadiusAnimation.Commit(
                animation,
                this.rippleAnimatorName,
                length: (uint)rippleAnimationDuration,
                easing: Easing.Linear,
                finished: this.OnRippleFinished,
                repeat: () => canRepeat);

            if (fadeoutRipple)
            {
                var fadeOutAnimation = new Animation(this.OnFadeAnimationUpdate, 0, this.alphaValue);
                fadeOutAnimation.Commit(
                    animation,
                    this.fadeOutName,
                    length: (uint)this.RippleFadeInOutAnimationDuration,
                    easing: Easing.Linear,
                    finished: null,
                    repeat: () => canRepeat);
                rippleRadiusAnimation.WithConcurrent(fadeOutAnimation);
            }
        }

        /// <summary>
        /// Fadeanimation update method.
        /// </summary>
        /// <param name="value">The animation update value.</param>
        internal void OnFadeAnimationUpdate(double value)
        {
            this.alphaValue = (float)value;
            this.InvalidateDrawable();
        }

        /// <summary>
        /// Ripple animation update method.
        /// </summary>
        /// <param name="value">Animation update value.</param>
        internal void OnRippleAnimationUpdate(double value)
        {
            this.rippleDiameter = (float)value;
            this.InvalidateDrawable();
        }

        /// <summary>
        /// Ripple animation finished method.
        /// </summary>
        internal void OnRippleAnimationFinished()
        {
            AnimationExtensions.AbortAnimation(animation, this.rippleAnimatorName);
            this.rippleDiameter = 0;
            this.InvalidateDrawable();
        }

        #endregion

        #region Private methods

        private void InvalidateDrawable()
        {
            if (drawable is IDrawableLayout drawableLayout)
                drawableLayout.InvalidateDrawable();
            else if (drawable is IDrawableView drawableView)
                drawableView.InvalidateDrawable();
        }


        private Microsoft.Maui.IElement? GetParent()
        {
            if (drawable is IDrawableLayout drawableLayout)
                return drawableLayout!;
            else if (drawable is IDrawableView drawableView)
              return  drawableView;

            return null;
        }
        /// <summary>
        /// Method used to initial radius for ripple.
        /// </summary>
        /// <param name="initialRippleFactor">The initial ripple factor.</param>
        /// <returns>Returns the converted radius value.</returns>
        private float GetRippleRadiusFromFactor(float initialRippleFactor)
        {
            if (this.Width > 0 && this.Height > 0)
            {
                return (float)(Math.Min(this.Width, this.Height) / 2 * initialRippleFactor);
            }
            else if (GetParent() is View  parent && parent.Width > 0 && parent.Height > 0)
            {
                float parentWidth = (float)parent.Width;
                float parentHeight = (float)parent.Height;
                return (float)(Math.Min(parentWidth, parentHeight) / 2 * initialRippleFactor);
            }

            return 0;
        }

        /// <summary>
        /// Get the maximum radius based on the pythagoras theorem in the view.
        /// </summary>
        /// <param name="pivot">The touch point.</param>
        /// <returns>Final radius.</returns>
        private float GetFinalRadius(Point pivot)
        {
            if (this.Width > 0 && this.Height > 0)
            {
                float width = (float)(pivot.X > this.Width / 2 ? pivot.X : this.Width - pivot.X);
                float height = (float)(pivot.Y > this.Height / 2 ? pivot.Y : this.Height - pivot.Y);
                return (float)Math.Sqrt((width * width) + (height * height));
            }
            else if (GetParent() is View parent && parent.Width > 0 && parent.Height > 0)
            {
                float parentWidth = (float)parent.Width;
                float parentHeight = (float)parent.Height;
                float width = (float)(pivot.X > parentWidth / 2 ? pivot.X : parentWidth - pivot.X);
                float height = (float)(pivot.Y > parentHeight / 2 ? pivot.Y : parentHeight - pivot.Y);
                return (float)Math.Sqrt((width * width) + (height * height));
            }
            else
            {
                return (float)Math.Sqrt((pivot.X * pivot.X) + (pivot.Y * pivot.Y));
            }
        }

        /// <summary>
        /// Expand ripple ellipse method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        private void ExpandRippleEllipse(ICanvas canvas)
        {
            canvas.FillCircle((float)this.touchPoint.X, (float)this.touchPoint.Y, this.rippleDiameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rect"></param>
        private void ExpandRippleEllipse(ICanvas canvas, Rect rect)
        {
            canvas.SaveState();
            canvas.ClipRectangle(rect);
            canvas.FillCircle((float)this.touchPoint.X, (float)this.touchPoint.Y, this.rippleDiameter);
            canvas.RestoreState();
            canvas.ResetState();
        }

        /// <summary>
        /// Ripple Animation finished method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="isCompleted">The completed property.</param>
        private void OnRippleFinished(double value, bool isCompleted)
        {
            if (this.CanRemoveRippleAnimation)
            {
                AnimationExtensions.AbortAnimation(animation, this.rippleAnimatorName);
                this.rippleDiameter = 0;
                this.InvalidateDrawable();
            }

             if (this.CanRemoveRippleAnimation || !animation.AnimationIsRunning(this.rippleAnimatorName))
             {
                if (GetParent() != null && ((this.drawable as View) as SfEffectsView) != null)
                {
                    if ((GetParent() as View) is SfEffectsView effectView && ((effectView.TouchUpEffects == SfEffects.None || effectView.AutoResetEffects.GetAllItems().Contains(AutoResetEffects.Ripple) || effectView.TouchUpEffects == SfEffects.Ripple || effectView.TouchUpEffects.GetAllItems().Contains(SfEffects.Ripple) || effectView.TouchUpEffects.GetAllItems().Contains(SfEffects.None)) &&
                        (effectView.LongPressEffects.GetAllItems().Contains(SfEffects.None) || !effectView.LongPressHandled || effectView.LongPressEffects.GetAllItems().Contains(SfEffects.Ripple))))
                    {
                        effectView?.InvokeAnimationCompletedEvent();
                    }
                }
            }
        }    

        #endregion
    }
}
