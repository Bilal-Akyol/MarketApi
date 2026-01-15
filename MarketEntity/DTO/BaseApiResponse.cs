using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class BaseApiResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public BaseApiResponse()
        {
            this.Errors = new List<string>();
        }
    }
}
