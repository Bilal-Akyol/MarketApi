using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class AboutGetResponse : BaseApiResponse
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

}
