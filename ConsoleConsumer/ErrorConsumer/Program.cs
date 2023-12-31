﻿using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost", UserName = "user", Password = "password" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

var queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName,
    exchange: "direct_logs",
    routingKey: "error");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, e) =>
{
    var body = e.Body;
    var message = Encoding.UTF8.GetString(body.ToArray());

    Console.WriteLine("Error!\n {0}", message);
};

channel.BasicConsume(queue: queueName,
    autoAck: true,
    consumer: consumer);

Console.WriteLine("Subscribed ...");

Console.ReadLine();