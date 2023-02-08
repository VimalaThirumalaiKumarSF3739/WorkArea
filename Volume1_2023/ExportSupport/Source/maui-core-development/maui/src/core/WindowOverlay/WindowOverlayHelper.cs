using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Application = Microsoft.Maui.Controls.Application;
#if ANDROID
using Android.App;
using Android.Views;
using AndroidX.AppCompat.Widget;
using PlatformRect = Android.Graphics.Rect;
using PlatformRootView = Android.Views.ViewGroup;
using PlatformView = Android.Views.View;
using Window = Android.Views.Window;
#elif IOS || MACCATALYST
using UIKit;
using PlatformRootView = UIKit.UIView;
#elif WINDOWS
using Microsoft.UI.Xaml.Controls;
using PlatformRootView = Microsoft.UI.Xaml.Controls.Panel;
#else
using PlatformRootView = System.Object;
#endif

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// Helper class for <see cref="WindowOverlay"/>.
    /// </summary>
    internal static class WindowOverlayHelper
    {
        #region Fields

        /// <summary>
        /// Gets the application window.
        /// </summary>
        internal static IWindow? window => Application.Current?.MainPage?.Window;

        /// <summary>
        /// Gets the device density.
        /// </summary>
        internal static float density => window != null ? window.RequestDisplayDensity() : 1f;

        /// <summary>
        /// Gets the root view of the device.
        /// </summary>
        internal static PlatformRootView? PlatformRootView => GetPlatformRootView();

#if ANDROID

        /// <summary>
        /// Gets the decor view frame.
        /// </summary>
        internal static PlatformRect? decorViewFrame => UpdateDecorFrame();

        internal static PlatformView? decorViewContent => GetPlatformWindow()?.DecorView;
#endif
        #endregion

        #region Private methods

        /// <summary>
        /// Helps to get the root view of the device for each platform.
        /// </summary>
        /// <returns>Root view of the device.</returns>
        private static PlatformRootView? GetPlatformRootView()
        {
            PlatformRootView? rootView = null;

            if (window != null)
            {
#if ANDROID
                rootView = window.Content.ToPlatform() as ViewGroup;
                while (rootView != null && rootView is not ContentFrameLayout)
                {
                    if (rootView.Parent != null)
                    {
                        rootView = rootView.Parent as ViewGroup;
                    }
                }
#elif IOS
                UIView currentPage = window.ToPlatform();
                if (currentPage is UIWindow platformWindow)
                {
                    rootView = platformWindow?.RootViewController?.View;
                }
#elif WINDOWS
                if (window.Handler is WindowHandler windowHandler && windowHandler.PlatformView is Microsoft.UI.Xaml.Window platformWindow)
                {
                    rootView = platformWindow.Content as Panel;
                }
#endif
            }

            return rootView;
        }

#if ANDROID

        /// <summary>
        /// Gets the activity window for Android.
        /// </summary>
        /// <returns>Returns the window of the platform view.</returns>
        private static Window? GetPlatformWindow()
        {
            if (window != null && window.Handler is WindowHandler windowHandler && windowHandler.PlatformView is Activity platformActivity)
            {
                if (platformActivity == null || platformActivity.WindowManager == null
                    || platformActivity.WindowManager.DefaultDisplay == null)
                {
                    return null;
                }

                return platformActivity.Window;
            }

            return null;
        }

        /// <summary>
        /// Helps to get the decor view frame for Android.
        /// </summary>
        /// <returns>Returns the decor view frame.</returns>
        private static PlatformRect? UpdateDecorFrame()
        {
            PlatformRect? decorViewFrame = null;
            var platformWindow = GetPlatformWindow();

            if (platformWindow != null)
            {
                if (decorViewFrame == null)
                {
                    decorViewFrame = new PlatformRect();
                }
                else if (decorViewFrame.Handle != IntPtr.Zero)
                {
                    decorViewFrame.SetEmpty();
                }

                platformWindow.DecorView.GetWindowVisibleDisplayFrame(decorViewFrame);
            }

            return decorViewFrame;
        }
#endif
        #endregion
    }
}
