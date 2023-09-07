using Domain.Entities;
using Infrastructure.IServices;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks() =>
        Ok(await _bookService.GetBooks());

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetBook(long id) =>
        Ok(await _bookService.GetBook(id));

    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] Book book) =>
        Ok(await _bookService.CreateBook(book));

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateBook(long id, [FromBody] Book book)
    {
        await _bookService.UpdateBook(id, book);
        return Ok();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteBook(long id)
    {
        await _bookService.DeleteBook(id);
        return Ok();
    }
}