using AzureApp.Classes;
using AzureApp.Interfaces;
using AzureApp.iOS.Dependencies;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(ProcessProvider))]

namespace AzureApp.iOS.Dependencies
{
    public class ProcessProvider : IProcessProvider
    {
        public Stream GetDummyStream()
        {
            return AppDelegate.GetStream();
        }

        public async Task<string> ProcessParticipantAsync(Stream stream)
        {
            Byte[] content = ToByteArrayFromStream(stream);
            return await UploadToBlobStorageAsync(content);
        }

        private byte[] ToByteArrayFromStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private async Task<string> UploadToBlobStorageAsync(Byte[] blobContent)
        {
            string containerName = AzureConstants.ContainerName;
            return await PutBlobAsync(containerName, blobContent);
        }

        private async Task<string> PutBlobAsync(string containerName, Byte[] blobContent)
        {
            string result = string.Empty;
            string identifier = string.Format("{0}.jpg", Guid.NewGuid().ToString());
            Int32 blobLength = blobContent.Length;
            const String blobType = "BlockBlob";

            String urlPath = String.Format("{0}/{1}", containerName, identifier);
            string queryString = AzureConstants.SaSQueryString;
            Uri uri = new Uri(AzureConstants.BlobEndPoint + urlPath + '?' + queryString);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-ms-blob-type", blobType);

            HttpContent requestContent = new ByteArrayContent(blobContent);
            HttpResponseMessage response = await client.PutAsync(uri, requestContent);

            if (response.IsSuccessStatusCode == true)
            {
                result = identifier;
            }
            else
            {
                result = string.Empty;
            }

            return result;
        }
    }
}