using SocialNetwork.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.Infrastructure.Persistence
{
    //Extension method - decorator pattern
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region contexts
            if(configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(o => o.UseInMemoryDatabase("ApplicationDB"));
            }
            else
            {
                services.AddDbContext<ApplicationContext>(options =>
                            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                            m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
    }
            #endregion

            //#region repositories
            //services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddTransient<IAdsRepository, AdRepository>();
            //services.AddTransient<ICategoriesRepository, CategoryRepository>();
            //services.AddTransient<IUsersRepository, UserRepository>();
            //#endregion
        }
    }
}
