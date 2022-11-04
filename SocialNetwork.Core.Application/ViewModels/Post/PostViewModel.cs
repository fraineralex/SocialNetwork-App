using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.ViewModels.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public string? ImagePath { get; set; }

        public int? UserId { get; set; }
        public DateTime Created { get; set; }

        public ICollection<CommentViewModel>? Comments { get; set; }

        public UserViewModel Users { get; set; }

        //public string? User { get; set; }
    }
}
