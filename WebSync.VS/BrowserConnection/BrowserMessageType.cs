using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RoslynSpike.BrowserConnection
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BrowserMessageType
    {
        MatchUrl,
        OpenFile,

        // Project
        GetProjectNames,
        GetProject,

        // Website
        CreateWebsite,
        DeleteWebsite,
        UpdateWebsite,

        // PageType
        CreatePageType,
        DeletePageType,
        UdpatePageType,

        // ComponentType
        CreateComponentType,
        DeleteCompoenentType,
        UpdateComponentType,

        // ComponentInstance
        AddCompoenentInstance,
        DeleteComponentInstance,
        UpdateComponentInstance,
    }
}