using RoslynSpike.Ember.DTO;

namespace WebSync.VS.Sync.Browser.Messages
{
    internal class WebSiteMessage : ProjectMessage
    {
        public ComponentTypeDto ComponentType;
    }

    internal class MatchUrlMessage : ProjectMessage
    {
        public string url;
    }

}
