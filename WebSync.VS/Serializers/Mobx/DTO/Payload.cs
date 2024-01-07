using RoslynSpike.SessionWeb.Models;
using System.Collections.Generic;
using System.Linq;

namespace RoslynSpike.Ember.DTO
{
    public class Payload
    {
        public string projectName;
        public List<ServiceDto> webSites;
        public List<ComponentTypeDto> componentTypes;

        public Payload(IProjectInfo project)
        {
            projectName = project.ProjectName;
            webSites = project.Services.Values.Select(s => new ServiceDto(s)).ToList();
            // TODO: need to find a way to extract list of pages for the service
            if (webSites.Count() > 0)
            {
                webSites[0].pageTypes = project.PageTypes.Values.Select(p => new PageTypeDto(p)).ToList();
            }
            componentTypes = project.ComponentTypes.Values.Select(c => new ComponentTypeDto(c)).ToList();
        }
    }
}