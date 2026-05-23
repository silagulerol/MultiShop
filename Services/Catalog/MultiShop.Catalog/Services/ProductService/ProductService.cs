using AutoMapper;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using MultiShop.Catalog.Dtos.ProductDtos;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Brand> _brandCollection;
        private readonly IMongoCollection<ProductVariant> _productVariantCollection;
        private readonly IMapper _mapper;

        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _brandCollection = database.GetCollection<Brand>(databaseSettings.BrandCollectionName);
            _productVariantCollection = database.GetCollection<ProductVariant>(databaseSettings.ProductVariantCollectionName);
            _mapper = mapper;
        }

        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            await _productCollection.InsertOneAsync(_mapper.Map<Product>(createProductDto));
        }

        public async Task DeleteProductAsync(string id)
        {
            await _productCollection.DeleteOneAsync(x => x.ProductId == id);
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            var values = await _productCollection
                .Find(_ => true)
                .SortByDescending(x => x.IsActive)
                .ThenByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.IsBestSeller)
                .ThenBy(x => x.StartingPrice)
                .ToListAsync();

            await ResolveBrandsAsync(values);

            return _mapper.Map<List<ResultProductDto>>(values);
        }

        public async Task<GetByIdProductDto> GetByIdProductAsync(string id)
        {
            var value = await _productCollection.Find(x => x.ProductId == id).FirstOrDefaultAsync();
            await ResolveBrandAsync(value);
            return _mapper.Map<GetByIdProductDto>(value);

        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var value = _mapper.Map<Product>(updateProductDto);
            await _productCollection.ReplaceOneAsync(x => x.ProductId == value.ProductId, value);
        }

        public async Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            var values = await _productCollection
                .Find(_ => true)
                .SortByDescending(x => x.IsActive)
                .ThenByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.IsBestSeller)
                .ThenBy(x => x.StartingPrice)
                .ToListAsync();

            await ResolveCategoriesAsync(values);
            await ResolveBrandsAsync(values);

            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }

        public async Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryByCategoryIdAsync(string CategoryId)
        {
            var values = await _productCollection
                .Find(x => x.CategoryId == CategoryId)
                .SortByDescending(x => x.IsActive)
                .ThenByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.IsBestSeller)
                .ThenBy(x => x.StartingPrice)
                .ToListAsync();

            await ResolveCategoriesAsync(values);
            await ResolveBrandsAsync(values);
            
            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }

        public async Task<List<ResultProductDto>> GetProductsByVendorIdAsync(string vendorId)
        {
            var values = await _productCollection
                .Find(x => x.VendorId == vendorId)
                .SortByDescending(x => x.IsActive)
                .ThenByDescending(x => x.IsFeatured)
                .ThenBy(x => x.StartingPrice)
                .ToListAsync();

            await ResolveBrandsAsync(values);

            return _mapper.Map<List<ResultProductDto>>(values);
        }

        public async Task<List<ResultProductsWithCategoryDto>> GetProductsWithCategoryByVendorIdAsync(string vendorId)
        {
            var values = await _productCollection
                .Find(x => x.VendorId == vendorId)
                .SortByDescending(x => x.IsActive)
                .ThenByDescending(x => x.IsFeatured)
                .ThenBy(x => x.StartingPrice)
                .ToListAsync();

            await ResolveCategoriesAsync(values);
            await ResolveBrandsAsync(values);

            return _mapper.Map<List<ResultProductsWithCategoryDto>>(values);
        }

        public async Task<List<ResultProductDto>> SearchProductAsync(string? searchKey)
        {
            var normalizedQuery = NormalizeText(searchKey);
            var tokens = Tokenize(normalizedQuery);

            if (!tokens.Any())
            {
                return new List<ResultProductDto>();
            }

            var tokenRegexes = tokens
                .Select(x => new BsonRegularExpression(Regex.Escape(x), "i"))
                .ToList();
            var phraseRegex = new BsonRegularExpression(Regex.Escape(normalizedQuery), "i");

            var brandFilters = tokenRegexes
                .Select(regex => Builders<Brand>.Filter.Regex(x => x.BrandName, regex))
                .ToList();
            var matchingBrands = await _brandCollection
                .Find(Builders<Brand>.Filter.Or(brandFilters))
                .ToListAsync();
            var matchingBrandIds = matchingBrands
                .Select(x => x.BrandId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            var categoryFilters = tokenRegexes
                .Select(regex => Builders<Category>.Filter.Regex(x => x.Name, regex))
                .ToList();
            var matchingCategories = await _categoryCollection
                .Find(Builders<Category>.Filter.Or(categoryFilters))
                .ToListAsync();
            var matchingCategoryIds = matchingCategories
                .Select(x => x.CategoryId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            var variantFilters = tokenRegexes
                .SelectMany(regex => new[]
                {
                    Builders<ProductVariant>.Filter.Regex(x => x.VariantName, regex),
                    Builders<ProductVariant>.Filter.Regex(x => x.Size, regex),
                    Builders<ProductVariant>.Filter.Regex(x => x.Color, regex),
                    Builders<ProductVariant>.Filter.Regex(x => x.MaterialOption, regex),
                    Builders<ProductVariant>.Filter.Regex(x => x.CapacityOption, regex),
                    Builders<ProductVariant>.Filter.Regex(x => x.StyleOption, regex),
                    Builders<ProductVariant>.Filter.Regex(x => x.Sku, regex)
                })
                .ToList();
            var variantFilter = Builders<ProductVariant>.Filter.Or(variantFilters);
            var matchingVariants = await _productVariantCollection
                    .Find(variantFilter)
                    .ToListAsync();
            var matchingVariantProductIds = matchingVariants
                .Select(x => x.ProductId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            var productTokenFilters = tokenRegexes
                .SelectMany(regex => new[]
                {
                    Builders<Product>.Filter.Regex(x => x.ProductName, regex),
                    Builders<Product>.Filter.Regex(x => x.Description, regex),
                    Builders<Product>.Filter.Regex(x => x.Slug, regex),
                    Builders<Product>.Filter.Regex(x => x.BadgeText, regex)
                })
                .ToList();

            var candidateFilters = new List<FilterDefinition<Product>>();
            candidateFilters.AddRange(productTokenFilters);
            candidateFilters.Add(Builders<Product>.Filter.Regex(x => x.ProductName, phraseRegex));
            candidateFilters.Add(Builders<Product>.Filter.Regex(x => x.Slug, phraseRegex));

            if (matchingBrandIds.Any())
            {
                candidateFilters.Add(Builders<Product>.Filter.In(x => x.BrandId, matchingBrandIds));
            }

            if (matchingCategoryIds.Any())
            {
                candidateFilters.Add(Builders<Product>.Filter.In(x => x.CategoryId, matchingCategoryIds));
            }

            if (matchingVariantProductIds.Any())
            {
                candidateFilters.Add(Builders<Product>.Filter.In(x => x.ProductId, matchingVariantProductIds));
            }

            if (tokens.Contains("bestseller") || tokens.Contains("best") && tokens.Contains("seller"))
            {
                candidateFilters.Add(Builders<Product>.Filter.Eq(x => x.IsBestSeller, true));
            }

            if (tokens.Contains("recycled") || tokens.Contains("recyclable"))
            {
                candidateFilters.Add(Builders<Product>.Filter.Eq(x => x.IsRecyclableProduct, true));
            }

            var candidateFilter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq(x => x.IsActive, true),
                Builders<Product>.Filter.Or(candidateFilters)
            );

            var candidateProducts = await _productCollection
                .Find(candidateFilter)
                .SortByDescending(x => x.IsFeatured)
                .ThenByDescending(x => x.IsBestSeller)
                .ThenByDescending(x => x.AverageRating)
                .ThenByDescending(x => x.ReviewCount)
                .ThenBy(x => x.StartingPrice)
                .ToListAsync();

            await ResolveBrandsAsync(candidateProducts);
            await ResolveCategoriesAsync(candidateProducts);

            var candidateProductIds = candidateProducts.Select(x => x.ProductId).ToList();
            var variantsByProductId = await GetVariantsByProductIdsAsync(candidateProductIds);
            var minimumScore = tokens.Count == 1 ? 10 : 25;

            Console.WriteLine($"SearchKey: {searchKey}");
            Console.WriteLine($"Tokens: {string.Join(", ", tokens)}");
            Console.WriteLine($"Matched Brands: {matchingBrandIds.Count}");
            Console.WriteLine($"Matched Categories: {matchingCategoryIds.Count}");
            Console.WriteLine($"Matched Variant ProductIds: {matchingVariantProductIds.Count}");
            Console.WriteLine($"Candidate Count: {candidateProducts.Count}");

            var scoredProducts = candidateProducts
                .Select(product =>
                {
                    var searchScore = CalculateSearchScore(
                        product,
                        variantsByProductId.TryGetValue(product.ProductId, out var variants)
                            ? variants
                            : new List<ProductVariant>(),
                        normalizedQuery,
                        tokens);

                    Console.WriteLine($"{product.ProductName} => score: {searchScore.Score}, matched tokens: {string.Join(", ", searchScore.MatchedTokens)}");

                    return new
                    {
                        Product = product,
                        searchScore.Score,
                        searchScore.MatchedTokenCount
                    };
                })
                .Where(x => x.Score >= minimumScore && HasRequiredTokenCoverage(x.MatchedTokenCount, tokens.Count))
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.Product.AverageRating)
                .ThenByDescending(x => x.Product.ReviewCount)
                .Select(x => x.Product)
                .ToList();

            Console.WriteLine($"Result Count: {scoredProducts.Count}");

            return _mapper.Map<List<ResultProductDto>>(scoredProducts);
        }

        public async Task<ProductFilterResponseDto> FilterProductsAsync(ProductFilterRequestDto request)
        {
            request ??= new ProductFilterRequestDto();
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 ? 24 : Math.Min(request.PageSize, 60);

            var filters = new List<FilterDefinition<Product>>
            {
                Builders<Product>.Filter.Eq(x => x.IsActive, true)
            };

            if (!string.IsNullOrWhiteSpace(request.CategoryId))
            {
                filters.Add(Builders<Product>.Filter.Eq(x => x.CategoryId, request.CategoryId));
            }

            var brandIds = NormalizeList(request.Brands);
            if (brandIds.Any())
            {
                filters.Add(Builders<Product>.Filter.In(x => x.BrandId, brandIds));
            }

            AddPriceFilter(filters, request.MinPrice, request.MaxPrice);
            AddFlagFilter(filters, request.IsFreeShipping, x => x.IsFreeShipping);
            AddFlagFilter(filters, request.IsBestSeller, x => x.IsBestSeller);
            AddFlagFilter(filters, request.IsFeatured, x => x.IsFeatured);
            AddFlagFilter(filters, request.IsRecyclable, x => x.IsRecyclableProduct);
            AddFlagFilter(filters, request.IsWomenEntrepreneur, x => x.IsWomenEntrepreneurProduct);

            if (request.Rating.HasValue)
            {
                filters.Add(Builders<Product>.Filter.Gte(x => x.AverageRating, request.Rating.Value));
            }

            var variantProductIds = await ResolveFilteredVariantProductIdsAsync(request);
            if (variantProductIds is not null)
            {
                if (!variantProductIds.Any())
                {
                    return new ProductFilterResponseDto
                    {
                        Page = page,
                        PageSize = pageSize,
                        Products = new List<ResultProductsWithCategoryDto>()
                    };
                }

                filters.Add(Builders<Product>.Filter.In(x => x.ProductId, variantProductIds));
            }

            var productFilter = Builders<Product>.Filter.And(filters);
            var allMatchingProducts = await _productCollection
                .Find(productFilter)
                .ToListAsync();

            await ResolveCategoriesAsync(allMatchingProducts);
            await ResolveBrandsAsync(allMatchingProducts);

            var sortedProducts = ApplyProductSorting(allMatchingProducts, request.SortBy).ToList();
            var pagedProducts = sortedProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var optionProducts = await GetOptionScopeProductsAsync(request.CategoryId);
            await ResolveCategoriesAsync(optionProducts);
            await ResolveBrandsAsync(optionProducts);
            var optionProductIds = optionProducts.Select(x => x.ProductId).ToList();
            var optionVariants = await _productVariantCollection
                .Find(Builders<ProductVariant>.Filter.In(x => x.ProductId, optionProductIds))
                .ToListAsync();

            var response = new ProductFilterResponseDto
            {
                Products = _mapper.Map<List<ResultProductsWithCategoryDto>>(pagedProducts),
                Brands = BuildBrandOptions(optionProducts),
                Colors = BuildVariantOptions(optionVariants, x => x.Color),
                Sizes = BuildVariantOptions(optionVariants, x => x.Size),
                Materials = BuildVariantOptions(optionVariants, x => x.MaterialOption),
                Capacities = BuildVariantOptions(optionVariants, x => x.CapacityOption),
                Styles = BuildVariantOptions(optionVariants, x => x.StyleOption),
                CategoryName = optionProducts.FirstOrDefault()?.Category?.Name,
                TotalCount = sortedProducts.Count,
                Page = page,
                PageSize = pageSize
            };

            return response;
        }

        private static string NormalizeText(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var lowerValue = value.Trim().ToLowerInvariant();
            var withoutPunctuation = Regex.Replace(lowerValue, @"[^\p{L}\p{Nd}\s]+", " ");
            return Regex.Replace(withoutPunctuation, @"\s+", " ").Trim();
        }

        private static List<string> Tokenize(string normalizedValue)
        {
            if (string.IsNullOrWhiteSpace(normalizedValue))
            {
                return new List<string>();
            }

            return normalizedValue
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x.Length >= 2)
                .Distinct()
                .ToList();
        }

        private static bool ContainsToken(string? value, string token)
        {
            return NormalizeText(value).Contains(token, StringComparison.OrdinalIgnoreCase);
        }

        private static bool HasRequiredTokenCoverage(int matchedTokenCount, int tokenCount)
        {
            if (tokenCount <= 1)
            {
                return matchedTokenCount >= 1;
            }

            return matchedTokenCount == tokenCount;
        }

        private static List<string> NormalizeList(IEnumerable<string>? values)
        {
            return values?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList() ?? new List<string>();
        }

        private static void AddFlagFilter(
            List<FilterDefinition<Product>> filters,
            bool? value,
            System.Linq.Expressions.Expression<Func<Product, bool>> field)
        {
            if (value == true)
            {
                filters.Add(Builders<Product>.Filter.Eq(field, true));
            }
        }

        private static void AddPriceFilter(List<FilterDefinition<Product>> filters, decimal? minPrice, decimal? maxPrice)
        {
            if (!minPrice.HasValue && !maxPrice.HasValue)
            {
                return;
            }

            var discountedFilters = new List<FilterDefinition<Product>>();
            var priceFilters = new List<FilterDefinition<Product>>();

            if (minPrice.HasValue)
            {
                discountedFilters.Add(Builders<Product>.Filter.Gte(x => x.StartingDiscountedPrice, minPrice.Value));
                priceFilters.Add(Builders<Product>.Filter.Gte(x => x.StartingPrice, minPrice.Value));
            }

            if (maxPrice.HasValue)
            {
                discountedFilters.Add(Builders<Product>.Filter.Lte(x => x.StartingDiscountedPrice, maxPrice.Value));
                priceFilters.Add(Builders<Product>.Filter.Lte(x => x.StartingPrice, maxPrice.Value));
            }

            filters.Add(Builders<Product>.Filter.Or(
                Builders<Product>.Filter.And(discountedFilters),
                Builders<Product>.Filter.And(priceFilters)));
        }

        private async Task<List<string>?> ResolveFilteredVariantProductIdsAsync(ProductFilterRequestDto request)
        {
            var variantFilters = new List<FilterDefinition<ProductVariant>>();

            AddVariantInFilter(variantFilters, request.Colors, x => x.Color);
            AddVariantInFilter(variantFilters, request.Sizes, x => x.Size);
            AddVariantInFilter(variantFilters, request.Materials, x => x.MaterialOption);
            AddVariantInFilter(variantFilters, request.Capacities, x => x.CapacityOption);
            AddVariantInFilter(variantFilters, request.Styles, x => x.StyleOption);

            if (request.InStock == true)
            {
                variantFilters.Add(Builders<ProductVariant>.Filter.Gt(x => x.Stock, 0));
                variantFilters.Add(Builders<ProductVariant>.Filter.Eq(x => x.IsAvailable, true));
            }

            if (!variantFilters.Any())
            {
                return null;
            }

            var variants = await _productVariantCollection
                .Find(Builders<ProductVariant>.Filter.And(variantFilters))
                .ToListAsync();

            return variants
                .Select(x => x.ProductId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();
        }

        private static void AddVariantInFilter(
            List<FilterDefinition<ProductVariant>> filters,
            IEnumerable<string>? values,
            System.Linq.Expressions.Expression<Func<ProductVariant, string?>> field)
        {
            var normalizedValues = NormalizeList(values);
            if (normalizedValues.Any())
            {
                filters.Add(Builders<ProductVariant>.Filter.In(field, normalizedValues));
            }
        }

        private static IEnumerable<Product> ApplyProductSorting(IEnumerable<Product> products, string? sortBy)
        {
            return (sortBy ?? string.Empty).Trim().ToLowerInvariant() switch
            {
                "price_asc" => products.OrderBy(GetEffectivePrice).ThenByDescending(x => x.AverageRating),
                "price_desc" => products.OrderByDescending(GetEffectivePrice).ThenByDescending(x => x.AverageRating),
                "newest" => products.OrderByDescending(x => x.CreatedDate),
                "popular" => products.OrderByDescending(x => x.FavoriteCount).ThenByDescending(x => x.ViewCount),
                "rating" => products.OrderByDescending(x => x.AverageRating).ThenByDescending(x => x.ReviewCount),
                _ => products.OrderByDescending(x => x.IsFeatured)
                    .ThenByDescending(x => x.IsBestSeller)
                    .ThenByDescending(x => x.AverageRating)
                    .ThenBy(x => GetEffectivePrice(x))
            };
        }

        private static decimal GetEffectivePrice(Product product)
        {
            return product.StartingDiscountedPrice.HasValue && product.StartingDiscountedPrice.Value < product.StartingPrice
                ? product.StartingDiscountedPrice.Value
                : product.StartingPrice;
        }

        private async Task<List<Product>> GetOptionScopeProductsAsync(string? categoryId)
        {
            var filters = new List<FilterDefinition<Product>>
            {
                Builders<Product>.Filter.Eq(x => x.IsActive, true)
            };

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                filters.Add(Builders<Product>.Filter.Eq(x => x.CategoryId, categoryId));
            }

            return await _productCollection
                .Find(Builders<Product>.Filter.And(filters))
                .ToListAsync();
        }

        private static List<ProductFilterOptionDto> BuildBrandOptions(IEnumerable<Product> products)
        {
            return products
                .Where(x => !string.IsNullOrWhiteSpace(x.BrandId))
                .GroupBy(x => new
                {
                    x.BrandId,
                    Label = string.IsNullOrWhiteSpace(x.Brand?.BrandName) ? x.BrandId : x.Brand.BrandName
                })
                .OrderBy(x => x.Key.Label)
                .Select(x => new ProductFilterOptionDto
                {
                    Value = x.Key.BrandId,
                    Label = x.Key.Label,
                    Count = x.Count()
                })
                .ToList();
        }

        private static List<ProductFilterOptionDto> BuildVariantOptions(
            IEnumerable<ProductVariant> variants,
            Func<ProductVariant, string?> selector)
        {
            return variants
                .Select(selector)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!.Trim())
                .GroupBy(x => x, StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x.Key)
                .Select(x => new ProductFilterOptionDto
                {
                    Value = x.Key,
                    Label = x.Key,
                    Count = x.Select(v => v).Count()
                })
                .ToList();
        }

        private static SearchScoreResult CalculateSearchScore(Product product, List<ProductVariant> variants, string normalizedQuery, List<string> tokens)
        {
            if (!product.IsActive)
            {
                return new SearchScoreResult(0, new HashSet<string>());
            }

            var score = 0;
            var matchedTokens = new HashSet<string>();

            if (!string.IsNullOrWhiteSpace(normalizedQuery)
                && NormalizeText(product.ProductName).Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
            {
                score += 50;
            }

            if (!string.IsNullOrWhiteSpace(normalizedQuery)
                && NormalizeText(product.Slug).Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
            {
                score += 40;
            }

            foreach (var token in tokens)
            {
                if (ContainsToken(product.ProductName, token))
                {
                    score += 20;
                    matchedTokens.Add(token);
                }

                if (ContainsToken(product.Brand?.BrandName, token))
                {
                    score += 18;
                    matchedTokens.Add(token);
                }

                if (ContainsToken(product.Category?.Name, token))
                {
                    score += 14;
                    matchedTokens.Add(token);
                }

                if (variants.Any(x => ContainsToken(x.VariantName, token)))
                {
                    score += 12;
                    matchedTokens.Add(token);
                }

                if (variants.Any(x =>
                    ContainsToken(x.Color, token)
                    || ContainsToken(x.Size, token)
                    || ContainsToken(x.CapacityOption, token)
                    || ContainsToken(x.StyleOption, token)
                    || ContainsToken(x.MaterialOption, token)
                    || ContainsToken(x.Sku, token)))
                {
                    score += 10;
                    matchedTokens.Add(token);
                }

                if (ContainsToken(product.BadgeText, token))
                {
                    score += 8;
                    matchedTokens.Add(token);
                }

                if (ContainsToken(product.Description, token))
                {
                    score += 4;
                    matchedTokens.Add(token);
                }

                if ((token == "bestseller" || token == "best" || token == "seller") && product.IsBestSeller)
                {
                    score += 20;
                    matchedTokens.Add(token);
                }

                if ((token == "recycled" || token == "recyclable") && product.IsRecyclableProduct)
                {
                    score += 20;
                    matchedTokens.Add(token);
                }
            }

            if (tokens.All(x => matchedTokens.Contains(x)))
            {
                score += 25;
            }

            if (product.IsBestSeller)
            {
                score += 3;
            }

            if (product.IsFeatured)
            {
                score += 2;
            }

            return new SearchScoreResult(score, matchedTokens);
        }

        private sealed record SearchScoreResult(int Score, HashSet<string> MatchedTokens)
        {
            public int MatchedTokenCount => MatchedTokens.Count;
        }

        private async Task<Dictionary<string, List<ProductVariant>>> GetVariantsByProductIdsAsync(List<string> productIds)
        {
            if (!productIds.Any())
            {
                return new Dictionary<string, List<ProductVariant>>();
            }

            var variants = await _productVariantCollection
                .Find(Builders<ProductVariant>.Filter.In(x => x.ProductId, productIds))
                .ToListAsync();

            return variants
                .GroupBy(x => x.ProductId)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        private async Task ResolveCategoriesAsync(List<Product> products)
        {
            foreach (var item in products)
            {
                var category = await _categoryCollection
                    .Find(x => x.CategoryId == item.CategoryId)
                    .FirstOrDefaultAsync();

                if (category is not null)
                {
                    item.Category = category;
                }
            }
        }

        private async Task ResolveBrandsAsync(List<Product> products)
        {
            foreach (var item in products)
            {
                await ResolveBrandAsync(item);
            }
        }

        private async Task ResolveBrandAsync(Product? product)
        {
            if (product is null || string.IsNullOrWhiteSpace(product.BrandId))
            {
                return;
            }

            var brand = await _brandCollection
                .Find(x => x.BrandId == product.BrandId)
                .FirstOrDefaultAsync();

            if (brand is not null)
            {
                product.Brand = brand;
            }
        }
    }
}
