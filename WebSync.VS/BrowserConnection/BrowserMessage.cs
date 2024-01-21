using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NLog;

namespace RoslynSpike.BrowserConnection
{
    public class BrowserMessage
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public BrowserMessage(BrowserMessageType type, object data)
        {
            Type = type;
            Data = data;
        }

        public BrowserMessageType Type { get; }
        public object Data { get; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(new {Type = Type.ToString(), Data});
        }

        public static BrowserMessage Deserialize(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<BrowserMessage>(data);
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw;
            }
        }
    }
}