using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Shimmer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShimmerDrawable"/> class.
    /// </summary>
    internal class ShimmerDrawable : SfDrawableView
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ShimmerDrawable"/> class.
        /// </summary>
        /// <param name="shimmer">The instance for the <see cref="SfShimmer"/> class.</param>
        internal ShimmerDrawable(IShimmer shimmer)
        {
            this.shimmer = shimmer;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Hold the shimmer control customization details.
        /// </summary>
        private IShimmer shimmer;

        /// <summary>
        /// The available size alloted in arrange override.
        /// </summary>
        private Size availableSize;

        /// <summary>
        /// The gradient brush for shimmer wave effect.
        /// </summary>
        private LinearGradientBrush? gradient;

        /// <summary>
        /// The path to draw built in views like persona, video, shopping etc.
        /// </summary>
        private PathF? path;

        #endregion

        #region Override methods

        /// <summary>
        /// Method used to draw shimmer view visual elements. 
        /// </summary>
        /// <param name="canvas">Represents rendering canvas.</param>
        /// <param name="dirtyRect">Represents rendering region.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            // It is not required to update the view when the size is not changed.
            // Used this logic to update the view only when the size gets changed or if the path is null(Path=null was set in dynamic property changes).
            if (this.availableSize.Height != dirtyRect.Height ||
                this.availableSize.Width != dirtyRect.Width || this.path == null)
            {
                this.availableSize = dirtyRect.Size;
                this.UpdateShimmerView();
            }

            if (this.path == null || !this.shimmer.IsActive)
            {
                return;
            }

            // To save the current state of the canvas returned by framework.
            canvas.SaveState();

            // When animation duration is zero, animation won't be triggered hence the color won't be set for gradient brush.
            // So, we are setting the color directly to the canvas.
            if (this.shimmer.AnimationDuration <= 0)
            {
                canvas.FillColor = ((SolidColorBrush)this.shimmer.Fill).Color;
            }
            else
            {
                canvas.SetFillPaint(this.gradient, this.path.Bounds);
            }

            canvas.FillPath(this.path);

            // We have used canvas for our drawing purpose and now returning it previous saved state as returned by framework.
            canvas.RestoreState();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// To update shimmer view and shimmer wave elements.
        /// </summary>
        private void UpdateShimmerView()
        {
            this.CreateShimmerViewPath();
            this.CreateWavePaint();
            this.CreateWaveAnimator();
        }

        /// <summary>
        /// Creating animator based on animation duration.
        /// </summary>
        internal void CreateWaveAnimator()
        {
            if (this.shimmer.AnimationDuration <= 0)
            {
                this.InvalidateDrawable();
                return;
            }

            this.CreateAnimator();
        }

        /// <summary>
        /// To update the custom view when changed dynamically. 
        /// Setting path to null whenever the custom view was changed in order to update the control.
        /// </summary>
        internal void UpdateShimmerDrawable()
        {
            this.path = null;
            // Need to invalidate the shimmer drawable when the custom view or type or repeat count was changed dynamically when duration is 0,
            // Since the animation duration is 0, the OnDraw method will not be called in shimmer drawable to invalidate the view.
            // OnDraw will be invalidated if the animation duration is >0 on OnAnimationUpdate method. 
            // So we need to invalidate the view manually.
            if (this.shimmer.AnimationDuration <= 0)
            {
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// To create path for the shimmer view.
        /// </summary>
        private void CreateShimmerViewPath()
        {
            if (this.availableSize.Width <= 0 || this.availableSize.Height <= 0)
            {
                return;
            }

            this.path = new PathF();
            if (this.shimmer.Type == ShimmerType.Custom)
            {
                this.CreateCustomViewPath();
            }
            else
            {
                this.CreateDefaultPath();
            }
        }

        /// <summary>
        /// To create path and draw the custom view children.
        /// </summary>
        private void CreateCustomViewPath()
        {
            if (this.shimmer.CustomView == null)
            {
                return;
            }

            this.DrawCustomViewChildren(this.shimmer.CustomView, new Point(this.shimmer.CustomView.X, this.shimmer.CustomView.Y));
        }

        /// <summary>
        /// To draw the child view present in the layout view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="position">The position.</param>
        private void DrawCustomViewChildren(View view, Point position)
        {
            //  <StackLayout>
            //      <BoxView HeightRequest="20" WidthRequest="100"/>
            //      <Grid>
            //          <BoxView HeightRequest="40" WidthRequest="50"/>
            //      </Grid>
            //      <Grid HeightRequest="20" WidthRequest="100"/>
            //  </StackLayout>
            //
            //
            // This method enters with stack layout. Since stack layout is a type of Layout so it may or may not contain child view.
            // The above example stack layout has 3 children.
            // First child is BoxView, second child is Grid with box view as a children and third child is Grid with no children.
            // If the child is a type of view then it will be added to the path (Box view will be drawn) in else statement.
            // The second child (ie. grid) is a type of Layout so it will enter into the if statement and it has child and hence the child box view will be drawn by calling this recursive method.
            // The third child (ie. grid) is a type of Layout so it will enter into the if statement and it has no child and hence it will not be drawn.

            if (view == null || this.path == null)
            {
                return;
            }

            if (view is Layout layout)
            {
                foreach (View item in layout.Children)
                {
                    this.DrawCustomViewChildren(item, new Point(item.X + position.X, item.Y + position.Y));
                }
            }
            else
            {
                CornerRadius radius = 0;
                Point viewPosition = new Point(position.X, position.Y);
                float width = (float)view.Bounds.Width;
                float height = (float)view.Bounds.Height;

                if (view is ShimmerView shimmerView)
                {
                    if (shimmerView.ShapeType == ShimmerShapeType.Circle)
                    {
                        this.path.AppendCircle((float)viewPosition.X + width / 2, (float)viewPosition.Y + height / 2, Math.Min(width / 2, height / 2));
                        return;
                    }
                    else if (shimmerView.ShapeType == ShimmerShapeType.RoundedRectangle)
                    {
                        radius = 5;
                    }
                }

                if (view is BoxView boxView)
                {
                    radius = boxView.CornerRadius;
                }

                this.path.AppendRoundedRectangle((float)viewPosition.X, (float)viewPosition.Y, width, height, (float)radius.TopLeft, (float)radius.TopRight, (float)radius.BottomLeft, (float)radius.BottomRight);
            }
        }

        /// <summary>
        /// To create path data for build-in shimmer types.
        /// </summary>
        private void CreateDefaultPath()
        {
            int repeatCount = this.shimmer.RepeatCount >= 1 ? this.shimmer.RepeatCount : 1;

            // The padding value for left and right side of the shimmer view.
            float sidePadding = 10;
            float totalWidth = (float)this.availableSize.Width - 2 * sidePadding;

            // When repeat count was set, this spacing will be used in between the repeated shimmer view.
            float repeatViewSpacing = 10;

            // This will be the maximum height of the single shimmer view.
            float shimmerHeight = (float)(this.availableSize.Height - ((repeatCount + 1) * repeatViewSpacing)) / repeatCount;
            RectF shimmerRect = new RectF(sidePadding, repeatViewSpacing, totalWidth, shimmerHeight);

            switch (this.shimmer.Type)
            {
                case ShimmerType.CirclePersona:
                    this.CreateCirclePersonaPath(shimmerRect);
                    break;
                case ShimmerType.RectanglePersona:
                    this.CreateRectanglePersonaPath(shimmerRect);
                    break;
                case ShimmerType.Profile:
                    this.CreateProfilePath(shimmerRect);
                    break;
                case ShimmerType.Article:
                    this.CreateArticlePath(shimmerRect);
                    break;
                case ShimmerType.Video:
                    this.CreateVideoPath(shimmerRect);
                    break;
                case ShimmerType.Feed:
                    this.CreateFeedPath(shimmerRect);
                    break;
                case ShimmerType.Shopping:
                    this.CreateShoppingPath(shimmerRect);
                    break;
            }
        }

        /// <summary>
        /// To draw the profile shimmer view.
        /// </summary>
        /// <param name="shimmerRect">The shimmer rectangle.</param>
        private void CreateProfilePath(RectF shimmerRect)
        {
            if (this.path == null)
            {
                return;
            }

            // Corner radius for the rectangle.
            int cornerRadius = 2;
            int repeatCount = this.shimmer.RepeatCount >= 1 ? this.shimmer.RepeatCount : 1;
            float circleSize = 0.3f;

            // Radius for the profile circle.
            float radius = shimmerRect.Height * circleSize / 2f;
            float diameter = radius * 2;

            // The initial or first y position for the profile view.
            float y = (float)(this.availableSize.Height / 2) - ((repeatCount * shimmerRect.Height) + (repeatCount - 1) * shimmerRect.Y) / 2;
            y = y <= shimmerRect.Y ? shimmerRect.Y : y;

            // The height of the profile rectangle.
            float rectangleHeight = shimmerRect.Height * 0.05f;

            // The row spacing in between the paths.
            float commonRowSpacing = (shimmerRect.Height - diameter - rectangleHeight * 6) / 8;

            for (int i = 0; i < repeatCount; i++)
            {
                path.AppendCircle(shimmerRect.X + shimmerRect.Width / 2, y + radius, radius);
                path.AppendRoundedRectangle(new RectF(shimmerRect.X + shimmerRect.Width * 0.3f, y + diameter + commonRowSpacing * 3, shimmerRect.Width * 0.4f, rectangleHeight * 2), cornerRadius);
                path.AppendRoundedRectangle(new RectF(shimmerRect.X + shimmerRect.Width * 0.1f, y + diameter + commonRowSpacing * 4 + rectangleHeight * 2, shimmerRect.Width * 0.8f, rectangleHeight * 2), cornerRadius);
                path.AppendRoundedRectangle(new RectF(shimmerRect.X, y + diameter + commonRowSpacing * 7 + rectangleHeight * 4, shimmerRect.Width, rectangleHeight), cornerRadius);
                path.AppendRoundedRectangle(new RectF(shimmerRect.X, y + diameter + commonRowSpacing * 8 + rectangleHeight * 5, shimmerRect.Width, rectangleHeight), cornerRadius);
                y = shimmerRect.Y + y + shimmerRect.Height;
            }
        }

        /// <summary>
        /// To draw the video shimmer view.
        /// </summary>
        /// <param name="shimmerRect">The shimmer rectangle.</param>
        private void CreateVideoPath(RectF shimmerRect)
        {
            if (this.path == null)
            {
                return;
            }

            // Column spacing in between the paths.
            float columnSpacing = 15;

            // Corner radius for the rectangle.
            int cornerRadius = 5;
            int repeatCount = this.shimmer.RepeatCount >= 1 ? this.shimmer.RepeatCount : 1;
            float circleSize = 0.2f;

            // Radius for the video circle.
            float radius = shimmerRect.Width * circleSize / 2f;
            radius = Math.Min(shimmerRect.Height * 0.3f / 2, radius);
            float diameter = radius * 2;

            // The initial or first y position for the video view.
            float y = (float)(this.availableSize.Height / 2) - ((repeatCount * shimmerRect.Height) + (repeatCount - 1) * shimmerRect.Y) / 2;
            y = y <= shimmerRect.Y ? shimmerRect.Y : y;

            // The top and bottom row spacing for the circle rectangle.
            float circleRowSpacing = diameter * 0.05f;

            // The height of the circle rectangle height.
            float circleRectangleHeight = diameter * 0.4f;

            // Row space between the circle and the rectangle.
            float rowSpacing = shimmerRect.Height * 0.1f;

            // The height of the video rectangle.
            float videoRectangleHeight = shimmerRect.Height - diameter - rowSpacing;

            // The x-position for the persona rectangle.
            float xValue = shimmerRect.X + columnSpacing + diameter;

            for (int i = 0; i < repeatCount; i++)
            {
                path.AppendRoundedRectangle(new RectF(shimmerRect.X, y, shimmerRect.Width, videoRectangleHeight), cornerRadius);
                path.AppendCircle(shimmerRect.X + radius, y + radius + videoRectangleHeight + rowSpacing, radius);
                path.AppendRoundedRectangle(new RectF(xValue, y + circleRowSpacing + videoRectangleHeight + rowSpacing, shimmerRect.Width - columnSpacing - diameter - shimmerRect.Width * 0.1f, circleRectangleHeight), cornerRadius);
                path.AppendRoundedRectangle(new RectF(xValue, y + radius + circleRowSpacing + videoRectangleHeight + rowSpacing, shimmerRect.Width - columnSpacing - diameter - shimmerRect.Width * 0.1f, circleRectangleHeight), cornerRadius);
                y = shimmerRect.Y + y + shimmerRect.Height;
            }
        }

        /// <summary>
        /// To draw the feed shimmer view.
        /// </summary>
        /// <param name="shimmerRect">The shimmer rectangle.</param>
        private void CreateFeedPath(RectF shimmerRect)
        {
            if (this.path == null)
            {
                return;
            }

            // Column spacing in between the paths.
            float columnSpacing = 10;

            // Corner radius for the rectangle.
            int cornerRadius = 5;
            int repeatCount = this.shimmer.RepeatCount >= 1 ? this.shimmer.RepeatCount : 1;

            float circleSize = 0.25f;

            // Radius for the feed circle.
            float radius = shimmerRect.Height * circleSize / 2f;
            radius = Math.Min(shimmerRect.Width * 0.2f / 2, radius);
            float diameter = radius * 2;

            // The initial or first y position for the feed view.
            float y = (float)(this.availableSize.Height / 2) - ((repeatCount * shimmerRect.Height) + (repeatCount - 1) * shimmerRect.Y) / 2;
            y = y <= shimmerRect.Y ? shimmerRect.Y : y;

            // for solving the center issue
            if (shimmerRect.Height * 0.25 > diameter)
            {
                y = y + shimmerRect.Height * circleSize / 2 - radius;
            }

            // The row spacing for the circle rectangle.
            float rowSpacing = (float)diameter * 0.05f;

            // The common row spacing for the feed.
            float commonRowSpacing = (float)shimmerRect.Height * 0.05f;

            // The circle rectangle height.
            float circularItemHeight = (float)diameter * 0.4f;

            // The height of the subject rectangle
            float rectangleHeight = (float)shimmerRect.Height * 0.075f;

            // The hight of the feed box.
            float feedRectangleHeight = shimmerRect.Height * 0.4f;

            float xValue = shimmerRect.X + diameter + columnSpacing;

            for (int i = 0; i < repeatCount; i++)
            {
                path.AppendCircle(shimmerRect.X + radius, y + radius, radius);
                path.AppendRoundedRectangle(new RectF(xValue, y + rowSpacing, shimmerRect.Width - columnSpacing - diameter, circularItemHeight), cornerRadius);
                path.AppendRoundedRectangle(new RectF(xValue, y + diameter - rowSpacing - circularItemHeight, shimmerRect.Width * 0.4f, circularItemHeight), cornerRadius);
                path.AppendRoundedRectangle(new RectF(shimmerRect.X, y + diameter + commonRowSpacing * 2, shimmerRect.Width, feedRectangleHeight), cornerRadius);
                path.AppendRoundedRectangle(new RectF(shimmerRect.X, y + diameter + commonRowSpacing * 3 + feedRectangleHeight, shimmerRect.Width, rectangleHeight), cornerRadius);
                path.AppendRoundedRectangle(new RectF(shimmerRect.X, y + diameter + commonRowSpacing * 4 + feedRectangleHeight + rectangleHeight, shimmerRect.Width, rectangleHeight), cornerRadius);
                y = y + shimmerRect.Height + shimmerRect.Y;
            }
        }

        /// <summary>
        /// To draw the shopping shimmer view.
        /// </summary>
        /// <param name="shimmerRect">The shimmer rectangle.</param>
        private void CreateShoppingPath(RectF shimmerRect)
        {
            if (this.path == null)
            {
                return;
            }

            // Corner radius for the shopping rectangle.
            int cornerRadius = 5;
            int repeatCount = this.shimmer.RepeatCount >= 1 ? this.shimmer.RepeatCount : 1;

            // The initial or first y position for the shopping view.
            float y = (float)(this.availableSize.Height / 2) - ((repeatCount * shimmerRect.Height) + (repeatCount - 1) * shimmerRect.Y) / 2;
            y = y <= shimmerRect.Y ? shimmerRect.Y : y;

            // The video rectangle size factor.
            float videoRectSizeFactor = 0.65f;

            // The calculated video rectangle size.
            float videoRectSize = shimmerRect.Height * videoRectSizeFactor;

            // The factor value for the rectangle path height.
            float rectangleHeightFactor = 0.5714f;

            // The height of the rectangle.
            float rectangleHeight = (float)(shimmerRect.Height - videoRectSize) * rectangleHeightFactor / 2;

            // The row space between the rectangle.
            float rowSpacing = (shimmerRect.Height - videoRectSize - 2 * rectangleHeight) / 5;

            for (int i = 0; i < repeatCount; i++)
            {
                path.AppendRoundedRectangle(new RectF(shimmerRect.X, y, shimmerRect.Width, videoRectSize), cornerRadius);
                path.AppendRoundedRectangle(new RectF(shimmerRect.X, y + rowSpacing * 3 + videoRectSize, shimmerRect.Width, rectangleHeight), cornerRadius);
                path.AppendRoundedRectangle(new RectF(shimmerRect.X, y + rowSpacing * 5 + videoRectSize + rectangleHeight, shimmerRect.Width, rectangleHeight), cornerRadius);
                y = shimmerRect.Y + y + shimmerRect.Height;
            }
        }

        /// <summary>
        /// To draw the article shimmer view.
        /// </summary>
        /// <param name="shimmerRect">The shimmer rectangle.</param>
        private void CreateArticlePath(RectF shimmerRect)
        {
            if (this.path == null)
            {
                return;
            }

            // Column spacing in between the paths.
            float columnSpacing = 10;

            // Corner radius for the rectangle.
            int cornerRadius = 5;
            int repeatCount = this.shimmer.RepeatCount >= 1 ? this.shimmer.RepeatCount : 1;

            // The article box size.
            float articleBoxSize = 0.2f;

            // The factor value for the rectangle path height and the row space.
            float rectangleHeightFactor = 0.15f, rowSpacingFactor = 0.05f;

            // Calculating the article box width.
            float articleBoxWidth = shimmerRect.Width * articleBoxSize;
            articleBoxWidth = Math.Min(shimmerRect.Height, articleBoxWidth);

            // The initial or first y position for the article view.
            float y = (float)(this.availableSize.Height / 2) - ((repeatCount * articleBoxWidth) + (repeatCount - 1) * shimmerRect.Y) / 2;
            y = y <= shimmerRect.Y ? shimmerRect.Y : y;

            // The row space between the paths.
            float rowSpacing = articleBoxWidth * rowSpacingFactor;

            // The height of the article rectangle.
            float rectangleHeight = articleBoxWidth * rectangleHeightFactor;

            // The x-position for the persona rectangle.
            float xValue = shimmerRect.X + articleBoxWidth + columnSpacing;

            for (int i = 0; i < repeatCount; i++)
            {
                path.AppendRoundedRectangle(new RectF(shimmerRect.X, y, articleBoxWidth, articleBoxWidth), cornerRadius);
                path.AppendRoundedRectangle(new RectF(xValue, y + rowSpacing, shimmerRect.Width - columnSpacing - articleBoxWidth, rectangleHeight), cornerRadius);
                path.AppendRoundedRectangle(new RectF(xValue, y + rowSpacing * 2 + rectangleHeight, shimmerRect.Width * 0.4f, rectangleHeight), cornerRadius);

                path.AppendRoundedRectangle(new RectF(xValue, y + articleBoxWidth - rectangleHeight * 2 - rowSpacing * 2, shimmerRect.Width - columnSpacing - articleBoxWidth, rectangleHeight), cornerRadius);
                path.AppendRoundedRectangle(new RectF(xValue, y + articleBoxWidth - rectangleHeight - rowSpacing, shimmerRect.Width * 0.4f, rectangleHeight), cornerRadius);
                y = shimmerRect.Y + y + articleBoxWidth;
            }
        }

        /// <summary>
        /// To draw the rectangle persona shimmer view.
        /// </summary>
        /// <param name="shimmerRect">The shimmer rectangle.</param>
        private void CreateRectanglePersonaPath(RectF shimmerRect)
        {
            if (this.path == null)
            {
                return;
            }

            // Column spacing in between the paths.
            float columnSpacing = 10;

            // Corner radius for the persona rectangle.
            int cornerRadius = 5;
            int repeatCount = this.shimmer.RepeatCount >= 1 ? this.shimmer.RepeatCount : 1;

            // The persona box size
            float personaBoxSize = 0.2f;

            // The factor value for the rectangle path height and space at the top and bottom.
            float rectangleHeightFactor = 0.32f, rowSpacingFactor = 0.1f;

            // Calculating the height or width based on minimum value.
            float personaBoxWidth = shimmerRect.Width * personaBoxSize;
            personaBoxWidth = Math.Min(shimmerRect.Height, personaBoxWidth);

            // The initial or first y position for the persona view.
            float y = (float)(this.availableSize.Height / 2) - ((repeatCount * personaBoxWidth) + (repeatCount - 1) * shimmerRect.Y) / 2;
            y = y <= shimmerRect.Y ? shimmerRect.Y : y;

            // The row space between the paths.
            float rowSpacing = personaBoxWidth * rowSpacingFactor;

            // The height of the persona rectangle.
            float rectangleHeight = personaBoxWidth * rectangleHeightFactor;

            // The x-position for the persona rectangle.
            float xValue = shimmerRect.X + personaBoxWidth + columnSpacing;

            for (int i = 0; i < repeatCount; i++)
            {
                path.AppendRoundedRectangle(new RectF(shimmerRect.X, y, personaBoxWidth, personaBoxWidth), cornerRadius);
                path.AppendRoundedRectangle(new RectF(xValue, y + rowSpacing, shimmerRect.Width - columnSpacing - personaBoxWidth, rectangleHeight), cornerRadius);
                path.AppendRoundedRectangle(new RectF(xValue, y + personaBoxWidth - rowSpacing - rectangleHeight, shimmerRect.Width * 0.4f, rectangleHeight), cornerRadius);
                y = shimmerRect.Y + y + personaBoxWidth;
            }
        }

        /// <summary>
        /// To draw the circle persona shimmer view.
        /// </summary>
        /// <param name="shimmerRect">The shimmer rectangle.</param>
        private void CreateCirclePersonaPath(RectF shimmerRect)
        {
            if (this.path == null)
            {
                return;
            }

            // Column spacing in between the circle and rectangle.
            float columnSpacing = 10;

            // Corner radius for the persona rectangle.
            int cornerRadius = 5;
            int repeatCount = this.shimmer.RepeatCount >= 1 ? this.shimmer.RepeatCount : 1;

            // The persona circle size.
            float circleSize = 0.2f;

            // The factor value for the rectangle path height and space at the top and bottom.
            float rectangleHeightFactor = 0.33f, rowSpacingFactor = 0.1f;

            // Radius for the persona circle.
            float radius = shimmerRect.Width * circleSize / 2f;
            radius = Math.Min(shimmerRect.Height / 2, radius);
            float diameter = radius * 2;

            // The initial or first y position for the persona view.
            float y = (float)(this.availableSize.Height / 2) - ((repeatCount * diameter) + (repeatCount - 1) * shimmerRect.Y) / 2;
            y = y <= shimmerRect.Y ? shimmerRect.Y : y;

            // The space at the top and bottom of the rectangle.
            float rowSpacing = diameter * rowSpacingFactor;

            // The height of the persona rectangle.
            float rectangleHeight = diameter * rectangleHeightFactor;

            // The x-position for the persona rectangle.
            float xValue = shimmerRect.X + diameter + columnSpacing;

            for (int i = 0; i < repeatCount; i++)
            {
                path.AppendCircle(shimmerRect.X + radius, y + radius, radius);
                path.AppendRoundedRectangle(new RectF(xValue, y + rowSpacing, shimmerRect.Width - columnSpacing - diameter, rectangleHeight), cornerRadius);
                path.AppendRoundedRectangle(new RectF(xValue, y + diameter - rowSpacing - rectangleHeight, shimmerRect.Width * 0.4f, rectangleHeight), cornerRadius);
                y = shimmerRect.Y + y + diameter;
            }
        }

        /// <summary>
        /// To create gradient brush for shimmer wave animation.
        /// </summary>
        internal void CreateWavePaint()
        {
            if (this.gradient == null)
            {
                this.gradient = new LinearGradientBrush();
            }

            switch (shimmer.WaveDirection)
            {
                case ShimmerWaveDirection.LeftToRight:
                    this.gradient.StartPoint = new Point(0, 0);
                    this.gradient.EndPoint = new Point(1, 0);
                    break;
                case ShimmerWaveDirection.TopToBottom:
                    this.gradient.StartPoint = new Point(0, 0);
                    this.gradient.EndPoint = new Point(0, 1);
                    break;
                case ShimmerWaveDirection.RightToLeft:
                    this.gradient.StartPoint = new Point(1, 0);
                    this.gradient.EndPoint = new Point(0, 0);
                    break;
                case ShimmerWaveDirection.BottomToTop:
                    this.gradient.StartPoint = new Point(0, 1);
                    this.gradient.EndPoint = new Point(0, 0);
                    break;
                default:
                    this.gradient.StartPoint = new Point(0, 0);
                    this.gradient.EndPoint = new Point(1, 1);
                    break;
            }
        }

        /// <summary>
        /// To create wave animation for the shimmer view control.
        /// </summary>
        private void CreateAnimator()
        {
            // We are returning, because we don't need to create animator in not active state.
            if (!this.shimmer.IsActive || this.availableSize == Size.Zero)
            {
                return;
            }

            // Calculating the wave width in factor value.
            float x = (float)(this.shimmer.WaveWidth / this.availableSize.Width);

            // Validating the factor, because the factor should lie between 0 and 1.
            x = (x < 0) ? 0 : (x > 1) ? 1 : x;
            if (AnimationExtensions.AnimationIsRunning(this, "ShimmerAnimation"))
            {
                this.AbortAnimation("ShimmerAnimation");
            }

            Animation parentAnimation = new Animation(OnAnimationUpdate, 0,
                            1 + x, Easing.Linear, null);
            parentAnimation.Commit(this, "ShimmerAnimation", 16, (uint)this.shimmer.AnimationDuration,
                Easing.Linear, null, () => true);
        }

        /// <summary>
        /// To update the shimmer wave animation with respect to the animator value.
        /// </summary>
        /// <param name="animationValue">The animation value.</param>
        private void OnAnimationUpdate(double animationValue)
        {
            float offset = (float)animationValue;

            // Calculating the wave width in factor value.
            float x = (float)(this.shimmer.WaveWidth / (this.availableSize.Width));

            // Validating the factor, because the factor should lie between 0 and 1.
            x = (x < 0) ? 0 : (x > 1) ? 1 : x;

            float gradientOffset3 = Math.Clamp(offset, 0, 1);
            float gradientOffset2 = Math.Clamp(offset - (x / 2), 0, 1);
            float gradientOffset1 = Math.Clamp(offset - x, 0, 1);
            if (this.gradient != null)
            {
                this.gradient.GradientStops = new GradientStopCollection
                {
                    new GradientStop(){Color =  ((SolidColorBrush)this.shimmer.Fill).Color,Offset = gradientOffset1},
                    new GradientStop(){Color = this.shimmer.WaveColor,Offset = gradientOffset2},
                    new GradientStop(){Color = ((SolidColorBrush)this.shimmer.Fill).Color,Offset = gradientOffset3},
                };
            }

            this.InvalidateDrawable();
        }

        #endregion

    }
}
