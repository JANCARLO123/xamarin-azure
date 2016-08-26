using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AzureApp.Classes
{
    public class Utilities
    {
        public static void DisplayMessage(string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await App.Current.MainPage.DisplayAlert(AzureConstants.ApplicationName, message, "Ok");
            });
        }
    }
}
