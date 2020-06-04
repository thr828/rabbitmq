using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HeadersConsumer1
{
    class Program
    {
        /// <summary>
        /// Headers Exchange通过x-match来实现对queue的And、Or的精准匹配
        /// </summary>
        /// <param name="args"></param>
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
                    channel.QueueDeclare("headersqueue1", true, false, false, null);

                    channel.QueueBind("headersqueue1", "headersexchange", string.Empty, 
                        new Dictionary<string, object>()
                        {
                            {"x-match","any"},
                            {"username","jack" },
                            {"password","123456"},
                        });

                    EventingBasicConsumer basicConsumer = new EventingBasicConsumer(channel);
                    basicConsumer.Received += (sender, e) =>
                    {
                        var body = e.Body;
                        Console.WriteLine(Encoding.UTF8.GetString(body.ToArray()));
                    };
                    channel.BasicConsume("headersqueue1", true, basicConsumer);
                    Console.WriteLine(" Consumer1端启动完成.");
                    Console.ReadLine();
                }

            }
        }
    }
}
