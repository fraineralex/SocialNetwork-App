
namespace SocialNetwork.Core.Domain.Entities
{
    public class Friends
    {
        public int Id { get; set; }

        //Foreign key
        public int SenderId { get; set; }

        public int ReceptorId { get; set; }

        public bool? IsAccepted { get; set; }

        public DateTime? CreatedAt { get; set; }

        //Navigation property
        public Users? Users { get; set; }
    }
}
