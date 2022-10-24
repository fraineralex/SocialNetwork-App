namespace SocialNetwork.Core.Application.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public int? AuthorId { get; set; }  

        public string? AuthorUsername { get; set; }
        public string? AuthorProfileImage { get; set; }

        public int? PostId { get; set; }
    }
}
