using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Helper class to validate license.
    /// </summary>
    internal static class LicenseHelper
    {
#if SyncfusionLicense
        static bool isNeedtoQuit = false;
#endif
        static bool isNeedtoQuitSet = false;   
        static string licenseMessage = string.Empty;

        /// <summary>
        /// Checks if the given license key is valid for View type controls.
        /// </summary>
        /// <param name="view">View</param>
        /// <param name="isInternalDependentControl">ControlName</param>
        internal static void ValidateLicense(this View view, bool isInternalDependentControl = false)
        {
#if SyncfusionLicense
            ValidateLicense(isInternalDependentControl);
#endif
        }

        //// This ValidateLicense method is added to validate license for page type controls.
        //// This is method will removed when ValidateLicense() for view is modified to support all controls

        /// <summary>
        /// Checks if the given license key is valid for page type controls.
        /// </summary>
        /// <param name="page">Page</param>
        internal static void ValidateLicense(this Page page)
        {
#if SyncfusionLicense
           ValidateLicense(false);
#endif
        }

        /// <summary>
        /// Shows a alert if the given license key is not valid.
        /// </summary>
        /// <param name="message">string</param>
        internal static async void ShowLicenseMessage(string message)
        {
            Page? page = Application.Current?.MainPage;
            if (page != null)
            {
                try
                {
                    string cancelButtonText = "OK";
                    if (isNeedtoQuitSet)
                    {
                        cancelButtonText = "Quit";
                    }
#if ANDROID
                    var alert = new Syncfusion.Maui.Core.Helper.CustomDisplayAlert();
                    bool result = await alert.DisplayAlert("Syncfusion License", licenseMessage, "Claim License", cancelButtonText);
#else
                    bool result = await page.DisplayAlert("Syncfusion License", licenseMessage, "Claim License", cancelButtonText);
#endif
#if SyncfusionLicense
                    if (result)
                    {
                        await Browser.Default.OpenAsync(Syncfusion.Licensing.FusionLicenseProvider.ClaimLicenseKeyURL, BrowserLaunchMode.SystemPreferred);
                    }

                    if (isNeedtoQuitSet)
                    {
#if IOS
		        Environment.Exit(0);
#else
                        Application.Current?.Quit();
#endif
                    }
#endif
                }
                catch (Exception)
                {

                }
            }

            LicensePopupClosed?.Invoke(page, new EventArgs());
        }

        /// <summary>
        /// Checks if the given license key is valid.
        /// </summary>
        /// <param name="isInternalDependentControl"></param>
        internal static void ValidateLicense(bool isInternalDependentControl)
        {
#if SyncfusionLicense
            string message = Syncfusion.Licensing.FusionLicenseProvider.GetLicenseType(Syncfusion.Licensing.Platform.MAUI,out isNeedtoQuit, isInternalDependentControl);
            
            if (!string.IsNullOrEmpty(message) && !isNeedtoQuitSet)
            {
                licenseMessage = message;
                if (Application.Current != null)
                {
                    if (Application.Current.MainPage != null)
                    {
                        MainThread.BeginInvokeOnMainThread(() => ShowLicenseMessage(licenseMessage));
                    }
                    else
                    {
                        Application.Current.PropertyChanged -= Current_PropertyChanged;
                        Application.Current.PropertyChanged += Current_PropertyChanged;
                    }
                }
            }
            if (isNeedtoQuit)
                isNeedtoQuitSet = true;
#endif
        }

        private static void Current_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null && e.PropertyName.Equals("MainPage") && Application.Current?.MainPage != null)
            {
                Application.Current.MainPage.Loaded -= MainPage_Loaded;
                Application.Current.MainPage.Loaded += MainPage_Loaded;
            }
        }

        private static void MainPage_Loaded(object? sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() => ShowLicenseMessage(licenseMessage));
        }

        /// <summary>
        /// Event that is raised when the syncfusion license popup is dismissed.
        /// </summary>
        internal static event EventHandler<EventArgs>? LicensePopupClosed;

    }
}
