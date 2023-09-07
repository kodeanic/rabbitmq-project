using System.Text;
using System.Text.Json;
using Application.Common.Exceptions;
using Infrastructure.IServices;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace Application.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateOrder(string email, List<long> bookIds)
    {
        var books = await _context.Books.Where(x => bookIds.Contains(x.Id)).ToListAsync();

        if (bookIds.Count != books.Count)
        {
            SendError("error", "Некоторые книги были не найдены");
            throw new NotFoundException("Некоторые книги были не найдены");
        }

        var order = new
        {
            Email = email,
            Books = books.Select(x => $"{x.Author} - {x.Title}").ToList(),
            Price = books.Sum(x => x.Price)
        };

        SendMessage(JsonSerializer.Serialize(order));
    }

    private void SendMessage(string message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "user",
            Password = "password"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "orders",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
            routingKey: "orders",
            basicProperties: null,
            body: body);
    }

    private void SendError(string routingKey, string message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "user",
            Password = "password"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "direct_logs",
            routingKey: routingKey,
            basicProperties: null,
            body: body);
    }
}