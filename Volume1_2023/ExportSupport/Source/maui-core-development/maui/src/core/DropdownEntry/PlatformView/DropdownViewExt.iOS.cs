using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;
using ContentView = Microsoft.Maui.Platform.ContentView;

namespace Syncfusion.Maui.Core.Platform
{
    /// <summary>
    /// The DropdownViewExt class which is native class to access native properties and method of popup.
    /// </summary>
    public class DropdownViewExt : ContentView
    {
        #region Fields

        private UIView? referenceView;
        private BackgroundView? backgroundView;
        private UIView? shadowView;
        private UIView? dropdownView;
        private UIView? anchorView;
        private readonly nfloat dropdDownCornerRadius = 4;
        private double popupHeight = 400;
        private double popupWidth = 0;
        private int popupX = 0;
        private int popupY = 0;

        /// <summary>
        /// The remaining space.
        /// </summary>
        private double remainingSpace = 0;
        /// <summary>
        /// Keyboard show observer 
        /// </summary>
        private NSObject? keyboardShowObserver;

        /// <summary>
        /// Keyboard hide observer
        /// </summary>
        private NSObject? keyboardHideObserver;
        /// <summary>
        /// The height of the key board.
        /// </summary>
        private nfloat keyboardHeight;


        #endregion

        #region Event

        /// <summary>
        /// Occurs when the current selection is changed.
        /// </summary>
        internal event EventHandler<EventArgs>? PopupClosed;

        /// <summary>
        /// Invokes <see cref="PopupClosed"/> event.
        /// </summary>
        /// <param name="args">args.</param>
        internal void RaisePopupClosedEvent(EventArgs args)
        {
            this.PopupClosed?.Invoke(this, args);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DropdownViewExt"/> class.
        /// </summary>
        public DropdownViewExt()
        {
            Initialize();
        }

        #endregion

        #region Properties

        internal CGPoint PreviousHitTestPoint { get; set; }

        internal bool IsPopUpOpen { get; set; }

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
                if (this.AnchorView != null)
                {
                    var frame = this.AnchorView.ConvertRectToView(this.AnchorView.Bounds, this.Window);
                    this.remainingSpace = this.Window.Bounds.Height - (Math.Abs(frame.Y) + frame.Height);
                    if (this.remainingSpace - this.keyboardHeight < value)
                    {
                        GetTopDropDownLocation(frame, value);
                    }
                    else
                    {
                        GetBottomDropDownLocation(frame, value);

                    }
                }
            }
        }

        internal UIView? AnchorView
        {
            get
            {
                return anchorView;
            }
            set
            {
                anchorView = value;
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
            }
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            this.backgroundView = new BackgroundView(this);
            this.shadowView = new UIView()
            {
                BackgroundColor = UIColor.White,
            };

            this.shadowView.Layer.CornerRadius = dropdDownCornerRadius;

            this.shadowView.Layer.MasksToBounds = false;
            this.shadowView.Layer.ShadowColor = UIColor.Gray.CGColor;
            this.shadowView.Layer.ShadowOffset = new CoreGraphics.CGSize(0f, 2f);
            this.shadowView.Layer.ShadowOpacity = 1;
            
            this.shadowView.Hidden = true;
            this.backgroundView.Hidden = true;

            this.dropdownView = new UIView();
            this.dropdownView.ClipsToBounds = true;
            this.dropdownView.Layer.CornerRadius = dropdDownCornerRadius;

            this.shadowView.Add(this.dropdownView);
            this.keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(this.ShowKeyboard);
            this.keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(this.HideKeyboard);
        }


        /// <summary>
        /// Show keyboard
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="args">The keyboard arguments</param>
        private void ShowKeyboard(object? sender, UIKit.UIKeyboardEventArgs args)
        {
#if   !MACCATALYST
            var keyboardPosition = UIKeyboard.FrameBeginFromNotification(args.Notification);
            this.keyboardHeight = (nfloat)keyboardPosition.Height;
#endif
        }

        /// <summary>
        /// Hide keyboard
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="args">The keyboard arguments</param>
        private void HideKeyboard(object? sender, UIKit.UIKeyboardEventArgs args)
        {
            this.keyboardHeight = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        internal void UpdatePopupContent(UIView view)
        {
            this.UpdateReferenceView();

            if (dropdownView != null && !dropdownView.Subviews.Contains(view))
            {
                dropdownView.AddSubview(view);
            }
        }

        internal void HidePopup()
        {
            if (this.shadowView != null && !this.shadowView.Hidden)
            {
                this.shadowView.Hidden = true;
            }

            if (this.backgroundView != null && !this.backgroundView.Hidden)
            {
                this.backgroundView.Hidden = true;
            }

            this.remainingSpace = 0;

            this.IsPopUpOpen = false;
            this.RaisePopupClosedEvent(EventArgs.Empty);
        }

        internal void ClearPopup(CGPoint point)
        {
            if (anchorView != null)
            {
                var frame = anchorView.ConvertRectToView(anchorView.Frame, backgroundView);

                if (!frame.Contains(point))
                {
                    this.HidePopup();
                }
            }
        }


        private void UpdateReferenceView()
        {
            referenceView ??= this.Window;
            if (referenceView != null)
            {
                if (backgroundView != null && !referenceView.Subviews.Contains(backgroundView))
                {
                    referenceView.AddSubview(backgroundView);
                }
                if (shadowView != null && !referenceView.Subviews.Contains(shadowView))
                {
                    referenceView.AddSubview(shadowView);
                }
            }
        }

        private void GetBottomDropDownLocation(CGRect frame,double popupHeight)
        {
            if (shadowView != null)
            {
                 shadowView.Frame = new CoreGraphics.CGRect(frame.X + (nfloat)this.PopupX, frame.Y + frame.Height + (nfloat)this.PopupY, this.PopupWidth, popupHeight);
                if (dropdownView != null && dropdownView.Subviews.Length > 0 && dropdownView.Subviews[0] != null)
                {
                    dropdownView.Frame = new CoreGraphics.CGRect(0, 0, shadowView.Frame.Width, shadowView.Frame.Height);
                    dropdownView.Subviews[0].Frame = new CoreGraphics.CGRect(0, 0, shadowView.Frame.Width, shadowView.Frame.Height);
                }
            }
       }

        private void GetTopDropDownLocation(CGRect frame,double popupHeight)
        {
            if (shadowView != null)
            {
                shadowView.Frame = new CoreGraphics.CGRect(frame.X + (nfloat)this.PopupX, frame.Y - popupHeight, this.PopupWidth, popupHeight);
                if (dropdownView != null && dropdownView.Subviews.Length > 0 && dropdownView.Subviews[0] != null)
                {
                    dropdownView.Frame = new CoreGraphics.CGRect(0, 0, shadowView.Frame.Width, shadowView.Frame.Height);
                    dropdownView.Subviews[0].Frame = new CoreGraphics.CGRect(0, 0, shadowView.Frame.Width, shadowView.Frame.Height);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void ShowPopup()
        {
            this.UpdateReferenceView();
            if (referenceView != null && backgroundView != null && shadowView != null && this.AnchorView != null && this.dropdownView != null)
            {
                if (backgroundView.Superview == null)
                {
                    referenceView.InsertSubview(backgroundView, 0);
                }
                backgroundView.Frame = new CGRect(0, 0, referenceView.Frame.Width, referenceView.Frame.Height);

                this.UpdateViewVisibility();

                var frame = this.AnchorView.ConvertRectToView(this.AnchorView.Bounds, this.Window);
             
                this.remainingSpace = this.Window.Bounds.Height - (Math.Abs(frame.Y) + frame.Height);

                if(this.PopupWidth == 0)
                {
                    this.PopupWidth = frame.Width;
                }

                if (this.remainingSpace - this.keyboardHeight < PopupHeight )
                {
                    GetTopDropDownLocation(frame,this.PopupHeight);
                }
                else
                {
                    GetBottomDropDownLocation(frame,this.PopupHeight);
                }
                
                
                this.IsPopUpOpen = true;
            }
        }

        private void UpdateViewVisibility()
        {
            if (this.shadowView != null && this.shadowView.Hidden)
            {
                this.shadowView.Hidden = false;
            }

            if (this.backgroundView != null && this.backgroundView.Hidden)
            {
                this.backgroundView.Hidden = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {

                if (AnchorView != null)
                {
                    this.AnchorView.Dispose();
                    this.AnchorView = null;
                }

                if (shadowView != null)
                {
                    this.shadowView.Dispose();
                    this.shadowView = null;
                }

                if (referenceView != null)
                {
                    this.referenceView.Dispose();
                    this.referenceView = null;
                }

                if (backgroundView != null)
                {
                    this.backgroundView.Dispose();
                    this.backgroundView = null;
                }

            }
        }

        #endregion
    }


    /// <summary>
    /// The background view class.
    /// </summary>
    internal class BackgroundView : UIView
    {

        private DropdownViewExt? popupExt;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundView" /> class.
        /// </summary>
        /// <param name="popupViewExt">The autocomplete view.</param>
        public BackgroundView(DropdownViewExt popupViewExt)
        {
            popupExt = popupViewExt;
        }


        /// <summary>
        /// The touch position for background view.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="uievent">The UIEvent.</param>
        /// <returns>The return value.</returns>
        public override UIView HitTest(CGPoint point, UIEvent? uievent)
        {
            if (uievent != null && uievent.AllTouches != null)
            {
                if (uievent.AllTouches.AnyObject != null && uievent.AllTouches.AnyObject is UITouch touch)
                {
                    if ((touch == null || touch.Phase == UITouchPhase.Began) && this.popupExt?.PreviousHitTestPoint != point)
                    {
                        if (this.popupExt != null && this.popupExt.PreviousHitTestPoint != point)
                        {
                            popupExt.ClearPopup(point);
                            popupExt.PreviousHitTestPoint = point;
                        }
                    }
                }

# if !MACCATALYST
                if (popupExt != null && popupExt.IsPopUpOpen)
                {
                    if (this.popupExt.PreviousHitTestPoint != point)
                    {
                        popupExt.ClearPopup(point);
                        popupExt.PreviousHitTestPoint = point;
                    }
                }
#endif
            }

#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
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
                if(this.popupExt != null)
                {
                    this.popupExt = null;
                }
            }
        }
    }
}