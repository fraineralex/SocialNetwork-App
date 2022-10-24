using SocialNetwork.Core.Domain.Common;

namespace SocialNetwork.Core.Domain.Entities
{
    public class Comments : AuditableBaseEntity
    {
        public string? Content { get; set; }

        //Foreign key
        public int? PostId { get; set; }
        public int? UserId { get; set; }

        //Navigation property
        public Users? Users { get; set; }
        public Posts? Posts { get; set; }
    }
}
