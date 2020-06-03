using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQPublish1
{
    /// <summary>
    /// 最简单的方式(publish-comsumer)
    /// </summary>
    class Program
    {
        public  static void Main(string[] args)
        {
            ConnectionFactory  connectionFactory=new ConnectionFactory()
            {
                HostName = "127.0.0.1",
                UserName = "datamip",
                Password = "datamip",
            };
            using (var connection=connectionFactory.CreateConnection())
            {
                using (var channel=connection.CreateModel())
                {
                    //1.使用默认的exchange ampq default 
                    //或者声明其他的exchange
                    channel.ExchangeDeclare("myexchange",ExchangeType.Direct,true,false,null);
                    
                    //2.声明队列
                    channel.QueueDeclare("hello", true, false, false, null);
                    
                    //在声明exchange的情况下，需要将队列、exchange、routingkey绑定
                    channel.QueueBind("hello","myexchange","hello",null);
                    //3.发布消息
                    for (int i = 0; i < 100000; i++)
                    {
                        var  body = Encoding.UTF8.GetBytes("hello world"+i);
                        Console.WriteLine(body);
                        channel.BasicPublish("","hello",basicProperties:null,body:body);
                    }
                   
                    

                }
            }
        }
    }
}