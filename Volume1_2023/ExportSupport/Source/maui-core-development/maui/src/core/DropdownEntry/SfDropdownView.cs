
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal class SfDropdownView : ContentView
    {
        private View? popupListView;
#if !WINDOWS

        private double popupWidth;
        private double popupHeight;
#endif

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="AnchorView"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AnchorView"/> bindable property.
        /// </value>
        public static readonly BindableProperty AnchorViewProperty =
            BindableProperty.Create(nameof(AnchorView), typeof(View), typeof(SfDropdownView), null, BindingMode.OneWay, null, propertyChanged: OnAnchorViewChanged);

        /// <summary>
        /// Identifies the <see cref="PopupHeight"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="PopupHeight"/> bindable property.
        /// </value>
        public static readonly BindableProperty PopupHeightProperty =
            BindableProperty.Create(nameof(PopupHeight), typeof(double), typeof(SfDropdownView), null, BindingMode.OneWay, null, propertyChanged: OnPopupHeightChanged);

        /// <summary>
        /// Identifies the <see cref="PopupWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="PopupWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty PopupWidthProperty =
            BindableProperty.Create(nameof(PopupWidth), typeof(double), typeof(SfDropdownView), null, BindingMode.OneWay, null, propertyChanged: OnPopupWidthChanged);


        /// <summary>
        /// Identifies the <see cref="PopupX"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="PopupX"/> bindable property.
        /// </value>
        public static readonly BindableProperty PopupXProperty =
            BindableProperty.Create(nameof(PopupX), typeof(int), typeof(SfDropdownView), 0, BindingMode.OneWay, null, propertyChanged: OnPopupXChanged);

        /// <summary>
        /// Identifies the <see cref="PopupY"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="PopupY"/> bindable property.
        /// </value>
        public static readonly BindableProperty PopupYProperty =
            BindableProperty.Create(nameof(PopupY), typeof(int), typeof(SfDropdownView), 0, BindingMode.OneWay, null, propertyChanged: OnPopupYChanged);

        /// <summary>
        /// Identifies the <see cref="IsOpen"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsOpen"/> bindable property.
        /// </value>
        public static readonly BindableProperty IsOpenProperty =
          BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(SfDropdownView), false, propertyChanged: OnPopupStateChanged);
#if WINDOWS
        internal static readonly BindableProperty IsListViewClickedProperty =
          BindableProperty.Create(nameof(IsListViewClicked), typeof(bool), typeof(SfDropdownView), false, propertyChanged: null);
#endif
#endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        internal View AnchorView
        {
            get { return (View)this.GetValue(AnchorViewProperty); }
            set { this.SetValue(AnchorViewProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal double PopupHeight
        {
            get { return (double)this.GetValue(PopupHeightProperty); }
            set { this.SetValue(PopupHeightProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal double PopupWidth
        {
            get { return (double)this.GetValue(PopupWidthProperty); }
            set { this.SetValue(PopupWidthProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal int PopupX
        {
            get { return (int)this.GetValue(PopupXProperty); }
            set { this.SetValue(PopupXProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal int PopupY
        {
            get { return (int)this.GetValue(PopupYProperty); }
            set { this.SetValue(PopupYProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }


        /// <summary>
        /// 
        /// </summary>
        internal bool IsDropDownCicked
        {
            get;
            set;
        }

#if WINDOWS
        /// <summary>
        /// 
        /// </summary>
        internal bool IsListViewClicked
        {
            get { return (bool)GetValue(IsListViewClickedProperty); }
            set { SetValue(IsListViewClickedProperty, value); }
        }
#endif

        #endregion

        #region Property Changed

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnPopupHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownView popup && popup.Handler is SfDropdownViewHandler handler)
                handler?.UpdatePopupHeight((double)newValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnPopupWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownView popup && popup.Handler is SfDropdownViewHandler handler)
                handler?.UpdatePopupWidth((double)newValue);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnPopupXChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownView popup && popup.Handler is SfDropdownViewHandler handler)
                handler?.UpdatePopupX((int)newValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnPopupYChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownView popup && popup.Handler is SfDropdownViewHandler handler)
                handler?.UpdatePopupY((int)newValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnAnchorViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfDropdownView popup && popup.Handler is SfDropdownViewHandler handler)
                handler?.UpdateAnchorView((View)newValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnPopupStateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bool)newValue)
            {
                (bindable as SfDropdownView)?.HidePopup();
            }
            else
            {
                (bindable as SfDropdownView)?.ShowPopup();
            }

        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfDropdownView"/> class.
        /// </summary>
        public SfDropdownView()
        {
            this.IsClippedToBounds = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        internal void HidePopup()
        {
            if (this.Handler is SfDropdownViewHandler handler)
                handler?.HidePopup();
            RaisePopupClosedEvent(EventArgs.Empty);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView"></param>
        internal void UpdatePopupContent(View listView)
        {
            popupListView = listView;
            if (this.Handler is SfDropdownViewHandler handler)
                handler?.UpdatePopupContent(listView);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void ShowPopup()
        {
            if (this.Handler is SfDropdownViewHandler handler)
                handler?.ShowPopup();
            RaisePopupOpenedEvent(EventArgs.Empty);

        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the current selection is changed.
        /// </summary>
        internal event EventHandler<EventArgs>? PopupClosed;

        /// <summary>
        /// Occurs when the current selection is changed.
        /// </summary>
        internal event EventHandler<EventArgs>? PopupOpened;

        /// <summary>
        /// Invokes <see cref="PopupClosed"/> event.
        /// </summary>
        /// <param name="args">args.</param>
        internal void RaisePopupClosedEvent(EventArgs args)
        {
            this.PopupClosed?.Invoke(this, args);
        }

        /// <summary>
        /// Invokes <see cref="PopupOpened"/> event.
        /// </summary>
        /// <param name="args">args.</param>
        internal void RaisePopupOpenedEvent(EventArgs args)
        {
            this.PopupOpened?.Invoke(this, args);
        }

        #endregion

        #region Override methods

#if !WINDOWS
        protected override Size ArrangeOverride(Rect bounds)
        {

            if (this.popupWidth != bounds.Width || this.popupHeight != bounds.Height)
            {

                popupWidth = bounds.Width;
                popupHeight = bounds.Height;
                if (this.Handler != null && this.Handler is SfDropdownViewHandler handler)
                {
#if MACCATALYST
                   handler?.UpdatePopupHeight(this.PopupHeight);
                   handler?.UpdatePopupWidth(this.PopupWidth);
                   handler?.UpdatePopupX(this.PopupX);
                   handler?.UpdatePopupY(this.PopupY);
#endif
                    if (this.IsOpen)
                    {
                       handler?.ShowPopup();
                    }
                }
            }

            return base.ArrangeOverride(bounds);

        }
#endif

        /// <summary>
        /// 
        /// </summary>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            if (this.Handler != null && this.Handler is SfDropdownViewHandler handler)
            {
                handler.UpdatePopupHeight(this.PopupHeight);
                handler.UpdatePopupWidth(this.PopupWidth);
                handler.UpdatePopupX(this.PopupX);
                handler.UpdatePopupY(this.PopupY);
                if (popupListView != null)
                {
                    handler.UpdatePopupContent(popupListView);
                }
                handler.UpdateAnchorView(this.AnchorView);
                if (IsOpen)
                {
                    handler.ShowPopup();
                }
            }
        }
        #endregion
    }
}