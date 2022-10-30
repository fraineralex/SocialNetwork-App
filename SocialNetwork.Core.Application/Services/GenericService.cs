using AutoMapper;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Services
{
    public class GenericService<SaveViewModel, ViewModel, Model> : IGenericService<SaveViewModel, ViewModel, Model>
        where SaveViewModel : class
        where ViewModel : class
        where Model : class
    {
        private readonly IGenericRepository<Model> _genericRepository;
        private readonly IMapper _mapper;


        public GenericService(IGenericRepository<Model> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public virtual async Task<List<ViewModel>> GetAllViewModel()
        {
            var entityList = await _genericRepository.GetAllAsync();

            return _mapper.Map<List<ViewModel>>(entityList);
        }

        public virtual async Task<SaveViewModel> Add(SaveViewModel vm)
        {
            Model entity = _mapper.Map<Model>(vm);

            entity = await _genericRepository.AddAsync(entity);

            SaveViewModel entityVm = _mapper.Map<SaveViewModel>(entity);

            return entityVm;
        }

        public virtual async Task Update(SaveViewModel vm, int id)
        {
            Model entity = _mapper.Map<Model>(vm);

            await _genericRepository.UpdateAsync(entity, id);
        }

        public virtual async Task Delete(int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);
            await _genericRepository.DeleteAsync(entity);
        }

        public virtual async Task<SaveViewModel> GetSaveViewModelById(int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);

            SaveViewModel vm = _mapper.Map<SaveViewModel>(entity);

            return vm;
        }
    }
}
