using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MultiShop.DtoLayer.CommentDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.FavoriteServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.CommentServices;
using System.Security.Claims;

namespace MultiShop.WebUI.Controllers
{
    public class ProductListController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICommentService _commentService;
        private readonly IProductService _productService;
        private readonly IFavoriteService _favoriteService;

        public ProductListController(IHttpClientFactory httpClientFactory, ICommentService commentService, IProductService productService, IFavoriteService favoriteService)
        {
            _httpClientFactory = httpClientFactory;
            _commentService = commentService;
            _productService = productService;
            _favoriteService = favoriteService;
        }

        public async Task<IActionResult> Index(string id)
        {
            var filterRequest = BuildProductFilterRequest(id);
            var filterResponse = await _productService.FilterProductsAsync(filterRequest);

            ViewBag.i = id;
            ViewBag.FilterRequest = filterRequest;
            ViewBag.FilterResponse = filterResponse;
            ViewBag.FavoriteProductIds = await GetFavoriteProductIdsAsync();
            ViewBag.directory1 = "Home";
            ViewBag.directory2 = "Shop";
            ViewBag.directory3 = string.IsNullOrWhiteSpace(filterResponse.CategoryName) ? "Products" : filterResponse.CategoryName;
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
            createCommentDto.CreatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            createCommentDto.Rating = 2;
            createCommentDto.Status = false;
            createCommentDto.ProductId = "69ac232cf828e19d93037994";
            createCommentDto.ImageUrl = "test";

            await _commentService.CreateCommentAsync(createCommentDto);
            return RedirectToAction("Index", "Default");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string search)
        {
            ViewBag.SearchKey = search;
            ViewBag.FavoriteProductIds = await GetFavoriteProductIdsAsync();

            if (string.IsNullOrWhiteSpace(search))
            {
                return View(new List<ResultProductDto>());
            }
            
            var values = await _productService.SearchProductAsync(search);

            return View(values);
        }

        private async Task<HashSet<string>> GetFavoriteProductIdsAsync()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return new HashSet<string>();
            }

            var userId = User.FindFirstValue("sub")
                         ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return new HashSet<string>();
            }

            var favorites = await _favoriteService.GetFavoritesByUserIdAsync(userId);
            return favorites.Select(x => x.ProductId).ToHashSet();
        }

        private ProductFilterRequestDto BuildProductFilterRequest(string categoryId)
        {
            var query = Request.Query;

            return new ProductFilterRequestDto
            {
                CategoryId = categoryId,
                Brands = GetQueryValues("brand", "brands"),
                MinPrice = GetDecimal("minPrice"),
                MaxPrice = GetDecimal("maxPrice"),
                Colors = GetQueryValues("color", "colors"),
                Sizes = GetQueryValues("size", "sizes"),
                Materials = GetQueryValues("material", "materials"),
                Capacities = GetQueryValues("capacity", "capacities", "storage"),
                Styles = GetQueryValues("style", "styles"),
                Rating = GetDouble("rating"),
                SortBy = GetString("sort", "sortBy"),
                Page = GetInt("page") ?? 1,
                PageSize = GetInt("pageSize") ?? 24,
                IsFreeShipping = GetBool("freeShipping", "isFreeShipping"),
                IsBestSeller = GetBool("bestSeller", "isBestSeller"),
                IsFeatured = GetBool("featured", "isFeatured"),
                IsRecyclable = GetBool("recyclable", "isRecyclable"),
                IsWomenEntrepreneur = GetBool("womenEntrepreneur", "isWomenEntrepreneur"),
                InStock = GetBool("inStock")
            };

            List<string> GetQueryValues(params string[] keys)
            {
                return keys
                    .Where(key => query.ContainsKey(key))
                    .SelectMany(key => query[key].SelectMany(SplitQueryValues))
                    .Where(value => !string.IsNullOrWhiteSpace(value))
                    .Select(value => value.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }

            string? GetString(params string[] keys)
            {
                return keys
                    .Where(key => query.ContainsKey(key))
                    .Select(key => query[key].FirstOrDefault())
                    .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
            }

            decimal? GetDecimal(string key)
            {
                var value = GetString(key);
                return decimal.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var parsed)
                    ? parsed
                    : null;
            }

            double? GetDouble(string key)
            {
                var value = GetString(key);
                return double.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var parsed)
                    ? parsed
                    : null;
            }

            int? GetInt(string key)
            {
                var value = GetString(key);
                return int.TryParse(value, out var parsed) ? parsed : null;
            }

            bool? GetBool(params string[] keys)
            {
                var value = GetString(keys);
                if (value is null)
                {
                    return null;
                }

                return value.Equals("true", StringComparison.OrdinalIgnoreCase)
                    || value.Equals("on", StringComparison.OrdinalIgnoreCase)
                    || value.Equals("1", StringComparison.OrdinalIgnoreCase);
            }
        }

        private static IEnumerable<string> SplitQueryValues(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? Enumerable.Empty<string>()
                : value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
    }
}
