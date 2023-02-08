using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Windows.Foundation;
using Microsoft.UI.Xaml;
using Syncfusion.Maui.Core.Platform;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Handler class for popup view.
    /// </summary>
	public partial class SfDropdownViewHandler : ContentViewHandler
    {
        #region Fields

        DropdownViewExt? popupViewExt;
        private SfDropdownView? popupView;

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected override ContentPanel CreatePlatformView()
        {
            if (VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a LayoutViewGroup");
            }
            else
            {
                popupView = (SfDropdownView)this.VirtualView;
            }

            this.popupViewExt = new DropdownViewExt()
            {

            };

            if (this.popupViewExt !=null)
            {
                this.popupViewExt.Loaded += PopupViewExt_Loaded;
            }

            if (this.popupViewExt?.Popup != null)
            {
                this.popupViewExt.Popup.Closed += Popup_Closed;
            }

            if (this.popupViewExt != null)
            {
                return this.popupViewExt;

            }
            else
            {
                return new DropdownViewExt();
            }

        }


        private void PopupViewExt_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.popupViewExt?.parentGrid != null && this.popupViewExt.parentGrid.Children[0] != null)
            {
                this.popupViewExt.parentGrid.Children[0].PointerPressed += SfPopupViewHandler_PointerPressed;
            }
        }

        private void SfPopupViewHandler_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
           
           if (this.popupView != null)
            {
                this.popupView.IsListViewClicked = true;
            }
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformView"></param>
        protected override void DisconnectHandler(ContentPanel platformView)
        {
            base.DisconnectHandler(platformView);
            if(this.popupViewExt != null)
            {
                if (this.popupViewExt.Popup != null)
                {
                    this.popupViewExt.Popup.Closed -= Popup_Closed;
                }

                this.popupViewExt.Dispose();
                this.popupViewExt = null;
            }
        }

        private void Popup_Closed(object? sender, object e)
        {
            if (popupView != null)
            {
                popupView.IsOpen = false;
            }
        }


        /// <summary>
        /// Update popup content.
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
        /// <param name="height">The height.</param>
        public void UpdatePopupHeight(double height)
        {
            if (popupViewExt != null)
            {
                this.popupViewExt.PopupHeight = height;
            }
        }

        /// <summary>
        /// Update popup width method.
        /// </summary>
        /// <param name="width">The width.</param>
        public void UpdatePopupWidth(double width)
        {
            if (popupViewExt != null)
            {
                this.popupViewExt.PopupWidth = width;
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
