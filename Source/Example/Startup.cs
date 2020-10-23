using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;

namespace TwT.HealthChecks.Mqtt.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var managedClient = GetManagedMqttClientOptions(GetOptions(), TimeSpan.FromSeconds(10));
            var clientFactory = new MqttFactory();
            var client = clientFactory.CreateManagedMqttClient();


            services
                .AddHealthChecksUI()
                .AddInMemoryStorage()
                .Services
                .AddHealthChecks()
                .AddMqtt(client, managedClient)
                .AddCheck<RandomHealthCheck>("random")
                .AddUrlGroup(new Uri("http://httpbin.org/status/200"))
                .Services
                .AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app
                .UseRouting()
                .UseEndpoints(config =>
                {
                    config.MapHealthChecks("healthz", new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
                    config.MapHealthChecksUI();
                    config.MapDefaultControllerRoute();
                });

        }

        private IMqttClientOptions GetOptions()
        {
            return new MqttClientOptionsBuilder()
                .WithTcpServer("192.168.2.1", 1883)
                .WithTls(x =>
                {
                    x.UseTls = false;
                    x.SslProtocol = SslProtocols.Tls12;
                    x.AllowUntrustedCertificates = true;
                    x.Certificates = new List<X509Certificate>(); ;
                })
                .WithCredentials("USERNAME", "PASSWORD")
                .WithProtocolVersion(MqttProtocolVersion.V311)
                .WithClientId("TwT.Mqtt")
                .Build();
        }

        private IManagedMqttClientOptions GetManagedMqttClientOptions(IMqttClientOptions options, TimeSpan reconnectDelay)
        {
            return new ManagedMqttClientOptionsBuilder()
                .WithClientOptions(options)
                .WithAutoReconnectDelay(reconnectDelay)
                .Build();
        }
    }
}
