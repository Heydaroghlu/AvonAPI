﻿using Avon.Application.DTOs.SubCategoryDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.SubCategoryValidators
{
    public class SubCategoryEditDtoValidator : AbstractValidator<SubCategoryEditDto>
    {
        public SubCategoryEditDtoValidator()
        {
            RuleFor(x => x.Name)
              .NotEmpty().NotNull()
              .MaximumLength(50);

            RuleFor(x => x.CategoryId)
               .NotEmpty().NotNull()
               .GreaterThanOrEqualTo(0);
        }
    }
}
