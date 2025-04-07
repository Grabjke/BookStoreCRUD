using BookStore.Domain.Entities;
using BookStore.Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Domain.Repository.EntityFramework;

public class EFGenre:IGenreRepository
{
    private readonly AppDbContext _context;

    public EFGenre(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task DeleteGenreAsync(int id)
    {
        var genre = await _context.Genre.FindAsync(id);

        if (genre != null)
        {
            _context.Genre.Remove(genre);
            await _context.SaveChangesAsync();
        }
    }

    public async Task CreateGenreAsync(Genre obj)
    {
        await _context.Genre.AddAsync(obj);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateGenreAsync(int id, string title, IEnumerable<int>? bookIds)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Genre name cannot be empty");

        // Проверяем существование жанра
        var genreExists = await _context.Genre.AnyAsync(g => g.Id == id);
        if (!genreExists)
            throw new KeyNotFoundException($"Genre with ID {id} not found");

        // Обновляем название жанра
        await _context.Genre
            .Where(g => g.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(g => g.Title, title));

        // Обновляем книги жанра 
        if (bookIds != null && bookIds.Any())
        {
            // Загружаем жанр с текущими книгами
            var genre = await _context.Genre
                .Include(g => g.Books)
                .FirstAsync(g => g.Id == id);

            // Получаем новые книги для жанра
            var newBooks = await _context.Books
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            // Обновляем коллекцию книг
            genre.Books?.Clear();
            foreach (var book in newBooks)
            {
                genre.Books?.Add(book);
            }

            await _context.SaveChangesAsync();
        }
    }
}