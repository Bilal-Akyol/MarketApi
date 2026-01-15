using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateRequest>
    {
        private const long MaxBytes = 2 * 1024 * 1024; // 2MB

        public ProductCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı boş olamaz.")
                .Length(2, 100).WithMessage("Ürün adı 2-100 karakter olmalıdır.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stok 0'dan küçük olamaz.");

            RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Kategori seçilmelidir.");

            // Foto: En az 1, istersen 0 da bırakabiliriz ama sen product foto olacak dedin.
            RuleFor(x => x.Photos)
                .NotNull().WithMessage("Foto listesi boş olamaz.")
                .Must(p => p.Count > 0).WithMessage("En az 1 foto eklenmelidir.");

            RuleForEach(x => x.Photos).ChildRules(photo =>
            {
                photo.RuleFor(p => p.ContentType)
                    .NotEmpty().WithMessage("Foto content-type boş olamaz.")
                    .Must(ct => ct == "image/jpeg" || ct == "image/png")
                    .WithMessage("Sadece image/jpeg veya image/png kabul edilir.");

                photo.RuleFor(p => p.Base64)
                    .NotEmpty().WithMessage("Foto base64 boş olamaz.")
                    .Must(b64 =>
                    {
                        // 2MB kontrol
                        var bytes = Base64SizeHelper.GetBytesFromBase64(b64);
                        return bytes > 0 && bytes <= MaxBytes;
                    })
                    .WithMessage("Fotoğraf maksimum 2MB olmalıdır (base64).");
            });
        }
    }
}
