using SocialNetwork.Core.Application.ViewModels.Post;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IPostsService : IGenericService<PostViewModel, SavePostViewModel>
    {
        //Task<List<PostViewModel>> GetAllViewModelWithFilters(FilterViewModel vm);
        //Task<DetailsAdViewModel> GetAdDetailsByIdAsync(int id);
        List<PostViewModel> FilterAdsByCategory(List<PostViewModel> adViewModelList, string categoryId);
    }
}
