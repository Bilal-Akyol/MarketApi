using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class LogoutSessionValidator : AbstractValidator<LogoutSessionRequest>
    {
        public LogoutSessionValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Geçersiz kullanıcı bilgisi.");

            RuleFor(x => x.SessionId)
                .GreaterThan(0).WithMessage("Kapatılacak oturum bilgisi zorunludur.");
        }
    }
}
