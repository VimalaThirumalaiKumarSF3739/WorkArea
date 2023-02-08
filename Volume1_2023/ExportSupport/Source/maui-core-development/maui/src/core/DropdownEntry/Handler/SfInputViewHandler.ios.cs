using System;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Core.Platform;
using UIKit;

namespace Syncfusion.Maui.Core
{ 
    /// <summary>
    /// The SfInputViewHandler class to access the native entry properties.
    /// </summary>
    public partial class SfInputViewHandler : EntryHandler
    {
        #region Fields

        private MauiTextFieldExt? textExt;
        private SfInputView? inputView;

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override MauiTextField CreatePlatformView()
        {
            if (VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a AppCompatEditText");
            }
            else
            {
                inputView = (SfInputView)this.VirtualView;
            }

            textExt = new MauiTextFieldExt();
            textExt.KeyPressed += TextExt_KeyPressed;
            return textExt;
           
        }

        private void TextExt_KeyPressed(object? sender, EventArgs e)
        {
            if (inputView != null && textExt != null)
            {
                inputView.IsDeletedButtonPressed = textExt.IsDeleteKeyPressed;
            }
        }

        internal void UpdateButtonSize()
        {
            if (inputView != null && textExt != null)
            {
                textExt.ButtonSize = inputView.ButtonSize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformView"></param>
        protected override void DisconnectHandler(MauiTextField platformView)
        {
            base.DisconnectHandler(platformView);
            if (textExt != null)
            {
                textExt.KeyPressed -= TextExt_KeyPressed;
                textExt.Dispose();
                textExt = null;
            }
        }
        #endregion
    }
}
