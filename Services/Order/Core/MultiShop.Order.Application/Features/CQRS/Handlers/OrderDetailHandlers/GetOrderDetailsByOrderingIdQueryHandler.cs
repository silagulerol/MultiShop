using MultiShop.Order.Application.Features.CQRS.Queries.OrderDetailQueries;
using MultiShop.Order.Application.Features.CQRS.Results.OrderDetailsResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers
{
    public class GetOrderDetailsByOrderingIdQueryHandler
    {
        private readonly IRepository<OrderDetail> _repository;

        public GetOrderDetailsByOrderingIdQueryHandler(IRepository<OrderDetail> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetOrderDetailQueryResult>> Handle(GetOrderDetailsByOrderingIdQuery query)
        {
            var allValues = await _repository.GetAllAsync();
            var values = allValues.Where(x => x.OrderingId == query.OrderingId).ToList();

            return values.Select(x => new GetOrderDetailQueryResult
            {
                OrderDetailId = x.OrderDetailId,
                OrderingId = x.OrderingId,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
                ProductTotalPrice = x.ProductTotalPrice
            }).ToList();
        }
    }
}