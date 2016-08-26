# xamarin-azure

With this code you can upload directly from the Xamarin App the image file to the blob container, however, for the table storage record we have two ways: using Mobile Services or creating your own API, I took the second way.

For this project ensure you have correctly deployed the sample WebAPI application in Azure and create an Azure Storage correctly, after create the storage generate the Shared Access Signature for an specific container... you can find all configuration settings in AzureApp\Classes\AzureConstants.cs. 

happy coding. RC :)
