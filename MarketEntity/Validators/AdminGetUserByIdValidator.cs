using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class AdminGetUserByIdValidator : AbstractValidator<AdminGetUserByIdRequest>
    {
        public AdminGetUserByIdValidator()
        {
            RuleFor(x => x.TargetUserId)
                .GreaterThan(0).WithMessage("Geçersiz kullanıcı bilgisi.");
        }
    }
}
