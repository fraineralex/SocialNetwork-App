using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IPostsService : IGenericService<SavePostViewModel, PostViewModel, Posts>
    {
        Task<List<PostViewModel>> GetAllViewModelWithInclude();
        Task<PostViewModel> GetPostViewModelById(int id);
    }
}
