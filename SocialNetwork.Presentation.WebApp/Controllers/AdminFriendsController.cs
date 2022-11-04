using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.Services;
using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Application.ViewModels.Friend;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Middlewares;

namespace SocialNetwork.Presentation.WebApp.Controllers
{
    public class AdminFriendsController : Controller
    {
        private readonly IFriendsService _friendsService;
        private readonly IUsersService _userService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;

        public AdminFriendsController(IFriendsService friendsService, ValidateUserSession validateUserSession, IMapper mapper, IUsersService usersService, IHttpContextAccessor httpContextAccessor)
        {
            _friendsService = friendsService;
            _userService = usersService;
            _validateUserSession = validateUserSession;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }
            ViewBag.Page = "friend";
            return View("Friends", await _friendsService.GetAllViewModelWithInclude());
        }

        public async Task<IActionResult> AddFriend()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            ViewBag.Page = "friend";
            return View("AddFriend");
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(AddFriendSaveViewModel AddFriendVm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            if (!ModelState.IsValid)
            {

                ViewBag.Page = "friend";
                return View("AddFriend", AddFriendVm);

            }

            SaveUserViewModel user = await _userService.GetAUserByUsernameAsync(AddFriendVm.Username);

            if (user != null)
            {
                if (user.Username == userViewModel.Username)
                {
                    ModelState.AddModelError("userVaidation", "you can't add your self in your friend list, try another user.");
                    ViewBag.Page = "friend";
                    return View("AddFriend", AddFriendVm);
                }

                if (user.IsActive == false)
                {
                    ModelState.AddModelError("userVaidation", "This account is desatived, try another user.");
                    ViewBag.Page = "friend";
                    return View("AddFriend", AddFriendVm);
                }
                bool areFriend = await _friendsService.CheckIfAreFriend(userViewModel.Id, user.Id);

                if (areFriend)
                {
                    ModelState.AddModelError("userVaidation", "This user is your friend yet.");
                    ViewBag.Page = "friend";
                    return View("AddFriend", AddFriendVm);
                }

                SaveFriendViewModel SaveFriendVm = new()
                {
                    SenderId = userViewModel.Id,
                    ReceptorId = user.Id,
                    CreatedAt = DateTime.Now,
                    IsAccepted = true

                };

                SaveFriendViewModel friendViewModel = await _friendsService.Add(SaveFriendVm);

                return RedirectToRoute(new { controller = "AdminFriends", action = "Index" });
            }
            else
            {
                ModelState.AddModelError("userVaidation", "Don't exist a user with this username, try again.");
                ViewBag.Page = "friend";
                return View("AddFriend", AddFriendVm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFriend(int receptorId)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            var friend = await _friendsService.GetFriendByReceptor(receptorId);

            if(friend != null)
            {
                await _friendsService.Delete(friend.Id);
            }

            return RedirectToRoute(new { controller = "AdminFriends", action = "Index" });

        }
    }
}
