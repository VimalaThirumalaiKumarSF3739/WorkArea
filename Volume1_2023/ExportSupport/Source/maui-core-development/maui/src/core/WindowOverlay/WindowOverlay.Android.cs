using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System;
using MauiView = Microsoft.Maui.Controls.View;
using PlatformRect = Android.Graphics.Rect;
using PlatformView = Android.Views.View;

namespace Syncfusion.Maui.Core.Internals
{
    internal partial class SfWindowOverlay
    {
        #region Fields

        private ViewGroup? rootView;

        private WindowOverlayStack? overlayStack;

        private PlatformRect? decorViewFrame;

        private float density = 1f;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns a <see cref="WindowOverlayStack"/>.
        /// </summary>
        /// <param name="context">Passes the information about the view group.</param>
        /// <returns></returns>
        public virtual WindowOverlayStack CreateStack(Context context)
        {
            WindowOverlayStack? windowOverlayStack = null;
            IMauiContext? mauiContext = window?.Handler?.MauiContext;
            if (mauiContext != null)
            {
                windowOverlayStack = (WindowOverlayStack?)overlayStackView?.ToPlatform(mauiContext);
            }

            return windowOverlayStack != null ? windowOverlayStack : new WindowOverlayStack(context);
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

            IMauiContext? context = child.Handler?.MauiContext ?? window?.Handler?.MauiContext;
            if (context != null)
            {
                PlatformView childView = child.ToPlatform(context);
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

                float posX = (float)x * density;
                float posY = (float)y * density;
                details.X = posX;
                details.Y = posY;
                details.HorizontalAlignment = horizontalAlignment;
                details.VerticalAlignment = verticalAlignment;

                if (childView.Parent == null)
                {
                    overlayStack.AddView(childView,
                        new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
                    childView.LayoutChange += OnChildLayoutChanged;
                }

                if (childView.Width > 0 && childView.Height > 0)
                {
                    AlignPosition(horizontalAlignment, verticalAlignment,
                        childView.Width, childView.Height,
                        ref posX, ref posY);
                    childView.SetX(posX);
                    childView.SetY(posY);
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

            if (relative.Width < 0 || relative.Height < 0)
            {
                // TODO: Handle relative view layout here, if needed.
                return;
            }

            IMauiContext? context = child.Handler?.MauiContext ?? window?.Handler?.MauiContext;
            if (context != null && relative.Handler != null && relative.Handler.MauiContext != null)
            {
                PlatformView childView = child.ToPlatform(context);
                PlatformView relativeView = relative.ToPlatform(relative.Handler.MauiContext);
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

                float posX = (float)x * density;
                float posY = (float)y * density;
                details.X = posX;
                details.Y = posY;
                details.HorizontalAlignment = horizontalAlignment;
                details.VerticalAlignment = verticalAlignment;
                details.Relative = relativeView;

                if (childView.Parent == null)
                {
                    overlayStack.AddView(childView,
                        new ViewGroup.LayoutParams(
                            ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
                    childView.LayoutChange += OnChildLayoutChanged;
                }

                if (childView.Width > 0 && childView.Height > 0)
                {
                    AlignPositionToRelative(horizontalAlignment, verticalAlignment,
                        childView.Width, childView.Height,
                        relativeView.Width, relativeView.Height,
                        ref posX, ref posY);

                    int[] location = new int[2];
                    relativeView.GetLocationOnScreen(location);

                    float relativePosX = posX + location[0] - (decorViewFrame?.Left ?? 0f);
                    float positionX = Math.Max(0, relativePosX > (float)((decorViewFrame?.Right ?? 0f) - childView.Width) ?
                        (float)((decorViewFrame?.Right ?? 0f) - childView.Width - (decorViewFrame?.Left ?? 0f)) : relativePosX);
                    
                    float relativePosY = posY + location[1] - (decorViewFrame?.Top ?? 0f);
                    float positionY = Math.Max(0, relativePosY > (float)((decorViewFrame?.Bottom ?? 0f) - childView.Height) ?
                        (float)((decorViewFrame?.Bottom ?? 0f) - childView.Height - (decorViewFrame?.Top ?? 0f)) : relativePosY);

                    // TODO: Need to consider left decor view frame?
                    childView?.SetX(positionX);
                    childView?.SetY(positionY);
                }
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
                PlatformView childView = view.ToPlatform(view.Handler.MauiContext);
                childView.LayoutChange -= OnChildLayoutChanged;
                childView.RemoveFromParent();
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
                overlayStack.LayoutChange -= OnOverlayStackLayoutChange;
                overlayStack.RemoveFromParent();
                overlayStack = null;
            }

            decorViewFrame?.Dispose();
            decorViewFrame = null;

            hasOverlayStackInRoot = false;
        }

        #endregion

        #region Private Methods

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

            density = WindowOverlayHelper.density;
            rootView = WindowOverlayHelper.PlatformRootView;

            if (rootView == null)
            {
                return;
            }

            if (window.Handler is not WindowHandler windowHandler || windowHandler.MauiContext == null)
            {
                return;
            }

            if (windowHandler.PlatformView is not Activity platformActivity)
            {
                return;
            }

            decorViewFrame = WindowOverlayHelper.decorViewFrame;

            for (int i = rootView.ChildCount - 1; i >= 0; i--)
            {
                if (rootView.GetChildAt(i) is WindowOverlayStack mostRecentStack)
                {
                    overlayStack = mostRecentStack;
                    hasOverlayStackInRoot = true;
                    return;
                }
            }

            if (overlayStack == null && windowHandler.MauiContext.Context != null)
            {
                overlayStack = CreateStack(windowHandler.MauiContext.Context);
                rootView.AddView(overlayStack,
                    new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent));

                overlayStack.LayoutChange += OnOverlayStackLayoutChange;
                overlayStack.BringToFront();
            }

            hasOverlayStackInRoot = true;
        }

        private void OnOverlayStackLayoutChange(object? sender, PlatformView.LayoutChangeEventArgs e)
        {
            decorViewFrame = WindowOverlayHelper.decorViewFrame;
        }

        private void OnChildLayoutChanged(object? sender, PlatformView.LayoutChangeEventArgs e)
        {
            // TODO: Optimize the code.
            if (sender == null)
            {
                return;
            }

            PlatformView childView = (PlatformView)sender;
            if (positionDetails.TryGetValue(childView, out PositionDetails? details) && details != null)
            {
                float posX = details.X;
                float posY = details.Y;
                PlatformView? relativeView = details.Relative;

                if (relativeView == null && childView.Width > 0 && childView.Height > 0)
                {
                    AlignPosition(details.HorizontalAlignment, details.VerticalAlignment,
                        childView.Width, childView.Height,
                        ref posX, ref posY);
                    childView.SetX(posX);
                    childView.SetY(posY);
                }
                else if (relativeView != null && relativeView.Width > 0
                    && relativeView.Height > 0 && childView.Width > 0 && childView.Height > 0)
                {
                    AlignPositionToRelative(details.HorizontalAlignment, details.VerticalAlignment,
                        childView.Width, childView.Height,
                        relativeView.Width, relativeView.Height,
                        ref posX, ref posY);

                    int[] location = new int[2];
                    relativeView.GetLocationOnScreen(location);

                    float relativePosX = posX + location[0] - (decorViewFrame?.Left ?? 0f);
                    float positionX = Math.Max(0, relativePosX > (float)((decorViewFrame?.Right ?? 0f) - childView.Width) ?
                        (float)((decorViewFrame?.Right ?? 0f) - childView.Width - (decorViewFrame?.Left ?? 0f)) : relativePosX);

                    float relativePosY = posY + location[1] - (decorViewFrame?.Top ?? 0f);
                    float positionY = Math.Max(0, relativePosY > (float)((decorViewFrame?.Bottom ?? 0f) - childView.Height) ?
                        (float)((decorViewFrame?.Bottom ?? 0f) - childView.Height - (decorViewFrame?.Top ?? 0f)) : relativePosY);

                    // TODO: Need to consider left decor view frame?
                    childView?.SetX(positionX);
                    childView?.SetY(positionY);
                }
            }
        }

        private void ClearChildren()
        {
            if (overlayStack != null && positionDetails.Count > 0)
            {
                foreach (PlatformView view in positionDetails.Keys)
                {
                    view.LayoutChange -= OnChildLayoutChanged;
                    view.RemoveFromParent();
                }

                positionDetails.Clear();
            }
        }

        #endregion
    }
}
