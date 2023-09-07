namespace ConsoleConsumer.Models;

public class Order
{
    public string Email { get; set; }

    public List<string> Books { get; set; }

    public decimal Price { get; set; }
}