using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class UserLoginValidator:AbstractValidator<UserLoginRequest>
    {
        public UserLoginValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Boş Olamaz")
                .Length(2, 50).WithMessage("Email Uzunluğu minimum 2 maksimum 50 olmalıdır")
                .EmailAddress().WithMessage("Email adresi geçersizdir ");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz")
                .Length(2, 50).WithMessage("Şifre Minimum 2 Maksimum 50");

        }
        
    }
}
