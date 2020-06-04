using RabbitMQ.Client;
using System;
using System.Text;

namespace TopicPublish
{
    /// <summary>
    /// Topic Exchange之正则表达式对RoutingKey进行归类的使用和配置
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

                    //3.发布消息 exchange、queue、routingkey 声明和绑定在consumer端
                    for (int i = 0; i < 10000; i++)
                    {
                        var body = Encoding.UTF8.GetBytes("hello world" + i);
                        var level = i % 13 == 0 ? i+".com" : i+".cn";
                        var content= Encoding.UTF8.GetBytes(level);
                        channel.BasicPublish("topicexchange", level, basicProperties: null, body: content);
                    }
                }

            }
        }
    }
}
