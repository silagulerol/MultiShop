using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.Abstract;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.BusinessLayer.Concrete
{
    public class CargoDetailManager : ICargoDetailService
    {
        private readonly ICargoDetailDal _cargoDetailDal;
        private readonly List<string> _validStatuses = new()
        {
            "Preparing",
            "Shipped",
            "In Transit",
            "Delivered",
            "Cancelled"
        };
        public CargoDetailManager(ICargoDetailDal cargoDetailDal)
        {
            _cargoDetailDal = cargoDetailDal;
        }

        public void TDelete(int id)
        {
            _cargoDetailDal.Delete(id);
        }

        public List<CargoDetail> TGetAll()
        {
            return _cargoDetailDal.GetAll();
        }

        public CargoDetail TGetById(int id)
        {
            return _cargoDetailDal.GetById(id);
        }

        public void TInsert(CargoDetail entity)
        {
            entity.CreatedDate = DateTime.Now;
            if (!_validStatuses.Contains(entity.CargoStatus))
            {
                throw new Exception("Invalid cargo status.");
            }
            if (string.IsNullOrWhiteSpace(entity.CargoStatus))
            {
                entity.CargoStatus = "Preparing";
            }

            _cargoDetailDal.Insert(entity);
        }

        public void TUpdate(CargoDetail entity)
        {
            if (!_validStatuses.Contains(entity.CargoStatus))
            {
                throw new Exception("Invalid cargo status.");
            }
            if (string.IsNullOrWhiteSpace(entity.CargoStatus))
            {
                entity.CargoStatus = "Preparing";
            }

            _cargoDetailDal.Update(entity);
        }

        public List<CargoDetail> TGetByVendorId(string vendorId)
        {
            return _cargoDetailDal.GetByVendorId(vendorId);
        }

        public CargoDetail TGetByOrderDetailId(int orderDetailId)
        {
            return _cargoDetailDal.GetByOrderDetailId(orderDetailId);
        }
    }
}