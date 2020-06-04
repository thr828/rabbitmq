using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FanoutConsumer1
{
    class Program
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
                   channel.ExchangeDeclare("fanoutexchange",ExchangeType.Fanout,true,
                       false,null);
                   channel.QueueDeclare("fanoutqueue1", true, false, false, null);
                   
                   channel.QueueBind("fanoutqueue1","fanoutexchange",string.Empty,null);
                   
                   EventingBasicConsumer basicConsumer  =new EventingBasicConsumer(channel);
                   basicConsumer.Received += (sender,e) =>
                   {
                       var body = e.Body;
                       Console.WriteLine(Encoding.UTF8.GetString(body.ToArray()));
                   };
                   channel.BasicConsume("fanoutqueue1", true, basicConsumer);
                   Console.WriteLine(" Consumer1端启动完成.");
                   Console.ReadLine();
               }
               
           }
        }
    }
}