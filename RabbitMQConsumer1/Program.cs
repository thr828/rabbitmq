using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQConsumer1
{
    /// <summary>
    /// 最简单的方式(publish-comsumer)
    /// </summary>
    public  class Program 
    {
        static void Main(string[] args)
        {
           ConnectionFactory connectionFactory=new ConnectionFactory()
           {
               HostName = "127.0.0.1",
               UserName = "datamip",
               Password = "datamip",
           };

           using (var connection=connectionFactory.CreateConnection())
           {
               using (var channel=connection.CreateModel())
               {
                   channel.QueueDeclare("hello", true, false, false, null);
                   EventingBasicConsumer consumer=new EventingBasicConsumer(channel);
                   consumer.Received += (sender,e) =>
                   {
                       var body = e.Body;
                       var msg = Encoding.UTF8.GetString(body.ToArray());
                       Console.WriteLine(" [x] Received {0}", msg);
                   };

                   channel.BasicConsume("hello", true, consumer);
                   Console.WriteLine(" Press [enter] to exit.");
                   Console.ReadLine();

               }
               
           }
        }
    }
}