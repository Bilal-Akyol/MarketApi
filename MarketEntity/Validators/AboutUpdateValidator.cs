using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class AboutUpdateValidator : AbstractValidator<AboutUpdateRequest>
    {
        private const long MaxBytes = 2 * 1024 * 1024;
        public AboutUpdateValidator()
        {
            RuleFor(x => x.AboutId)
                .GreaterThan(0).WithMessage("AboutId zorunludur.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(150).WithMessage("Başlık en fazla 150 karakter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz.")
                .MaximumLength(5000).WithMessage("İçerik en fazla 5000 karakter olabilir.");

            // ImageBase64 geldiyse ContentType da gelmeli 
            When(x => !string.IsNullOrWhiteSpace(x.ImageBase64), () =>
            {
                RuleFor(x => x.ImageContentType)
                    .NotEmpty().WithMessage("Görsel content-type zorunludur.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.ImageContentType), () =>
            {
                RuleFor(x => x.ImageBase64)
                    .NotEmpty().WithMessage("Görsel base64 zorunludur.");
            });
        }
    }
}
