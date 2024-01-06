using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RoslynSpike.BrowserConnection;
using RoslynSpike.Ember.DTO;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember
{
    internal class MobxSerializer : IProjectInfoSerializer
    {
        public object Serialize(IProjectInfo project)
        {
            return new Payload(project);
        }
    }
}