using AzureApp.Entities;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AzureApp.Classes
{
    public class Sender
    {
        public static async Task<bool> SaveDataInCloudAsync(Participant participant)
        {
            bool result = false;
            var data = await HttpManager.PerformPostRequestAsync("saveparticipant", participant);
            if (data != null)
            {
                bool register_result = JsonConvert.DeserializeObject<bool>(data);

                if (register_result)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}