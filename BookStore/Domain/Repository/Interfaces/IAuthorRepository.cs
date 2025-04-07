using BookStore.Domain.Entities;

namespace BookStore.Domain.Repository.Interfaces;

public interface IAuthorRepository
{
    Task DeleteAuthorAsync(int id);
    Task UpdateAuthorAsync(int id, string name, IEnumerable<int>? bookIds);
    Task CreateAuthorAsync(Author author);
}