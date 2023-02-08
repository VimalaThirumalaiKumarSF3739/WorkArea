using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Helper
{
    /// <summary>
    /// 
    /// </summary>
    internal class CustomDisplayAlert
    {

#if ANDROID
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="okButton"></param>
        /// <param name="cancelButton"></param>
        /// <returns></returns>
        internal Task<bool> DisplayAlert(string title, string message, string okButton, string cancelButton)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();            
            var builder = new Android.App.AlertDialog.Builder(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity);

            builder.SetTitle(title);
            builder.SetMessage(message);

            builder.SetPositiveButton(okButton, (senderAlert, args) =>
            {
                taskCompletionSource.SetResult(true);
            });

            builder.SetNeutralButton(cancelButton, (senderAlery, args) =>
            {
                taskCompletionSource.SetResult(false);
            });

            var alertDialog = builder.Create();
            alertDialog!.SetCanceledOnTouchOutside(false);
            alertDialog.Show();

            return taskCompletionSource.Task;
        }

#endif
    }
}
