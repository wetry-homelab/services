namespace Kubernox.Model
{
    public class Configuration
    {
        public ProxmoxProvider Proxmox { get; set; }
        public PostgreDatabaseProvider Postgre { get; set; }
        public RabbitMqProvider Rabbitmq { get; set; }
        public RedisProvider Redis { get; set; }
        public PrometheusProvider Prometheus { get; set; }
    }
}
