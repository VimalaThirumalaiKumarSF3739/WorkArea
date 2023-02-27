using Microsoft.Maui.Animations;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core
{
    internal abstract class BusyIndicatorAnimation : Microsoft.Maui.Animations.Animation
    {
        #region Fields

        internal SfView? drawableView;

        double secondsSinceLastUpdate;

        private double actualDuration = 0;

        internal double sizeFactor = 0.5;

        internal Rect actualRect = new();

        internal double defaultHeight = 0;

        internal double defaultWidth = 0;

        private double defaultDuration = 100;

        private double animationduration = 0.5;

        private Color color = Color.FromArgb("#FF512BD4");

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        internal double DefaultDuration
        {
            get { return this.defaultDuration; }
            set
            {
                this.defaultDuration = value;
                this.SetActualDuration();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        internal double AnimationDuration
        {
            get
            {
                return animationduration;
            }
            set
            {
                this.animationduration = value;

                if (animationduration > 1)
                    animationduration = 1;

                this.SetActualDuration();
            }
        }

        internal Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        #endregion;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BusyIndicatorAnimation"/> class.
        /// </summary>
        /// /// <param name="animationManagerValue"></param>
        public BusyIndicatorAnimation(IAnimationManager animationManagerValue)
        {
            this.animationManger = animationManagerValue;
            this.Easing = Microsoft.Maui.Easing.SinIn;
            this.Repeats = true;

            this.SetActualDuration();
        }

        #endregion

        #region Virtual Methods

        protected virtual void OnDrawAnimation(SfView view, ICanvas canvas)
        {

        }

        protected virtual void OnUpdateAnimation()
        {

        }

        #endregion

        #region Override Methods

        protected override void OnTick(double millisecondsSinceLastUpdate)
        {
            base.OnTick(millisecondsSinceLastUpdate);

            this.secondsSinceLastUpdate += millisecondsSinceLastUpdate;

            if (this.secondsSinceLastUpdate > this.actualDuration)
            {
                this.UpdateActualRect();
                this.OnUpdateAnimation();

                this.secondsSinceLastUpdate = 0;
            }
        }

        #endregion

        #region Methods

        internal void UpdateActualRect()
        {
            if (drawableView == null)
                return;

            double centerX1 = this.drawableView.Width / 2;
            double centerY1 = this.drawableView.Height/2;

            double centerX2 = this.defaultWidth / 2;
            double centerY2 = this.defaultHeight / 2;

            centerX2 = centerX2 * sizeFactor;
            centerY2 = centerY2 * sizeFactor;

            this.actualRect.X = centerX1 - centerX2;
            this.actualRect.Y = centerY1 - centerY2;
            this.actualRect.Width = this.defaultWidth * this.sizeFactor;
            this.actualRect.Height = this.defaultHeight * this.sizeFactor;

        }

        internal void DrawAnimation(SfView view, ICanvas canvas)
        {
            if (this.drawableView == null)
            {
                this.drawableView = view;
            }
           
            this.OnDrawAnimation(view, canvas);
        }

        private void SetActualDuration()
        {
            this.actualDuration = this.AnimationDuration * this.DefaultDuration;
        }

        internal void RunAnimation(bool canStart)
        {
            this.HasFinished = !canStart;

            if (canStart)
                this.Resume();
            else
                this.Pause();
        }
    
        #endregion
    }
}
