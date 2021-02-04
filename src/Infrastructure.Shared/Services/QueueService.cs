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
                VirtualHost = configuration["RabbitMq:VirtualHost"],
                HostName = configuration["RabbitMq:HostName"]
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
                channel.QueueDeclare(queue: "cluster", true, false, false, null);

                byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
                channel.BasicPublish(exchange: exchange, routingKey: routingKey, body: messageBodyBytes, basicProperties: null);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception on publishing message in queue");
            }
        }
    }
}
