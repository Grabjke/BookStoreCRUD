using BookStore.Domain.Entities;
using BookStore.Domain.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers.Admin;

public partial class AdminController
{
    [HttpDelete("DeleteAuthor")]
    public async Task<ActionResult> DeleteAuthorById(int id)
    {
        try
        {
            await _dataManager.AuthorRepository.DeleteAuthorAsync(id);
            return Ok();

        }
        catch (Exception e)
        {
            _logger.LogError(e,"Ошибка при удалении автора");
            throw new Exception("Error during the Delete");
        }
    }

    [HttpPut("UpdateAuthor")]
    public async Task<ActionResult> UpdateAuthor([FromRoute]int id,[FromBody] UpdateAuthorDto dto)
    {
        try
        {
            await _dataManager.AuthorRepository.UpdateAuthorAsync(
                id,
                dto.Name,
                dto.Books
            );

            return Ok();

        }
        catch (Exception e)
        {
            _logger.LogError(e,"Ошибка при обновлении автора");
            throw new Exception("Error during the update");
        }
    }

    [HttpPost("CreateAuthor")]
    public async Task<ActionResult> CreateAuthor(Author author)
    {
        var validateResult = await _authorValidator.ValidateAsync(author);
           
        if (!validateResult.IsValid)
        {
            return BadRequest(new {errors=validateResult.Errors});
        }
        try
        {
            _logger.LogInformation("Создание автора:{Name}",author.Name);
            await _dataManager.AuthorRepository.CreateAuthorAsync(author);
            
            _logger.LogDebug("Автор создан с {ID}",author.Id);
            return Ok();


        }
        catch (Exception e)
        {
            _logger.LogError(e,"Ошибка при создании автора");
            return StatusCode(500);
        }
    }
    
}