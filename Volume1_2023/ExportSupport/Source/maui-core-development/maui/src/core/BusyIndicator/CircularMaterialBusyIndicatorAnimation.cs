using Microsoft.Maui.Animations;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal class CircularMaterialBusyIndicatorAnimation : BusyIndicatorAnimation
    {
        #region Fields

        private float materialStartAngle = 0;

        private float materialEndAngle = -30;

        private readonly float stepLength = 7;

        private readonly float maximumStepLength = 30;

        private float arcLength = 30;

        private bool isCollapsing = false;
        
        private float easingValue = 0.1f;

        private bool isReset = false;

        private readonly float strokeSize = 10;

        private readonly float maximumArcLength = 330;

        private readonly float minimumArchLength = 120;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularMaterialBusyIndicatorAnimation"/> class.
        /// </summary>
        /// <param name="animationManagerValue"></param>
        public CircularMaterialBusyIndicatorAnimation(IAnimationManager animationManagerValue) : base(animationManagerValue)
        {
            this.DefaultDuration = 50;
            this.defaultHeight = 75;
            this.defaultWidth = 75;
        }

        #endregion

        #region Override Methods

        protected override void OnDrawAnimation(SfView view, ICanvas canvas)
        {
            base.OnDrawAnimation(view, canvas);

            canvas.StrokeColor = this.Color;
            canvas.StrokeSize = this.strokeSize * (float)this.sizeFactor;

            canvas.DrawArc(this.actualRect, this.materialStartAngle, this.materialEndAngle, false, false);

        }

        protected override void OnUpdateAnimation()
        {
            base.OnUpdateAnimation();

            this.UpdateAnimation();
            this.drawableView?.InvalidateDrawable();
        }

        #endregion

        #region Methods

        private void CheckArcLength()
        {
            if (arcLength < this.minimumArchLength)
            {
                isReset = isCollapsing;
                isCollapsing = true;
            }
            else if (arcLength > this.maximumArcLength)
            {
                isReset = isCollapsing;
                isCollapsing = false;
            }
        }

        private void CheckEasingValue()
        {
            if (isReset != isCollapsing)
            {
                isReset = isCollapsing;
                easingValue = 0.1f;
            }

            this.easingValue = this.easingValue + this.easingValue;

            if (easingValue > this.maximumStepLength)
                easingValue = this.maximumStepLength;
        }

        private void UpdateAnimation()
        {
            materialStartAngle = materialStartAngle - this.stepLength;
            materialEndAngle = materialEndAngle - this.stepLength;

            arcLength = materialStartAngle - materialEndAngle;

            CheckArcLength();
            CheckEasingValue();

            if (isCollapsing)
                materialEndAngle = materialEndAngle - easingValue;             
            else
               materialStartAngle = materialStartAngle - easingValue;

        }

        #endregion

    }
}
