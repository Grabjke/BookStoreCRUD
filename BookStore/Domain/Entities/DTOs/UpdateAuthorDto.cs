namespace BookStore.Domain.Entities.DTOs;

public class UpdateAuthorDto
{
    public string Name { get; set; } 
    public IEnumerable<int>? Books { get; set;} 
}