using System.Collections.Generic;

namespace beClean.DAL.Models
{

    public class DeviceData
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public IEnumerable<Datum> Data { get; set; }
    }
    public class Datum
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }

}
