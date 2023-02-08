using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Platform;

namespace Syncfusion.Maui.Core.Platform
{
    internal class MauiTextFieldExt : MauiTextField
    {
        private bool isDeletedKeyPressed = false;
        private double buttonSize = 0;

        /// <summary>
        /// 
        /// </summary>
        internal event EventHandler<EventArgs>? KeyPressed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        internal void RaiseKeyPressedEvent(EventArgs args)
        {
            this.KeyPressed?.Invoke(this, args);
        }

        internal bool IsDeleteKeyPressed
        {
            get
            {
                return isDeletedKeyPressed;
            }
            set
            {
                isDeletedKeyPressed = value;
                this.RaiseKeyPressedEvent(new EventArgs());
            }
        }

        internal double ButtonSize
        {
            get
            {
                return buttonSize;
            }
            set
            {
                buttonSize = value;
                this.SetNeedsLayout();
                this.SetNeedsDisplay();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MauiTextFieldExt"/> class.
        /// </summary>
        public MauiTextFieldExt()
        {
            this.BorderStyle = UIKit.UITextBorderStyle.RoundedRect;
        }

        /// <summary>
        /// Delete back ward method.
        /// </summary>
        public override void DeleteBackward()
        {
            IsDeleteKeyPressed = true;
            base.DeleteBackward();
        }

       
    }
}
