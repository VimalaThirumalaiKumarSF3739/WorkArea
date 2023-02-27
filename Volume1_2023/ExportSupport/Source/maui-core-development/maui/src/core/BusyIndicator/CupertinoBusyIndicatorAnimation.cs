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
    internal class CupertinoBusyIndicatorAnimation : BusyIndicatorAnimation
    {

        #region Fields

        private readonly float itemNumber = 12;

        private float alphaValue = 255f;

        private readonly int lineSize = 6;

        private readonly float rotateAngle = 30f;

        private readonly float minimumAlphaValue = 150;

        private readonly float alphaStepValue = 30f;

        private readonly float strokeSize = 10;

        private int currentLine = 0;

        private readonly float defaultAlphaValue = 1;

        #endregion

        #region Constructor

        public CupertinoBusyIndicatorAnimation(IAnimationManager animationManagerValue ): base(animationManagerValue)
        {
            this.DefaultDuration = 300;
            this.defaultHeight = 100;
            this.defaultWidth = 100;

#if IOS || MACCATALYST
            this.minimumAlphaValue = 0.2f;
            this.alphaStepValue = 0.05f;
#endif
        }

        #endregion

        #region Override Methods

        protected override void OnDrawAnimation(SfView view, ICanvas canvas)
        {
            base.OnDrawAnimation(view, canvas);

            canvas.StrokeSize = this.lineSize;
            canvas.StrokeLineCap = LineCap.Round;

            this.DrawLines(canvas,this.actualRect);

        }

        protected override void OnUpdateAnimation()
        {
            base.OnUpdateAnimation();

            this.UpdateAnimation();
            this.drawableView?.InvalidateDrawable();
        }

        #endregion

        #region Methods

        private void DrawLines(ICanvas canvas, RectF bounds)
        {
            canvas.Translate(bounds.Center.X, bounds.Center.Y);
          
            canvas.Rotate(this.currentLine * this.rotateAngle);
            
#if IOS || MACCATALYST
            this.alphaValue = 1f;
#else
            this.alphaValue = 1;
#endif
            canvas.StrokeColor = this.Color;
            canvas.StrokeSize = this.strokeSize * (float)this.sizeFactor;
            for (int i = 0; i < this.itemNumber; i++)
            {

#if IOS || MACCATALYST
                if (this.alphaValue >= this.minimumAlphaValue)
                    this.alphaValue -= this.alphaStepValue;
#else
                if (this.alphaValue <= this.minimumAlphaValue)
                    this.alphaValue += this.alphaStepValue;
#endif
                canvas.Alpha = this.alphaValue;
                canvas.DrawLine(bounds.Height / 4, 0, bounds.Height / 2, 0);
                canvas.Rotate(-this.rotateAngle);
            }
            canvas.Rotate(this.itemNumber - this.currentLine * this.rotateAngle);

            canvas.Rotate(-this.rotateAngle / 2.5f);

            canvas.Translate(-bounds.Center.X, -bounds.Center.Y);
            canvas.Alpha = this.defaultAlphaValue;

        }

        private void UpdateAnimation()
        {
            if (this.currentLine >= this.itemNumber)       
                this.currentLine = 0;          
            else
                this.currentLine++;
        }

#endregion
    }

}
