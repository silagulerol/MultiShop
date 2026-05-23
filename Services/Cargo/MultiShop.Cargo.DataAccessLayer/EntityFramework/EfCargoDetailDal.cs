using Microsoft.EntityFrameworkCore;
using MultiShop.Cargo.DataAccessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.Concrete;
using MultiShop.Cargo.DataAccessLayer.Repositories;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.DataAccessLayer.EntityFramework
{
    public class EfCargoDetailDal : GenericRepository<CargoDetail>, ICargoDetailDal
    {
        private readonly CargoContext _context;

        public EfCargoDetailDal(CargoContext context) : base(context)
        {
            _context = context;
        }

        public List<CargoDetail> GetByVendorId(string vendorId)
        {
            return _context.CargoDetails
                .Where(x => x.VendorId == vendorId)
                .ToList();
        }

        public CargoDetail GetByOrderDetailId(int orderDetailId)
        {
            return _context.CargoDetails
                .Include(x => x.CargoCompany)
                .FirstOrDefault(x => x.OrderDetailId == orderDetailId);
        }
    }
}
