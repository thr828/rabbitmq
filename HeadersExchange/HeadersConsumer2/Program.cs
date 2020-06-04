using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HeadersConsumer2
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                UserName = "datamip",
                Password = "datamip",
            };
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("headersexchange", ExchangeType.Headers, true,
                        false, null);
                    //注意此处分发的consumer名称要不一样
                    channel.QueueDeclare("headersqueue2", true, false, false, null);

                    channel.QueueBind("headersqueue2", "headersexchange", string.Empty,
                        new Dictionary<string, object>()
                        {
                            {"x-match","all"},
                            {"username","jack" },
                            {"password","123456"},
                        });

                    EventingBasicConsumer basicConsumer = new EventingBasicConsumer(channel);
                    basicConsumer.Received += (sender, e) =>
                    {
                        var body = e.Body;
                        Console.WriteLine(Encoding.UTF8.GetString(body.ToArray()));
                    };
                    channel.BasicConsume("headersqueue2", true, basicConsumer);
                    Console.WriteLine(" Consumer2端启动完成.");
                    Console.ReadLine();
                }

            }
        }
    }
}
