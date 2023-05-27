﻿using Avon.Application.DTOs.NewCategoryDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.NewCategoryValidators
{
    public class NewsCategoryEditDtoValidator : AbstractValidator<NewsCategoryEditDto>
    {

        public NewsCategoryEditDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().NotEmpty()
                .MaximumLength(100);
        }

    }
}
