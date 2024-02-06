using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using IConnectionFactory = RabbitMQ.Client.IConnectionFactory;

namespace RabbitWithBaseListener.Controllers
{
    public abstract class BaseMessageHandler<T> : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        protected IConnection _connection;
        protected IModel _channel;
        protected IConnectionFactory _connectionFactory;

        protected BaseMessageHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            RegisterSubscribers();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection?.Close();

            return Task.CompletedTask;
        }


        protected void InitializeRabbitMqConnection()
        {
            _connection = _connectionFactory.CreateConnection();

            CreateChannel();
        }

        protected virtual void RegisterSubscribers()
        {
            throw new NotImplementedException();
        }

        protected virtual void RegisterMessageHandlers()
        {
            throw new NotImplementedException();
        }

        protected virtual async void OnMessageReceived(object model, BasicDeliverEventArgs ea)
        {
            string message = string.Empty;

            message = Encoding.UTF8.GetString(ea.Body.ToArray());

            using var scope = _serviceProvider.CreateScope();
            await ProcessMessage(scope, message);

            _channel.BasicAck(ea.DeliveryTag, false);
        }


        protected virtual async Task ProcessMessage(IServiceScope scope, string message)
        {
            await Task.Run(() => throw new NotImplementedException());
        }

        private void CreateChannel()
        {
            if (_connection == null)
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action.");

            if (!_connection.IsOpen)
                throw new InvalidOperationException("No RabbitMQ connections are open to perform this action.");

            _channel?.Dispose();

            _channel = _connection.CreateModel();
        }
    }
}
