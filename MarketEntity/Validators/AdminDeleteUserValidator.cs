using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class AdminDeleteUserValidator : AbstractValidator<AdminDeleteUserRequest>
    {
        public AdminDeleteUserValidator()
        {
            RuleFor(x => x.TargetUserId)
                .GreaterThan(0).WithMessage("Silinecek kullanıcı bilgisi zorunludur.");
        }
    }
}
