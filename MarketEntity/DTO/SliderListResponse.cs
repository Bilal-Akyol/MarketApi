using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class SliderListResponse : BaseApiResponse
    {
        public List<SliderListItem> Sliders { get; set; } = new();
    }
}
