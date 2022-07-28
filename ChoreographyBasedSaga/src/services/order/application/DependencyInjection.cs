using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(PlaceOrder).Assembly);

            return services;
        }
    }
}
