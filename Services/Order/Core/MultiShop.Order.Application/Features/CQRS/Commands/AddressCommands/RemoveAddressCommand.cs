using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Commands.AddressCommands
{
    public class RemoveAddressCommand
    {
        /* Remove işlemi için sadece addressId'ye ihtiyacım var çünkü silme işlemi yaparken sadece id'ye ihtiyacım var.
        Ben bu methodu çağırırken bir instance üzerinden çağıracağım bu nedenle constructor  oluşturuyorum ve parametre olarak id alıyorum.
                
                Constructor ekleyince şunu garanti ediyorsun:
                Bu command id olmadan oluşturulamaz.
         */
        public int AddressId { get; set; }
        public RemoveAddressCommand(int id )
        {
            AddressId = id;
        }
    }
}
