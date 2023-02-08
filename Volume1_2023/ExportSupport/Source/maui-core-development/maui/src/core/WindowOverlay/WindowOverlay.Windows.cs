using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using MauiView = Microsoft.Maui.Controls.View;
using PlatformPoint = Windows.Foundation.Point;

namespace Syncfusion.Maui.Core.Internals
{
    internal partial class SfWindowOverlay
    {
        #region Fields

        private Panel? rootView;

        private WindowOverlayStack? overlayStack;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns a <see cref="WindowOverlayStack"/>.
        /// </summary>
        public virtual WindowOverlayStack CreateStack()
        {
            WindowOverlayStack? windowOverlayStack = null;
            IMauiContext? context = window?.Handler?.MauiContext;
            if (context != null)
            {
                windowOverlayStack = (WindowOverlayStack?)overlayStackView?.ToPlatform(context);
            }

            return windowOverlayStack != null ? windowOverlayStack : new WindowOverlayStack();
        }

        /// <summary>
        /// Adds or updates the child layout absolutely to the overlay stack.
        /// </summary>
        /// <param name="child">Adds the child to the floating window.</param>
        /// <param name="x">Positions the child in the x point from the application left.</param>
        /// <param name="y">Positions the child in the y point from the application top.</param>
        /// <param name="horizontalAlignment">The horizontal alignment behaves as like below,
        /// <list type="bullet">
        /// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Left"/>, the child left position will starts from the x.</description></item>
        /// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Right"/>, the child right position will starts from the x.</description></item>
        /// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Center"/>, the child center position will be the x.</description></item>
        /// </list></param>
        /// <param name="verticalAlignment">The vertical alignment behaves as like below,
        /// <list type="bullet">
        /// <item><description>For <see cref="WindowOverlayVerticalAlignment.Top"/>, the child top position will starts from the y.</description></item>
        /// <item><description>For <see cref="WindowOverlayVerticalAlignment.Bottom"/>, the child bottom position will starts from the y.</description></item>
        /// <item><description>For <see cref="WindowOverlayVerticalAlignment.Center"/>, the child center position will be the y.</description></item>
        /// </list></param>
        public void AddOrUpdate(
            MauiView child,
            double x,
            double y,
            WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left,
            WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
        {
            AddToWindow();
            if (!hasOverlayStackInRoot || overlayStack == null || child == null)
            {
                return;
            }

            IMauiContext? context = window?.Handler?.MauiContext;
            if (context != null)
            {
                FrameworkElement childView = child.ToPlatform(context);
                PositionDetails details;
                if (positionDetails.ContainsKey(childView))
                {
                    details = positionDetails[childView];
                }
                else
                {
                    details = new PositionDetails();
                    positionDetails.Add(childView, details);
                }

                float posX = (float)x;
                float posY = (float)y;
                details.X = posX;
                details.Y = posY;
                details.HorizontalAlignment = horizontalAlignment;
                details.VerticalAlignment = verticalAlignment;

                if (!overlayStack.Children.Contains(childView))
                {
                    overlayStack.Children.Add(childView);
                    childView.LayoutUpdated += OnChildLayoutChanged;
                }

                if (childView.DesiredSize.Width <= 0 || childView.DesiredSize.Height <= 0)
                {
                    childView.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
                }

                AlignPosition(horizontalAlignment, verticalAlignment,
                    (float)childView.DesiredSize.Width, (float)childView.DesiredSize.Height,
                    ref posX, ref posY);
                Canvas.SetLeft(childView, posX);
                Canvas.SetTop(childView, posY);
            }
        }

        /// <summary>
        /// Adds or updates the child layout relatively to the overlay stack. After the relative positioning, the x and y will the added
        /// with the left and top positions.
        /// </summary>
        /// <param name="child">Adds the child to the floating window.</param>
        /// <param name="relative">Positions the child relatively to the relative view.</param>
        /// <param name="x">Adds the x point to the child left after the relative positioning.</param>
        /// <param name="y">Adds the y point to the child top after the relative positioning.</param>
        /// <param name="horizontalAlignment">The horizontal alignment behaves as like below,
        /// <list type="bullet">
        /// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Left"/>, the child left position will starts from the relative.Left.</description></item>
        /// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Right"/>, the child right position will starts from the relative.Right.</description></item>
        /// <item><description>For <see cref="WindowOverlayHorizontalAlignment.Center"/>, the child center position will be the relative.Center.</description></item>
        /// </list></param>
        /// <param name="verticalAlignment">The vertical alignment behaves as like below,
        /// <list type="bullet">
        /// <item><description>For <see cref="WindowOverlayVerticalAlignment.Top"/>, the child bottom position will starts from the relative.Top.</description></item>
        /// <item><description>For <see cref="WindowOverlayVerticalAlignment.Bottom"/>, the child top position will starts from the relative.Bottom.</description></item>
        /// <item><description>For <see cref="WindowOverlayVerticalAlignment.Center"/>, the child center position will be the relative.Center.</description></item>
        /// </list></param>
        public void AddOrUpdate(
            MauiView child,
            MauiView relative,
            double x = 0,
            double y = 0,
            WindowOverlayHorizontalAlignment horizontalAlignment = WindowOverlayHorizontalAlignment.Left,
            WindowOverlayVerticalAlignment verticalAlignment = WindowOverlayVerticalAlignment.Top)
        {
            AddToWindow();
            if (!hasOverlayStackInRoot || overlayStack == null || child == null || relative == null)
            {
                return;
            }

            if (relative.Width < 0 || relative.Height < 0)
            {
                // TODO: Handle relative view layout here, if needed.
                return;
            }

            IMauiContext? context = window?.Handler?.MauiContext;
            if (context != null && relative.Handler != null && relative.Handler.MauiContext != null)
            {
                FrameworkElement childView = child.ToPlatform(context);
                FrameworkElement relativeView = relative.ToPlatform(relative.Handler.MauiContext);
                PositionDetails details;
                if (positionDetails.ContainsKey(childView))
                {
                    details = positionDetails[childView];
                }
                else
                {
                    details = new PositionDetails();
                    positionDetails.Add(childView, details);
                }

                float posX = (float)x;
                float posY = (float)y;
                details.X = posX;
                details.Y = posY;
                details.HorizontalAlignment = horizontalAlignment;
                details.VerticalAlignment = verticalAlignment;
                details.Relative = relativeView;

                if (!overlayStack.Children.Contains(childView))
                {
                    overlayStack.Children.Add(childView);
                    childView.LayoutUpdated += OnChildLayoutChanged;
                }

                if (childView.DesiredSize.Width <= 0 && childView.DesiredSize.Height <= 0)
                {
                    childView.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
                }

                AlignPositionToRelative(horizontalAlignment, verticalAlignment,
                    (float)childView.DesiredSize.Width, (float)childView.DesiredSize.Height,
                    relativeView.ActualSize.X, relativeView.ActualSize.Y,
                    ref posX, ref posY);

                GeneralTransform transform = relativeView.TransformToVisual(rootView);
                PlatformPoint relativeViewOrigin = transform.TransformPoint(new PlatformPoint(0, 0));

                if (rootView == null)
                {
                    return;
                }

                var relativePosX = posX + relativeViewOrigin.X;
                double positionX = Math.Max(0, relativePosX > (rootView.DesiredSize.Width - childView.DesiredSize.Width) ?
                    (rootView.DesiredSize.Width - childView.DesiredSize.Width) : relativePosX);

                var RelativePosY = posY + relativeViewOrigin.Y;
                double positionY = Math.Max(0, RelativePosY > (rootView.DesiredSize.Height - childView.DesiredSize.Height) ?
                    (rootView.DesiredSize.Height - childView.DesiredSize.Height) : RelativePosY);

                Canvas.SetLeft(childView, positionX);
                Canvas.SetTop(childView, positionY);
            }
        }

        /// <summary>
        /// Eliminates the view from the floating window.
        /// </summary>
        /// <param name="view">Specifies the view to be removed from the floating window.</param>
        public void Remove(MauiView view)
        {
            if (hasOverlayStackInRoot && view != null
                && view.Handler != null && view.Handler.MauiContext != null)
            {
                FrameworkElement childView = view.ToPlatform(view.Handler.MauiContext);
                childView.LayoutUpdated -= OnChildLayoutChanged;
                overlayStack?.Children.Remove(childView);
                positionDetails.Remove(childView);
            }
        }

        /// <summary>
        /// Removes the current overlay window from root view with all its children.
        /// </summary>
        public void RemoveFromWindow()
        {
            if (overlayStack != null)
            {
                ClearChildren();
                rootView?.Children.Remove(overlayStack);
                overlayStack = null;
            }

            hasOverlayStackInRoot = false;
        }

        #endregion

        #region Private methods

        private void Initialize()
        {
            if (hasOverlayStackInRoot)
            {
                return;
            }

            if (window == null || window.Content == null)
            {
                return;
            }

            FrameworkElement root = window.Content.ToPlatform();
            if (root == null)
            {
                return;
            }

            rootView = WindowOverlayHelper.PlatformRootView;

            if (rootView == null)
            {
                return;
            }

            overlayStack ??= CreateStack();
            if (!rootView.Children.Contains(overlayStack))
            {
                rootView.Children.Add(overlayStack);
            }

            overlayStack.SetValue(Canvas.ZIndexProperty, 99);

            hasOverlayStackInRoot = true;
        }

        private void OnChildLayoutChanged(object? sender, object e)
        {
            // TODO: Optimize the code.
            if (sender == null)
            {
                return;
            }

            FrameworkElement childView = (FrameworkElement)sender;
            if (positionDetails.TryGetValue(childView, out PositionDetails? details) && details != null)
            {
                FrameworkElement? relativeView = details.Relative;
                float posX = details.X;
                float posY = details.Y;
                if (relativeView == null && childView.Width > 0 && childView.Height > 0)
                {
                    AlignPosition(details.HorizontalAlignment, details.VerticalAlignment,
                        (float)childView.DesiredSize.Width, (float)childView.DesiredSize.Height,
                        ref posX, ref posY);
                    Canvas.SetLeft(childView, posX);
                    Canvas.SetTop(childView, posY);
                }
                else if (relativeView != null && relativeView.Width > 0 && relativeView.Height > 0)
                {
                    AlignPositionToRelative(details.HorizontalAlignment, details.VerticalAlignment,
                        (float)childView.DesiredSize.Width, (float)childView.DesiredSize.Height,
                        relativeView.ActualSize.X, relativeView.ActualSize.Y,
                        ref posX, ref posY);

                    GeneralTransform transform = relativeView.TransformToVisual(rootView);
                    PlatformPoint relativeViewOrigin = transform.TransformPoint(new PlatformPoint(0, 0));

                    if (rootView == null)
                    {
                        return;
                    }

                    double relativePosX = posX + relativeViewOrigin.X - details.X;
                    double positionX = Math.Max(details.X, relativePosX > (rootView.DesiredSize.Width - childView.DesiredSize.Width) ?
                        (rootView.DesiredSize.Width - childView.DesiredSize.Width) : relativePosX);

                    double relativePosY = posY + relativeViewOrigin.Y - details.Y;
                    double positionY = Math.Max(details.Y, relativePosY > (rootView.DesiredSize.Height - childView.DesiredSize.Height) ?
                        (rootView.DesiredSize.Height - childView.DesiredSize.Height) : relativePosY);

                    Canvas.SetLeft(childView, positionX);
                    Canvas.SetTop(childView, positionY);
                }
            }
        }

        private void ClearChildren()
        {
            if (overlayStack != null && positionDetails.Count > 0)
            {
                foreach (FrameworkElement childView in positionDetails.Keys)
                {
                    childView.LayoutUpdated -= OnChildLayoutChanged;
                    overlayStack.Children.Remove(childView);
                }

                positionDetails.Clear();
            }
        }

        #endregion
    }
}
