namespace MultiShop.DtoLayer.OrderDtos.OrderDetailDtos
{
    public class ResultOrderTimelineStepDto
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsCompleted { get; set; }
        public DateTime? Date { get; set; }
    }
}
