using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;
using System.Net.Security;

namespace RabbitWithBaseListener.Models
{
    public class BaseRabbitMqDmzSettings
    {
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual IConnectionFactory CreateConnectionFactory()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = this.HostName,
                Port = this.Port,
                UserName = this.UserName,
                Password = this.Password,
                VirtualHost = this.VirtualHost,
                AutomaticRecoveryEnabled = true
            };

            return connectionFactory;
        }
    }
}
