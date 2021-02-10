using Docker.DotNet;
using Docker.DotNet.Models;
using Kubernox.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kubernox
{
    public class ContainerDeployService
    {
        private readonly DockerClient client;

        private const string DbContainerName = "kubernox_db";
        private const string QueueContainerName = "kubernox_queue";
        private const string CacheContainerName = "kubernox_cache";
        private const string PrometheusContainerName = "kubernox_prometheus";
        private const string ServiceContainerName = "kubernox_service";
        private const string DatacenterWorkerContainerName = "kubernox_datacenter_worker";
        private const string MonitoringWorkerContainerName = "kubernox_monitoring_worker";

        public ContainerDeployService()
        {
            client = new DockerClientConfiguration().CreateClient();
        }

        public async Task<bool> InstantiateDatabaseContainer(PostgreDatabaseProvider database, CancellationToken cancellationToken)
        {
            Console.WriteLine("---- Starting Deploy Database Container ----");

            await DownloadImage("postgres", "latest", cancellationToken);

            var volumes = new Dictionary<string, EmptyStruct>();
            volumes.Add($"{database.Storage}:/var/lib/postgresql/data", new EmptyStruct());

            var createParameters = new CreateContainerParameters()
            {
                Image = "postgres:latest",
                Name = DbContainerName,
                Env = new List<string>() { $"POSTGRES_PASSWORD={database.Password}" },
                Volumes = volumes
            };

            return await DeployAndStartAsync(DbContainerName, createParameters, cancellationToken);
        }

        public async Task<bool> InstantiateQueueContainer(RabbitMqProvider queue, CancellationToken cancellationToken)
        {
            Console.WriteLine("---- Starting Deploy Queue Container ----");
            await DownloadImage("rabbitmq", "3", cancellationToken);

            var createParameters = new CreateContainerParameters()
            {
                Image = "rabbitmq:3",
                Name = QueueContainerName,
                Env = new List<string>() { $"RABBITMQ_DEFAULT_USER={queue.Username}", $"RABBITMQ_DEFAULT_PASS={queue.Password}" },
            };

            return await DeployAndStartAsync(QueueContainerName, createParameters, cancellationToken);
        }

        public async Task<bool> InstantiateCacheContainer(RedisProvider redis, CancellationToken cancellationToken)
        {
            Console.WriteLine("---- Starting Deploy Cache Container ----");
            await DownloadImage("redis", cancellationToken);

            var createParameters = new CreateContainerParameters()
            {
                Image = "redis",
                Name = CacheContainerName,
                Cmd = new List<string>() { $"redis-server", "--requirepass", $"{redis.Password}" }
            };

            return await DeployAndStartAsync(CacheContainerName, createParameters, cancellationToken);
        }

        public async Task<bool> InstantiatePrometheusContainer(PrometheusProvider prometheus, CancellationToken cancellationToken)
        {
            Console.WriteLine("---- Starting Deploy Prometheus Container ----");
            await DownloadImage("prom/prometheus", "latest", cancellationToken);


            var ports = new Dictionary<string, EmptyStruct>();
            ports.Add("9090:9090", new EmptyStruct());

            var volumes = new Dictionary<string, EmptyStruct>();
            volumes.Add($"{prometheus.Path}:/etc/prometheus/prometheus.yml", new EmptyStruct());

            var createParameters = new CreateContainerParameters()
            {
                Image = "prom/prometheus:latest",
                Name = PrometheusContainerName,
                ExposedPorts = ports,
                Volumes = volumes
            };

            return await DeployAndStartAsync(PrometheusContainerName, createParameters, cancellationToken);
        }

        public async Task<bool> InstantiateKubernoxServiceContainer(Configuration configuration, CancellationToken cancellationToken)
        {
            Console.WriteLine("---- Starting Deploy Kubernox Container ----");
            await DownloadImage("kubernox/service", cancellationToken);

            var createParameters = new CreateContainerParameters()
            {
                Image = "wetry/kubernox",
                Name = CacheContainerName,
                Env = new List<string>()
                {
                    $"Proxmox__Uri={configuration.Proxmox.Host}",
                    $"Proxmox__Token=PVEAPIToken={configuration.Proxmox.Username}@{configuration.Proxmox.AuthType}!{configuration.Proxmox.TokenId}={configuration.Proxmox.AccessToken}",
                    $"ConnectionStrings__Default=Data Source={configuration.Postgre.Host};Initial Catalog={configuration.Postgre.DbName};User ID={configuration.Postgre.Username};Password={configuration.Postgre.Password}",
                    $"ConnectionStrings__Redis={configuration.Redis.Host}",
                    $"RabbitMq__User={configuration.Rabbitmq.Username}",
                    $"RabbitMq__Password={configuration.Rabbitmq.Password}",
                    $"RabbitMq__HostName={configuration.Rabbitmq.Host}",
                    $"RabbitMq__VirtualHost={configuration.Rabbitmq.Virtualhost}",
                    $"RabbitMq__Port={configuration.Rabbitmq.Port}"
                }
            };

            return await DeployAndStartAsync(ServiceContainerName, createParameters, cancellationToken);
        }

        public async Task<bool> InstantiateKubernoxDatacenterWorkerContainer(Configuration configuration, CancellationToken cancellationToken)
        {
            Console.WriteLine("---- Starting Deploy Kubernox Datacenter Worker Container ----");
            await DownloadImage("wetry/kubernox-datacenter-worker", cancellationToken);

            var createParameters = new CreateContainerParameters()
            {
                Image = "wetry/kubernox-datacenter-worker",
                Name = CacheContainerName,
                Env = new List<string>()
                {
                    $"Proxmox__Uri={configuration.Proxmox.Host}",
                    $"Proxmox__Token=PVEAPIToken={configuration.Proxmox.Username}@{configuration.Proxmox.AuthType}!{configuration.Proxmox.TokenId}={configuration.Proxmox.AccessToken}",
                    $"ConnectionStrings__Default=Data Source={configuration.Postgre.Host};Initial Catalog={configuration.Postgre.DbName};User ID={configuration.Postgre.Username};Password={configuration.Postgre.Password}"
                }
            };

            return await DeployAndStartAsync(DatacenterWorkerContainerName, createParameters, cancellationToken);
        }

        public async Task<bool> InstantiateKubernoxMonitoringWorkerContainer(Configuration configuration, CancellationToken cancellationToken)
        {
            Console.WriteLine("---- Starting Deploy Kubernox Container ----");
            await DownloadImage("wetry/kubernox-monitoring-worker", cancellationToken);

            var createParameters = new CreateContainerParameters()
            {
                Image = "wetry/kubernox-monitoring-worker",
                Name = CacheContainerName,
                Env = new List<string>()
                {
                    $"Proxmox__Uri={configuration.Proxmox.Host}",
                    $"Proxmox__Token=PVEAPIToken={configuration.Proxmox.Username}@{configuration.Proxmox.AuthType}!{configuration.Proxmox.TokenId}={configuration.Proxmox.AccessToken}",
                    $"ConnectionStrings__Default=Data Source={configuration.Postgre.Host};Initial Catalog={configuration.Postgre.DbName};User ID={configuration.Postgre.Username};Password={configuration.Postgre.Password}"
                }
            };

            return await DeployAndStartAsync(MonitoringWorkerContainerName, createParameters, cancellationToken);
        }

        private async Task DownloadImage(string image, CancellationToken cancellationToken)
        {
            var progress = new Progress<JSONMessage>();

            await client.Images.CreateImageAsync(new ImagesCreateParameters()
            {
                FromImage = image,
                Tag = "latest"
            }, null, progress, cancellationToken);

            progress.ProgressChanged += Progress_ProgressChanged;
        }

        private async Task DownloadImage(string image, string tag, CancellationToken cancellationToken)
        {
            var progress = new Progress<JSONMessage>();

            await client.Images.CreateImageAsync(new ImagesCreateParameters()
            {
                FromImage = image,
                Tag = tag
            }, null, progress, cancellationToken);

            progress.ProgressChanged += Progress_ProgressChanged;
        }

        private void Progress_ProgressChanged(object sender, JSONMessage e)
        {
            Console.WriteLine($"{e.ProgressMessage}");
        }

        private async Task<bool> DeployAndStartAsync(string name, CreateContainerParameters parameters, CancellationToken cancellationToken)
        {
            Exception error = null;

            do
            {
                try
                {
                    IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
                                                                                        new ContainersListParameters()
                                                                                        {
                                                                                            Limit = 1000
                                                                                        });

                    var container = containers.FirstOrDefault(c => c.Names.Contains($"/{name}"));

                    if (container == null)
                    {
                        var containerCreateResult = await client.Containers.CreateContainerAsync(parameters, cancellationToken);
                    }

                    Console.WriteLine($"------ {name} Creation Done");

                    if (container != null && container.State == "running")
                    {
                        Console.WriteLine($"------ {name} Running");
                        return true;
                    }

                    var started = await client.Containers.StartContainerAsync(name, null, cancellationToken);

                    if (started)
                    {
                        Console.WriteLine($"------ {name} Running");
                        return true;
                    }
                }
                catch (Exception e)
                {
                    error = e;
                    Console.WriteLine(e.Message);
                }
            } while (error != null && !cancellationToken.IsCancellationRequested);

            return false;
        }
    }
}
