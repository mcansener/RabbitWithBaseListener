using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitWithBaseListener.Models;
using System.Runtime;
using System.Text.Json;

namespace RabbitWithBaseListener.Controllers
{
    public class SimpleMessageHandler : BaseMessageHandler<SimpleMessageHandler>
    {
        private readonly RabbitMqSettings _rabbitMqSettings;

        public SimpleMessageHandler(IOptions<RabbitMqSettings> rabbitMqSettings, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
            _connectionFactory = _rabbitMqSettings.CreateConnectionFactory();

            InitializeRabbitMqConnection();
        }

        protected override void RegisterSubscribers()
        {
            if (_rabbitMqSettings == null || _channel == null)
                return;

            RegisterMessageHandlers();
        }

        protected override void RegisterMessageHandlers()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnMessageReceived;

            _channel.BasicConsume(_rabbitMqSettings.QueueName, true, "", false, false, null, consumer);
        }

        protected override async Task ProcessMessage(IServiceScope scope, string message)
        {
            try
            {
                var simpleMessage = JsonSerializer.Deserialize<SimpleMessage>(message);
                //Do your logic
            }
            catch (Exception)
            {

            }
        }
    }
}
