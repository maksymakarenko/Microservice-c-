using System.Text;
using System.Text.Json;
using MyMicro.Dtos;
using RabbitMQ.Client;

namespace MyMicro.AsyncDS
{
    public class MessageBC : IMessageBC
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection ? _connection;
        private readonly IModel ? _channel;

        public MessageBC(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory(){
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", 
                                        type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQConnectionShutdown;

                Console.WriteLine("--> Connected to the MessageBus");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the MessageBus: {ex.Message}");
            }
        }

        public void PublishNewPlat(PlatformPublishedDto platform)
        {
            var message = JsonSerializer.Serialize(platform);

            if(_connection.IsOpen)
            {
                Console.WriteLine("--> Connection opened, sending message");
                SendMessage(message);
            }
            else
                Console.WriteLine("--> Connection closed, sending is unavailable");
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                                routingKey: "",
                                basicProperties: null,
                                body: body);

            Console.WriteLine($"--> Message has sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus disposed");
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

        }

        private void RabbitMQConnectionShutdown(object ? sender, ShutdownEventArgs eventArgs)
        {
            Console.WriteLine("--> Shutdown connection");
        }
    }
}