using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class ChangeMyPasswordValidator : AbstractValidator<ChangeMyPasswordRequest>
    {
        public ChangeMyPasswordValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Geçersiz kullanıcı bilgisi.");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Mevcut şifre zorunludur.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Yeni şifre zorunludur.")
                .MinimumLength(6).WithMessage("Yeni şifre en az 6 karakter olmalıdır.");
        }
    }
}
