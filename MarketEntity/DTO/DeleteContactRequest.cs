using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class DeleteContactRequest : BaseApiRequest
    {
        public long ContactId { get; set; }
    }
}
