using Microsoft.EntityFrameworkCore;
using Project_Lykos.Worker;
using Microsoft.Extensions.Configuration;
using System.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Lykos Worker Service";
    })
    .ConfigureServices((ctx, services) =>
    {
        services.AddSingleton<QueueHelper>();
        services.AddHostedService<Project_Lykos.Worker.WindowsBackgroundService>();
        services.AddDbContext<Project_Lykos.Data.LykosQueueContext>(options =>
        {
            options.UseSqlServer(ctx.Configuration.GetConnectionString("LykosQueue"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                })
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
        });
    })
    .Build();

await host.RunAsync();
