using BookStore.Domain;
using BookStore.Domain.Entities;
using BookStore.Domain.Entities.DTOs;
using BookStore.Domain.Repository.Query;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.Extensions.Caching.Memory;


namespace BookStore.Controllers.User;

[ApiController]
[Route("api/[controller]")]
public partial class UserController : ControllerBase
{
    private readonly DataManager _dataManager;
    private readonly ILogger<UserController> _logger;
    private readonly IMemoryCache _cache;



    public UserController(DataManager dataManager, ILogger<UserController> logger,  IMemoryCache cache)
    {
        _dataManager = dataManager;
        _logger = logger;
        _cache = cache;

    }

    [HttpGet("GetBookByQuery")]
    public async Task<ActionResult<IEnumerable<BookResponse>>> GetBookByQuery([FromQuery] BookQuery bookQuery)
    {
        
        
        try
        {
        
            
            var books = await _dataManager.BookRepository.GetAllBooksByQueryAsync(bookQuery);
            var bookResponse = books.Select(x => x.Adapt<BookResponse>());
            return Ok(bookResponse);

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при получении книг");
            return StatusCode(500);
        }

    }

    [HttpGet("GetAllBooks")]
    public async Task<ActionResult<IEnumerable<BookResponse>>> GetAllBooks()
    {
        const string cacheKey = "all_books";
        try
        {
            
            if (_cache.TryGetValue(cacheKey, out List<BookResponse> cachedBooks))
            {
                _logger.LogDebug("Список книг получен из кэша");
                return Ok(cachedBooks);
            }
            
            var books = await _dataManager.BookRepository.GetAllBooksAsync();

            
            var booksResponse = books.Select(x => x.Adapt<BookResponse>());
            
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
            _cache.Set(cacheKey, books, cacheOptions);
            
            
            return Ok(booksResponse);

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при получении книг");
            return StatusCode(500);
        }

    }

    [HttpGet("GetBookById")]
    public async Task<ActionResult<BookResponse>> GetBookById(int id)
    {
        var cacheKey = $"book_{id}";
        try
        {
            
            if (_cache.TryGetValue(cacheKey, out BookResponse?  cachedBook))
            {
                _logger.LogDebug("Книга {Id} получена из кэша", id);
                return Ok(cachedBook);
            }

            var book = await _dataManager.BookRepository.GetBookByIdAsync(id);
            
            if (book == null)
            {
                return NotFound("Книга не найдена"); 
            }

            var bookResponse = book.Adapt<BookResponse>();
            
            //
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(15)) 
                .SetAbsoluteExpiration(TimeSpan.FromHours(1))   
                .SetPriority(CacheItemPriority.Normal);

            //
            _cache.Set(cacheKey, book, cacheOptions);
            
            return Ok(bookResponse);

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при получении книги");
            return StatusCode(500);
        }


    }




}

