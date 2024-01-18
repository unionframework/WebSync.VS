using System.Collections.Generic;

namespace RoslynSpike.SessionWeb.Models
{
    public interface IComponentsContainer: ICodeModelWithId {
        List<IComponentInstance> ComponentInstances { get; }
    }

    public interface IComponentType : IComponentsContainer
    {
        string BaseComponentTypeId { get; }
        bool IsCustom { get; }
    }
}