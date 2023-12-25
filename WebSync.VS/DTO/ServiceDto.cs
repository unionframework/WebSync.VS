using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO {
    public class ServiceDto : DtoBase
    {
        public ServiceDto(IService service) : base(service.Id) {
        }
    }
}