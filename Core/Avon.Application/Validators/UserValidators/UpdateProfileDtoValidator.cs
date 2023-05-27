using Avon.Application.DTOs.UserDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.UserValidators
{
    public class UpdateProfileDtoValidator : AbstractValidator<UserUpdateProfileDto>
    {

        public UpdateProfileDtoValidator()
        {

            RuleFor(x => x.Name)
                .NotNull().NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Address)
                .NotNull().NotEmpty()
                .MaximumLength(250);

            RuleFor(x => x.Surname)
                .NotNull().NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Phone)
                .NotNull().NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .NotNull().NotEmpty()
                .MaximumLength(50);
        }
    }
}
