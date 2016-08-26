using AzureApp.Interfaces;
using AzureApp.iOS.Dependencies;
using ObjCRuntime;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(SpecificPlatform))]

namespace AzureApp.iOS.Dependencies
{
    public class SpecificPlatform : ISpecificPlatform
    {
        public bool CheckIfSimulator()
        {
            if (Runtime.Arch == Arch.SIMULATOR)
                return true;
            return false;
        }

        public void CloseApplication()
        {
            throw new NotImplementedException();
        }
    }
}