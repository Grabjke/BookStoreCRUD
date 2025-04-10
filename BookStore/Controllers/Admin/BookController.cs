using BookStore.Domain;
using BookStore.Domain.Entities;
using BookStore.Domain.Entities.DTOs;
using BookStore.Domain.Entities.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;


namespace BookStore.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]
public partial class AdminController:ControllerBase
{
    private readonly DataManager _dataManager;
    private readonly IValidator<Book> _bookValidator;
    private readonly IValidator<Genre> _genreValidator;
    private readonly IValidator<Author> _authorValidator;
    private readonly ILogger<AdminController> _logger;
    


    public AdminController(DataManager dataManager, IValidator<Book> bookValidator, ILogger<AdminController> logger,
        IValidator<Author> authorValidator,IValidator<Genre> genreValidator,IMemoryCache cache)
    {
        _dataManager = dataManager;
        _bookValidator = bookValidator;
        _genreValidator = genreValidator;
        _authorValidator = authorValidator;
        _logger = logger;
        
    }

    [HttpPost("CreateBook")]
    public async Task<ActionResult> CreateBook(Book book)
    {
        
         var validateResult = await _bookValidator.ValidateAsync(book);
         if (!validateResult.IsValid)
         {
             return BadRequest(new {errors=validateResult.Errors});
         }

        try
        {
            _logger.LogInformation("Создание книги:{Title}",book.Title);
            
            await _dataManager.BookRepository.CreateBook(book);
            
            _logger.LogDebug("Книга создана с {ID}",book.Id);
            return Ok();

        }
        catch (Exception e)
        {
            _logger.LogError(e,"Ошибка при создании книги");
            return StatusCode(500);
        }

        
    }

    [HttpPatch("DeleteBook")]
    public async Task<ActionResult> DeleteBook(int id)
    {
        try
        {
            await _dataManager.BookRepository.DeleteBookByIdAsync(id);
            return Ok();

        }
        catch (Exception e)
        {
            _logger.LogError(e,"Ошибка при удалении книги");
            throw new Exception("Error during the Delete");
        }
       
    }

    [HttpPut("UpdateBook")]
    public async Task<ActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdateBookDto dto)
    {
        try
        {
            await _dataManager.BookRepository.UpdateBookAsync(
                id,
                dto.Title,
                dto.Description,
                dto.Price,
                dto.Pages,
                dto.Availability,
                dto.IsDeleted,
                dto.AuthorIds,
                dto.GenreIds);
            return Ok();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Ошибка при обновлении книги");
            throw new Exception("Error during the update");
        }

        


    }
    
}