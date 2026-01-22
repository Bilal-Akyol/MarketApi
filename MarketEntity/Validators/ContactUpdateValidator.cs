using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class ContactUpdateValidator:AbstractValidator<ContactUpdateRequest>
    {
        public ContactUpdateValidator() 
        {
            RuleFor(x => x.ContactId)
                .GreaterThan(0).WithMessage("ContactId geçersiz.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık Boş olamaz")
                .MaximumLength(150).WithMessage("Maximum 150 karakterli olabilir");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon Boş Olamaz")
                .MaximumLength(30).WithMessage("Telefon Maximum 30 Karakterli olmalı");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş Olamaz")
                .EmailAddress().WithMessage("Email Formatı Geçersiz")
                .MaximumLength(150).WithMessage("Email maximum 150 karakterli olmalı");
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres boş olamaz")
                .MaximumLength(500).WithMessage("Adres maximum 500 karakterli olmalı");

        }
    }
}
