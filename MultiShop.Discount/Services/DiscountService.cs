using MultiShop.Discount.Dtos;

namespace MultiShop.Discount.Services
{
    public class DiscountService : IDiscountService
    {

        public Task CreateCoupon(CreateCouponDto createCouponDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCoupon(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResultCouponDto>> GetAllDiscountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GetByIdCoupon> GetByIdCoupon()
        {
            throw new NotImplementedException();
        }

        public Task UpdateCoupon(UpdateCouponDto updateCouponDto)
        {
            throw new NotImplementedException();
        }
    }
}
