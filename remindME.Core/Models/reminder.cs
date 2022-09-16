using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;


namespace remindME.Core.Models
{

    public class Reminder
    {
        [JsonProperty(PropertyName = "datetime")]
        public string datetime { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string message { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

    }
}
