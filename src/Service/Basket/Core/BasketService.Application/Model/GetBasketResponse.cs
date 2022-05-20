using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Application.Model
{
    public class GetBasketResponse
    {
        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
        public BasketViewModel Basket { get; set; }
    }
}
