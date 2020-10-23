# TwT.HealthChecks.Mqtt
This health check add-on checks the state of the an Mqtt broker. The NuGet can be found [here](https://www.nuget.org/packages/TwT.HealthChecks.Mqtt/).
============
[![Current Version](https://img.shields.io/badge/version-1.0.0-green.svg)](https://github.com/TimoTielens/TwT.HealthChecks.Mqtt)
[![GitHub Issues](https://img.shields.io/github/issues/TimoTielens/TwT.HealthChecks.Mqtt.svg)](https://github.com/TimoTielens/TwT.HealthChecks.Mqtt/issues)
[![GitHub Stars](https://img.shields.io/github/stars/TimoTielens/TwT.HealthChecks.Mqtt.svg)](https://github.com/TimoTielens/TwT.HealthChecks.Mqtt) 

The `AddMqtt()` method adds a new health check with a specified name and the implementation of type `IHealthCheck`. This is a custom class that implements `IHealthCheck`, which takes either a IMqttClient or a IManagedMqttClient client a constructor parameter. Besides the client it also takes either a IMqttClientOptions or a IManagedMqttClientOptions parameter. This executes a simple query to check if the connection to the Mqtt broker is successful. It returns `HealthCheckResult.Healthy()` if the query was executed successfully and a `FailureStatus` with the actual exception when it fails.

## Features
- Check if connection to broker can be set up
  - Mqtt managed client
  - Mqtt unmanaged client
- Ping the broker if the connection is setup

## Getting started
Configure the services and add mqtt Health Check this way:
    
            services
                .AddHealthChecksUI()
                .AddInMemoryStorage()
                .Services
                .AddHealthChecks()
                .AddMqtt(client, managedClient);

## Example
Besides the actual implementation this repo also holds an example project that can be used as a playground and test out the application. Do note that you need to provide your own Mqtt server to test against.

## License/Copyright
This project is distributed under the Apache license version 2.0 (see the [LICENSE](https://github.com/TimoTielens/TwT.HealthChecks.Mqtt/blob/main/LICENSE.txt) file in the project root).

By submitting a pull request to this project, you agree to license your contribution under the Apache license version 2.0 to this project.