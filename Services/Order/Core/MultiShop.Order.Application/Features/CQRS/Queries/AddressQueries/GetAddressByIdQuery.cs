using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Queries.AddressQueries
{
    public class GetAddressByIdQuery
    {
        //Ben bu methodu çağırırken bir instance üzerinden çağıracağım bu nedenle constructor  oluşturuyorum ve parametre olarak id alıyorum
        public int AddressId { get; set; }
        public GetAddressByIdQuery(int id)
        {
            AddressId = id;
        }
    }
}
