using Microsoft.UI.Xaml;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using MauiView = Microsoft.Maui.Controls.View;
using Key = Windows.System.VirtualKey;
using Microsoft.UI.Xaml.Input;

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
                        nativeView.KeyDown += PlatformView_KeyDown;
                        nativeView.KeyUp += PlatformView_KeyUp;
                    }
                }
            }
        }

        internal void CreateNativeListener()
        {
            SubscribeNativeKeyEvents(MauiView);
        }

        private void HandleKeyActions(KeyRoutedEventArgs e, bool isKeyDown)
        {
            KeyboardKey key = KeyboardListenerExtension.ConvertToKeyboardKey(e.Key);
            var args = new KeyEventArgs(key)
            {
                IsShiftKeyPressed = KeyboardListenerExtension.CheckShiftKeyPressed(),
                IsCtrlKeyPressed = KeyboardListenerExtension.CheckControlKeyPressed(),
                IsAltKeyPressed = KeyboardListenerExtension.CheckAltKeyPressed(),
                IsCommandKeyPressed = false
            };

            OnKeyAction(args, isKeyDown);
            e.Handled = args.Handled;
        }
        private void PlatformView_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            this.HandleKeyActions(e, false);
        }

        private void PlatformView_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            this.HandleKeyActions(e, true);
        }

        internal void UnsubscribeNativeKeyEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                UIElement? nativeView = handler.PlatformView as UIElement;

                if (nativeView != null)
                {
                    nativeView.KeyDown -= PlatformView_KeyDown;
                    nativeView.KeyUp -= PlatformView_KeyUp;
                }
            }
        }
    }
}
