using MultiShop.Discount.Dtos;

namespace MultiShop.Discount.Services
{
public interface IDiscountService
    {
    Task<List<ResultCouponDto>> GetAllDiscountAsync();
    Task<GetByIdCoupon> GetByIdCoupon();
    Task CreateCoupon(CreateCouponDto createCouponDto);
    Task UpdateCoupon(UpdateCouponDto updateCouponDto);
    Task DeleteCoupon(int id);

    }
}
