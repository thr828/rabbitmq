using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQPublish2
{
    /// <summary>
    /// routing（有选择的接收信息）
    /// </summary>
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
               using (var  channel=connection.CreateModel())
               {
                   
                   //3.发布消息 exchange、queue、routingkey 声明和绑定在consumer端
                   for (int i = 0; i < 100000; i++)
                   {
                       var  body = Encoding.UTF8.GetBytes("hello world"+i);
                       var level = i % 13 == 0 ? "info" : "error";
                       channel.BasicPublish("myexchange",level,basicProperties:null,body:body);
                   }
               }
               
           }
        }
    }
}