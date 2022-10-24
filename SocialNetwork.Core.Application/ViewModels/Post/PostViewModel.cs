namespace SocialNetwork.Core.Application.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public string? ImagePath { get; set; }

        public string? User { get; set; }

        public int? UserId { get; set; }
    }
}
