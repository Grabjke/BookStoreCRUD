using BookStore.Domain.Entities.Enums;

namespace BookStore.Domain.Entities.DTOs;

public class UpdateBookDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Pages { get; set; }
    public AvailabilityStatus Availability { get; set; }
    public bool IsDeleted { get; set; }
    public IEnumerable<int>? AuthorIds { get; set; } 
    public IEnumerable<int>? GenreIds { get; set; }
}