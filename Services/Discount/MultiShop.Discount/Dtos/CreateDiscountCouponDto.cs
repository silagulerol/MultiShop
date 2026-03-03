namespace MultiShop.Discount.Dtos
{
    public class CreateDiscountCouponDto
    {
        public string Code { get; set; }
        public int rate { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidDate { get; set; }

    }
}
