using RoslynSpike.SessionWeb.Models;
using System;
using System.Collections.Generic;

namespace RoslynSpike.Ember.DTO
{
    public class ComponentInstanceDto : DtoBase
    {
        public string componentTypeId { get; }
        public string name { get; }
        public object rootSelector { get; }
        public IEnumerable<string> constructorParams { get; }
        public int fieldIndex;
        public string fieldName;

        //public String parentId;
        //public int fieldIndex;
        //public String componentTypeId;
        //public String fieldName;
        //public String name;
        //public AnnotationDto initializationAttribute;

        public ComponentInstanceDto(IComponentInstance component) : base(component.Id)
        {
            componentTypeId = component.ComponentType;
            name = component.Name;
            if (component.RootSelector != null)
            {
                // TODO(**): save scss value even if we can't parse it
                rootSelector = new
                {
                    combineWithRoot = component.RootSelector.CombineWithRoot,
                    scss = component.RootSelector.Value,
                    css = component.RootSelector.Css,
                    xpath = component.RootSelector.Xpath
                };
            }
            constructorParams = component.ConstructorParams;
        }
    }
}