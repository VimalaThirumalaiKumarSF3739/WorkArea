#if ANDROID
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using Java.Lang;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Platform
{
    internal class AppCompatEditTextExt : AppCompatEditText
    {
        private bool isDeletedKeyPressed = false;

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

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCompatEditTextExt"/> class.
        /// </summary>
        /// <param name="context"></param>
        public AppCompatEditTextExt(Context context) : base(context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCompatEditTextExt"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attributes.</param>
        public AppCompatEditTextExt(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCompatEditTextExt"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attributes.</param>
        /// <param name="defStyleAttr">The default style attributes. </param>
        public AppCompatEditTextExt(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCompatEditTextExt"/> class.
        /// </summary>
        /// <param name="javaReference">The java reference.</param>
        /// <param name="transfer">The transfer.</param>
        protected AppCompatEditTextExt(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        #endregion

        /// <summary>
        /// Create input connection method.
        /// </summary>
        /// <param name="outAttrs">Out attribbutes.</param>
        /// <returns>Input connection.</returns>
        public override IInputConnection? OnCreateInputConnection(EditorInfo? outAttrs)
        {
            return new CustomInputConnection(this, false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class CustomInputConnection : BaseInputConnection
    {
        /// <summary>
        /// Custom input connection method.
        /// </summary>
        /// <param name="targetView">The target view.</param>
        /// <param name="fullEditor">The full editor.</param>
        public CustomInputConnection(View? targetView, bool fullEditor) : base(targetView, fullEditor)
        {
        }

        /// <summary>
        /// Custom input coneection method.
        /// </summary>
        /// <param name="javaReference"></param>
        /// <param name="transfer"></param>
        protected CustomInputConnection(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        /// <summary>
        /// Send key event method.
        /// </summary>
        /// <param name="e">The key event args.</param>
        /// <returns>The bool value.</returns>
        public override bool SendKeyEvent(KeyEvent? e)
        {
            return base.SendKeyEvent(e);
        }
    }
}
