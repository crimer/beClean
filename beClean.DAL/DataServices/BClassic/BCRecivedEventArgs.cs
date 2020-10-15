using System;

namespace beClean.Services.DataServices.BClassic
{
    public class BCRecivedEventArgs : EventArgs
    {
        public byte[] Bytes { get; set; }
        public string RawJson { get; set; }

        public BCRecivedEventArgs(byte[] bytes, string rawJson)
        {
            Bytes = bytes;
            RawJson = rawJson;
        }
    }
}
