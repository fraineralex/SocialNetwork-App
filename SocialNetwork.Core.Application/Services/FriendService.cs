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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;
        private readonly IMapper _mapper;

        public FriendService(IFriendsRepository friendsRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(friendsRepository, mapper)
        {
            _friendsRepository = friendsRepository;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            _mapper = mapper;
        }

        public async Task<List<FriendViewModel>> GetAllViewModelWithInclude()
        {
            var friendList = await _friendsRepository.GetAllWithIncludeAsync(new List<string> { "Posts" });

            return friendList.Where(friend => friend.SenderId == userViewModel.Id || friend.ReceptorId == userViewModel.Id && friend.IsAccepted == true).Select(friend => new FriendViewModel
            {
                Id = friend.Id,
                SenderId = friend.SenderId,
                ReceptorId = friend.ReceptorId,
                CreatedAt = friend.CreatedAt,
                IsAccepted = friend.IsAccepted,
                Posts = _mapper.Map<ICollection<PostViewModel>>(friend.Posts)

            }).ToList();
        }

        public override async Task<SaveFriendViewModel> Add(SaveFriendViewModel vm)
        {
            vm.SenderId = userViewModel.Id;

            return await base.Add(vm);
        }

        public override async Task Update(SaveFriendViewModel vm, int id)
        {
            vm.SenderId = userViewModel.Id;

            await base.Update(vm, id);
        }

    }


}
