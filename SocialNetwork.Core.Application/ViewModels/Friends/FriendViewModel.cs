﻿
namespace SocialNetwork.Core.Application.Friends
{
    public class FriendViewModel
    {
        public string? Id { get; set; }

        public string? SenderUsername { get; set; }

        public string? SenderProfileImage { get; set; }

        public bool? IsAccepted { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
