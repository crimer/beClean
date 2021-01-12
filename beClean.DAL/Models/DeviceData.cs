using System.Collections.Generic;

namespace beClean.Services.Models
{

    public class DeviceData
    {
        public IEnumerable<Datum> Data { get; set; }
    }
    public class Datum
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

}
