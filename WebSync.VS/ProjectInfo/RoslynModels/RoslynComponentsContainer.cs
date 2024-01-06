using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public abstract class RoslynComponentsContainer<T> : RoslynNamedTypeWrapper<T>,IComponentsContainer {
        public List<IComponentInstance> ComponentInstances { get; set; }

        protected RoslynComponentsContainer(INamedTypeSymbol type) : base(type)
        {
        }

        public override void Fill() {
            base.Fill();

            ComponentInstances = new List<IComponentInstance>();

            var fields = Type.GetMembers().Where(m => m.Kind == SymbolKind.Field || m.Kind == SymbolKind.Property);
            foreach (var symbol in fields)
            {
                var attrs = symbol.GetAttributes();
                var webComponentAttr = GetAutoInitAttribute(attrs);
                if (webComponentAttr == null)
                {
                    // . no autoinit attribute
                    continue;
                }
                if (webComponentAttr != null)
                {
                    // . this field should be auto initialized with Union
                    var componentInstance = GetComponentInstance(symbol, webComponentAttr);
                    if (componentInstance != null)
                        ComponentInstances.Add(componentInstance);
                }
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="attrs"></param>
        /// <param name="attributeTypeName"></param>
        /// <returns>
        ///     Returns null if collection has several attributes of the same type
        /// </returns>
        private static AttributeData GetAttributeOfType(ImmutableArray<AttributeData> attrs, string attributeTypeName) {
            var weAttrs = attrs.Where(a => a.AttributeClass.Name == attributeTypeName).ToList();
            return weAttrs.Count == 1 ? weAttrs.First() : null;
        }

        protected RoslynComponentInstance GetComponentInstance(ISymbol symbol, AttributeData webComponentAttr)
        {
            var component = new RoslynComponentInstance(TypeName, symbol, webComponentAttr);
            component.Fill();
            return component;
        }

        protected AttributeData GetAutoInitAttribute(ImmutableArray<AttributeData> attrs)
        {
            return GetAttributeOfType(attrs, ReflectionNames.AUTOINIT_ATTRRIBUTE);
        }
    }
}