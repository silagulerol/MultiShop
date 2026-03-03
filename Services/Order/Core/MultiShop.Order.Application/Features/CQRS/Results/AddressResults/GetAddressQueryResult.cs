using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Results.AddressResults
{
    public class GetAddressQueryResult
    {
        //Bu sınıf bizim address sınıfımızdaki propertylere sahip olacak ve bunları listelememizi sağlayacak
        // Address sınıfımızdaki propertyler Id, UserId, Street, City, State, ZipCode gibi doamain katmanımızda  
        public int AddressId { get; set; }
        public string UserId { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Detail { get; set; }
    }
}
