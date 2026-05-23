using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;

namespace MultiShop.DtoLayer.OrderDtos.OrderDetailDtos
{
    public class ResultOrderDetailPageDto
    {
        public ResultOrderingByUserId Order { get; set; } = new();
        public string ReceiverName { get; set; } = "";
        public string Address { get; set; } = "";
        public string EffectiveOrderStatus { get; set; } = "";
        public string CargoCompanyName { get; set; } = "";
        public string TrackingNumber { get; set; } = "";
        public DateTime? EstimatedDeliveryDate { get; set; }
        public List<ResultOrderDetailDto> Products { get; set; } = new();
        public List<ResultOrderTimelineStepDto> Timeline { get; set; } = new();
    }
}
