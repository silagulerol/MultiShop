using MultiShop.Cargo.DataAccessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.Concrete;
using MultiShop.Cargo.DataAccessLayer.Repositories;
using MultiShop.Cargo.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.DataAccessLayer.EntityFramework
{
    public class EfVendorCargoCompanyDal : GenericRepository<VendorCargoCompany>, IVendorCargoCompanyDal
    {
        private readonly CargoContext _context;

        public EfVendorCargoCompanyDal(CargoContext context) : base(context)
        {
            _context = context;
        }

        public List<VendorCargoCompany> GetByVendorId(string vendorId)
        {
            return _context.VendorCargoCompanies
                .Where(x => x.VendorId == vendorId)
                .ToList();
        }
    }
}