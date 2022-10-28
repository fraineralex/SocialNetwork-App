using AutoMapper;
using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Core.Application.ViewModels.Friend;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region "Post profile"
            CreateMap<Posts, PostViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreateBy, opt => opt.Ignore())
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore());

            CreateMap<Posts, SavePostViewModel>()
                .ForMember(x => x.Comments, opt => opt.Ignore())
                .ForMember(x => x.ImageFile, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.CreateBy, opt => opt.Ignore())
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.Users, opt => opt.Ignore())
                .ForMember(x => x.Comments, opt => opt.Ignore());
            #endregion

            #region "Comments profile"
            CreateMap<Comments, CommentViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreateBy, opt => opt.Ignore())
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore());

            CreateMap<Comments, SaveCommentViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreateBy, opt => opt.Ignore())
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.Users, opt => opt.Ignore())
                .ForMember(x => x.Posts, opt => opt.Ignore());
            #endregion

            #region "User profile"
            CreateMap<Users, UserViewModel>()
                .ReverseMap()
                .ForMember(x => x.CreateBy, opt => opt.Ignore())
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore());

            CreateMap<Users, SaveUserViewModel>()
                .ForMember(x => x.ConfirmPassword, opt => opt.Ignore())
                .ForMember(x => x.FileImage, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.CreateBy, opt => opt.Ignore())
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore());
            #endregion

            #region "Friend profile"
            CreateMap<Friends, FriendViewModel>();

            CreateMap<Friends, SaveFriendViewModel>();
            #endregion
        }

    }
}
