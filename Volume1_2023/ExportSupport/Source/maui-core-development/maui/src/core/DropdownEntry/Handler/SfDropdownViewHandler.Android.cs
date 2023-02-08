
using Android.Graphics;
using Android.Views;
using Java.Lang;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Core.Platform;
using System;
using View = Microsoft.Maui.Controls.View;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Handler class for popup view.
    /// </summary>
    public partial class SfDropdownViewHandler : ContentViewHandler
    {
        #region Fields

        private DropdownViewExt? popupViewExt;
        private SfDropdownView? popupView;
        private Android.Views.View? inputView;

        #endregion

        #region Override methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected override ContentViewGroup CreatePlatformView()
        {
            if (VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a PopupView");
            }
            else
            {
                popupView = (SfDropdownView)this.VirtualView;
            }

            this.popupViewExt = new DropdownViewExt(Context);

            if (this.popupViewExt.PopupWindow != null)
            {
                this.popupViewExt.PopupWindow.DismissEvent += PopUpWindow_DismissEvent;
                this.popupViewExt.PopupWindow.TouchIntercepted += PopUpWindow_TouchIntercepted;
            }

            return this.popupViewExt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformView"></param>
        protected override void DisconnectHandler(ContentViewGroup platformView)
        {
            base.DisconnectHandler(platformView);
            if (this.popupViewExt != null)
            {
                if (this.popupViewExt.PopupWindow != null)
                {
                    this.popupViewExt.PopupWindow.DismissEvent -= PopUpWindow_DismissEvent;
                    this.popupViewExt.PopupWindow.TouchIntercepted -= PopUpWindow_TouchIntercepted;
                }

                this.popupViewExt.Dispose();
                this.popupViewExt = null;
            }
            if (this.inputView != null)
            {
                this.inputView.Dispose();
                this.inputView = null;
            }
        }

        #endregion

        #region Methods

        private void PopUpWindow_DismissEvent(object? sender, EventArgs e)
        {
            if (popupView != null)
            {
                popupView.IsOpen = false;
            }
        }

        private void PopUpWindow_TouchIntercepted(object? sender, Android.Views.View.TouchEventArgs e)
        {
            if (e.Event?.Action == MotionEventActions.Outside && inputView != null)
            {
                Rect? rect = LocateView(inputView);

                if (rect != null && rect.Contains((int)e.Event?.RawX!, (int)e.Event?.RawY!))
                {

                    if (popupView != null && popupView.IsDropDownCicked && popupView.IsOpen)
                    {
                        e.Handled = false;

                    }
                    else
                    {
                        e.Handled = true;
                    }


                }
                else
                {
                    e.Handled = false;

                    if (popupView != null)
                    {
                        popupView.IsDropDownCicked = false;
                        popupView.IsOpen = false;
                    }
                }
            }
            else if (e.Event?.Action == MotionEventActions.Up || e.Event?.Action == MotionEventActions.Down || e.Event?.Action == MotionEventActions.Move)
            {
                e.Handled = false;
                if (popupView != null)
                {
                    popupView.IsDropDownCicked = false;
                }
            }

        }

        private static Rect? LocateView(Android.Views.View v)
        {
            int[] loc_int = new int[2];

            if (v == null) return null;

            try
            {
                v.GetLocationOnScreen(loc_int);
            }
            catch (NullPointerException)
            {
                //Happens when the view doesn't exist on screen anymore.
                return null;
            }

            Rect location = new()
            {
                Left = loc_int[0],
                Top = loc_int[1]
            };
            location.Right = location.Left + v.Width;
            location.Bottom = location.Top + v.Height;
            return location;
        }

        /// <summary>
        /// Update popup content method.
        /// </summary>
        /// <param name="listView"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void UpdatePopupContent(View listView)
        {
            _ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

            popupViewExt?.UpdatePopupContent(listView.ToPlatform(MauiContext));
        }

        /// <summary>
        /// Hide popup method.
        /// </summary>
        public void HidePopup()
        {
            popupViewExt?.HidePopup();
        }

        /// <summary>
        /// Show popup method.
        /// </summary>
        public void ShowPopup()
        {
            if (popupView != null)
            {
                popupView.IsOpen = true;
            }

            popupViewExt?.ShowPopup();
        }

        /// <summary>
        /// Update popup height method.
        /// </summary>
        /// <param name="height"></param>
        public void UpdatePopupHeight(double height)
        {
            if (popupViewExt != null)
            {
                popupViewExt.PopupHeight = height;
            }
        }

        /// <summary>
        /// Update popup width method.
        /// </summary>
        /// <param name="width"></param>
        public void UpdatePopupWidth(double width)
        {
            if (popupViewExt != null)
            {
                popupViewExt.PopupWidth = width;
            }
        }

        /// <summary>
        /// Update PopupX method.
        /// </summary>
        /// <param name="x"></param>
        public void UpdatePopupX(int x)
        {
            if (popupViewExt != null)
            {
                popupViewExt.PopupX = x;
            }
        }

        /// <summary>
        /// Update PopupY method.
        /// </summary>
        /// <param name="y"></param>
        public void UpdatePopupY(int y)
        {
            if (popupViewExt != null)
            {
                popupViewExt.PopupY = y;
            }
        }

        /// <summary>
        /// Update anchor view method.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void UpdateAnchorView(View view)
        {
            _ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");

            if (popupViewExt != null)
            {
                inputView = view.ToPlatform(MauiContext);
                popupViewExt.AnchorView = view.ToPlatform(MauiContext);
            }
        }

        #endregion
    }
}
 