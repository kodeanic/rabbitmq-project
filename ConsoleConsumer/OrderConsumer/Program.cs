using System.Text;
using System.Text.Json;
using ConsoleConsumer.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost", UserName = "user", Password = "password" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "orders",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, e) =>
{
    var body = e.Body;
    var message = Encoding.UTF8.GetString(body.ToArray());

    var order = JsonSerializer.Deserialize<Order>(message);

    Console.WriteLine("New order:\n Email: {0}", order!.Email);
    foreach (var book in order.Books)
        Console.WriteLine("  - {0}", book);
    Console.WriteLine(" Price: {0}", order.Price);
};

channel.BasicConsume(queue: "orders",
    autoAck: true,
    consumer: consumer);

Console.WriteLine("Subscribed ...");

Console.ReadLine();