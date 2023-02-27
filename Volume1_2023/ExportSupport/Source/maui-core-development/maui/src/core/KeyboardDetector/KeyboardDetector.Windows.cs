using Microsoft.UI.Xaml;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using MauiView = Microsoft.Maui.Controls.View;
using Key = Windows.System.VirtualKey;
using Microsoft.UI.Xaml.Input;
using Windows.UI.Core;

namespace Syncfusion.Maui.Core.Internals
{
    public partial class KeyboardDetector
    {
        internal void SubscribeNativeKeyEvents(MauiView? mauiView)
        {
            if (mauiView != null)
            {
                var handler = mauiView.Handler;
                UIElement? nativeView = handler?.PlatformView as UIElement;
                if (nativeView != null)
                {
                    if (keyboardListeners.Count > 0)
                    {
                        nativeView.KeyDown += this.PlatformView_KeyDown;
                        nativeView.KeyUp += this.PlatformView_KeyUp;
                        nativeView.PreviewKeyDown += this.PlatformView_PreviewKeyDown;
                    }
                }
            }
        }

        internal void CreateNativeListener()
        {
            SubscribeNativeKeyEvents(MauiView);
        }

        private void HandleKeyActions(KeyRoutedEventArgs e, KeyActions keyAction)
        {
            KeyboardKey key = KeyboardListenerExtension.ConvertToKeyboardKey(e.Key);
            var args = new KeyEventArgs(key)
            {
                IsShiftKeyPressed = KeyboardListenerExtension.CheckShiftKeyPressed(),
                IsCtrlKeyPressed = KeyboardListenerExtension.CheckControlKeyPressed(),
                IsAltKeyPressed = KeyboardListenerExtension.CheckAltKeyPressed(),
                IsCommandKeyPressed = false
            };

            args.KeyAction = keyAction;
            OnKeyAction(args);
            e.Handled = args.Handled;
        }
        private void PlatformView_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            this.HandleKeyActions(e, KeyActions.KeyUp);
        }

        private void PlatformView_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            this.HandleKeyActions(e, KeyActions.KeyDown);
        }

        private void PlatformView_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            this.HandleKeyActions(e, KeyActions.PreviewKeyDown);
        }

        internal void UnsubscribeNativeKeyEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                UIElement? nativeView = handler.PlatformView as UIElement;

                if (nativeView != null)
                {
                    nativeView.KeyDown -= this.PlatformView_KeyDown;
                    nativeView.KeyUp -= this.PlatformView_KeyUp;
                    nativeView.PreviewKeyDown -= this.PlatformView_PreviewKeyDown;
                }
            }
        }
    }
}
