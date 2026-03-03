using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Domain.Entities
{
    public class OrderDetail
    {
        // OrderDetail: Sipariş detaylarını temsil eder. Her bir siparişin hangi ürünleri içerdiğini, miktarını ve fiyatını tutar.
        public int OrderDetailId { get; set; }
        public string ProductId { get; set; } 
        public string ProductName { get; set; } 
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal ProductTotalPrice { get; set; }

        public int OrderingId { get; set; }
        public Ordering Ordering { get; set; }

    }
}
