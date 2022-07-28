using api.Consumers;
using application;
using infrastructure;

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
            ConsumerType = typeof(StockDecreaseFailedConsumer),
            QueueName = builder.Configuration.GetValue<string>("RabbitMQ:Queues:Consume:OrderFailed")
        },
        new infrastructure.DependencyInjection.Consumer
        {
            ConsumerType = typeof(PaymentSuccessedConsumer),
            QueueName = builder.Configuration.GetValue<string>("RabbitMQ:Queues:Consume:PaymentSuccessed")
        }
    };
});
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
