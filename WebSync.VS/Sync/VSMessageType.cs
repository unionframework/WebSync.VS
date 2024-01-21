using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebSync.VS.Sync
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum VSMessageType
    {
        UrlMatchResult,
        ProjectNames,
        Project
    }
}