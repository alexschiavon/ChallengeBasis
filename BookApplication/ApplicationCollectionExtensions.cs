using Finance.DataAccessAdapter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.Application
{
    public static class ApplicationCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            //services.AddScoped<ITransactionService, TransactionService>();
           
            services.AddDataBaseModule(configuration);
            return services;
        }
    }
}
