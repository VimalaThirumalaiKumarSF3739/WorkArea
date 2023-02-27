using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// 
    /// </summary>
    public partial class KeyboardDetector : IDisposable
    {
        private List<IKeyboardListener> keyboardListeners;
        internal readonly View MauiView;
        private bool _disposed;
        private bool isViewListenerAdded;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mauiView"></param>
        public KeyboardDetector(View mauiView)
        {
            MauiView = mauiView;
            keyboardListeners = new List<IKeyboardListener>();
            if (mauiView.Handler != null)
            {
                SubscribeNativeKeyEvents(mauiView);
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
                SubscribeNativeKeyEvents(view);
        }

        private void MauiView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
        {
            UnsubscribeNativeKeyEvents(e.OldHandler);
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
                isViewListenerAdded = false;
                ClearListeners();
                this.Unsubscribe(MauiView);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public void AddListener(IKeyboardListener listener)
        {
            if (keyboardListeners == null)
                keyboardListeners = new List<IKeyboardListener>();
            if (!keyboardListeners.Contains(listener))
                keyboardListeners.Add(listener);

            // If dynamically call AddKeyboardListener mehtod or call after the MauiView's handler set, native listeners will be created in the below code. 
            if (!isViewListenerAdded)
            {
                CreateNativeListener();
            }
            isViewListenerAdded = true;
        }


        /// <summary>
        /// 
        /// </summary>
        public void ClearListeners()
        {
            keyboardListeners!.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasListener()
        {
            return keyboardListeners?.Count > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public void RemoveListener(IKeyboardListener listener)
        {
            if (listener is IKeyboardListener keyListener && keyboardListeners != null && keyboardListeners.Contains(keyListener))
                keyboardListeners.Remove(keyListener);
        }

        internal void OnKeyAction(KeyEventArgs args)
        {
            if (keyboardListeners.Count == 0)
                return;

            if (args.KeyAction == KeyActions.PreviewKeyDown)
            {
                foreach (IKeyboardListener listener in keyboardListeners)
                {
                    listener.OnPreviewKeyDown(args);
                }
            }
            else if (args.KeyAction == KeyActions.KeyDown)
            {
                foreach (IKeyboardListener listener in keyboardListeners)
                {
                    listener.OnKeyDown(args);
                }
            }
            else
            {
                foreach (IKeyboardListener listener in keyboardListeners)
                {
                    listener.OnKeyUp(args);
                }
            }
        }

        /// <summary>
        /// Unsubscribe the events 
        /// </summary>
        /// <param name="mauiView"></param>
        private void Unsubscribe(View? mauiView)
        {
            if (mauiView != null)
            {
                UnsubscribeNativeKeyEvents(mauiView.Handler!);
                mauiView.HandlerChanged -= MauiView_HandlerChanged;
                mauiView.HandlerChanging -= MauiView_HandlerChanging;                
                mauiView = null;
            }
        }
    }
}
