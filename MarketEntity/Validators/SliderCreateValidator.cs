using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class SliderCreateValidator : AbstractValidator<SliderCreateRequest>
    {
        private const long MaxBytes = 2 * 1024 * 1024; // 2MB

        public SliderCreateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Slider başlığı boş olamaz.")
                .Length(2, 100).WithMessage("Başlık 2-100 karakter olmalıdır.");

            RuleFor(x => x.RedirectUrl)
                .NotEmpty().WithMessage("RedirectUrl boş olamaz.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("RedirectUrl geçerli bir URL olmalıdır.");

            RuleFor(x => x.ImageContentType)
                .NotEmpty().WithMessage("ImageContentType boş olamaz.")
                .Must(ct => ct == "image/jpeg" || ct == "image/png")
                .WithMessage("Sadece image/jpeg veya image/png kabul edilir.");

            RuleFor(x => x.ImageBase64)
                .NotEmpty().WithMessage("Slider image base64 boş olamaz.")
                .Must(b64 =>
                {
                    var bytes = Base64SizeHelper.GetBytesFromBase64(b64);
                    return bytes > 0 && bytes <= MaxBytes;
                })
                .WithMessage("Slider görseli maksimum 2MB olmalıdır.");

        }
    }
}
