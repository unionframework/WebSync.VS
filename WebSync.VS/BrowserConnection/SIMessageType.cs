using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RoslynSpike.BrowserConnection
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SIMessageType
    {
        MatchUrl,
        UrlMatchResult,

        OpenFile,

        // Project
        GetProjectNames,
        ProjectNames,
        GetProject,
        Project,

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
        UpdateCompoenentInstance,
    }
}