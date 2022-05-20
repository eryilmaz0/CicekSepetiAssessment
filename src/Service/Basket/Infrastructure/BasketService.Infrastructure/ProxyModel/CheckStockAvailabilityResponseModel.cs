using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Infrastructure.ProxyModel
{
    public class CheckStockAvailabilityResponseModel
    {
        public bool IsAvailable { get; set; }
        public string ResultMessage { get; set; }
    }
}
