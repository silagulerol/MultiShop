using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.WebUI.Services.CommentServices;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Controllers
{
    public class ProductListController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICommentService _commentService;

        public ProductListController(IHttpClientFactory httpClientFactory, ICommentService commentService)
        {
            _httpClientFactory = httpClientFactory;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index(string id)
        {
            ViewBag.i = id;
            ViewBag.directory1 = "Home";
            ViewBag.directory2 = "Shop";
            ViewBag.directory3 = "Products";
            return View();
        }
        public async Task<IActionResult> IndexAll()
        {
            ViewBag.directory1 = "Home";
            ViewBag.directory2 = "Shop";
            ViewBag.directory3 = "Products";
            return View();
        }
        public async Task<IActionResult> ProductDetail(string id)
        {
            ViewBag.x = id;
            ViewBag.directory1 = "Home";
            ViewBag.directory2 = "Shop";
            ViewBag.directory3 = "Product Detail";
            return View();
        }

        [HttpGet]
        public PartialViewResult AddComment()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CreateCommentDto createCommentDto)
        {

            //formada eksik kalan bilgiler:
            createCommentDto.CreatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            createCommentDto.Rating = 2;
            createCommentDto.Status = false;
            createCommentDto.ProductId = "69ac232cf828e19d93037994";
            createCommentDto.ImageUrl = "test";

            await _commentService.CreateCommentAsync(createCommentDto);
            return RedirectToAction("Index", "Default");
        }
    }
}
