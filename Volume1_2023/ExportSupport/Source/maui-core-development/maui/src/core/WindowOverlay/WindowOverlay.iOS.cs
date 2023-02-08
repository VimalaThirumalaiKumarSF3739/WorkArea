using CoreGraphics;
using Microsoft.Maui;
using Microsoft.Maui.Platform;
using System;
using System.Linq;
using UIKit;
using MauiView = Microsoft.Maui.Controls.View;

namespace Syncfusion.Maui.Core.Internals
{
    internal partial class SfWindowOverlay
    {
        #region Fields

        private UIView? rootView;

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
                UIView childView = child.ToPlatform(context);

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

                if (!childView.IsDescendantOfView(overlayStack))
                {
                    overlayStack.AddSubview(childView);
                    childView.Frame = overlayStack.Frame;                    
                }

                // When set the AutoSizeMode, view size is not updated properly. Need to measure the view when set AutoSizeMode.
                childView.SizeToFit();

                if (!childView.Frame.IsEmpty)
                {
                    AlignPosition(horizontalAlignment, verticalAlignment,
                        (float)childView.Frame.Width, (float)childView.Frame.Height,
                        ref posX, ref posY);
                    childView.Frame = new CGRect(posX, posY, childView.Frame.Width, childView.Frame.Height);
                }
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

            if (relative.Frame.Width < 0 || relative.Frame.Height < 0)
            {
                // TODO: Handle relative view layout here, if needed.
                return;
            }

            IMauiContext? context = window?.Handler?.MauiContext;
            if (context != null && relative.Handler != null && relative.Handler.MauiContext != null)
            {
                UIView childView = child.ToPlatform(context);
                UIView relativeView = relative.ToPlatform(relative.Handler.MauiContext);
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

                if (!childView.IsDescendantOfView(overlayStack))
                {
                    overlayStack.AddSubview(childView);
                    childView.Frame = overlayStack.Frame;
                    childView.SizeToFit();
                }

                // TODO: No need to handle child layout?
                //if (childView.Frame.Width > 0 && childView.Frame.Height > 0)
                //{
                AlignPositionToRelative(horizontalAlignment, verticalAlignment,
                    (float)childView.Frame.Width, (float)childView.Frame.Height,
                    (float)relativeView.Frame.Width, (float)relativeView.Frame.Height,
                    ref posX, ref posY);

                CGPoint relativeViewOrigin = relativeView.ConvertPointToView(new CGPoint(0, 0), rootView);

                if(rootView == null)
                {
                    return;
                }

                double relativePosX = posX + relativeViewOrigin.X;
                double positionX = Math.Max(0, relativePosX > (float)(rootView.Frame.Width - childView.Frame.Width ) ?
                    (float)(rootView.Frame.Width - childView.Frame.Width) : relativePosX);
                double relativePosY = posY + relativeViewOrigin.Y;
                double positionY = Math.Max(0, relativePosY > (float)(rootView.Frame.Height - childView.Frame.Height) ?
                    (float)(rootView.Frame.Height - childView.Frame.Height) : relativePosY);

                childView.Frame = new CGRect(
                    positionX,
                    positionY,
                    childView.Frame.Width,
                    childView.Frame.Height);
                //}
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
                UIView childView = view.ToPlatform(view.Handler.MauiContext);
                childView.RemoveFromSuperview();
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
                overlayStack.RemoveFromSuperview();
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

            rootView = WindowOverlayHelper.PlatformRootView;

            if (rootView == null)
            {
                return;
            }

            overlayStack ??= CreateStack();

            if (!rootView.Subviews.Contains(overlayStack))
            {
                rootView.AddSubview(overlayStack);
                rootView.BringSubviewToFront(overlayStack);
                overlayStack.Frame = rootView.Frame;
            }

            hasOverlayStackInRoot = true;
        }

        private void ClearChildren()
        {
            if (overlayStack != null && positionDetails.Count > 0)
            {
                overlayStack.ClearSubviews();
                positionDetails.Clear();
            }
        }

        #endregion
    }
}
