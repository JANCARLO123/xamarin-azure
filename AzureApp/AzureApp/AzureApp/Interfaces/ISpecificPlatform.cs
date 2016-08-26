namespace AzureApp.Interfaces
{
    public interface ISpecificPlatform
    {
        bool CheckIfSimulator();

        void CloseApplication();
    }
}