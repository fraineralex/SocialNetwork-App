using AutoMapper;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Services
{
    public class UserService : IUsersService
    {
        private readonly IUsersRepository _userRepository;
        private readonly IMapper _mapper;


        public UserService(IUsersRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserViewModel>> GetAllViewModel()
        {
            var userList = await _userRepository.GetAllAsync();

            return _mapper.Map<List<UserViewModel>>(userList);
        }

        public async Task<SaveUserViewModel> Add(SaveUserViewModel vm)
        {
            Users user = _mapper.Map<Users>(vm);

            user = await _userRepository.AddAsync(user);

            SaveUserViewModel userVm = _mapper.Map<SaveUserViewModel>(user);

            return userVm;
        }

        public async Task Update(SaveUserViewModel vm, int id)
        {
            Users user = _mapper.Map<Users>(vm);

            await _userRepository.UpdateAsync(user, id);
        }

        public async Task Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            await _userRepository.DeleteAsync(user);
        }

        public async Task<SaveUserViewModel> GetSaveViewModelById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            SaveUserViewModel vm = _mapper.Map<SaveUserViewModel>(user);

            return vm;
        }

        public async Task<UserViewModel> Login(LoginViewModel loginVm)
        {
            Users user = await _userRepository.LoginAsync(loginVm);

            if (user == null)
            {
                return null;
            }

            UserViewModel userVm = _mapper.Map<UserViewModel>(user);

            return userVm;
        }

        public async Task<Users> GetAUserByUsernameAsync(string username)
        {
            var userslist = await _userRepository.GetAllAsync();

            var userFiltered = userslist.Where(user => user.Username == username).FirstOrDefault();

            return userFiltered;
        }

    }
}
