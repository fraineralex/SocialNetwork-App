
namespace SocialNetwork.Core.Application.Comment
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public int? AuthorId { get; set; }

        public int? PostId { get; set; }
    }

}
