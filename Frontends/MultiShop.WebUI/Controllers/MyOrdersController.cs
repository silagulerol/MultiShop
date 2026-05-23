using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.OrderDtos.OrderDetailDtos;
using MultiShop.WebUI.Services.CargoServices.CargoDetailServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.Interfaces;
using MultiShop.WebUI.Services.OrderServices.OrderAddressServices;
using MultiShop.WebUI.Services.OrderServices.OrderDetailServices;
using MultiShop.WebUI.Services.OrderServices.OrderOrderingServices;

namespace MultiShop.WebUI.Controllers
{
    [Authorize]
    public class MyOrdersController : Controller
    {
        private const string AddressFallback = "Address information is not available yet.";

        private readonly IUserService _userService;
        private readonly IOrderOrderingService _orderOrderingService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductService _productService;
        private readonly ICargoDetailService _cargoDetailService;
        private readonly IOrderAddressService _orderAddressService;

        public MyOrdersController(
            IUserService userService,
            IOrderOrderingService orderOrderingService,
            IOrderDetailService orderDetailService,
            IProductService productService,
            ICargoDetailService cargoDetailService,
            IOrderAddressService orderAddressService)
        {
            _userService = userService;
            _orderOrderingService = orderOrderingService;
            _orderDetailService = orderDetailService;
            _productService = productService;
            _cargoDetailService = cargoDetailService;
            _orderAddressService = orderAddressService;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var user = await _userService.GetUserInfo();
            var userOrders = await _orderOrderingService.GetOrderingByUserId(user.Id);
            var order = userOrders.FirstOrDefault(x => x.OrderingId == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderingIdAsync(id);
            var products = new List<ResultOrderDetailDto>();
            var productStatuses = new List<string>();
            string cargoCompanyName = "";
            string trackingNumber = "";

            foreach (var item in orderDetails)
            {
                Console.WriteLine($"OrderDetail ProductId: {item.ProductId}");

                var product = await _productService.GetByIdProductAsync(item.ProductId ?? "");
                var cargo = await _cargoDetailService.GetByOrderDetailIdAsync(item.OrderDetailId);
                var isCatalogProductFound = !string.IsNullOrWhiteSpace(product?.ProductId);

                if (cargo != null)
                {
                    cargoCompanyName = string.IsNullOrWhiteSpace(cargo.CargoCompanyName)
                        ? cargoCompanyName
                        : cargo.CargoCompanyName;
                    trackingNumber = string.IsNullOrWhiteSpace(cargo.TrackingNumber)
                        ? trackingNumber
                        : cargo.TrackingNumber;
                }

                item.ProductId = item.ProductId ?? "";
                item.ProductName = !string.IsNullOrWhiteSpace(product?.ProductName)
                    ? product.ProductName
                    : item.ProductName;
                item.BrandName = product?.BrandName ?? "";
                item.ImageUrl = product?.MainImageUrl ?? "";
                item.VendorId = string.IsNullOrWhiteSpace(item.VendorId)
                    ? product?.VendorId ?? ""
                    : item.VendorId;
                item.VendorName = string.IsNullOrWhiteSpace(item.VendorName)
                    ? item.VendorId ?? ""
                    : item.VendorName;
                item.CargoCompanyName = cargo?.CargoCompanyName;
                item.EstimatedDeliveryDate = order.OrderDate.AddDays(3);
                item.IsCatalogProductFound = isCatalogProductFound;
                
                if (cargo != null)
                {
                    item.HasShipment = true;
                    item.TrackingNumber = string.IsNullOrWhiteSpace(cargo.TrackingNumber)
                        ? item.TrackingNumber
                        : cargo.TrackingNumber;
                    item.ShipmentStatus = string.IsNullOrWhiteSpace(cargo.CargoStatus)
                        ? item.ShipmentStatus
                        : cargo.CargoStatus;
                }

                productStatuses.Add(GetEffectiveProductStatus(item.OrderStatus, item.ShipmentStatus));
                products.Add(item);
            }

            var effectiveOrderStatus = GetAggregateOrderStatus(productStatuses);

            var receiverName = string.IsNullOrWhiteSpace($"{user.Name} {user.Surname}".Trim())
                ? user.UserName
                : $"{user.Name} {user.Surname}".Trim();
            var address = await GetOrderAddressAsync(order.AddressId, user.Id);

            var model = new ResultOrderDetailPageDto
            {
                Order = order,
                ReceiverName = receiverName,
                Address = address,
                EffectiveOrderStatus = effectiveOrderStatus,
                CargoCompanyName = string.IsNullOrWhiteSpace(cargoCompanyName) ? "Cargo company is not available yet." : cargoCompanyName,
                TrackingNumber = string.IsNullOrWhiteSpace(trackingNumber) ? "Tracking number is not available yet." : trackingNumber,
                EstimatedDeliveryDate = order.OrderDate.AddDays(3),
                Products = products,
                Timeline = BuildTimeline(effectiveOrderStatus, order.OrderDate)
            };

            ViewBag.UserEmail = user.Email;
            ViewBag.UserName = user.UserName;
            ViewBag.UserFullName = receiverName;

            return View(model);
        }

        private async Task<string> GetOrderAddressAsync(int addressId, string userId)
        {
            var address = await _orderAddressService.GetAddressByIdAsync(addressId);

            if (address == null || address.UserId != userId)
            {
                return AddressFallback;
            }

            var addressParts = new[]
            {
                address.Detail,
                address.District,
                address.City
            }
            .Where(x => !string.IsNullOrWhiteSpace(x));

            var fullAddress = string.Join(", ", addressParts);

            return string.IsNullOrWhiteSpace(fullAddress) ? AddressFallback : fullAddress;
        }

        private static List<ResultOrderTimelineStepDto> BuildTimeline(string status, DateTime orderDate)
        {
            var normalizedStatus = NormalizeStatus(status);
            var statusRank = normalizedStatus switch
            {
                OrderLifecycleStatus.DeliveredNormalized => 5,
                OrderLifecycleStatus.InTransitNormalized => 4,
                OrderLifecycleStatus.ShippedNormalized => 3,
                OrderLifecycleStatus.PreparingNormalized => 2,
                OrderLifecycleStatus.PendingNormalized => 1,
                _ => 0
            };

            var timeline = new List<ResultOrderTimelineStepDto>
            {
                new()
                {
                    Title = "Order received",
                    Description = "Your order has been created successfully.",
                    IsCompleted = statusRank >= 1,
                    Date = orderDate
                },
                new()
                {
                    Title = "Preparing",
                    Description = "Products are being prepared by the seller.",
                    IsCompleted = statusRank >= 2,
                    Date = statusRank >= 2 ? orderDate.AddDays(1) : null
                },
                new()
                {
                    Title = "Shipped",
                    Description = "Your package is on the way.",
                    IsCompleted = statusRank >= 3,
                    Date = statusRank >= 3 ? orderDate.AddDays(2) : null
                },
                new()
                {
                    Title = "In transit",
                    Description = "Your package is moving through the cargo network.",
                    IsCompleted = statusRank >= 4,
                    Date = statusRank >= 4 ? orderDate.AddDays(3) : null
                },
                new()
                {
                    Title = "Delivered",
                    Description = "Your order has been delivered.",
                    IsCompleted = statusRank >= 5,
                    Date = statusRank >= 5 ? orderDate.AddDays(4) : null
                }
            };

            if (normalizedStatus == OrderLifecycleStatus.CancelledNormalized)
            {
                timeline.Add(new ResultOrderTimelineStepDto
                {
                    Title = "Cancelled",
                    Description = "Your order has been cancelled.",
                    IsCompleted = true,
                    Date = orderDate
                });
            }

            return timeline;
        }

        private static string GetEffectiveProductStatus(string? orderStatus, string? shipmentStatus)
        {
            return string.IsNullOrWhiteSpace(shipmentStatus)
                ? NormalizeStatus(orderStatus)
                : NormalizeStatus(shipmentStatus);
        }

        private static string GetAggregateOrderStatus(List<string> effectiveProductStatuses)
        {
            if (!effectiveProductStatuses.Any())
            {
                return OrderLifecycleStatus.Pending;
            }

            var statuses = effectiveProductStatuses
                .Select(NormalizeStatus)
                .ToList();

            if (statuses.All(x => x == OrderLifecycleStatus.CancelledNormalized))
            {
                return OrderLifecycleStatus.Cancelled;
            }

            if (statuses.All(x => x == OrderLifecycleStatus.DeliveredNormalized))
            {
                return OrderLifecycleStatus.Delivered;
            }

            if (statuses.Any(x => x == OrderLifecycleStatus.InTransitNormalized))
            {
                return OrderLifecycleStatus.InTransit;
            }

            if (statuses.Any(x => x == OrderLifecycleStatus.ShippedNormalized))
            {
                return OrderLifecycleStatus.Shipped;
            }

            if (statuses.Any(x => x == OrderLifecycleStatus.PreparingNormalized))
            {
                return OrderLifecycleStatus.Preparing;
            }

            if (statuses.Any(x => x == OrderLifecycleStatus.DeliveredNormalized))
            {
                return OrderLifecycleStatus.InTransit;
            }

            return OrderLifecycleStatus.Pending;
        }

        private static string NormalizeStatus(string? status)
        {
            return status?.Trim().ToLowerInvariant() switch
            {
                OrderLifecycleStatus.DeliveredNormalized => OrderLifecycleStatus.DeliveredNormalized,
                "completed" => OrderLifecycleStatus.DeliveredNormalized,
                "cargo delivered" => OrderLifecycleStatus.DeliveredNormalized,
                "teslim edildi" => OrderLifecycleStatus.DeliveredNormalized,
                OrderLifecycleStatus.InTransitNormalized => OrderLifecycleStatus.InTransitNormalized,
                "intransit" => OrderLifecycleStatus.InTransitNormalized,
                "on the way" => OrderLifecycleStatus.InTransitNormalized,
                OrderLifecycleStatus.ShippedNormalized => OrderLifecycleStatus.ShippedNormalized,
                "shipping" => OrderLifecycleStatus.ShippedNormalized,
                "kargoya verildi" => OrderLifecycleStatus.ShippedNormalized,
                OrderLifecycleStatus.PreparingNormalized => OrderLifecycleStatus.PreparingNormalized,
                "order received" => OrderLifecycleStatus.PendingNormalized,
                OrderLifecycleStatus.PendingNormalized => OrderLifecycleStatus.PendingNormalized,
                OrderLifecycleStatus.CancelledNormalized => OrderLifecycleStatus.CancelledNormalized,
                _ => OrderLifecycleStatus.PendingNormalized
            };
        }

        private static class OrderLifecycleStatus
        {
            public const string Pending = "Pending";
            public const string Preparing = "Preparing";
            public const string Shipped = "Shipped";
            public const string InTransit = "In Transit";
            public const string Delivered = "Delivered";
            public const string Cancelled = "Cancelled";

            public const string PendingNormalized = "pending";
            public const string PreparingNormalized = "preparing";
            public const string ShippedNormalized = "shipped";
            public const string InTransitNormalized = "in transit";
            public const string DeliveredNormalized = "delivered";
            public const string CancelledNormalized = "cancelled";
        }
    }
}
