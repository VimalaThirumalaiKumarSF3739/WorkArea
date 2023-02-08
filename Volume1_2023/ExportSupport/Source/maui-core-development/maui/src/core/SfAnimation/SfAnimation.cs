using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Animations;
using System;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// <see cref="SfAnimation"/> is used to create a animatable view over a certain time period 
    /// with customized options. 
    /// </summary>
    internal class SfAnimation : Microsoft.Maui.Animations.Animation
    {
        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the SfAnimation class.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="step"></param>
        /// <param name="finished"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="easing"></param>
        /// <param name="duration"></param>
        internal SfAnimation(IView view, Action<double>? step = null, Action? finished = null, double start = 0.0,
            double end = 1.0, Easing? easing = null, double duration = 1.0)
        {
            localView = view;
            Step = step;
            Finished = finished;
            Start = start;
            End = end;
            Easing = easing ?? Easing.Linear;
            Duration = duration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the animation begin value.
        /// </summary>
        /// <value>
        /// Defaults to 0.0.
        /// </value>
        internal double Start { get; set; }

        /// <summary>
        /// Gets or sets the animation end value.
        /// </summary>
        /// <value>
        /// Defaults to 1.0.
        /// </value>
        internal double End { get; set; }

        #endregion

        #region Private Fields

        private readonly IView localView;
        private bool isForwarding = false;
        private bool isReversing = false;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Forwards the animation value from Start to End.
        /// </summary>
        internal void Forward()
        {
            animationManger ??= GetAnimationManager(localView.Handler!.MauiContext!);
            if (!isForwarding)
            {
                Pause();
                CurrentTime = Progress == 0 ? 0.0 : Progress * Duration + StartDelay;
                HasFinished = false;
                isForwarding = true;
                isReversing = false;
                Resume();
            }
        }

        /// <summary>
        /// Reverses the animation value from End to Start.
        /// </summary>
        internal void Reverse()
        {
            animationManger ??= GetAnimationManager(localView.Handler!.MauiContext!);
            if (!isReversing)
            {
                Pause();
                CurrentTime = Progress == 1 ? 0.0 : (1 - Progress) * Duration + StartDelay;
                HasFinished = false;
                isForwarding = false;
                isReversing = true;
                Resume();
            }
        }

        #endregion

        #region Override Methods

        /// <inheritdoc/>
        public override void Reset()
        {
            base.Reset();
            Progress = 0.0;
            isForwarding = false;
            isReversing = false;
        }

        /// <inheritdoc/>
        protected override void OnTick(double millisecondsSinceLastUpdate)
        {
            if (HasFinished)
            {
                return;
            }

            double secondsSinceLastUpdate = millisecondsSinceLastUpdate / 1000.0;
            CurrentTime += secondsSinceLastUpdate;
            if (CurrentTime < StartDelay)
            {
                return;
            }

            double start = CurrentTime - StartDelay;
            double animatingPercent = Math.Min(start / Duration, 1);
            double percent = isForwarding ? animatingPercent : 1 - animatingPercent;
            Update(percent);

            if (HasFinished)
            {
                Finished?.Invoke();
                isForwarding = false;
                isReversing = false;
                if (Repeats)
                {
                    Reset();
                }
            }
        }

        /// <inheritdoc/>
        public override void Update(double percent)
        {
            try
            {
                Progress = Easing.Ease(percent);
                Step?.Invoke(GetAnimatingValue());
                HasFinished = isForwarding ? percent == 1 : percent == 0;
            }
            catch (Exception e)
            {
                HasFinished = true;
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        }

        #endregion

        #region Private Method

        private IAnimationManager GetAnimationManager(IMauiContext mauiContext)
        {
            return mauiContext.Services.GetRequiredService<IAnimationManager>();
        }

        private double GetAnimatingValue()
        {
            return Start + (End - Start) * Progress;
        }

        #endregion
    }
}