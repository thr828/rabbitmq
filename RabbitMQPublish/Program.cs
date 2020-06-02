using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQPublish
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                 HostName = "127.0.0.1",
                 UserName = "datamip",
                 Password = "datamip",
            };
            //第一步：创建connection
            using (var connection=factory.CreateConnection())
            {
                //第二步：在connection上创建一个channe
                var channel = connection.CreateModel();
                //第三步：申明交换机【因为rabbitmq 已经有了自定义的ampq default exchange】
                
                //第四步：创建一个队列（queue）
                channel.QueueDeclare("mytest2", true, false, false, null);
                var msg = Encoding.UTF8.GetBytes("你好");
                
                //第五步:发布消息
                channel.BasicPublish(string.Empty,routingKey:"mytest2",basicProperties:null,body:msg);

            }
        }
    }
}