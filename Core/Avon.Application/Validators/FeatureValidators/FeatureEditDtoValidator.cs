﻿using Avon.Application.DTOs.FeatureDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.FeatureValidators
{
    public class FeatureEditDtoValidator : AbstractValidator<FeatureEditDto>
    {
        public FeatureEditDtoValidator()
        {
            RuleFor(x => x.Icon)
                .NotEmpty().NotNull();
            RuleFor(x => x.Title)
              .NotEmpty().NotNull()
              .MaximumLength(25);
        }
    }
}