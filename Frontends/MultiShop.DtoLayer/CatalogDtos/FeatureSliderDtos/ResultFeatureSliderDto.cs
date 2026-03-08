using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos
{
    public class ResultFeatureSliderDto
    {
        public string FeatureSliderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        // UI'da en fazla 3 tane Feature bulunabilir. Bu yüzden active olanları belirlemek için status property'si ekledik. 
        public bool Status { get; set; }
    }
}
