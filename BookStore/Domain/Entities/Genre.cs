namespace BookStore.Domain.Entities
{
    public class Genre:BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        public ICollection<Book>? Books { get; set; }       
    }
}
