using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Core.Platform;
using Syncfusion.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentView = Microsoft.Maui.Platform.ContentView;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    ///  Handler class for popup view.
    /// </summary>
    public partial class SfDropdownViewHandler : ContentViewHandler
    {
        #region Fields

        private DropdownViewExt? popupViewExt;
        private SfDropdownView? popupView;

        #endregion

        #region Override methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ContentView CreatePlatformView()
        {
            if (VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a PopupView");
            }
            else
            {
                popupView = (SfDropdownView)this.VirtualView;
            }

            popupViewExt = new DropdownViewExt();
            popupViewExt.PopupClosed += PopupViewExt_PopupClosed;
            return popupViewExt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformView"></param>
        protected override void DisconnectHandler(ContentView platformView)
        {
            base.DisconnectHandler(platformView);
            if (this.popupViewExt != null)
            {
                this.popupViewExt.PopupClosed -= PopupViewExt_PopupClosed;
                this.popupViewExt.Dispose();
                this.popupViewExt = null;
            }
        }

        #endregion

        #region Methods

        private void PopupViewExt_PopupClosed(object? sender, EventArgs e)
        {
            if (popupView != null)
            {
                popupView.IsOpen = false;
            }
        }

        /// <summary>
        /// Update popup content method.
        /// </summary>
        /// <param name="listView">The listview.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void UpdatePopupContent(View listView)
        {
            _ = MauiContext ?? throw new InvalidOperationException($"{nameof(MauiContext)} should have been set by base class.");
            popupViewExt?.UpdatePopupContent(listView.ToPlatform(MauiContext));
        }

        /// <summary>
        /// Hide popup method.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void HidePopup()
        {
            popupViewExt?.HidePopup();
        }

        /// <summary>
        /// Show popup method.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
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
                popupViewExt.AnchorView = view.ToPlatform(MauiContext);
            }
        }

        #endregion

    }
}
