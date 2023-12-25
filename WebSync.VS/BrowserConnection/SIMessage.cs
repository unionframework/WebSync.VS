using System;
using Newtonsoft.Json;
using NLog;

namespace RoslynSpike.BrowserConnection
{
    public class SIMessage
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();

        public SIMessage(SIMessageType type, string data)
        {
            Type = type;
            Data = data;
        }

        public SIMessageType Type { get; }
        public string Data { get; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(new {Type = Type.ToString(), Data});
        }

        public static SIMessage Deserialize(string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<SIMessage>(data);
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw;
            }
        }

        public static SIMessage CreateProjectNamesMessage(string data) => new SIMessage(SIMessageType.ProjectNames, data);
        
        public static SIMessage CreateProjectMessage(string data) => new SIMessage(SIMessageType.Project, data);

        public static SIMessage CreateUrlMatchResultessage(string data) => new SIMessage(SIMessageType.UrlMatchResult, data);
    }
}