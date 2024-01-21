using RoslynSpike.BrowserConnection;
using RoslynSpike.Ember.DTO;
using RoslynSpike.SessionWeb.Models;

namespace WebSync.VS.Serializers.Mobx
{
    internal class MobxProjectInfoSerializer : IProjectInfoSerializer
    {
        public object Serialize(IProjectInfo project)
        {
            return new Payload(project);
        }
    }
}