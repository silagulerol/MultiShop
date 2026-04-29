using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CommentServices;
using MultiShop.WebUI.Services.StatisticServices.CatalogStatisticServices;
using MultiShop.WebUI.Services.StatisticServices.DiscountStatisticServices;
using MultiShop.WebUI.Services.StatisticServices.MessageStatisticServices;
using MultiShop.WebUI.Services.StatisticServices.UserStatisticServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatisticController : Controller
    {
        private readonly ICatalogStatisticService _catalogStatisticService;
        private readonly IUserStatisticService _userStatisticService;
        private readonly ICommentService _commentService;
        private readonly IDiscountStatisticService _discountStatisticService;

        public StatisticController(ICatalogStatisticService catalogStatisticService, IUserStatisticService userStatisticService, ICommentService commentService, IDiscountStatisticService discountStatisticService)
        {
            _catalogStatisticService = catalogStatisticService;
            _userStatisticService = userStatisticService;
            _commentService = commentService;
            _discountStatisticService = discountStatisticService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.getBrandCount = await _catalogStatisticService.GetBrandCount();
            ViewBag.getProductCount = await _catalogStatisticService.GetProductCount();
            ViewBag.getCategoryCount = await _catalogStatisticService.GetCategoryCount();
            ViewBag.getMaxPriceProductName = await _catalogStatisticService.GetMaxPriceProductName();
            ViewBag.getMinPriceProductName = await _catalogStatisticService.GetMinPriceProductName();

            ViewBag.getUserCount = await _userStatisticService.GetUsercount();
            //ViewBag.getTotalCommentCount = await _commentService.GetTotalCommentCount();
            //ViewBag.getActiveCommentCount = await _commentService.GetActiveCommentCount();
            //ViewBag.getPassiveCommentCount = await _commentService.GetPassiveCommentCount();
            //ViewBag.getDiscountCouponCount = await _discountStatisticService.GetDiscountCouponCount();

            return View();
        }
    }
}
