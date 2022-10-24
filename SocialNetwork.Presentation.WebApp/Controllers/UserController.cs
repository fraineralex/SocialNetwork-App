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
                    userVm.ProfileImagePath = "Test";//UploadFile(saveUserViewModel.FileImage, userVm.Id);
                    await _userService.Update(userVm);
                }
                return RedirectToRoute(new { controller = "User", action = "Index" });
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

        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imageURL = null)
        {
            if (file != null)
            {

                if (isEditMode && file == null)
                {
                    return imageURL;
                }

                //get directory path
                string basePath = $"/Images/{id}/ProfileImage";
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

                //create folder if not exist
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //get file path
                Guid guid = Guid.NewGuid();
                FileInfo fileInfo = new(file.FileName);
                string fileName = fileInfo.Name + fileInfo.Extension;
                string fileNameWhitPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWhitPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                if (isEditMode && imageURL != null)
                {
                    string[] oldImagePath = imageURL.Split("/");
                    string oldImageName = oldImagePath[^1];
                    string completeImageOldPath = Path.Combine(path, oldImageName);

                    if (System.IO.File.Exists(completeImageOldPath))
                    {
                        System.IO.File.Delete(completeImageOldPath);
                    }
                }

                return $"{basePath}/{fileName}";
            }
            else
            {
                if (isEditMode && file == null)
                {
                    return imageURL;
                }

                return null;
            }
        }
    }
}
