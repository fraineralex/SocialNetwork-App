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

namespace EMarketApp.Core.Application.Services
{
    public class CommentService : GenericService<SaveCommentViewModel, CommentViewModel, Comments>, ICommentsService
    {
        private readonly ICommentsRepository _commentsRepository;
        private readonly IUsersService _usersService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;
        private readonly IMapper _mapper;

        public CommentService(ICommentsRepository commentsRepository, IUsersService usersService, IHttpContextAccessor httpContextAccessor, IMapper mapper) : base(commentsRepository, mapper)
        {
            _commentsRepository = commentsRepository;
            _usersService = usersService;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            _mapper = mapper;
        }

        public async Task<List<CommentViewModel>> GetAllViewModelWithInclude()
        {
            var commentList = await _commentsRepository.GetAllWithIncludeAsync(new List<string> { "Users" });


            return commentList.Select(comment => new CommentViewModel
            {
                Id = comment.Id,
                Content = comment.Content,
                UserId = comment.UserId,
                Created = comment.Created.ToString("d/M/yyyy hh:mm"),
                Users = _mapper.Map<UserViewModel>(comment.Users)

            }).ToList();
        }

        public override async Task<SaveCommentViewModel> Add(SaveCommentViewModel vm)
        {
            vm.Created = vm.Created = DateTime.Now.ToString();
            return await base.Add(vm);
        }


    }


}
