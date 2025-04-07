using BookStore.Domain.Entities.Enums;

namespace BookStore.Domain.Entities.DTOs;

public class BookResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public decimal Price { get; set; }
    public int Pages { get; set; }
    public AvailabilityStatus AvailabilityStatus  { get; set; }
    public ICollection<Author>? Authors { get; set; }
    public ICollection<Genre>? Genres { get; set; }
}