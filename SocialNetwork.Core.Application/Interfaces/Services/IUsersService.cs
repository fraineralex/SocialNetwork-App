using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IUsersService : IGenericService<UserViewModel, SaveUserViewModel>
    {
        Task<UserViewModel> Login(LoginViewModel loginVm);
        Task<Users> GetAUserByUsernameAsync(string username);
    }
}
