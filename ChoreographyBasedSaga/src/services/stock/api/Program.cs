using api.Consumers;
using application;
using infrastructure;
using infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddInfrastructure(x =>
{
    x.Configuration = builder.Configuration;
    x.Consumers = new List<infrastructure.DependencyInjection.Consumer>
    {
        new infrastructure.DependencyInjection.Consumer
        {
            ConsumerType = typeof(OrderPlacedConsumer),
            QueueName = builder.Configuration.GetValue<string>("RabbitMQ:Queues:Consume:OrderPlaced")
        }
    };
});
builder.Services.AddApplication();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var stockDbContext = scope.ServiceProvider.GetRequiredService<StockDbContext>();
    stockDbContext.SeedData();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
