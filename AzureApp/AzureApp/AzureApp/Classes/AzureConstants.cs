namespace AzureApp.Classes
{
    public static class AzureConstants
    {
        /// <summary>
        ///Change the dummy settings for the correct Azure settings.
        /// </summary>

        public static string ApplicationName = "AzureApp";
        public static string WebApiUri = "https://AZUREWEBAPI.azurewebsites.net/api/{0}";
        public static string Account = "AzureApp";
        public static string SharedKeyAuthorizationScheme = "SharedKey";
        public static string BlobEndPoint = "https://AZURE_STORAGE_ACCOUNT.blob.core.windows.net/";
        public static string Key = "AZURE_TABLESTORAGE_KEY";
        public static string ContainerName = "files";
        public static string FileLocation = BlobEndPoint + ContainerName;
        public static string SaSQueryString = "st=&se=&sp=&sv=&sr=&sig=";
    }
}