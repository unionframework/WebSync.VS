namespace RoslynSpike.Ember.DTO {
    public class DtoBase {
        public string id { get; set; }

        public DtoBase(string id) {
            this.id = id;
        }
    }
}