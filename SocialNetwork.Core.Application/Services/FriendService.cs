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
        private readonly IPostsService _postsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;
        private readonly IMapper _mapper;

        public FriendService(IFriendsRepository friendsRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper, IPostsService postsService) : base(friendsRepository, mapper)
        {
            _friendsRepository = friendsRepository;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            _mapper = mapper;
            _postsService = postsService;
        }

        public async Task<List<PostViewModel>> GetAllViewModelWithInclude()
        {
            var friendList = await _friendsRepository.GetAllAsync();

            List<PostViewModel> postVmList = new();

            foreach (var friend in friendList.Where(friend => friend.SenderId == userViewModel.Id).ToList())
            {

                List<PostViewModel> posts = await _postsService.GetPostViewModelByUserId(friend.ReceptorId);

                foreach(var post in posts)
                {
                    postVmList.Add(post);
                }
            }

            return postVmList;
        }

        public async Task<bool> CheckIfAreFriend(int SenderId, int ReceptorId)
        {
            var friendList = await _friendsRepository.GetAllAsync();

            var areFriend = friendList.Where(friend => friend.SenderId == SenderId && friend.ReceptorId == ReceptorId).ToList();

            return areFriend.Count != 0;
        }

        public async Task<FriendViewModel> GetFriendByReceptor(int ReceptorId)
        {
            var friendList = await _friendsRepository.GetAllAsync();

            var friendVm = friendList.Where(friend => friend.SenderId == userViewModel.Id && friend.ReceptorId == ReceptorId).FirstOrDefault();

            return _mapper.Map<FriendViewModel>(friendVm);
        }
        

    }


}
