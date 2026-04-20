using MultiShop.Discount.Dtos;

namespace MultiShop.Discount.Services
{
public interface IDiscountService
    {
    Task<List<ResultDiscountCouponDto>> GetAllDiscountAsync();
    Task<GetByIdDiscountCoupon> GetByIdDiscountCouponAsync(int id);
    Task CreateDiscountCouponAsync(CreateDiscountCouponDto createCouponDto);
    Task UpdateDiscountCouponAsync(UpdateDiscountCouponDto updateCouponDto);
    Task DeleteDiscountCouponAsync(int id);
    Task<ResultDiscountCouponDto> GetDiscountCode(string code);

    }
}
