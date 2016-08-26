using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureApp.WebAPI.Entities
{
    public class Participant : TableEntity
    {
        public Participant()
        {
            this.Timestamp = DateTime.Now;
        }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public string Age { get; set; }

        public string Weight { get; set; }

        public string FileName { get; set; }

    }
}