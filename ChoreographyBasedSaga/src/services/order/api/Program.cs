using api.Consumers;
using application;
using IdentityServer4.AccessTokenValidation;
using infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());

    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});
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

builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = builder.Configuration.GetValue<string>("IdentityServer:BaseUrl");
                    options.ApiName = "order-api";
                    options.RequireHttpsMetadata = false;
                    options.SupportedTokens = SupportedTokens.Jwt;
                });

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
