using NUnit.Framework;
using RoslynSpike.SessionWeb.Models;
using System.Collections.Generic;

namespace RoslynSpike.Ember.DTO {
    public class ServiceDto : DtoBase
    {
        public List<PageInstanceDto> pageInstances;

        public ServiceDto(IService service) : base(service.Id) {
        }
    }
}