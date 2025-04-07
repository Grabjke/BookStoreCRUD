using BookStore.Domain.Entities;
using BookStore.Domain.Entities.Enums;
using BookStore.Domain.Repository.Interfaces;
using BookStore.Domain.Repository.Query;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Domain.Repository.EntityFramework
{
    public class EFBook : IBookRepository
    {
        private readonly AppDbContext _context;

        public EFBook(AppDbContext context)
        {
            _context = context;  
        }
        
        public async Task CreateBook(Book book)
        {
            
            await _context.Books.AddAsync(book);
            
            
            await _context.SaveChangesAsync();

        }

        public async Task DeleteBookByIdAsync(int id)
        {
         var book=  await _context.Books.FindAsync(id);

            if (book != null)
            {
                book.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Книга не найдена");
            }

        }

        public async Task<IEnumerable<Book>> GetAllBooksByQueryAsync(BookQuery query)
        {
            try
            {
                var books = _context.Books
                        .AsSplitQuery()
                        .Include(x => x.Genres)
                        .Include(x => x.Authors)
                        .Include(x => x.Availability)
                        .AsQueryable();
                    
                    

                books = query.Apply(books);

                return await books.ToListAsync();
            }
            catch 
            {
                throw new Exception("Ошибка при получении книг");
            }

        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Where(b=>!b.IsDeleted)
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.Genres)
                .Include(x => x.Authors)
                .Include(x => x.Availability)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(book != null)
            {
                return book;
            }
            else
            {
                throw new Exception("Книга не найдена");
            }

        }

        public async Task UpdateBookAsync(
            int id,
            string title,
            string description,
            decimal price,
            int pages,
            AvailabilityStatus availability,
            bool isDeleted,
            IEnumerable<int>? authorIds,
            IEnumerable<int>? genreIds)
        {
            
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty");

           
            var updatedRows = await _context.Books
                .Where(b => b.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(b => b.Title, title)
                    .SetProperty(b => b.Description, description)
                    .SetProperty(b => b.Price, price)
                    .SetProperty(b => b.Pages, pages)
                    .SetProperty(b => b.Availability, availability)
                    .SetProperty(b => b.IsDeleted, isDeleted));

            if (updatedRows == 0)
                throw new KeyNotFoundException($"Book with ID {id} not found");

            
            if (authorIds != null || genreIds != null)
            {
                var book = await _context.Books
                    .AsSplitQuery()
                    .Include(b => b.Authors)
                    .Include(b => b.Genres)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                    return;

              
                if (authorIds != null)
                {
                    book.Authors?.Clear();
                    var authorsToAdd = await _context.Authors
                        .Where(a => authorIds.Contains(a.Id))
                        .ToListAsync();
            
                    foreach (var author in authorsToAdd)
                        book.Authors?.Add(author);
                }

                
                if (genreIds != null)
                {
                    book.Genres?.Clear();
                    var genresToAdd = await _context.Genre
                        .Where(g => genreIds.Contains(g.Id))
                        .ToListAsync();
            
                    foreach (var genre in genresToAdd)
                        book.Genres?.Add(genre);
                }

                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books.
                AsNoTracking()
                .Where(x=>!x.IsDeleted).
                ToListAsync();
                
        }
    }
}
