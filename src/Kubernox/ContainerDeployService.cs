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

        public ContainerDeployService()
        {
            client = new DockerClientConfiguration().CreateClient();
        }

        public async Task<bool> InstantiateDatabaseContainer(PostgreDatabaseProvider database, CancellationToken cancellationToken)
        {
            Console.WriteLine("---- Starting Deploy Database Container ----");
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

        public async Task<bool> InstantiatQueueContainer(RabbitMqProvider queue, CancellationToken cancellationToken)
        {
            Console.WriteLine("---- Starting Deploy Queue Container ----");
            var createParameters = new CreateContainerParameters()
            {
                Image = "rabbitmq:3",
                Name = QueueContainerName,
                Env = new List<string>() { $"RABBITMQ_DEFAULT_USER={queue.Username}", $"RABBITMQ_DEFAULT_PASS={queue.Password}" },
            };

            return await DeployAndStartAsync(QueueContainerName, createParameters, cancellationToken);
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
