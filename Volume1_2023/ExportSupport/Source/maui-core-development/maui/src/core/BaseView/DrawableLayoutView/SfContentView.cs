﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core
{

    /// <summary>
    /// 
    /// </summary>
    [ContentProperty(nameof(Content))]
    public abstract class SfContentView : SfView
    {

        /// <summary>
        /// Identifies the <see cref="Content"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Content"/> bindable property.
        /// </value>
        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(SfContentView), null, BindingMode.OneWay, null, OnContentPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public View Content
        {
            get { return (View)this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Invoked whenever the <see cref="ContentProperty"/> is set for SfContentView.
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfContentView contentView)
            {
                if (contentView != null)
                {
                    contentView.OnContentChanged(oldValue, newValue);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnContentChanged(object oldValue, object newValue)
        {
            if (oldValue != null && oldValue is View oldView)
            {
                this.Remove(oldView);
            }
            if (newValue != null && newValue is View newView)
            {
                this.Add(newView);
            }
        }
    }
}