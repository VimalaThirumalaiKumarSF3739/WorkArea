using Microsoft.Maui.Animations;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.BusyIndicator
{
    internal class SingleCircleBusyIndictorAnimation : BusyIndicatorAnimation
    {
        #region Fields

        private readonly float itemNumber = 8;

        private readonly float lineSize = 12;

        private float startAngle = 0;

        private float endAngle = -40;

        private float count = 10;

        #endregion

        #region Constructor
        public SingleCircleBusyIndictorAnimation(IAnimationManager animationManagerValue) : base(animationManagerValue)
        {
            this.defaultWidth = 75;
            this.defaultHeight = 75;
        }
        #endregion

        #region Override Mehtods
        protected override void OnDrawAnimation(SfView view, ICanvas canvas)
        {
            base.OnDrawAnimation(view, canvas);

            canvas.StrokeSize = this.lineSize * (float)this.sizeFactor;
            canvas.StrokeColor = this.Color;

            for (int i = 0; i < itemNumber; i++)
            {
                canvas.DrawArc(actualRect, startAngle - count, endAngle - count, true, false);
                startAngle = endAngle - 5;
                endAngle = startAngle - 40;
            }

        }

        protected override void OnUpdateAnimation()
        {
            base.OnUpdateAnimation();

            count += 1;
            if (count > 360)
            {
                count = 1;
            }

            this.drawableView?.InvalidateDrawable();
        }
        #endregion
    }
}
