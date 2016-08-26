using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureApp.Classes
{
    public class HttpManager
    {
        public async static Task<string> PerformPostRequestAsync(string method, object entity)
        {
            string result = string.Empty;

            string page = string.Format(AzureConstants.WebApiUri, method);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Use the JSON formatter to create the content of the request body.
                var json = JsonConvert.SerializeObject(entity);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(page, httpContent))
                using (HttpContent content = response.Content)
                {
                    if (response.IsSuccessStatusCode)
                        result = await content.ReadAsStringAsync();
                }
            }

            return result;
        }
    }
}