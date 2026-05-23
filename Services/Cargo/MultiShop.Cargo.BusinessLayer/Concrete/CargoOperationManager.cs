using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.Abstract;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.BusinessLayer.Concrete
{
    public class CargoOperationManager : ICargoOperationService
    {
        private readonly ICargoOperationDal _cargoOperationDal;
        private readonly ICargoDetailDal _cargoDetailDal;

        public CargoOperationManager(
            ICargoOperationDal cargoOperationDal,
            ICargoDetailDal cargoDetailDal)
        {
            _cargoOperationDal = cargoOperationDal;
            _cargoDetailDal = cargoDetailDal;
        }

        public void TDelete(int id)
        {
            _cargoOperationDal.Delete(id);
        }

        public List<CargoOperation> TGetAll()
        {
            return _cargoOperationDal.GetAll();
        }

        public CargoOperation TGetById(int id)
        {
            return _cargoOperationDal.GetById(id);
        }

        public void TInsert(CargoOperation entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Status))
            {
                entity.Status = "Preparing";
            }

            if (entity.OperationDate == default)
            {
                entity.OperationDate = DateTime.Now;
            }

            _cargoOperationDal.Insert(entity);

            var cargoDetail = _cargoDetailDal.GetById(entity.CargoDetailId);

            if (cargoDetail != null)
            {
                cargoDetail.CargoStatus = entity.Status;
                _cargoDetailDal.Update(cargoDetail);
            }
        }

        public void TUpdate(CargoOperation entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Status))
            {
                entity.Status = "Preparing";
            }

            _cargoOperationDal.Update(entity);

            var cargoDetail = _cargoDetailDal.GetById(entity.CargoDetailId);

            if (cargoDetail != null)
            {
                cargoDetail.CargoStatus = entity.Status;
                _cargoDetailDal.Update(cargoDetail);
            }
        }

        public List<CargoOperation> TGetByCargoDetailId(int cargoDetailId)
        {
            return _cargoOperationDal.GetByCargoDetailId(cargoDetailId);
        }
    }
}