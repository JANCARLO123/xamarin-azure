using AzureApp.WebAPI.Classes;
using AzureApp.WebAPI.Classes.Storage;
using AzureApp.WebAPI.Filters;
using AzureApp.WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.IO;

namespace AzureApp.WebAPI.Controllers
{
    /// <summary>
    /// controller responsible to provide data to the mobile apps.
    /// </summary>
    /// 

    public class ProviderController : ApiController
    {

        /// <summary>
        /// register record.
        /// </summary>
        /// <returns></returns>
        [CustomException]
        [HttpPost]
        [Route("api/saveparticipant")]
        public IHttpActionResult SaveParticipant(Participant participant)
        {
            var storageConnectionString = StorageManager.GetConnectionString();
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(storageConnectionString);

            StorageManager sm = new StorageManager();
            var myTask = Task<bool>.Factory.StartNew(() => sm.RegisterParticipant(storageAccount, participant));
            bool result = myTask.Result;

            return Json(result);
        }
    }
}