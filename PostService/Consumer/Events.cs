using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Microsoft.EntityFrameworkCore;
using PostService.Context;
using System.Text;
using Newtonsoft.Json.Linq;
using PostService.Entity;

namespace PostService.Consumer
{
    public class Events
    {
        public Events()
        {
                
        }
        public void ListenForIntegrationEvents()
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var contextOptions = new DbContextOptionsBuilder<PostDbContext>()
                    .UseSqlite(@"Data Source=post.db")
                    .Options;
                var dbContext = new PostDbContext(contextOptions);

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
                var data = JObject.Parse(message);
                var type = ea.RoutingKey;
                if (type == "user.add")
                {
                    dbContext.Users.Add(new User()
                    {
                        ID = data["id"].Value<int>(),
                        Name = data["name"].Value<string>()
                    });
                    dbContext.SaveChanges();
                }
                else if (type == "user.update")
                {
                    var user = dbContext.Users.First(a => a.ID == data["id"].Value<int>());
                    user.Name = data["newname"].Value<string>();
                    dbContext.SaveChanges();
                }
            };
            channel.BasicConsume(queue: "user.postservice",
                                     autoAck: true,
                                     consumer: consumer);
        }
    }
}
