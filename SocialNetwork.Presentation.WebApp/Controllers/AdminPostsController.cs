﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Core.Application.Helpers;
using SocialNetwork.Core.Application.Interfaces.Services;
using SocialNetwork.Core.Application.ViewModels.Post;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Middlewares;

namespace SocialNetwork.Presentation.WebApp.Controllers
{
    public class AdminPostsController : Controller
    {
        private readonly IPostsService _postService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IMapper _mapper;

        public AdminPostsController(IPostsService postService, ValidateUserSession validateUserSession, IMapper mapper)
        {
            _postService = postService;
            _validateUserSession = validateUserSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Create()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            ViewBag.Page = "Create post";
            return View("SavePost");
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePostViewModel SaveViewModel)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View("SavePost", SaveViewModel);
            }

            SavePostViewModel postVm = await _postService.Add(SaveViewModel);

            if (postVm != null && SaveViewModel.ImageFile != null)
            {
                postVm.ImagePath = UploadImage.UploadFile(SaveViewModel.ImageFile, postVm.Id);
                await _postService.Update(postVm, postVm.Id);
            }

            return RedirectToRoute(new { controller = "home", action = "Index" });
            
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost(String Content)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            SavePostViewModel postViewModel = new()
            {
                Content = Content
            };


            SavePostViewModel postVm = await _postService.Add(postViewModel);

            return RedirectToRoute(new { controller = "home", action = "Index" });

        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            ViewBag.Page = "home";
            SavePostViewModel savepostViewModel = await _postService.GetSaveViewModelById(id);
            return View("SaveAd", savepostViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePostViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View("SavePost", vm);
            }

            if(vm.ImagePath != null)
            {
                SavePostViewModel postVm = await _postService.GetSaveViewModelById(vm.Id);
                vm.ImagePath = UploadImage.UploadFile(vm.ImageFile, vm.Id, "Posts", true, postVm.ImagePath);
            }

            await _postService.Update(vm, vm.Id);

            return RedirectToRoute(new { controller = "home", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            ViewBag.Page = "home";
            return View("DeletePost", await _postService.GetSaveViewModelById(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

            await _postService.Delete(id);

            //get directory path
            string basePath = $"/Images/{id}/Posts";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                foreach(FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo folder in directoryInfo.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }

            return RedirectToRoute(new { controller = "home", action = "Index" });
        }
        
    }
}