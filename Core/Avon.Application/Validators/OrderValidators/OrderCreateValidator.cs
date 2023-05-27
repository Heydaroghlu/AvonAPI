using Avon.Application.DTOs.OrderDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.OrderValidators
{
    public class OrderCreateValidator:AbstractValidator<OrderCreateDto>
    {
        public OrderCreateValidator()
        {
            RuleFor(x=>x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Surname).NotNull().NotEmpty();
            RuleFor(x => x.FatherName).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Address).NotNull().NotEmpty();
            RuleFor(x=>x.Phone).NotNull().NotEmpty();
            RuleFor(x=>x.ZipCode).NotNull().NotEmpty();
            RuleFor(x => x.StreetAddres).NotNull().NotEmpty();
            RuleFor(x=>x.Phone).NotNull().NotEmpty();
        }
    }
}
