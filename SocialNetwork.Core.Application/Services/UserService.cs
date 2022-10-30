using AutoMapper;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Services
{
    public class UserService : GenericService<SaveUserViewModel, UserViewModel, Users>, IUsersService
    {
        private readonly IUsersRepository _userRepository;
        private readonly IMapper _mapper;


        public UserService(IUsersRepository userRepository, IMapper mapper) : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
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
