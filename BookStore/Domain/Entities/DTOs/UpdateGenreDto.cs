namespace BookStore.Domain.Entities.DTOs;

public class UpdateGenreDto
{
    public string Title { get; set; } = string.Empty;

    public IEnumerable<int>? BookIds { get; set; }
}