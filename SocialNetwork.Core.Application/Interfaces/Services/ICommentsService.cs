using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Interfaces.Services
{
    public interface ICommentsService : IGenericService<SaveCommentViewModel, CommentViewModel, Comments>
    {
        Task<List<CommentViewModel>> GetAllViewModelWithInclude();
    }
}
