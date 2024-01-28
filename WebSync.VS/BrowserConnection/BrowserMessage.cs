using System;
using Newtonsoft.Json;
using NLog;

namespace RoslynSpike.BrowserConnection
{
    public class BrowserMessage
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public BrowserMessage(BrowserMessageType type, object data, string asyncId)
        {
            Type = type;
            Data = data;
            AsyncId = asyncId;
        }

        public BrowserMessageType Type { get; }
        public object Data { get; }
        public string AsyncId { get; }

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