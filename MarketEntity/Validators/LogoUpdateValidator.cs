using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class LogoUpdateValidator : AbstractValidator<LogoUpdateRequest>
    {
        public LogoUpdateValidator()
        {
            RuleFor(x => x.LogoId)
                .GreaterThan(0).WithMessage("LogoId geçersiz.");

            RuleFor(x => x.ImageBase64)
                .NotEmpty().WithMessage("Logo base64 boş olamaz.");

            RuleFor(x => x.ImageContentType)
                .NotEmpty().WithMessage("ContentType boş olamaz.")
                .MaximumLength(100).WithMessage("ContentType maksimum 100 karakter olmalıdır.");

            RuleFor(x => x.Title)
                .MaximumLength(100).WithMessage("Title maksimum 100 karakter olmalıdır.");
        }
    }
}
