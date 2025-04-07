using BookStore.Domain.Repository.Interfaces;

namespace BookStore.Domain;

public class DataManager
{
    public IAuthorRepository AuthorRepository { get; set; }
    public IBookRepository BookRepository { get; set; }
    public IGenreRepository GenreRepository { get; set; }
    


    public DataManager(IAuthorRepository authorRepository,IBookRepository bookRepository
    ,IGenreRepository genreRepository)
    {
        AuthorRepository = authorRepository;
        BookRepository = bookRepository;
        GenreRepository = genreRepository;
    }
}