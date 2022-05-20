using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Infrastructure.ApiResponse
{
    public class ApiResponse<TResponse>
    {
        public TResponse Data { get; set; }

        public ApiResponse(TResponse data)
        {
            this.Data = data;
        }
    }
}
