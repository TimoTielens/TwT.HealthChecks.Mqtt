using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace TwT.HealthChecks.Mqtt
{
    /// <summary>
    /// Health check for mqtt services 
    /// </summary>
    public static class MqttHealthCheckBuilderExtensions
    {
        /// <summary>
        /// Add a health check for mqtt services 
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
        /// <param name="client">Client that will be used to connect to the mqtt server to verify if it's state is OK</param>
        /// <param name="options">Options that need to be used to connect to the server if not yet connected</param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'Mqtt' will be used for the name.</param>
        /// <param name="failureStatus">The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.</param>
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <param name="timeout">An optional System.TimeSpan representing the timeout of the check.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns>
        public static IHealthChecksBuilder AddMqtt(this IHealthChecksBuilder builder, IManagedMqttClient client, IManagedMqttClientOptions options, string name = "Mqtt", HealthStatus failureStatus = HealthStatus.Unhealthy, IEnumerable<string> tags = default, TimeSpan? timeout = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            builder.Services.AddSingleton(sp => new MqttHealthCheck(client, options));

            return builder.Add(new HealthCheckRegistration(name, sp => sp.GetRequiredService<MqttHealthCheck>(),
                failureStatus,
                tags,
                timeout));
        }

        /// <summary>
        /// Add a health check for mqtt services 
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
        /// <param name="client">Client that will be used to connect to the mqtt server to verify if it's state is OK</param>
        /// <param name="options">Options that need to be used to connect to the server if not yet connected</param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'Mqtt' will be used for the name.</param>
        /// <param name="failureStatus">The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.</param>
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <param name="timeout">An optional System.TimeSpan representing the timeout of the check.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns>
        public static IHealthChecksBuilder AddMqtt(this IHealthChecksBuilder builder, IMqttClient client, IMqttClientOptions options, string name = "Mqtt", HealthStatus failureStatus = HealthStatus.Unhealthy, IEnumerable<string> tags = default, TimeSpan? timeout = default)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            builder.Services.AddSingleton(sp => new MqttHealthCheck(client, options));

            return builder.Add(new HealthCheckRegistration(name, sp => sp.GetRequiredService<MqttHealthCheck>(),
                failureStatus,
                tags,
                timeout));
        }
    }
}