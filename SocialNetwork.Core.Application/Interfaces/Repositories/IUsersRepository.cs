using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Interfaces.Repositories
{
    public interface IUsersRepository : IGenericRepository<Users>
    {
        Task<Users> LoginAsync(LoginViewModel loginVm);
    }
}
