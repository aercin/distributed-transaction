using core_application.Interfaces;
using core_infrastructure.DependencyManagements;
using core_infrastructure.Persistence;
using messageRelayService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<OrderWorker>();
        services.AddHostedService<StockWorker>();
        services.AddHostedService<PaymentWorker>();
        services.AddHostedService<OrchestratorWorker>();

        services.AddSingleton<IDbConnectionFactory, PostgreDbConnectionFactory>((serviceProvider) =>
        {
            return new PostgreDbConnectionFactory("order", hostContext.Configuration.GetConnectionString("OrderDb"));
        });
        services.AddSingleton<IDbConnectionFactory, PostgreDbConnectionFactory>((serviceProvider) =>
        {
            return new PostgreDbConnectionFactory("stock", hostContext.Configuration.GetConnectionString("StockDb"));
        });
        services.AddSingleton<IDbConnectionFactory, PostgreDbConnectionFactory>((serviceProvider) =>
        {
            return new PostgreDbConnectionFactory("payment", hostContext.Configuration.GetConnectionString("PaymentDb"));
        });
        services.AddSingleton<IDbConnectionFactory, PostgreDbConnectionFactory>((serviceProvider) =>
        {
            return new PostgreDbConnectionFactory("orchestrator", hostContext.Configuration.GetConnectionString("OrchestratorDb"));
        });

        services.AddDistributedCacheDependency(options =>
        {
            options.Endpoints = hostContext.Configuration.GetValue<string>("Redis:Endpoints");
            options.Password = hostContext.Configuration.GetValue<string>("Redis:Password");
            options.Database = hostContext.Configuration.GetValue<int>("Redis:Database");
        });
        services.AddAsyncIntegrationStyleDependency(options =>
        {
            options.RootUri = hostContext.Configuration.GetValue<string>("RabbitMQ:RootUri");
            options.UserName = hostContext.Configuration.GetValue<string>("RabbitMQ:UserName");
            options.Password = hostContext.Configuration.GetValue<string>("RabbitMQ:Password");
        });
    })
    .Build();

await host.RunAsync();
