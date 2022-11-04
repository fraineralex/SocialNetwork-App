using SocialNetwork.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Middlewares;
using SocialNetwork.Core.Application.ViewModels.Auth;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Presentation.WebApp.Models;
using System.Diagnostics;
using SocialNetwork.Core.Application;
using SocialNetwork.Core.Application.Dtos.Email;

namespace SocialNetwork.Controllers
{
    public class UserController : Controller
    {
        private readonly IUsersService _userService;
        private readonly IEmailService _emailService;
        private readonly ValidateUserSession _validateUserSession;

        public UserController(IUsersService userService, ValidateUserSession validateUserSession, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
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
                if (userVm.IsActive)
                {
                    HttpContext.Session.Set<UserViewModel>("user", userVm);
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                }

                ModelState.AddModelError("userVaidation", "Account deactivated, check your email to activate it.");
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

            SaveUserViewModel user = await _userService.GetAUserByUsernameAsync(saveUserViewModel.Username);

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

        public async Task<IActionResult> ActiveAccount(string username)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (username == null)
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            else
            {
                SaveUserViewModel user = await _userService.GetAUserByUsernameAsync(username);

                if (user != null)
                {
                    user.IsActive = true;
                    await _userService.Update(user, user.Id);

                    ModelState.TryAddModelError("ActiveSuccess", $"✅ Your account has been successfully activated");
                    ViewBag.Page = "login";
                    return View("login");

                }
                else
                {
                    ModelState.TryAddModelError("ActiveError", $"❌ No account was found for the user '{username}'");
                    ViewBag.Page = "login";
                    return View("login");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestorePassword(string Username)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (Username == null)
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            
            
            SaveUserViewModel user = await _userService.GetAUserByUsernameAsync(Username);

            if (user != null)
            {
                string newPassword = $"{user.Username}#{user.Id}".ToLower();
                user.Password = PasswordEncryption.ComputeSha256Hash(newPassword);
                await _userService.Update(user, user.Id);

                await _emailService.SendAsync(new EmailRequest
                {
                    To = user.Email,
                    Subject = "Your Social Network App password has been updated!\r\n",
                    Body = $"<h1>Welcome back to Social Network App 👨🏻‍🚀</h1>" +
                    $"<p>Hi {user.Name} {user.LastName},</p>" +
                    $"<p>Your password has been updated successfully. Your new password is <strong>{newPassword}</strong></p>" +
                    $"<p>Please try to don't forget your password again and log back in to Social Network App</p>" +
                    $"<p>Click the following link to redirect you to the Login page again 👉🏻 <a style='color: black; text-decoration-line: none;' href='http://localhost:7050/'> <strong> LOGIN 🏠</strong></a></p>"

                });

                ModelState.TryAddModelError("ActiveSuccess", $"✅ Your password has been successfully updated, check your email to get it.");
                ViewBag.Page = "login";
                return View("login");

            }
            else
            {
                ModelState.TryAddModelError("ActiveError", $"❌ No account was found for the user '{Username}'");
                ViewBag.Page = "login";
                return View("login");
            }
            
        }
    }
}
