namespace MultiShop.Order.Application.Features.CQRS.Queries.OrderDetailQueries
{
    public class GetOrderDetailsByOrderingIdQuery
    {
        public int OrderingId { get; set; }

        public GetOrderDetailsByOrderingIdQuery(int orderingId)
        {
            OrderingId = orderingId;
        }
    }
}