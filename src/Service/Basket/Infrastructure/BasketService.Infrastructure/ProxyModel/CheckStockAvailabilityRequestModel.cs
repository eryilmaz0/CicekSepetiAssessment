using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Infrastructure.ProxyModel
{
    public class CheckStockAvailabilityRequestModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
