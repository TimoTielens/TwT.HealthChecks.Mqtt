﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace TwT.HealthChecks.Mqtt.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}