using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Core.Application.ViewModels.Friend;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.ViewModels.Auth
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool? IsActive { get; set; }

        //Navigation property
        public ICollection<PostViewModel>? Posts { get; set; }
        public ICollection<CommentViewModel>? Comments { get; set; }
        public ICollection<FriendViewModel>? Friends { get; set; }
    }
}
