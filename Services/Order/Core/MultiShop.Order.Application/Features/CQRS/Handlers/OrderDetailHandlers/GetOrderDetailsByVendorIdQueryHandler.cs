using MultiShop.Order.Application.Features.CQRS.Queries.OrderDetailQueries;
using MultiShop.Order.Application.Features.CQRS.Results.OrderDetailsResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers
{
    public class GetOrderDetailsByVendorIdQueryHandler
    {
        private readonly IRepository<OrderDetail> _repository;

        public GetOrderDetailsByVendorIdQueryHandler(IRepository<OrderDetail> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetOrderDetailQueryResult>> Handle(GetOrderDetailsByVendorIdQuery query)
        {
            var values = await _repository.GetAllAsync();

            return values
                .Where(x => x.VendorId == query.VendorId)
                .Select(x => new GetOrderDetailQueryResult
                {
                    OrderDetailId = x.OrderDetailId,
                    ProductId = x.ProductId,
                    VendorId = x.VendorId,
                    ProductName = x.ProductName,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    ProductTotalPrice = x.ProductTotalPrice,
                    OrderingId = x.OrderingId,
                    OrderStatus=x.OrderStatus
                })
                .ToList();
        }
    }
}