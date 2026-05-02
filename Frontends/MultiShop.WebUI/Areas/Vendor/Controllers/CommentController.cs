using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.WebUI.Services.CommentServices;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService, IHttpClientFactory httpClientFactory)
        {
            _commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            var values= await _commentService.GetAllCommentAsync();
            return View(values);
        }

        [HttpGet]
        public async Task<IActionResult> CreateComment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDto createCommentDto)
        {
            await _commentService.CreateCommentAsync(createCommentDto);
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
           
        }


        public async Task<IActionResult> DeleteComment(string id)
        {
            await _commentService.DeleteCommentAsync(id);
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
            
        }


        [HttpGet]
        public async Task<IActionResult> UpdateComment(string id)
        {
            var values = await _commentService.GetByIdCommentAsync(id);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto updateCommentDto)
        {
            await _commentService.UpdateCommentAsync(updateCommentDto);
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
        }

        void CommentViewBag()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Comments";
            ViewBag.v3 = "Adding New Comment";
            ViewBag.v0 = "Comment Operation";
        }

    }
}
