using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumer2Error
{
    public class Program
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
                    //1.声明ExchangeType amqp direct
                    channel.ExchangeDeclare("myexchange",ExchangeType.Direct,true,false,null);
                   
                    //2.声明队列
                    channel.QueueDeclare("log_error", true, false, false, null);
                    
                    //3.
                    channel.QueueBind("log_error","myexchange","error",null);

                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        var body = e.Body;
                        var msg = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine(" [x] Received {0}", msg);
                    };

                    channel.BasicConsume("log_error", true, consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();

                }

            }
        }
    }
}