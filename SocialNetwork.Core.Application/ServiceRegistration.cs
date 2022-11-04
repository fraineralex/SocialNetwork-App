using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SocialNetwork.Core.Application
{
    //Extension method - decorator pattern
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #region services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IPostsService, PostService>();
            services.AddTransient<ICommentsService, CommentService>();
            services.AddTransient<IFriendsService, FriendService>();
            services.AddTransient<IUsersService, UserService>();
            #endregion
        }
    }
}
