using FluentValidation;
using MarketEntity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Validators
{
    public class AdminGetAllUsersValidator : AbstractValidator<AdminGetAllUsersRequest>
    {
        public AdminGetAllUsersValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Sayfa numarası 0'dan büyük olmalıdır.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Sayfa boyutu 0'dan büyük olmalıdır.")
                .LessThanOrEqualTo(100).WithMessage("Sayfa boyutu en fazla 100 olabilir.");
        }
    }
}
