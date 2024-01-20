using RoslynSpike.SessionWeb.Models;
using System.Collections.Generic;

namespace RoslynSpike.Ember.DTO
{
    public class InitializationAttributeDto
    {
        public IEnumerable<object> constructorArguments;
        public IDictionary<string, object> namedArguments;

        public InitializationAttributeDto(IEnumerable<object> constructorArguments, IDictionary<string, object> namedArguments)
        {
            this.constructorArguments = constructorArguments;
            this.namedArguments = namedArguments;
        }
    }

    public class ComponentInstanceDto : DtoBase
    {
        public string parentId { get; }
        public string componentTypeId { get; }
        public string name { get; }
        public object rootSelector { get; }
        public InitializationAttributeDto initializationAttribute;
        public int fieldIndex;
        public string fieldName;

        public ComponentInstanceDto(IComponentInstance component) : base(component.Id)
        {
            parentId = component.ParentId;
            componentTypeId = component.ComponentType;
            name = component.Name;
            fieldName = component.FieldName;
            //if (component.RootSelector != null)
            //{
            //    // TODO(**): save scss value even if we can't parse it
            //    rootSelector = new
            //    {
            //        combineWithRoot = component.RootSelector.CombineWithRoot,
            //        scss = component.RootSelector.Value,
            //        css = component.RootSelector.Css,
            //        xpath = component.RootSelector.Xpath
            //    };
            //}
            //constructorParams = component.ConstructorArguments;

            initializationAttribute = new InitializationAttributeDto(component.ConstructorArguments, component.NamedArguments);
        }
    }
}