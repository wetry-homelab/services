namespace Kubernox.Model
{
    public class Configuration
    {
        public PostgreDatabaseProvider Postgre { get; set; }
        public RabbitMqProvider Rabbitmq { get; set; }
        public BaseConfigurationItem Redis { get; set; }
    }
}
