using Domain.Entities;

namespace Infrastructure.IServices;

public interface IBookService
{
    Task<List<Book>> GetBooks();

    Task<Book> GetBook(long id);

    Task<long> CreateBook(Book book);

    Task UpdateBook(long id, Book book);

    Task DeleteBook(long id);
}