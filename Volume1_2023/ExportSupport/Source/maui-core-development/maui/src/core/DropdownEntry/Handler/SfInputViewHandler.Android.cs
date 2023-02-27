using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Handlers;
using Syncfusion.Maui.Core.Platform;
using System;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// The SfInputViewHandler class to access the native entry properties.
    /// </summary>
    public partial class SfInputViewHandler : EntryHandler
    {

        #region Fields

        private AppCompatEditTextExt? textExt;
        private SfInputView? inputView;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected override AppCompatEditText CreatePlatformView()
        {
            if (VirtualView == null)
            {
                throw new InvalidOperationException($"{nameof(VirtualView)} must be set to create a AppCompatEditText");
            }
            else
            {
                inputView = (SfInputView)this.VirtualView;
            }

            textExt = new AppCompatEditTextExt(Context);
            textExt.KeyPressed += TextExt_KeyPressed;
            return textExt;
        }

        private void TextExt_KeyPressed(object? sender, System.EventArgs e)
        {
            if(inputView!= null && textExt != null)
            {
                inputView.IsDeletedButtonPressed = textExt.IsDeleteKeyPressed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformView"></param>
        protected override void DisconnectHandler(AppCompatEditText platformView)
        {
            base.DisconnectHandler(platformView);
            if (textExt != null)
            {
                textExt.KeyPressed -= TextExt_KeyPressed;
                textExt.Dispose();
                textExt = null;
            }
        }
    }
}
 