using Android.OS;
using AzureApp.Droid.Dependencies;
using AzureApp.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(SpecificPlatform))]

namespace AzureApp.Droid.Dependencies
{
    public class SpecificPlatform : ISpecificPlatform
    {
        public bool CheckIfSimulator()
        {
            if (Build.Fingerprint != null)
            {
                if (Build.Fingerprint.Contains("vbox") ||
                    Build.Fingerprint.Contains("generic"))
                    return true;
            }
            return false;
        }

        public void CloseApplication()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}