using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Application.Model
{
    public class BasketViewModel
    {
        public string Id { get; set; }
        public string UserEmail { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public List<BasketItemViewModel> BasketItems { get; set; }

    }


    public class BasketItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public long Quantity { get; set; }
        public string DiscountCode { get; set; }
        public int DiscountRate { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }
}
