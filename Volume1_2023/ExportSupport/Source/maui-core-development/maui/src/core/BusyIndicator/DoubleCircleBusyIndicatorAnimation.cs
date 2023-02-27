using Microsoft.Maui.Animations;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.BusyIndicator
{
    internal class DoubleCircleBusyIndicatorAnimation : BusyIndicatorAnimation
    {
        #region Fields
        private readonly float itemNumber = 4;

        private float startAngle = 0;

        private float endAngle = -75;

        private float count = 15;

        private readonly float innerStroke = 6;

        private readonly float outerStroke = 8;

        private double centerX1;

        private double centerY1;

        private double innerCenterX2;

        private double innerCenterY2;

        private double outerCenterX2;

        private double outerCenterY2;

        private double innerX2;

        private double innerY2;

        private double outerX2;

        private double outerY2;

        #endregion

        #region Constructor
        public DoubleCircleBusyIndicatorAnimation(IAnimationManager animationManagerValue,SfView view) : base(animationManagerValue)
        {
            this.defaultHeight = 75;
            this.defaultWidth = 75;
            this.CalculateXYPositions(view);
        }
        #endregion

        #region Override Methods
        protected override void OnDrawAnimation(SfView view, ICanvas canvas)
        {
            base.OnDrawAnimation(view, canvas);
            canvas.StrokeColor = this.Color;

            for (int i = 0; i < itemNumber; i++)
            {
                this.innerCenterX2 = this.innerX2;
                this.innerCenterY2 = this.innerY2;
                this.outerCenterX2 = this.outerX2;
                this.outerCenterY2 = this.outerY2;
                
                this.innerCenterX2 = innerCenterX2 * sizeFactor;
                this.innerCenterY2 = innerCenterY2 * sizeFactor;

                this.actualRect.X = centerX1 - innerCenterX2;
                this.actualRect.Y = centerY1 - innerCenterY2;
                this.actualRect.Width = (this.defaultWidth * sizeFactor * 2) / 3;
                this.actualRect.Height = (this.defaultHeight * sizeFactor * 2) / 3;

                canvas.StrokeSize = innerStroke * (float)this.sizeFactor;
                canvas.DrawArc(actualRect, -(startAngle + count), -(endAngle + count), false, false);

                this.outerCenterX2 = outerCenterX2 * sizeFactor;
                this.outerCenterY2 = outerCenterY2 * sizeFactor;

                this.actualRect.X = centerX1 - outerCenterY2;
                this.actualRect.Y = centerY1 - outerCenterY2;
                this.actualRect.Width = this.defaultWidth * sizeFactor;
                this.actualRect.Height = this.defaultHeight * sizeFactor;

                canvas.StrokeSize = outerStroke * (float)this.sizeFactor;
                canvas.DrawArc(actualRect, startAngle + count, endAngle + count , true, false);
                startAngle = endAngle - 15;
                endAngle = startAngle - 75;
            }

        }

        protected override void OnUpdateAnimation()
        {
            base.OnUpdateAnimation();

            count += 15;
            if (count > 360)
            {
                count = 15;
            }

            this.drawableView?.InvalidateDrawable();
        }
        #endregion

        #region Method
        internal void CalculateXYPositions(SfView view)
        {
            if (view == null)
                return;
            this.centerX1 = view.Width / 2;
            this.centerY1 = view.Height / 2;

            this.innerX2 = this.defaultWidth / 3;
            this.innerY2 = this.defaultHeight / 3;

            this.outerX2 = this.defaultWidth / 2;
            this.outerY2 = this.defaultHeight / 2;

        }
        #endregion
    }
}
