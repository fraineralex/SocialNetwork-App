using SocialNetwork.Core.Application.ViewModels.Comment;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface ICommentsService : IGenericService<CommentViewModel, SaveCommentViewModel>
    {

    }
}
