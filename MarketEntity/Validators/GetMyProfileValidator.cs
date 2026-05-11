using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class GetMyProfileValidator : AbstractValidator<GetMyProfileRequest>
    {
        public GetMyProfileValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Geçersiz kullanıcı bilgisi.");
        }
    }
}
