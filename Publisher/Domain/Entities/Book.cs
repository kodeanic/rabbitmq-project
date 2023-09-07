namespace Domain.Entities;

public class Book
{
    public long Id { get; set; }

    public string Author { get; set; }

    public string Title { get; set; }

    public decimal Price { get; set; }
}