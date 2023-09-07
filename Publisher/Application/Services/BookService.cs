using Application.Common.Exceptions;
using Domain.Entities;
using Infrastructure.IServices;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class BookService : IBookService
{
    private readonly ApplicationDbContext _context;

    public BookService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Book>> GetBooks() =>
        _context.Books.ToListAsync();

    public async Task<Book> GetBook(long id)
    {
        var book = await _context.Books
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        return book ?? throw new NotFoundException("Такой книги не существует");
    }

    public async Task<long> CreateBook(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        return book.Id;
    }

    public async Task UpdateBook(long id, Book book)
    {
        var oldBook = await _context.Books
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (oldBook == null) throw new NotFoundException("Такой книги не существует");

        oldBook.Author = book.Author;
        oldBook.Title = book.Title;
        oldBook.Price = book.Price;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteBook(long id)
    {
        var book = await _context.Books
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (book == null) throw new NotFoundException("Такой книги не существует");

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}