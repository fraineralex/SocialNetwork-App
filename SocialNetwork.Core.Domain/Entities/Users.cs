
namespace SocialNetwork.Core.Domain.Entities
{
    public class Users
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImage { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public bool? IsActive { get; set; }

        //Navigation property
        public ICollection<Posts>? Posts { get; set; }
        public ICollection<Comments>? Comments { get; set; }
        public ICollection<Friends>? Friends { get; set; }


    }
}
