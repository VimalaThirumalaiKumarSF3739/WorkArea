using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TouchDetector : IDisposable
    {
        private readonly List<ITouchListener> touchListeners;
        private bool _disposed;
        internal readonly View MauiView;
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mauiView"></param>
        public TouchDetector(View mauiView)
        {
            MauiView = mauiView;
            touchListeners = new List<ITouchListener>();
            if (mauiView.Handler != null)
            {
                SubscribeNativeTouchEvents(mauiView);
            }
            else
            {
                mauiView.HandlerChanged += MauiView_HandlerChanged;
                mauiView.HandlerChanging += MauiView_HandlerChanging;
            }
        }
       
        private void MauiView_HandlerChanged(object? sender, EventArgs e)
        {
            if (sender is View view && view.Handler != null)
                SubscribeNativeTouchEvents(view);
        }

        private void MauiView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
        {
            UnsubscribeNativeTouchEvents(e.OldHandler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public void AddListener(ITouchListener listener)
        {
            if (!touchListeners.Contains(listener))
                touchListeners.Add(listener);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public void RemoveListener(ITouchListener listener)
        {
            if (touchListeners.Contains(listener))
                touchListeners.Remove(listener);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasListener()
        {
            return touchListeners.Count > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearListeners()
        {
            touchListeners.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (disposing)
            {
                this.ClearListeners();
                this.Unsubscribe(MauiView);
            }
        }

        internal void OnTouchAction(long pointerId, PointerActions action, Point point)
        {
            PointerEventArgs eventArgs = new PointerEventArgs(pointerId, action, point);
            OnTouchAction(eventArgs);
        }

        internal void OnTouchAction(long pointerId, PointerActions action, PointerDeviceType deviceType , Point point)
        {
            PointerEventArgs eventArgs = new PointerEventArgs(pointerId, action, deviceType, point);
            OnTouchAction(eventArgs);
        }

        internal void OnTouchAction(PointerEventArgs eventArgs)
        {
            foreach (var listener in touchListeners)
            {
                listener.OnTouch(eventArgs);
            }
        }

        internal bool OnScrollAction(long pointerId, Point origin, double direction, bool? handled = null)
        {
            ScrollEventArgs eventArgs = new ScrollEventArgs(pointerId, origin, direction);
            
            if (handled != null)
                eventArgs.Handled = handled.Value;
            
            foreach (var listener in touchListeners)
            {
                listener.OnScrollWheel(eventArgs);
            }
            return eventArgs.Handled;
        }

        /// <summary>
        /// Unsubscribe the events.
        /// </summary>
        private void Unsubscribe(View? mauiView)
        {
            if (mauiView != null)
            {
                UnsubscribeNativeTouchEvents(mauiView.Handler!);
                mauiView.HandlerChanged -= MauiView_HandlerChanged;
                mauiView.HandlerChanging -= MauiView_HandlerChanging;
                mauiView = null;
            }
        }
    }
}
