using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class AddCategoryValidator : AbstractValidator<AddCategoryRequest>
    {
        public AddCategoryValidator()
        {
            RuleFor(x => x.CategoryName)
                    .NotEmpty().WithMessage("Kategori adi boş geçilemez.")
                    .Length(2, 50).WithMessage("kategori adının uzunluğu minimum 2 maksimum 50 karakter olmalıdır.");

        }
    }
}
