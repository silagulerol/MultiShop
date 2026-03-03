namespace MultiShop.Discount.Dtos
{
    public class ResultDiscountCouponDto
    {
        public int CouponId { get; set; }
        public string Code { get; set; }
        public int rate { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidDate { get; set; }

    }
}
