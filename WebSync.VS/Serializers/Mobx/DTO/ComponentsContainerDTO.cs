using System.Collections.Generic;
using System.Linq;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO
{
    public class ComponentsContainerDTO:DtoBase {
        public List<ComponentInstanceDto> componentnstances;

        public ComponentsContainerDTO(IComponentsContainer container) : base(container.Id) {
            componentnstances = container.ComponentInstances.Select(c => new ComponentInstanceDto(c)).ToList();
        }
    }
}