using MauiView = Microsoft.Maui.Controls.View;
using Microsoft.Maui;
using Android.Views;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// 
    /// </summary>
    public partial class KeyboardDetector
    {
        internal void SubscribeNativeKeyEvents(MauiView? mauiView)
        {
            if (mauiView != null)
            {
                var handler = mauiView.Handler;
                View? nativeView = handler?.PlatformView as View;

                if (nativeView != null)
                {
                    if (keyboardListeners.Count > 0)
                    {
                        nativeView.KeyPress += PlatformView_KeyPress;
                    }
                }
            }
        }

        internal void CreateNativeListener()
        {
            SubscribeNativeKeyEvents(MauiView);
        }

        private void PlatformView_KeyPress(object? sender, Android.Views.View.KeyEventArgs e)
        {
            KeyboardKey key = KeyboardListenerExtension.ConvertToKeyboardKey(e.KeyCode);
            var args = new KeyEventArgs(key)
            {                                 
                IsShiftKeyPressed = e.Event!.MetaState.HasFlag(MetaKeyStates.ShiftOn),
                IsCtrlKeyPressed = e.Event!.MetaState.HasFlag(MetaKeyStates.CtrlOn),
                IsAltKeyPressed = e.Event!.MetaState.HasFlag(MetaKeyStates.AltOn),
                IsCommandKeyPressed = false
            };

            OnKeyAction(args, e.Event.Action != KeyEventActions.Up); 
            e.Handled = args.Handled;
        }

        internal void UnsubscribeNativeKeyEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                View? nativeView = handler.PlatformView as View;

                if (nativeView != null)
                {
                    nativeView.KeyPress -= PlatformView_KeyPress;
                }
            }
        }
    }
}
