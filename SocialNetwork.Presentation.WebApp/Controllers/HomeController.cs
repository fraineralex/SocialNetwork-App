using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Middlewares;
using SocialNetwork.Presentation.WebApp.Models;
using System.Diagnostics;

namespace SocialNetwork.Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostsService _postService;
        private readonly ValidateUserSession _validateUserSession;

        public HomeController(IPostsService postService, ValidateUserSession validateUserSession)
        {
            _postService = postService;
            _validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            ViewBag.Page = "home";
            return View(await _postService.GetAllViewModelWithInclude());
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}