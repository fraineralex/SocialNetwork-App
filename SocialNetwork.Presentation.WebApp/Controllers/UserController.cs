using SocialNetwork.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Middlewares;
using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Controllers
{
    public class UserController : Controller
    {
        private readonly IUsersService _userService;
        private readonly ValidateUserSession _validateUserSession;

        public UserController(IUsersService userService, ValidateUserSession validateUserSession)
        {
            _userService = userService;
            _validateUserSession = validateUserSession;
        }

        public IActionResult Index()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            ViewBag.Page = "login";
            return View("login");
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {

                ViewBag.Page = "login";
                return View("Login", loginViewModel);

            }

            UserViewModel userVm = await _userService.Login(loginViewModel);

            if (userVm != null)
            {
                HttpContext.Session.Set<UserViewModel>("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                ModelState.AddModelError("userVaidation", "Incorrect username or password");
            }

            ViewBag.Page = "login";
            return View("Login", loginViewModel);
        }

        public IActionResult Register()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            ViewBag.Page = "Sing up";
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel saveUserViewModel)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {

                ViewBag.Page = "Sing up";
                return View("Register", saveUserViewModel);

            }

            Users user = await _userService.GetAUserByUsernameAsync(saveUserViewModel.Username);

            if (user == null)
            {
                SaveUserViewModel userVm = await _userService.Add(saveUserViewModel);

                if (userVm != null && userVm.Id != 0)
                {
                    userVm.ProfileImage = UploadImage.UploadFile(saveUserViewModel.FileImage, userVm.Id, "ProfileImage");
                    await _userService.Update(userVm, userVm.Id);

                    return RedirectToRoute(new { controller = "User", action = "Index" });
                }

                ModelState.AddModelError("singUpVaidation", "Something went wrong, try again!");
                ViewBag.Page = "Sing up";
                return View("Register", saveUserViewModel);


            }
            else
            {
                ModelState.AddModelError("userVaidation", "This user name is already occupied by another user!");
                ViewBag.Page = "Sing up";
                return View("Register", saveUserViewModel);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
    }
}
