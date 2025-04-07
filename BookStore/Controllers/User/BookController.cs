using BookStore.Domain;
using BookStore.Domain.Entities.DTOs;
using BookStore.Domain.Repository.Query;
using Microsoft.AspNetCore.Mvc;
using Mapster;


namespace BookStore.Controllers.User;

[ApiController]
[Route("api/[controller]")]
public partial class UserController : ControllerBase
{
    private readonly DataManager _dataManager;
    private readonly ILogger<UserController> _logger;



    public UserController(DataManager dataManager, ILogger<UserController> logger)
    {
        _dataManager = dataManager;
        _logger = logger;
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
        try
        {
            var books = await _dataManager.BookRepository.GetAllBooksAsync();

            var booksResponse = books.Select(x => x.Adapt<BookResponse>());
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
        try
        {
            var book = await _dataManager.BookRepository.GetBookByIdAsync(id);
            
            if (book == null)
            {
                return NotFound("Книга не найдена"); 
            }

            var bookResponse = book.Adapt<BookResponse>();
            return Ok(bookResponse);

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при получении книги");
            return StatusCode(500);
        }


    }




}

