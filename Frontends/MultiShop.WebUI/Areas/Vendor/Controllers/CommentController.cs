using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.WebUI.Services.CommentServices;
using Newtonsoft.Json;
using System.Text;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using System.Security.Claims;

namespace MultiShop.WebUI.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles = "Vendor")]
    public class CommentController : Controller
    {
       private readonly ICommentService _commentService;
        private readonly IProductService _productService;

        public CommentController(ICommentService commentService, IProductService productService)
        {
            _commentService = commentService;
            _productService = productService;
        }        
        
        private string GetCurrentVendorId()
        {
            return User.FindFirst("sub")?.Value
                ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        private async Task<bool> IsProductOwner(string productId)
        {
            var product = await _productService.GetByIdProductAsync(productId);

            if (product == null)
                return false;

            return product.VendorId == GetCurrentVendorId();
        }


        public async Task<IActionResult> Index()
        {
            var vendorId = GetCurrentVendorId();

            var vendorProducts = await _productService.GetProductsByVendorIdAsync(vendorId);

            var allComments = new List<ResultCommentDto>();

            foreach (var product in vendorProducts)
            {
                var comments = await _commentService.GetCommentListByProductIdAsync(product.ProductId);

                if (comments != null)
                {
                    allComments.AddRange(comments);
                }
            }

            return View(allComments);
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
            return RedirectToAction("Index", "Comment", new { area = "Vendor" });
           
        }


        public async Task<IActionResult> DeleteComment(string id)
        {
            var comment = await _commentService.GetByIdCommentAsync(id);

            if (comment == null || !await IsProductOwner(comment.ProductId))
            {
                return Forbid();
            }

            await _commentService.DeleteCommentAsync(id);
            return Redirect("/Vendor/Comment/Index");
        }


        [HttpGet]
        public async Task<IActionResult> UpdateComment(string id)
        {
            var values = await _commentService.GetByIdCommentAsync(id);

            if (values == null || !await IsProductOwner(values.ProductId))
            {
                return Forbid();
            }

            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto updateCommentDto)
        {
            if (!await IsProductOwner(updateCommentDto.ProductId))
            {
                return Forbid();
            }

            await _commentService.UpdateCommentAsync(updateCommentDto);
            return Redirect("/Vendor/Comment/Index");
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
