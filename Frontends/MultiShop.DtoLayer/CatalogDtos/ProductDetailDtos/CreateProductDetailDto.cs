using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.DtoLayer.CatalogDtos.ProductDetailDtos
{
    public class CreateProductDetailDto
    {
        public string ProductLongDescription { get; set; }
        public string ProductInformation { get; set; }
        public string ProductId { get; set; }
    }
}
