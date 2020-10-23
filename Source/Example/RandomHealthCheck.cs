using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TwT.HealthChecks.Mqtt.Example
{
    public class RandomHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return DateTime.UtcNow.Minute % 2 == 0 ? Task.FromResult(HealthCheckResult.Healthy())
                : Task.FromResult(HealthCheckResult.Unhealthy("failed"));
        }
    }
}