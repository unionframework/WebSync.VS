using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO
{
    public class ComponentTypeDto : ComponentsContainerDTO
    {
        public string baseComponentType;
        public bool isCustom;

        public ComponentTypeDto(IComponentType component) : base(component)
        {
            baseComponentType = component.BaseComponentTypeId;
            isCustom = component.IsCustom;
        }
    }
}