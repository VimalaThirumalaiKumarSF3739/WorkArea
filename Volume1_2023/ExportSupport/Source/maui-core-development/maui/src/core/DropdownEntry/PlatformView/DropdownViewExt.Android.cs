using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Text;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Color = Microsoft.Maui.Graphics.Color;
using Microsoft.Maui.Platform;
using System;
using Android.Runtime;
using ARect = Android.Graphics.Rect;
using Rectangle = Microsoft.Maui.Graphics.Rect;
using Size = Microsoft.Maui.Graphics.Size;
using Android.Widget;
using Microsoft.Maui.Controls;
using View = Android.Views.View;
using Java.Lang;
using Rect = Android.Graphics.Rect;
using Android.Graphics.Drawables;

namespace Syncfusion.Maui.Core.Platform
{
    /// <summary>
    /// The DropdownViewExt class which is native class to access native properties and method of popup.
    /// </summary>
    public class DropdownViewExt : ContentViewGroup
    {
        #region Fields

        private double popupHeight = 400;
        private double popupWidth = 0;
        private int popupX = 0;
        private int popupY = 0;
        private View? anchorView;

        #endregion

        #region Properties

        internal PopupWindow? PopupWindow
        {
            get; set;
        }


        internal View? AnchorView
        {
            get
            {
                return anchorView;
            }
            set
            {
                anchorView = value;
                if(this.PopupWindow != null && value != null)
                    this.PopupWindow.Width = value.Width;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal double PopupHeight
        {
            get
            {
                return popupHeight; 
            }
            set
            {
                this.popupHeight = value;
                this.UpdatePopUpHeight();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal double PopupWidth
        {
            get
            {
                return popupWidth;
            }
            set
            {
                this.popupWidth = value;
                this.UpdatePopUpWidth();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal int PopupX
        {
            get
            {
                return popupX;
            }
            set
            {
                this.popupX = value;
                this.UpdatePopUpX();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal int PopupY
        {
            get
            {
                return popupY;
            }
            set
            {
                this.popupY = value;
                this.UpdatePopUpY();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DropdownViewExt"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public DropdownViewExt(Context context) : base(context)
        {
            this.Initialize(context);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropdownViewExt"/> class.
        /// </summary>
        /// <param name="javaReference">The java refernce.</param>
        /// <param name="transfer">The transfer.</param>
        public DropdownViewExt(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            var context = Context;
            ArgumentNullException.ThrowIfNull(context);
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="DropdownViewExt"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attributes.</param>
        public DropdownViewExt(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.Initialize(context);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropdownViewExt"/> class.
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="attrs">The attributes.</param>
        /// <param name="drawable">The drawable.</param>
        public DropdownViewExt(Context context, IAttributeSet attrs, IDrawable? drawable = null) : base(context, attrs)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropdownViewExt"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attributes.</param>
        /// <param name="defStyleAttr">The default style attributes.</param>
        public DropdownViewExt(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            this.Initialize(context);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DropdownViewExt"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attributes.</param>
        /// <param name="defStyleAttr">The default style attributes.</param>
        /// <param name="defStyleRes">The default style res.</param>
        public DropdownViewExt(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            this.Initialize(context);
        }

        #endregion

        #region Methods

        private void Initialize(Context context)
        {
            GradientDrawable drawable = new();
            drawable.SetColor(Android.Graphics.Color.White);
            drawable.SetCornerRadius(16);

            this.PopupWindow = new PopupWindow(context);
            this.PopupWindow.SetBackgroundDrawable(drawable);
            this.PopupWindow.Elevation = 10;
            this.PopupWindow.InputMethodMode = InputMethod.Needed;
            this.PopupWindow.OutsideTouchable = true;
            this.PopupWindow.ClippingEnabled = true;        
            this.PopupWindow.Height = (int)this.PopupHeight;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        internal void UpdatePopupContent(View view)
        {
            if (this.PopupWindow != null)
            {
                GradientDrawable drawable = new();
                drawable.SetColor(Android.Graphics.Color.White);
                drawable.SetCornerRadius(16);

                view.ClipToOutline = true;
                view.Background = drawable;
                this.PopupWindow.ContentView = view;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void HidePopup()
        {
            if (this.PopupWindow != null)
            {
                this.PopupWindow.Dismiss();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        internal void ShowPopup()
        {
            if (this.PopupWindow != null && this.AnchorView != null)
            {
                if(this.PopupWindow.Width == 0)
                {
                    this.PopupWindow.Width = this.AnchorView.Width;
                }

                if(PopupWindow.Height != this.PopupHeight)
                {
                    this.UpdatePopUpHeight();
                }
                this.PopupWindow.ShowAsDropDown(AnchorView);
                this.PopupWindow.Update(this.AnchorView, this.popupX, this.PopupY, this.PopupWindow.Width, this.PopupWindow.Height);           
            }
        }

        private void UpdatePopUpHeight()
        {
            if (this.PopupWindow != null && this.AnchorView != null)
            {
                if (this.Resources != null && this.Resources.DisplayMetrics != null)
                    this.PopupWindow.Height = (int)(this.PopupHeight * this.Resources.DisplayMetrics.Density);
                if (this.PopupWindow.Width > 0 && this.PopupWindow.Height > 0 && this.PopupWindow.IsShowing)
                {
                    this.PopupWindow.Update(this.AnchorView, this.popupX, this.PopupY, this.PopupWindow.Width, this.PopupWindow.Height);
                }
            }
        }

        private void UpdatePopUpWidth()
        {
            if (this.PopupWindow != null && this.AnchorView != null)
            {
                if (this.Resources != null && this.Resources.DisplayMetrics != null && this.PopupWidth != 0)
                    this.PopupWindow.Width = (int)(this.PopupWidth * this.Resources.DisplayMetrics.Density);

                if (this.PopupWindow.Width > 0 && this.PopupWindow.Height > 0 && this.PopupWindow.IsShowing)
                {
                    this.PopupWindow.Update(this.AnchorView, this.popupX, this.PopupY, this.PopupWindow.Width, this.PopupWindow.Height);
                }
            }
        }

        private void UpdatePopUpX()
        {       
            if (this.PopupWindow != null && this.AnchorView != null)
            {
                if (this.Resources != null && this.Resources.DisplayMetrics != null)
                    this.popupX = (int)(this.PopupX * this.Resources.DisplayMetrics.Density);
                if (this.PopupWindow.Width > 0 && this.PopupWindow.Height > 0 && this.PopupWindow.IsShowing)
                {
                    this.PopupWindow.Update(this.AnchorView, this.popupX, this.PopupY, this.PopupWindow.Width, this.PopupWindow.Height);
                }
            }
            
        }

        private void UpdatePopUpY()
        {
            if (this.PopupWindow != null && this.AnchorView != null)
            {
                if (this.Resources != null && this.Resources.DisplayMetrics != null)
                    this.popupY = (int)(this.PopupY * this.Resources.DisplayMetrics.Density);
                if (this.PopupWindow.Width > 0 && this.PopupWindow.Height > 0 && this.PopupWindow.IsShowing)
                {
                    this.PopupWindow.Update(this.AnchorView, this.popupX, this.PopupY, this.PopupWindow.Width, this.PopupWindow.Height);

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(disposing)
            {
                if(this.PopupWindow!= null)
                {
                    this.PopupWindow.Dispose();
                    this.PopupWindow = null;
                }

                if (this.AnchorView != null)
                {
                    this.AnchorView.Dispose();
                    this.AnchorView = null;
                }
            }
        }

        #endregion
    }
}