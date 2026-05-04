using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.EntityFramework;
using MultiShop.Cargo.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.BusinessLayer.Concrete
{
    public class VendorCargoCompanyManager : IVendorCargoCompanyService
    {
        private readonly IVendorCargoCompanyDal _dal;

        public VendorCargoCompanyManager(IVendorCargoCompanyDal dal)
        {
            _dal = dal;
        }

        public List<VendorCargoCompany> TGetByVendorId(string vendorId)
        {
            return _dal.GetByVendorId(vendorId);
        }

        public void TInsert(VendorCargoCompany entity) => _dal.Insert(entity);
        public void TUpdate(VendorCargoCompany entity) => _dal.Update(entity);
        public void TDelete(int id) => _dal.Delete(id);
        public VendorCargoCompany TGetById(int id) => _dal.GetById(id);
        public List<VendorCargoCompany> TGetAll() => _dal.GetAll();
}
}
