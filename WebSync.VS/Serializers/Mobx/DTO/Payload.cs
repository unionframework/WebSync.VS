using RoslynSpike.SessionWeb.Models;
using System.Collections.Generic;
using System.Linq;

namespace RoslynSpike.Ember.DTO
{
    public class Payload
    {
        public string projectName;
        public List<ServiceDto> services;
        public List<PageTypeDto> pageTypes;
        public List<ComponentTypeDto> componentTypes;

        public Payload(IProjectInfo project)
        {
            projectName = project.ProjectName;
            services = project.Services.Values.Select(s => new ServiceDto(s)).ToList();
            pageTypes = project.PageTypes.Values.Select(p => new PageTypeDto(p)).ToList();
            componentTypes = project.ComponentTypes.Values.Select(c => new ComponentTypeDto(c)).ToList();
        }
    }
}