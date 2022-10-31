using SocialNetwork.Core.Domain.Common;

namespace SocialNetwork.Core.Domain.Entities
{
    public class Posts : AuditableBaseEntity
    {
        public string? Content { get; set; }
        public string? ImagePath { get; set; }

        //Foreign Key
        public int UserId { get; set; }

        //Navigation property
        public ICollection<Comments>? Comments { get; set; }
        public Users? Users { get; set; }
    }
}
