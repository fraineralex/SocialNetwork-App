using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.Services;
using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;

namespace EMarketApp.Core.Application.Services
{
    public class PostService : GenericService<SavePostViewModel, PostViewModel, Posts>, IPostsService
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;
        private readonly IMapper _mapper;

        public PostService(IPostsRepository postsRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(postsRepository, mapper)
        {
            _postsRepository = postsRepository;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            _mapper = mapper;
        }

        public async Task<List<PostViewModel>> GetAllViewModelWithInclude()
        {
            var postList = await _postsRepository.GetAllWithIncludeAsync(new List<string> { "Comments" });

            return postList.Where(post => post.UserId == userViewModel.Id).Select(post => new PostViewModel
            {
                Id = post.Id,
                Content = post.Content,
                ImagePath = post.ImagePath             

            }).ToList();
        }

        public override async Task<SavePostViewModel> Add(SavePostViewModel vm)
        {
            vm.UserId = userViewModel.Id;

            return await base.Add(vm);
        }

        public override async Task Update(SavePostViewModel vm, int id)
        {
            vm.UserId = userViewModel.Id;

            await base.Update(vm, id);
        }

    }


}
