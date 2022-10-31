
namespace SocialNetwork.Core.Application.ViewModels.Comment
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public int? UserId { get; set; }

        public int? PostId { get; set; }
    }

}
