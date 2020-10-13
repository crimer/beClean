using System;

namespace beClean.DAL.DataServices.BClassic
{
    public class BCRecivedEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
        public string Content { get; set; }
        public BCRecivedEventArgs(byte[] data, string content)
        {
            Data = data;
            Content = content;
        }
    }
}
