using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace TwT.HealthChecks.Mqtt
{
    /// <summary>
    /// Represents a health check, which can be used to check the status of the mqtt server.
    /// </summary>
    public class MqttHealthCheck : IHealthCheck
    {
        /// <summary>
        /// Client that will be used to connect to the mqtt server to verify if it's state is OK
        /// </summary>
        public IMqttClient UnManagedMqttClient { get; private set; }

        /// <summary>
        /// Client that will be used to connect to the mqtt server to verify if it's state is OK
        /// </summary>
        public IManagedMqttClient ManagedMqttClient { get; private set; }

        /// <summary>
        /// Options that need to be used to connect to the server if not yet connected
        /// </summary>
        public IMqttClientOptions UnManagedMqttClientOptions { get; private set; }

        /// <summary>
        /// Options that need to be used to connect to the server if not yet connected
        /// </summary>
        public IManagedMqttClientOptions ManagedMqttClientOptions { get; private set; }


        /// <summary>
        /// Represents a health check, which can be used to check the status of the mqtt server.
        /// </summary>
        /// <param name="client">Client that will be used to connect to the mqtt server to verify if it's state is OK</param>
        /// <param name="options">Options that need to be used to connect to the server if not yet connected</param>
        public MqttHealthCheck(IManagedMqttClient client, IManagedMqttClientOptions options)
        {
            ManagedMqttClientOptions = options ?? throw new ArgumentNullException(nameof(options));
            ManagedMqttClient = client ?? throw new ArgumentNullException(nameof(client));
            ManagedMqttClient.StartAsync(ManagedMqttClientOptions);
        }

        /// <summary>
        /// Represents a health check, which can be used to check the status of the mqtt server.
        /// </summary>
        /// <param name="client">Client that will be used to connect to the mqtt server to verify if it's state is OK</param>
        /// <param name="options">Options that need to be used to connect to the server if not yet connected</param>
        public MqttHealthCheck(IMqttClient client, IMqttClientOptions options)
        {
            UnManagedMqttClientOptions = options;
            UnManagedMqttClient = client ?? throw new ArgumentNullException(nameof(client));
            UnManagedMqttClient.ConnectAsync(UnManagedMqttClientOptions);
        }

        /// <summary>
        /// Check if the client can connect to the mqtt server and ping it.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the health check.</param>
        /// <returns>A <see cref="Task{HealthCheckResult}"/> that completes when the health check has finished, yielding the status of the component being checked.</returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (UnManagedMqttClient != null)
                return CheckUnManagedMqttClient(context, cancellationToken);
            else
                return CheckManagedMqttClient(context, cancellationToken);
        }

        /// <summary>
        /// Check if the unmanaged client can connect to the mqtt server and ping it.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the health check.</param>
        /// <returns>A <see cref="Task{HealthCheckResult}"/> that completes when the health check has finished, yielding the status of the component being checked.</returns>
        private async Task<HealthCheckResult> CheckUnManagedMqttClient(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (UnManagedMqttClient.IsConnected)
                {
                    await UnManagedMqttClient.PingAsync(cancellationToken);
                    return HealthCheckResult.Healthy();
                }
                return new HealthCheckResult(context.Registration.FailureStatus, "Could not connect to the broker");
            }
            catch (Exception e)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, HandleExceptions(e));
            }
        }

        /// <summary>
        /// Check if the managed client can connect to the mqtt server and ping it.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the health check.</param>
        /// <returns>A <see cref="Task{HealthCheckResult}"/> that completes when the health check has finished, yielding the status of the component being checked.</returns>
        private async Task<HealthCheckResult> CheckManagedMqttClient(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ManagedMqttClient.IsConnected)
                {
                    await ManagedMqttClient.PingAsync(cancellationToken);
                    return HealthCheckResult.Healthy();
                }
                return new HealthCheckResult(context.Registration.FailureStatus, "Could not connect to the broker");
            }
            catch (Exception e)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, HandleExceptions(e));
            }
        }

        private string HandleExceptions(Exception e)
        {
            if (e.Message == "A task was canceled.")
                return "Could not connect to the broker";

            return e.Message;
        }
    }
}