using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels
{
    public class RoslynComponentType : RoslynComponentsContainer<IComponentType>, IComponentType
    {
        public string BaseComponentTypeId { get; private set; }

        public bool IsCustom { get {  return Type.Name != ReflectionNames.BASE_COMPONENT_TYPE; } }

        public RoslynComponentType(INamedTypeSymbol componentType) : base(componentType)
        {
        }

        public override void Fill()
        {
            base.Fill();
            BaseComponentTypeId = IsCustom ? null : Type.BaseType.ToString();
        }

        public override void SynchronizeTo(IComponentType model)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoslynComponentType componentType2))
            {
                return false;
            }

            if (ComponentInstances.Count != componentType2.ComponentInstances.Count ||
                !ComponentInstances.SequenceEqual(componentType2.ComponentInstances, new ComponentInstanceComparer()))
            {
                return false;
            }

            return string.Equals(BaseComponentTypeId, componentType2.BaseComponentTypeId);
        }
    }
}