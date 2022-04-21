using AsyncProjectionStressTest.Common;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(builder => builder.AddEnvironmentVariables())
    .ConfigureServices(
        (context, services) =>
        {
            services.AddCommonServices(true);
            services.Configure<MartenDbOptions>(context.Configuration.GetSection("Marten"));
        })
    .Build();

await host.RunAsync();