using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Services
{
    public class UserService : IUsersService
    {
        private readonly IUsersRepository _userRepository;

        public UserService(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserViewModel>> GetAllViewModel()
        {
            var userList = await _userRepository.GetAllWithIncludeAsync(new List<string> { "Ads" });

            return userList.Select(user => new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Username = user.Username,
                Password = user.Password,

            }).ToList();
        }

        public async Task<SaveUserViewModel> Add(SaveUserViewModel vm)
        {
            Users user = new();
            user.Name = vm.Name;
            user.LastName = vm.LastName;
            user.ProfileImage = "none";
            user.Email = vm.Email;
            user.Phone = vm.Phone;
            user.Username = vm.Username;
            user.Password = vm.Password;

            user = await _userRepository.AddAsync(user);

            SaveUserViewModel userVm = new();
            userVm.Id = user.Id;
            userVm.Name = user.Name;
            userVm.LastName = user.LastName;
            userVm.ProfileImagePath = user.ProfileImage;
            userVm.Email = user.Email;
            userVm.Phone = user.Phone;
            userVm.Username = user.Username;
            userVm.Password = user.Password;

            return userVm;
        }

        public async Task Update(SaveUserViewModel vm)
        {
            Users user = new();
            user.Id = vm.Id;
            user.Name = vm.Name;
            user.LastName = vm.LastName;
            user.ProfileImage = vm.ProfileImagePath;
            user.Email = vm.Email;
            user.Phone = vm.Phone;
            user.Username = vm.Username;
            user.Password = vm.Password;

            await _userRepository.UpdateAsync(user);
        }

        public async Task Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            await _userRepository.DeleteAsync(user);
        }

        public async Task<SaveUserViewModel> GetSaveViewModelById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            SaveUserViewModel vm = new()
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Username = user.Username,
                Password = user.Password,
            };

            return vm;
        }

        public async Task<UserViewModel> Login(LoginViewModel loginVm)
        {
            Users user = await _userRepository.LoginAsync(loginVm);

            if (user == null)
            {
                return null;
            }

            UserViewModel userVm = new();
            userVm.Id = user.Id;
            userVm.Name = user.Name;
            userVm.LastName = user.LastName;
            userVm.Email = user.Email;
            userVm.Phone = user.Phone;
            userVm.Username = user.Username;
            userVm.Password = user.Password;

            return userVm;
        }

        public async Task<Users> GetAUserByUsernameAsync(string username)
        {
            var userslist = await _userRepository.GetAllAsync();

            var userfiltered = userslist.Where(user => user.Username == username).FirstOrDefault();

            return userfiltered;
        }

    }
}
