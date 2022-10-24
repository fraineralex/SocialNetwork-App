using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.ViewModels.Auth;

namespace SocialNetwork.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
            UserViewModel userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");

            if(userViewModel == null)
            {
                return false;
            }

            return true;
        }
    }
}
