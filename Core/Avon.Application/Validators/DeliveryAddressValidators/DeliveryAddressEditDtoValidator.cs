using Avon.Application.DTOs.DeliveryAddressDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.DeliveryAddressValidators
{
    public class DeliveryAddressEditDtoValidator : AbstractValidator<DeliveryAddressEditDto>
    {
        public DeliveryAddressEditDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().NotNull()
                .MaximumLength(150);    
        }
    }
}
