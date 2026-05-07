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
    public class EfCargoOperationDal : GenericRepository<CargoOperation>, ICargoOperationDal
    {
        private readonly CargoContext _context;

        public EfCargoOperationDal(CargoContext context) : base(context)
        {
            _context = context;
        }

        public List<CargoOperation> GetByCargoDetailId(int cargoDetailId)
        {
            return _context.CargoOperations
                .Where(x => x.CargoDetailId == cargoDetailId)
                .OrderBy(x => x.OperationDate)
                .ToList();
        }
    }
}
