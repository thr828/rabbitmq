using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
           ConnectionFactory factory=new ConnectionFactory()
           {
               HostName = "127.0.0.1",
               UserName = "datamip",
               Password = "datamip",
           };

           var connection = factory.CreateConnection();

           var channel = connection.CreateModel();

           var result = channel.BasicGet("mytest2", true);
           if (result != null)
           {
               var msg = Encoding.UTF8.GetString(result.Body.ToArray());
               Console.WriteLine(msg);
           }

         

        }
    }
}