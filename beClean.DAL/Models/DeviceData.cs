using Newtonsoft.Json;
using System.Collections.Generic;

namespace beClean.Services.Models
{

    public class DeviceData
    {
        [JsonProperty("data")]
        public IEnumerable<Datum> Data { get; set; }
    }
    public class Datum
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }

}
