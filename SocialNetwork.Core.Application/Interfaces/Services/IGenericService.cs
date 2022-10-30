using SocialNetwork.Core.Application.ViewModels;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Model>
        where SaveViewModel : class
        where ViewModel : class
        where Model : class
    {
        Task<List<ViewModel>> GetAllViewModel();
        Task<SaveViewModel> Add(SaveViewModel vm);
        Task Update(SaveViewModel vm, int id);
        Task Delete(int id);
        Task<SaveViewModel> GetSaveViewModelById(int id);


    }
}
