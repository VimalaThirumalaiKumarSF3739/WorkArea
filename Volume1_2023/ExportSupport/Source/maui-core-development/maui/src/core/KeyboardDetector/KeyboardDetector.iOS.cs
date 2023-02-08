using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using UIKit;

namespace Syncfusion.Maui.Core.Internals
{
    public partial class KeyboardDetector 
    {        
        internal void SubscribeNativeKeyEvents(View? mauiView)
        {

        }

        internal void CreateNativeListener()
        {
            
        }

        internal void UnsubscribeNativeKeyEvents(IElementHandler handler)
        {

        }

        /// <summary>
        /// Processes the <see cref="IKeyboardListener.OnKeyDown(KeyEventArgs)"/> or <see cref="IKeyboardListener.OnKeyUp(KeyEventArgs)"/> events when the button is pressed or released from native view.
        /// </summary>
        /// <param name="presses">A set of <see cref="UIPress"/> instances that represent the new presses that occurred or the buttons that the user is no longer pressing.</param>
        /// <param name="evt">The event to which the presses belong.</param>
        /// <param name="isKeyDown">Whether key is pressed or released.</param>
        /// <returns>Returns whether the presses were handled or not.</returns>
        internal bool HandleKeyActions(NSSet<UIPress> presses, UIPressesEvent evt, bool isKeyDown)
        {
            UIKey uikey = presses!.AnyObject!.Key!;
            if (uikey != null)
            {
                KeyboardKey key = KeyboardListenerExtension.ConvertToKeyboardKey(uikey);
                var args = new KeyEventArgs(key)
                {
                    IsShiftKeyPressed = evt.ModifierFlags.HasFlag(UIKeyModifierFlags.Shift),
                    IsCtrlKeyPressed = evt.ModifierFlags.HasFlag(UIKeyModifierFlags.Control),
                    IsAltKeyPressed = evt.ModifierFlags.HasFlag(UIKeyModifierFlags.Alternate),
                    IsCommandKeyPressed = evt.ModifierFlags.HasFlag(UIKeyModifierFlags.Command)
                };

                OnKeyAction(args, isKeyDown);
                return args.Handled;
            }

            return false;
        }
    }
}
