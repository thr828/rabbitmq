using RabbitMQ.Client;
using System;
using System.Text;
using RabbitMQ.Client.Events;

namespace TopicConsumer2
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
                    //1.声明ExchangeType amqp direct
                    channel.ExchangeDeclare("topicexchange", ExchangeType.Topic, true, false, null);

                    //2.声明队列
                    channel.QueueDeclare("topicqueue2", true, false, false, null);

                    //3.
                    channel.QueueBind("topicqueue2", "topicexchange", "*.cn", null);

                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        var body = e.Body;
                        var msg = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine(" [x] Received {0}", msg);
                    };

                    channel.BasicConsume("topicqueue2", true, consumer);
                    Console.WriteLine(" Consumer2端启动完成.");
                    Console.ReadLine();

                }

            }
        }
    }
}
