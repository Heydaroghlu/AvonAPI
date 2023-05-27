using Avon.Application.DTOs.PromotionDTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.PromotionValidators
{
    public class PromotionCreateDtoValidator : AbstractValidator<PromotionCreateDto>
    {
        public PromotionCreateDtoValidator()
        {
            RuleFor(x => x.Key).NotNull().NotEmpty().MaximumLength(100);
        }
    }
}
