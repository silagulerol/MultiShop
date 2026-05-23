namespace MultiShop.Order.Application.Features.CQRS.Queries.OrderDetailQueries
{
    public class GetOrderDetailsByVendorIdQuery
    {
        public string VendorId { get; set; }

        public GetOrderDetailsByVendorIdQuery(string vendorId)
        {
            VendorId = vendorId;
        }
    }
}