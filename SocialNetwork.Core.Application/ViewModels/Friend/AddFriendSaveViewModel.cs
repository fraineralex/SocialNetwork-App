using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Core.Application.ViewModels.Friend
{
    public class AddFriendSaveViewModel
    {
        [Required(ErrorMessage = "You must enter the username of your friend")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
    }
}
