using BookDataAccessAdapter.Context;
using BookDataAccessAdapter.Repositories;
using BookDomain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finance.DataAccessAdapter
{
    public static class DataBaseModuleDependency
    {
        public static void AddDataBaseModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<ISubjectRepository, SubjectRepository>();

            //services.AddDbContext<BookContext>();

            services.AddDbContext<BookContext>(options =>
            {
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                options.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly(typeof(BookContext).Assembly.FullName));
            });
        }
    }
}
