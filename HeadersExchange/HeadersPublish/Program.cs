using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeadersPublish
{
    /// <summary>
    /// Headers Exchange通过x-match来实现对queue的And、Or的精准匹配
    /// </summary>
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
                    var properties = channel.CreateBasicProperties();
                    properties.Headers=new Dictionary<string, object>();
                    properties.Headers.Add("username", "jack");//    properties.Headers.Add("password", "123456");
                    properties.Headers.Add("password", "123456");
                    //3.发布消息 exchange、queue、routingkey 声明和绑定在consumer端
                    for (int i = 0; i < 100000; i++)
                    {
                        var body = Encoding.UTF8.GetBytes("hello world" + i);
                        Console.WriteLine(i);
                        //  var level = i % 13 == 0 ? "info" : "error";
                        channel.BasicPublish("headersexchange", string.Empty, 
                            basicProperties: properties, body: body);
                    }
                }

            }
        }
    }
}
