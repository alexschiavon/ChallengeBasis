using BookApplication.Services;
using BookDomain.Services;
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
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IAuthorService, AuthorService>();

            services.AddDataBaseModule(configuration);
            return services;
        }
    }
}
