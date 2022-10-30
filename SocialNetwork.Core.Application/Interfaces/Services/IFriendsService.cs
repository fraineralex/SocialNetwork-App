using SocialNetwork.Core.Application.ViewModels.Friend;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IFriendsService : IGenericService<SaveFriendViewModel, FriendViewModel, Friends>
    {
        Task<List<FriendViewModel>> GetAllViewModelWithInclude();
    }
}
