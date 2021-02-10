using Kubernox.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Kubernox
{
    class Program
    {
        private static readonly ContainerDeployService containerDeployService = new ContainerDeployService();

        async static Task Main(string[] args)
        {
            PrintHeader();
            await StartDeployment(await ExtractConfigurationAsync());
        }

        private static async Task StartDeployment(Configuration configuration)
        {
            var deploymentStack = new List<Task<bool>>();

            var cancellationToken = new CancellationToken();
            deploymentStack.Add(containerDeployService.InstantiateDatabaseContainer(configuration.Postgre, cancellationToken));
            deploymentStack.Add(containerDeployService.InstantiateQueueContainer(configuration.Rabbitmq, cancellationToken));
            deploymentStack.Add(containerDeployService.InstantiateCacheContainer(configuration.Redis, cancellationToken));
            deploymentStack.Add(containerDeployService.InstantiatePrometheusContainer(configuration.Prometheus, cancellationToken));

            var results = await Task.WhenAll(deploymentStack);

            if (results.Any(a => !a))
            {
                Console.WriteLine("Deployment failed.");
            }
            else
            {
                Console.WriteLine("Deployment success.");
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine(@"#################################################################");
            Console.WriteLine(@"#                                                               #");
            Console.WriteLine(@"#     ____  __.    ___.                                         #");
            Console.WriteLine(@"#    |    |/ _|__ _\_ |__   ___________  ____   _______  ___    #");
            Console.WriteLine(@"#    |      < |  |  \ __ \_/ __ \_  __ \/    \ /  _ \  \/  /    #");
            Console.WriteLine(@"#    |    |  \|  |  / \_\ \  ___/|  | \/   |  (  <_> >    <     #");
            Console.WriteLine(@"#    |____|__ \____/|___  /\___  >__|  |___|  /\____/__/\_ \    #");
            Console.WriteLine(@"#            \/         \/     \/           \/            \/    #");
            Console.WriteLine(@"#                                                               #");
            Console.WriteLine(@"#          Created by David Gilson & Patrick Grasseels          #");
            Console.WriteLine(@"#                                                               #");
            Console.WriteLine(@"#################################################################");
        }

        private static async Task<Configuration> ExtractConfigurationAsync()
        {
            var yaml = await File.ReadAllTextAsync("base-config.yaml");
            var deserializer = new DeserializerBuilder()
                                 .WithNamingConvention(UnderscoredNamingConvention.Instance)
                                 .Build();

            var configuration = deserializer.Deserialize<Configuration>(yaml);
            return configuration;
        }
    }
}
