
namespace SocialNetwork.Core.Application.Friends
{
    public class SaveFriendViewModel
    {
        public string? Id { get; set; }

        public int? SenderId { get; set; }

        public int? ReceptorId { get; set; }

        public bool? IsAccepted { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
