using SocialNetwork.Core.Application.ViewModels.Friend;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IFriendsService : IGenericService<SaveFriendViewModel, FriendViewModel, Friends>
    {
        Task<List<PostViewModel>> GetAllViewModelWithInclude();
        Task<bool> CheckIfAreFriend(int SenderId, int ReceptorId);
        Task<Friends> GetFriendByReceptor(int ReceptorId);
    }
}
