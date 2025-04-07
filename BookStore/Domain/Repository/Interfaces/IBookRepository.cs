using BookStore.Domain.Entities;
using BookStore.Domain.Entities.Enums;
using BookStore.Domain.Repository.Query;

namespace BookStore.Domain.Repository.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllBooksByQueryAsync(BookQuery query);
        Task CreateBook(Book book);
        Task DeleteBookByIdAsync(int id);

        public  Task UpdateBookAsync(
            int id,
            string title,
            string description,
            decimal price,
            int pages,
            AvailabilityStatus availability,
            bool isDeleted,
            IEnumerable<int>? authorIds,
            IEnumerable<int>? genreIds);
        

        Task<IEnumerable<Book>> GetAllBooksAsync();

    }
}
