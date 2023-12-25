using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RoslynSpike.BrowserConnection;
using RoslynSpike.Ember.DTO;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember {
    internal class EmberSerializer : ISessionWebSerializer {
        public string Serialize(IEnumerable<IWebInfo> webs) {
            var payload = new Payload();
            foreach (var web in webs) {
                SerializeSessionWeb(payload, web);
            }
            return JsonConvert.SerializeObject(payload);
        }

        public IEnumerable<IWebInfo> Deserialize(string data) {
            return JsonConvert.DeserializeObject<IEnumerable<IWebInfo>>(data);
        }

        private void SerializeSessionWeb(Payload payload, IWebInfo web) {
            payload.services = web.Services.Values.Select(s => new ServiceDto(s)).ToList();
            payload.pageTypes = web.PageTypes.Values.Select(p => new PageTypeDto(p)).ToList();
            payload.componentTypes = web.ComponentTypes.Values.Select(c => new ComponentTypeDto(c)).ToList();
            SerializeComponents(payload, web);
        }

        private void SerializeComponents(Payload payload, IWebInfo web)
        {
            payload.components = new List<ComponentDto>();
            SerializeComponents(payload, web.PageTypes.Values);
            SerializeComponents(payload, web.ComponentTypes.Values);
        }

        private static void SerializeComponents(Payload payload, IEnumerable<IComponentsContainer> containers)
        {
            foreach (var container in containers)
            {
                foreach (var component in container.Components)
                {
                    payload.components.Add(new ComponentDto(component));
                }
            }
        }
    }
}