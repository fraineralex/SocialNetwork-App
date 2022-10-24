using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace SocialNetwork.Core.Application
{
    //Extension method - decorator pattern
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            #region services
            //services.AddTransient<IAdsService, AdService>();
            //services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IUsersService, UserService>();
            #endregion
        }
    }
}
