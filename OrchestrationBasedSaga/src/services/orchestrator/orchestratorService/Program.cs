using infrastructure;
using infrastructure.Persistence;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddInfrastructure(options =>
        {
            options.Configuration = hostContext.Configuration;
            options.Consumers = new List<DependencyInjection.Consumer>
            {
                new DependencyInjection.Consumer
                {
                    StateInstanceType = typeof(OrderStateInstance),
                    QueueName = hostContext.Configuration.GetValue<string>("RabbitMQ:Queues:Consume:StateMachine")
                }
            };
        });
    })
    .Build();

await host.RunAsync();
