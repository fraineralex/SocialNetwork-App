using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public int? UserId { get; set; }  

        public int? PostId { get; set; }

        public UserViewModel? Users { get; set; }
        public PostViewModel? Posts { get; set; }
    }
}
