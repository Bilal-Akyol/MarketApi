using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class GetMySessionsValidator : AbstractValidator<GetMySessionsRequest>
    {
        public GetMySessionsValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Geçersiz kullanıcı bilgisi.");
        }
    }
}
