using BookStore.Domain.Entities;

namespace BookStore.Domain.Repository.Interfaces;

public interface IGenreRepository
{
    Task DeleteGenreAsync(int id);
    Task CreateGenreAsync(Genre obj);
    Task UpdateGenreAsync(int id, string title, IEnumerable<int>? bookIds);
}