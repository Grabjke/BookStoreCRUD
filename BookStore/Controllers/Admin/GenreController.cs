using BookStore.Domain.Entities;
using BookStore.Domain.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers.Admin;

public partial class AdminController
{
    [HttpPut("UpdateGenre")]
    public async Task<ActionResult> UpdateGenre([FromRoute] int id, [FromBody] UpdateGenreDto dto)
    {
        try
        {
            await _dataManager.GenreRepository.UpdateGenreAsync(
                id,
                dto.Title,
                dto.BookIds);

            return Ok();

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при обновлении жанра"); 
            return StatusCode(500, "Произошла ошибка при обновлении жанра"); 
        }
    }

    [HttpPost("CreateGenre")]
    public async Task<ActionResult> CreateGenre(Genre genre)
    {
        var validateResult = await _genreValidator.ValidateAsync(genre);
        if (!validateResult.IsValid)
        {
            return BadRequest(new {errors=validateResult.Errors});
        }

        try
        {
            await _dataManager.GenreRepository.CreateGenreAsync(genre);
            return Ok();

        }
        catch (Exception e)
        {
            _logger.LogError(e,"Ошибка при создании жанра");
            return StatusCode(500);
        }
       
    }

    [HttpDelete("DeleteGenre")]
    public async Task<ActionResult> DeleteGenre(int id)
    {
        try
        {
            await _dataManager.GenreRepository.DeleteGenreAsync(id);
            return Ok();

        }
        catch (Exception e)
        {
            _logger.LogError(e,"Ошибка при удалении жанра");
            throw new Exception("Error during the Delete");
        }
    }
    
}