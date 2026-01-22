using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class ContactCreateValidator : AbstractValidator<ContactCreateRequest>
    {
        public ContactCreateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MaximumLength(150).WithMessage("Başlık maksimum 150 karakter olmalıdır.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon boş olamaz.")
                .MaximumLength(30).WithMessage("Telefon maksimum 30 karakter olmalıdır.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş olamaz.")
                .EmailAddress().WithMessage("Email formatı geçersiz.")
                .MaximumLength(150).WithMessage("Email maksimum 150 karakter olmalıdır.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres boş olamaz.")
                .MaximumLength(500).WithMessage("Adres maksimum 500 karakter olmalıdır.");

            //RuleFor(x => x.MapUrl)
                //.MaximumLength(500).WithMessage("MapUrl maksimum 500 karakter olmalıdır.")
                //.When(x => !string.IsNullOrWhiteSpace(x.MapUrl));
        }
    }
}
