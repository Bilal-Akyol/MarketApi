using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class AboutCreateValidator : AbstractValidator<AboutCreateRequest>
    {
        private const long MaxBytes = 2 * 1024 * 1024;
        public AboutCreateValidator() 
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık Boş Olamaz")
                .MaximumLength(150).WithMessage("Başlık en fazla 150 karekter olabilir.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz")
                .MaximumLength(5000).WithMessage("İçerik en fazla 5000 karakter olabilir");
            RuleFor(x => x.ImageBase64)
               .NotEmpty().WithMessage("Hakkımızda görseli zorunludur.");

            RuleFor(x => x.ImageContentType)
                .NotEmpty().WithMessage("Görsel content-type zorunludur.")
                .MaximumLength(100).WithMessage("Content-type en fazla 100 karakter olabilir.");
        }
    }
}
