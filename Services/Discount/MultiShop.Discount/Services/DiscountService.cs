using Dapper;
using Microsoft.EntityFrameworkCore;
using MultiShop.Discount.Context;
using MultiShop.Discount.Dtos;
using MultiShop.Discount.Entities;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MultiShop.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly DapperContext _context;
        public DiscountService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateDiscountCouponAsync(CreateDiscountCouponDto createCouponDto)
        {
            string query = "INSERT INTO Coupons (Code, Rate, IsActive, ValidDate) values (@Code, @Rate, @IsActive, @ValidDate)";
            var parameters = new DynamicParameters();
            parameters.Add("@Code", createCouponDto.Code);
            parameters.Add("@Rate", createCouponDto.rate);
            parameters.Add("@IsActive", createCouponDto.IsActive);
            parameters.Add("@ValidDate", createCouponDto.ValidDate);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteDiscountCouponAsync(int id)
        {
            string query = "DELETE FROM Coupons WHERE CouponId = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            using (var connection = _context.CreateConnection())
            {
                //ExecuteAsync → Veri döndürmez, sadece etkilediği satır sayısını döndürür
                //ExecuteAsync şunlar için kullanılır: INSERT, UPDATE, DELETE. Yani data değiştirir, ama result set dönmez.
                //affectedRows → kaç satır etkilendiğini döner.Return type:Task<int>
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<List<ResultDiscountCouponDto>> GetAllDiscountAsync()
        {
            string query = "SELECT * FROM Coupons";
            using (var connection = _context.CreateConnection())
            {
                //QueryAsync → Veri döndürür. QueryAsync şunlar için kullanılır: SELECT. Yani veri çeker.
                //Return type: Task<IEnumerable<Coupon>>.  Dapper otomatik olarak:SQL sonucu, C# class property’leriyle map eder
                //ama türü hala IEnumerable<ResultCouponDto> olarak kalır. ToList() ile List<ResultCouponDto> yapar return ederiz.
                var values = await connection.QueryAsync<ResultDiscountCouponDto>(query);
                return values.ToList();
            }
        }

        public async Task<GetByIdDiscountCoupon> GetByIdDiscountCouponAsync(int id)
        {
            string query = "SELECT * FROM Coupons WHERE CouponId = @CouponId";
            var parameters = new DynamicParameters();
            parameters.Add("@CouponId", id);
            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QueryFirstOrDefaultAsync<GetByIdDiscountCoupon>(query, parameters);
                return value;
            }

        }

        public async Task<ResultDiscountCouponDto> GetDiscountCode(string code)
        {
            string query = "SELECT * FROM Coupons WHERE Code = @Code";
            var parameters = new DynamicParameters();
            parameters.Add("@Code", code);
            using (var connection = _context.CreateConnection())
            {
                var value = await connection.QueryFirstOrDefaultAsync<ResultDiscountCouponDto>(query, parameters);
                return value;
            }
        }

        public async Task UpdateDiscountCouponAsync(UpdateDiscountCouponDto updateCouponDto)
        {
            string query = "UPDATE Coupons SET Code = @Code, Rate = @Rate, IsActive = @IsActive, ValidDate = @ValidDate WHERE CouponId = @CouponId";
            var parameters = new DynamicParameters();
            parameters.Add("@Code", updateCouponDto.Code);
            parameters.Add("@Rate", updateCouponDto.rate);
            parameters.Add("@IsActive", updateCouponDto.IsActive);
            parameters.Add("@ValidDate", updateCouponDto.ValidDate);
            parameters.Add("@CouponId", updateCouponDto.CouponId);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

    }
}
