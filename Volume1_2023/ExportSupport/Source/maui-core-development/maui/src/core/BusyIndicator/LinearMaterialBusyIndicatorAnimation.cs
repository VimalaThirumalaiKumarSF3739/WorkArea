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
    internal class LinearMaterialBusyIndicatorAnimation : BusyIndicatorAnimation
    {
        #region Fields

        private double a1, a2, b1, b2 = 0d;

        private double y;

        private Point aPoint1 = new Point(0, 0);

        private Point aPoint2 = new Point(0, 0);

        private Point bPoint1 = new Point(0, 0);

        private Point bPoint2 = new Point(0, 0);

        private int strokeSize = 12;

        private bool isExpand = true;

        private double lineLength = 5;

        private int stepLength = 3;

        private int stepExpotentialLength = 5;

        private int maximumLineLength = 250;

        private int minimumLineLength = 30;

        #endregion

        #region Constructor

        public LinearMaterialBusyIndicatorAnimation(IAnimationManager animationManagerValue) : base(animationManagerValue)
        {
            this.DefaultDuration = 0;
        }

        #endregion

        #region Override Methods

        protected override void OnDrawAnimation(SfView view, ICanvas canvas)
        {
            base.OnDrawAnimation(view, canvas);

            if (this.drawableView == null)
                return;

            y = this.drawableView.Height / 2;

            this.b2 = this.a2 - this.drawableView.Width;
            this.b1 = this.a1 - this.drawableView.Width;

            this.aPoint1.X = a1;
            this.aPoint2.X = a2;
            this.bPoint1.X = b1;
            this.bPoint2.X = b2;

            this.aPoint1.Y = this.aPoint2.Y = this.bPoint1.Y = this.bPoint2.Y = this.y;
            canvas.StrokeColor = this.Color;
            canvas.StrokeSize = this.strokeSize * (float)this.sizeFactor;

            canvas.DrawLine(this.aPoint1, this.aPoint2);
            canvas.DrawLine(this.bPoint1, this.bPoint2);

        }

        protected override void OnUpdateAnimation()
        {
            base.OnUpdateAnimation();

            if (this.drawableView == null)
                return;

            this.a2 = this.a2 + this.stepLength;
            this.a1 = this.a1 + this.stepLength;

            if(this.a2 > (this.drawableView.Width*2))
            {
                this.a2 = this.b2;
                this.a1 = this.b1;
            }

            this.SetExpandingValue();

            this.drawableView.InvalidateDrawable();
        }

        #endregion

        #region Methods

        private void SetExpandingValue()
        {
            this.lineLength = this.a2 - this.a1;

            if (this.lineLength <= this.minimumLineLength)       
                this.isExpand = true;
            
            else if (this.lineLength >= this.maximumLineLength)
                this.isExpand = false;         

            if (this.isExpand)
                this.a2 = this.a2 + this.stepExpotentialLength;
            else
                this.a1 = this.a1 + this.stepExpotentialLength;
        }

        #endregion
    }
}
