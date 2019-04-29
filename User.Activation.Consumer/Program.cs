using System;
using System.IO;
using System.Threading.Tasks;
using Jaeger;
using Jaeger.Samplers;
using MassTransit;
using MetroBus;
using MetroBus.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;
using User.Activation.Consumer.Consumers;
using User.Activation.Consumer.Services.Implementations;

namespace User.Activation.Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(basePath: Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional : true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    //Init tracer
                    services.AddSingleton<ITracer>(t => InitTracer());

                    string rabbitMqUri = hostContext.Configuration.GetValue<string>("RabbitMqUri");
                    string rabbitMqUserName = hostContext.Configuration.GetValue<string>("RabbitMqUserName");
                    string rabbitMqPassword = hostContext.Configuration.GetValue<string>("RabbitMqPassword");

                    services.AddMetroBus(x =>
                    {
                        x.AddConsumer<UserActivationConsumer>();
                    });

                    services.AddSingleton<IBusControl>(provider => MetroBusInitializer.Instance
                        .UseRabbitMq(rabbitMqUri, rabbitMqUserName, rabbitMqPassword)
                        .RegisterConsumer<UserActivationConsumer>("user.activation.queue", provider)
                        .Build());

                    services.AddHostedService<BusService>();
                });

            await host.RunConsoleAsync();
        }

        private static ITracer InitTracer()
        {
            Environment.SetEnvironmentVariable("JAEGER_SERVICE_NAME", "User.Activation.Consumer");
            Environment.SetEnvironmentVariable("JAEGER_AGENT_HOST", "localhost");
            Environment.SetEnvironmentVariable("JAEGER_AGENT_PORT", "6831");
            Environment.SetEnvironmentVariable("JAEGER_SAMPLER_TYPE", "const");

            var loggerFactory = new LoggerFactory();

            var config = Jaeger.Configuration.FromEnv(loggerFactory);
            var tracer = config.GetTracer();

            GlobalTracer.Register(tracer);

            return tracer;
        }
    }
}