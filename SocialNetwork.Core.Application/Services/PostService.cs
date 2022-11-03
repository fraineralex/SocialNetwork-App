using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Repositories;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.Services;
using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Services
{
    public class PostService : GenericService<SavePostViewModel, PostViewModel, Posts>, IPostsService
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IUsersService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;
        private readonly IMapper _mapper;

        public PostService(IPostsRepository postsRepository, IUsersService userService, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(postsRepository, mapper)
        {
            _postsRepository = postsRepository;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            _mapper = mapper;
        }

        public async Task<List<PostViewModel>> GetAllViewModelWithInclude()
        {
            var postList = await _postsRepository.GetAllWithIncludeAsync(new List<string> { "Comments", "Users" });

            return postList.Where(post => post.UserId == userViewModel.Id).Select(post => new PostViewModel
            {
                Id = post.Id,
                Content = post.Content,
                ImagePath = post.ImagePath,
                UserId = post.UserId,
                Comments = _mapper.Map<ICollection<CommentViewModel>>(post.Comments),
                Users = _mapper.Map<UserViewModel>(post.Users)

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

        public virtual async Task<PostViewModel> GetPostViewModelById(int id)
        {
            var postList = await _postsRepository.GetAllWithIncludeAsync(new List<string> { "Comments", "Users" });

            var post = postList.Where(post => post.Id == id).FirstOrDefault();

            PostViewModel postVm = _mapper.Map<PostViewModel>(post);

            return postVm;
        }

        public virtual async Task<List<PostViewModel>> GetPostViewModelByUserId(int id)
        {
            var postList = await _postsRepository.GetAllWithIncludeAsync(new List<string> { "Comments", "Users" });

            var post = postList.Where(post => post.UserId == id).ToList();

            List<PostViewModel> postVmList = _mapper.Map<List<PostViewModel>>(post);

            return postVmList;
        }

    }


}
