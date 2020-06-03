using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumer2Else
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
                    channel.QueueDeclare("log_else", true, false, false, null);
                    
                    //3.
                    string[] arrays= {"debug", "info","warning"} ;
                    foreach (var p in arrays)
                    {
                        channel.QueueBind("log_else","myexchange",p,null);
                    }

                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        var body = e.Body;
                        var msg = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine(" [x] Received {0}", msg);
                    };

                    channel.BasicConsume("log_else", true, consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();

                }

            }
        }
    }
}
