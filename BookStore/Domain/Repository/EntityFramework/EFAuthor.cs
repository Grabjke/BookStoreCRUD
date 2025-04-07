using BookStore.Domain.Entities;
using BookStore.Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Domain.Repository.EntityFramework;

public class EFAuthor:IAuthorRepository
{
    private readonly AppDbContext _context;

    public EFAuthor(AppDbContext context)
    {
        _context = context;
    }
    public async Task DeleteAuthorAsync(int id)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author != null)
        {
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
        
    }

    public async Task UpdateAuthorAsync(int id, string name, IEnumerable<int>? booksIds)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        //  Обновляем имя автора
        var updatedRows = await _context.Authors
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(a => a.Name, name));
    
        if (updatedRows == 0)
            throw new KeyNotFoundException($"Author with ID {id} not found");

        // Обновляем книги автора
        if (booksIds != null && booksIds.Any())
        {
            var author = await _context.Authors
                .Include(x => x.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
        
            if (author == null)
                return;

            // Очищаем текущие книги
            author.Books.Clear();

            // Добавляем новые книги
            var booksToAdd = await _context.Books
                .Where(x => booksIds.Contains(x.Id))
                .ToListAsync();

            foreach (var book in booksToAdd)
                author.Books.Add(book);
            

            await _context.SaveChangesAsync();
        }
    }

    public async Task CreateAuthorAsync(Author author)
    {
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
    }
}