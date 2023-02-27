using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;

namespace Syncfusion.Maui.Shimmer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SfShimmer"/> class. 
    /// Represents a loading indicator control that provides modern animations when data is being loaded.
    /// </summary>
    public class SfShimmer : SfContentView, IShimmer
    {
        #region Bindable property

        /// <summary>
        /// Identifies the <see cref="AnimationDuration"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AnimationDuration"/> bindable property.
        /// </value>
        public static readonly BindableProperty AnimationDurationProperty =
            BindableProperty.Create(nameof(AnimationDuration), typeof(double), typeof(SfShimmer), 1000d, propertyChanged: OnAnimationDurationChanged);

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> bindable property.
        /// </value>
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(SfShimmer), null, defaultValueCreator: bindable => new SolidColorBrush(Color.FromArgb("#F3EDF7")), propertyChanged: OnFillPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CustomView"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CustomView"/> bindable property.
        /// </value>
        public static readonly BindableProperty CustomViewProperty =
            BindableProperty.Create(nameof(CustomView), typeof(View), typeof(SfShimmer), null, propertyChanged: OnCustomViewChanged);

        /// <summary>
        /// Identifies the <see cref="IsActive"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsActive"/> bindable property.
        /// </value>
        public static readonly BindableProperty IsActiveProperty =
            BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(SfShimmer), true, propertyChanged: OnIsActivePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="WaveColor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="WaveColor"/> bindable property.
        /// </value>
        public static readonly BindableProperty WaveColorProperty =
            BindableProperty.Create(nameof(WaveColor), typeof(Color), typeof(SfShimmer), Color.FromArgb("#FFFFFF"));

        /// <summary>
        /// Identifies the <see cref="WaveWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="WaveWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty WaveWidthProperty =
            BindableProperty.Create(nameof(WaveWidth), typeof(double), typeof(SfShimmer), 200d, propertyChanged: OnWaveWidthChanged);

        /// <summary>
        /// Identifies the <see cref="Type"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Type"/> bindable property.
        /// </value>
        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create(nameof(Type), typeof(ShimmerType), typeof(SfShimmer), ShimmerType.CirclePersona, propertyChanged: OnTypeChanged);

        /// <summary>
        /// Identifies the <see cref="WaveDirection"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="WaveDirection"/> bindable property.
        /// </value>
        public static readonly BindableProperty WaveDirectionProperty =
            BindableProperty.Create(nameof(WaveDirection), typeof(ShimmerWaveDirection), typeof(SfShimmer), ShimmerWaveDirection.Default, propertyChanged: OnWaveDirectionChanged);

        /// <summary>
        /// Identifies the <see cref="RepeatCount"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RepeatCount"/> bindable property.
        /// </value>
        public static readonly BindableProperty RepeatCountProperty =
            BindableProperty.Create(nameof(RepeatCount), typeof(int), typeof(SfShimmer), 1, propertyChanged: OnRepeatCountPropertyChanged);

        #endregion

        #region Fields

        /// <summary>
        /// Backing field to store the <see cref="shimmerDrawable"/> instance.
        /// </summary>
        private ShimmerDrawable? shimmerDrawable;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfShimmer"/> class.
        /// </summary>
        public SfShimmer()
        {
            this.shimmerDrawable = new ShimmerDrawable(this);
            this.Add(this.shimmerDrawable);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the duration of the wave animation in milliseconds.
        /// </summary>
        /// <value>The default value of <see cref="AnimationDuration"/> is 1000.</value>
        /// <example>
        /// The following code demonstrates, how to use the AnimationDuration property in the Shimmer.
        /// # [XAML](#tab/tabid-1)
        /// <code Lang="XAML"><![CDATA[
        /// <shimmer:SfShimmer x:Name="Shimmer"
        ///                    AnimationDuration="1500">
        /// </shimmer:SfShimmer>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code Lang="C#"><![CDATA[
        /// Shimmer.AnimationDuration = 1500;
        /// ]]></code>
        /// </example>
        public double AnimationDuration
        {
            get { return (double)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background color of shimmer view. 
        /// </summary>
        /// <value>The default value of <see cref="Fill"/> is "#F3EDF6".</value>
        /// <example>
        /// The following code demonstrates, how to use the Fill property in the Shimmer.
        /// # [XAML](#tab/tabid-3)
        /// <code Lang="XAML"><![CDATA[
        /// <shimmer:SfShimmer x:Name="Shimmer" 
        ///                    Fill="AliceBlue">
        /// </shimmer:SfShimmer>
        /// ]]></code>
        /// # [C#](#tab/tabid-4)
        /// <code Lang="C#"><![CDATA[
        /// Shimmer.Fill = Brush.AliceBlue;
        /// ]]></code>
        /// </example>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the custom view that is used for loading view.
        /// </summary>
        /// <value>The default value of <see cref="CustomView"/> is null.</value>
        /// <remarks>
        /// Custom view architecture will be shown only when the <see cref="Type"/> is <see cref="ShimmerType.Custom"/>.
        /// </remarks>
        /// <example>
        /// The following code demonstrates, how to use the CustomView property in the Shimmer.
        /// # [XAML](#tab/tabid-7)
        /// <code Lang="XAML"><![CDATA[
        ///  <shimmer:SfShimmer x:Name="Shimmer" 
        ///                     Type="Custom">
        ///      <shimmer:SfShimmer.CustomView>
        ///          <Grid HeightRequest="50" WidthRequest="200">
        ///              <Grid.RowDefinitions>
        ///                  <RowDefinition/>
        ///                  <RowDefinition/>
        ///              </Grid.RowDefinitions>
        ///              <Grid.ColumnDefinitions>
        ///                  <ColumnDefinition Width="0.25*"/>
        ///                  <ColumnDefinition Width="0.75*"/>
        ///              </Grid.ColumnDefinitions>
        /// 
        ///              <shimmer:ShimmerView ShapeType="Circle" Grid.RowSpan="2"/>
        ///              <shimmer:ShimmerView Grid.Column="1" Margin="5"/>
        ///              <shimmer:ShimmerView ShapeType="RoundedRectangle" Grid.Row="1" Grid.Column="1" Margin="5"/>
        ///          </Grid>
        ///      </shimmer:SfShimmer.CustomView>
        ///  </shimmer:SfShimmer>
        /// ]]></code>
        /// # [C#](#tab/tabid-8)
        /// <code Lang="C#"><![CDATA[
        /// Grid grid = new Grid
        /// {
        ///     HeightRequest = 50, 
        ///     WidthRequest = 200,
        ///     RowDefinitions =
        ///     {
        ///         new RowDefinition(),
        ///         new RowDefinition(),
        ///     },
        ///     ColumnDefinitions =
        ///     {
        ///         new ColumnDefinition { Width = new GridLength(0.25, GridUnitType.Star) },
        ///         new ColumnDefinition { Width = new GridLength(0.75, GridUnitType.Star) }
        ///     }
        /// };
        /// 
        /// ShimmerView circleView = new ShimmerView() { ShapeType = ShimmerShapeType.Circle};
        /// grid.SetRowSpan(circleView, 2);
        /// grid.Add(circleView);
        /// grid.Add(new ShimmerView { Margin = 5 }, 1);
        /// grid.Add(new ShimmerView { Margin = 5, ShapeType = ShimmerShapeType.RoundedRectangle }, 1, 1);
        /// ]]></code>
        /// </example>
        public View CustomView
        {
            get { return (View)GetValue(CustomViewProperty); }
            set { SetValue(CustomViewProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to load actual content of shimmer.
        /// </summary>
        /// <value>The default value of <see cref="IsActive"/> is true.</value>
        /// <example>
        /// The following code demonstrates, how to use the IsActive property in the Shimmer.
        /// # [XAML](#tab/tabid-9)
        /// <code Lang="XAML"><![CDATA[
        /// <shimmer:SfShimmer x:Name="Shimmer"
        ///                    IsActive="True">
        /// </shimmer:SfShimmer>
        /// ]]></code>
        /// # [C#](#tab/tabid-10)
        /// <code Lang="C#"><![CDATA[
        /// Shimmer.IsActive = true;
        /// ]]></code>
        /// </example>
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        /// <summary>
        /// Gets or sets the shimmer wave color.
        /// </summary>
        /// <value>The default value of <see cref="WaveColor"/> is "#FFFBFE".</value>
        /// <example>
        /// The following code demonstrates, how to use the WaveColor property in the Shimmer.
        /// # [XAML](#tab/tabid-11)
        /// <code Lang="XAML"><![CDATA[
        /// <shimmer:SfShimmer x:Name="Shimmer"
        ///                    WaveColor="AliceBlue">
        /// </shimmer:SfShimmer>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code Lang="C#"><![CDATA[
        /// Shimmer.WaveColor = Colors.AliceBlue;
        /// ]]></code>
        /// </example>
        public Color WaveColor
        {
            get { return (Color)GetValue(WaveColorProperty); }
            set { SetValue(WaveColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the wave.
        /// </summary>
        /// <value>The default value of <see cref="WaveWidth"/> is 200.</value>
        /// <example>
        /// The following code demonstrates, how to use the WaveWidth property in the Shimmer.
        /// # [XAML](#tab/tabid-13)
        /// <code Lang="XAML"><![CDATA[
        /// <shimmer:SfShimmer x:Name="Shimmer"
        ///                    WaveWidth="150">
        /// </shimmer:SfShimmer>
        /// ]]></code>
        /// # [C#](#tab/tabid-14)
        /// <code Lang="C#"><![CDATA[
        /// Shimmer.WaveWidth = 150;
        /// ]]></code>
        /// </example>
        public double WaveWidth
        {
            get { return (double)GetValue(WaveWidthProperty); }
            set { SetValue(WaveWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the built-in shimmer view type. 
        /// </summary>
        /// <value>The default value of <see cref="Type"/> is <see cref="ShimmerType.CirclePersona"/>.</value>
        /// <example>
        /// The following code demonstrates, how to use the Type property in the Shimmer.
        /// # [XAML](#tab/tabid-15)
        /// <code Lang="XAML"><![CDATA[
        /// <shimmer:SfShimmer x:Name="Shimmer"
        ///                    Type="Article">
        /// </shimmer:SfShimmer>
        /// ]]></code>
        /// # [C#](#tab/tabid-16)
        /// <code Lang="C#"><![CDATA[
        /// Shimmer.Type = ShimmerType.Article;
        /// ]]></code>
        /// </example>
        public ShimmerType Type
        {
            get { return (ShimmerType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the animation direction for Shimmer.
        /// </summary>
        /// <value>The default value of <see cref="WaveDirection"/> is <see cref="ShimmerWaveDirection.Default"/>.</value>
        /// <example>
        /// The following code demonstrates, how to use the WaveDirection property in the Shimmer.
        /// # [XAML](#tab/tabid-17)
        /// <code Lang="XAML"><![CDATA[
        /// <shimmer:SfShimmer x:Name="Shimmer" 
        ///                    WaveDirection="RightToLeft">
        /// </shimmer:SfShimmer>
        /// ]]></code>
        /// # [C#](#tab/tabid-18)
        /// <code Lang="C#"><![CDATA[
        /// Shimmer.WaveDirection = ShimmerWaveDirection.RightToLeft;
        /// ]]></code>
        /// </example>
        public ShimmerWaveDirection WaveDirection
        {
            get { return (ShimmerWaveDirection)GetValue(WaveDirectionProperty); }
            set { SetValue(WaveDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of times the built-in view should be repeated.
        /// </summary>
        /// <value>The default value of <see cref="RepeatCount"/> is 1.</value>
        /// <remarks>
        ///  The repeat count is applicable only for the built-in views and not for the custom view.
        /// </remarks>
        /// <example>
        /// The following code demonstrates, how to use the RepeatCount property in the Shimmer.
        /// # [XAML](#tab/tabid-19)
        /// <code Lang="XAML"><![CDATA[
        /// <shimmer:SfShimmer x:Name="Shimmer"
        ///                    RepeatCount="2">
        /// </shimmer:SfShimmer>
        /// ]]></code>
        /// # [C#](#tab/tabid-20)
        /// <code Lang="C#"><![CDATA[
        /// Shimmer.RepeatCount = 2;
        /// ]]></code>
        /// </example>
        public int RepeatCount
        {
            get { return (int)GetValue(RepeatCountProperty); }
            set { SetValue(RepeatCountProperty, value); }
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Measures the size of layout required for child elements.
        /// </summary>
        /// <param name="widthConstraint">The widthConstraint.</param>
        /// <param name="heightConstraint">The heightConstraint.</param>
        /// <returns>Return child element size.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            Size availableSize;
            Size contentSize = Size.Zero;
            Size customViewSize = Size.Zero;
            Size defaultSize = new Size(double.IsPositiveInfinity(widthConstraint) ? 300d : widthConstraint, double.IsPositiveInfinity(heightConstraint) ? 300d : heightConstraint);

            foreach (View child in this.Children)
            {
                if (child == this.Content)
                {
                    contentSize = child.Measure(widthConstraint, heightConstraint);
                }
                else if (child == this.CustomView)
                {
                    customViewSize = child.Measure(widthConstraint, heightConstraint);
                    // If shimmer type is "custom" then custom view size will be used for shimmer view size.
                    customViewSize = this.Type == ShimmerType.Custom ? customViewSize : defaultSize;
                }
                else if (child == this.shimmerDrawable)
                {
                    availableSize = IsActive ? customViewSize : contentSize;
                    if (availableSize == Size.Zero)
                    {
                        availableSize = defaultSize;
                    }

                    child.Measure(availableSize.Width, availableSize.Height);
                }
            }

            availableSize = IsActive ? customViewSize : contentSize;
            if (availableSize == Size.Zero)
            {
                availableSize = defaultSize;
            }

            return availableSize;
        }

        /// <summary>
        /// Called when <see cref="SfContentView.Content"/> property changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnContentChanged(object oldValue, object newValue)
        {
            View oldView = (View)oldValue;
            View newView = (View)newValue;
            if (oldView != null && this.Children.Contains(oldView))
            {
                this.Remove(oldView);
                if (oldView.Handler != null && oldView.Handler.PlatformView != null)
                {
                    oldView.Handler.DisconnectHandler();
                }
            }

            if (newView != null)
            {
                //// Adding it in 0th index to make sure it is at the bottom of the stack(1st index - custom view, 2nd index - shimmer drawable)
                //// So when the shimmer is active, the shimmer drawable will be visible (hiding the content) else content will be visible.
                this.Insert(0, newView);
                //// Shimmer content visible only when the shimmer is inactive.
                newView.Opacity = this.IsActive ? 0 : 1;
            }
        }

        #endregion

        #region Property changed methods

        /// <summary>
        /// Called when <see cref="CustomView"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnCustomViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfShimmer? shimmer = bindable as SfShimmer;
            if (shimmer == null)
            {
                return;
            }

            View oldView = (View)oldValue;
            View newView = (View)newValue;
            if (oldView != null && shimmer.Children.Contains(oldView))
            {
                shimmer.Remove(oldView);
                if (oldView.Handler != null && oldView.Handler.PlatformView != null)
                {
                    oldView.Handler.DisconnectHandler();
                }
            }

            if (newView == null)
            {
                if (shimmer.shimmerDrawable != null)
                {
                    shimmer.shimmerDrawable.UpdateShimmerDrawable();
                }

                return;
            }

            // Setting the input transparent to true to transfer the touch events from custom view to the content.
            newView.InputTransparent = true;

            // The custom view should be in index before the shimmer drawable.
            // Reason : 
            // Because we are drawing the custom view children in shimmer drawable. In order to get the correct bounds for each children 
            // in custom view, we need to add or insert the custom view prior to shimmer drawable to get the view measured and arranged. 
            // In that way we can get proper view bounds.
            if (shimmer.shimmerDrawable != null && shimmer.Children.Contains(shimmer.shimmerDrawable))
            {
                int index = shimmer.Children.Count - 1;
                index = index <= 0 ? 0 : index;
                shimmer.Insert(index, newView);
            }
            else
            {
                shimmer.Add(newView);
            }

            // Setting the opacity to 0 to hide the custom view.
            // A small line is clearly visible on view it its is 1;
            newView.Opacity = 0;
            if (shimmer.shimmerDrawable != null)
            {
                shimmer.shimmerDrawable.UpdateShimmerDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="IsActive"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnIsActivePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfShimmer? shimmer = bindable as SfShimmer;
            if (shimmer == null)
            {
                return;
            }

            bool isActive = (bool)newValue;
            if (isActive)
            {
                shimmer.shimmerDrawable = new ShimmerDrawable(shimmer);
                shimmer.Add(shimmer.shimmerDrawable);
                if (shimmer.Content != null)
                {
                    //// Shimmer content does not visible while the shimmer is active.
                    shimmer.Content.Opacity = 0;
                }
            }
            else
            {
                if (shimmer.Content != null)
                {
                    //// Need to visible the Shimmer content while the shimmer is inactive.
                    shimmer.Content.Opacity = 1;
                }

                if (shimmer.shimmerDrawable == null)
                {
                    return;
                }

                shimmer.Remove(shimmer.shimmerDrawable);
                if (shimmer.shimmerDrawable.Handler != null && shimmer.shimmerDrawable.Handler.PlatformView != null)
                {
                    shimmer.shimmerDrawable.Handler.DisconnectHandler();
                }

                shimmer.shimmerDrawable = null;
            }
        }

        /// <summary>
        /// Called when <see cref="AnimationDuration"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnAnimationDurationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfShimmer? shimmer = bindable as SfShimmer;
            if (shimmer == null || shimmer.shimmerDrawable == null)
            {
                return;
            }

            shimmer.shimmerDrawable.CreateWaveAnimator();
        }

        /// <summary>
        /// Called when <see cref="WaveWidth"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnWaveWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfShimmer? shimmer = bindable as SfShimmer;
            if (shimmer == null || shimmer.shimmerDrawable == null)
            {
                return;
            }

            shimmer.shimmerDrawable.CreateWaveAnimator();
        }

        /// <summary>
        /// Called when <see cref="Type"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfShimmer? shimmer = bindable as SfShimmer;
            if (shimmer == null)
            {
                return;
            }

            // Need to measure custom view or content when the type was changed dynamically from any build in view to custom view or vise versa.
            if ((ShimmerType)oldValue == ShimmerType.Custom || (ShimmerType)newValue == ShimmerType.Custom)
            {
                shimmer.InvalidateMeasure();
            }

            if (shimmer.shimmerDrawable != null)
            {
                shimmer.shimmerDrawable.UpdateShimmerDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="WaveDirection"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnWaveDirectionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfShimmer? shimmer = bindable as SfShimmer;
            if (shimmer == null || shimmer.shimmerDrawable == null)
            {
                return;
            }

            shimmer.shimmerDrawable.CreateWavePaint();
        }

        /// <summary>
        /// called when <see cref="RepeatCount"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnRepeatCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfShimmer? shimmer = bindable as SfShimmer;
            if (shimmer == null || shimmer.Type == ShimmerType.Custom || shimmer.shimmerDrawable == null)
            {
                return;
            }

            shimmer.shimmerDrawable.UpdateShimmerDrawable();
        }

        /// <summary>
        /// called when <see cref="Fill"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfShimmer? shimmer = bindable as SfShimmer;
            if (shimmer == null || shimmer.shimmerDrawable == null)
            {
                return;
            }

            // OnDraw method won't due to animation duration was 0. in that case we are invalidating manually to update the fill.
            if (shimmer.AnimationDuration <= 0)
            {
                shimmer.shimmerDrawable.InvalidateDrawable();
            }
        }

        #endregion
    }
}