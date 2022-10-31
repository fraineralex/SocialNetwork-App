﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Comment;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Middlewares;

namespace SocialNetwork.Presentation.WebApp.Controllers
{
    public class FriendsController : Controller
    {
        private readonly IFriendsService _friendsService;
        private readonly IUsersService _userService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IMapper _mapper;

        public FriendsController(IFriendsService friendsService, ValidateUserSession validateUserSession, IMapper mapper, IUsersService usersService)
        {
            _friendsService = friendsService;
            _userService = usersService;
            _validateUserSession = validateUserSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            ViewBag.Page = "home";
            return View("AdminFriends/Index", await _userService.GetAllViewModelWithInclude());
        }

    }
}
