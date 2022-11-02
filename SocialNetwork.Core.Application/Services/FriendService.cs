using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.Services;
using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Application.ViewModels.Friend;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;

namespace EMarketApp.Core.Application.Services
{
    public class FriendService : GenericService<SaveFriendViewModel, FriendViewModel, Friends>, IFriendsService
    {
        private readonly IFriendsRepository _friendsRepository;
        private readonly IUsersService _usersService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;
        private readonly IMapper _mapper;

        public FriendService(IFriendsRepository friendsRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IUsersService usersService) : base(friendsRepository, mapper)
        {
            _friendsRepository = friendsRepository;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            _mapper = mapper;
            _usersService = usersService;
        }

        public async Task<List<FriendViewModel>> GetAllViewModelWithInclude()
        {
            var friendList = await _friendsRepository.GetAllWithIncludeAsync(new List<string> { "Posts", "Users" });

            return friendList.Where(friend => friend.SenderId == userViewModel.Id && friend.IsAccepted == true).Select(friend => new FriendViewModel
            {
                Id = friend.Id,
                SenderId = friend.SenderId,
                ReceptorId = friend.ReceptorId,
                CreatedAt = friend.CreatedAt,
                IsAccepted = friend.IsAccepted,
                Posts = _mapper.Map<ICollection<PostViewModel>>(friend.Posts),
                Users = _mapper.Map<UserViewModel>(friend.Users)

            }).ToList();

        }

    }


}
