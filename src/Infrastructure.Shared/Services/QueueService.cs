using Application.Interfaces;
using Application.Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Text.Json;

namespace Infrastructure.Shared.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConnection connectionQueue;
        private readonly ILogger<QueueService> logger;

        public QueueService(ILogger<QueueService> logger, IConfiguration configuration)
        {
            this.logger = logger;

            ConnectionFactory factory = new ConnectionFactory()
            {
                UserName = configuration["RabbitMq:User"],
                Password = configuration["RabbitMq:Password"],
                HostName = configuration["RabbitMq:HostName"],
                Port = int.Parse(configuration["RabbitMq:Port"])
            };

            this.connectionQueue = factory.CreateConnection();
        }

        public void QueueClusterCreation(ClusterCreateMessage message)
        {
            var exchange = "cluster";
            var routingKey = "cluster";

            PublishMessage(message, exchange, routingKey);
        }

        public void QueueClusterUpdate(ClusterUpdateMessage message)
        {
            var exchange = "cluster";
            var routingKey = "cluster";

            PublishMessage(message, exchange, routingKey);
        }

        private void PublishMessage(object data, string exchange, string routingKey)
        {
            try
            {
                IModel channel = connectionQueue.CreateModel();
                channel.QueueDeclare(queue: "k3s_queue", false, false, false, null);

                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
                channel.BasicPublish(exchange: "", routingKey: "k3s_queue", body: messageBodyBytes, basicProperties: null);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception on publishing message in queue");
            }
        }
    }
}
