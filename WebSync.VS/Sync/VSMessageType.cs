using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebSync.VS.Sync
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum VSMessageType
    {
        UrlMatchResponse,
        CreateWebsiteResponse,
        ProjectNames,
        Project,
        ProjectUpdated
    }
}