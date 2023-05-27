using Avon.Domain.Entities;
﻿using Avon.Application.DTOs.ContactUsDTOs;
using Avon.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.ContactUsValidators
{
    public class ContactUsValidator : AbstractValidator<ContactUsCreateDto>
    {
        public ContactUsValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(25);
            RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Phone).NotNull().NotEmpty().MaximumLength(30);
            RuleFor(x => x.Message).NotNull().NotEmpty().MaximumLength(600);
        }
    }
}