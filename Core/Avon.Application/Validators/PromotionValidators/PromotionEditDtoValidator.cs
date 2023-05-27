using Avon.Application.DTOs.PromotionDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.PromotionValidators
{
    public class PromotionEditDtoValidator : AbstractValidator<PromotionEditDto>
    {
        public PromotionEditDtoValidator()
        {
            RuleFor(x => x.Key).NotNull().NotEmpty().MaximumLength(100);
        }
    }
}
