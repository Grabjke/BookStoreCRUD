using System.Text.Json.Serialization;
using BookStore.Domain.Entities.Enums;

namespace BookStore.Domain.Entities
{
    public class Book:BaseEntity
    {
        public string Title {  get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price {  get; set; }

        public int Pages {  get; set; }

        public AvailabilityStatus Availability {  get; set; }

        public bool IsDeleted {  get; set; }

        [JsonIgnore] 
        public ICollection<Author>? Authors { get; set; }

        [JsonIgnore] 
        public ICollection<Genre>? Genres { get; set; }
    }
}
