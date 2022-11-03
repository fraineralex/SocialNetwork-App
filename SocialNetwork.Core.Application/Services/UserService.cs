using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Application.ViewModels.Friend;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Core.Application.Dtos.Email;

namespace SocialNetwork.Core.Application.Services
{
    public class UserService : GenericService<SaveUserViewModel, UserViewModel, Users>, IUsersService
    {
        private readonly IUsersRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;


        public UserService(IUsersRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService) : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public override async Task<SaveUserViewModel> Add(SaveUserViewModel vm)
        {
           await _emailService.SendAsync(new EmailRequest
           {
                To = vm.Email,
                Subject = "Your Social Network App account has been created!\r\n",
                Body = $"<h1>Welcome to Social Network App 👨🏻‍🚀</h1>" +
                $"<p>Hi {vm.Name} {vm.LastName} 😃,\r\n\r\n" +
                $"Thanks for creating an account on Social Network App. Your username is <strong>{vm.Username}</strong>.</p>" +
                $"<p>Click the following link to active your account ➡️ <a href='http://localhost:7050/User/ActiveAccount/{vm.Username}'>ACTIVAR ✅</a></p>"

           });

            return await base.Add(vm);
        }


        public async Task<List<UserViewModel>> GetAllViewModelWithInclude()
        {
            var userList = await _userRepository.GetAllWithIncludeAsync(new List<string> { "Friends, Posts, Comments" });

            return userList.Select(user => new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Username = user.Username,
                ProfileImage = user.ProfileImage,
                Friends = _mapper.Map<ICollection<FriendViewModel>>(user.Friends),
                Posts = _mapper.Map<ICollection<PostViewModel>>(user.Posts)
            }).ToList();
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

        public async Task<SaveUserViewModel> GetAUserByUsernameAsync(string username)
        {
            var userslist = await _userRepository.GetAllAsync();

            var userFiltered = userslist.Where(user => user.Username == username).FirstOrDefault();

            return _mapper.Map<SaveUserViewModel>(userFiltered);
        }

        public async Task<UserViewModel> GetUserViewModelById(int id)
        {
            var userList = await _userRepository.GetAllWithIncludeAsync(new List<string> { "Posts", "Comments" });

            UserViewModel userVm = _mapper.Map<UserViewModel>(userList.Where(user => user.Id == id).FirstOrDefault());

            return userVm;
        }

    }
}
