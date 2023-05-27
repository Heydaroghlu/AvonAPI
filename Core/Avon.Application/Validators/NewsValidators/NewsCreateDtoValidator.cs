using Avon.Application.DTOs.NewDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Validators.NewsValidators
{
    public class NewsCreateDtoValidator : AbstractValidator<NewsCreateDto>
    {
        public NewsCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().NotNull()
                .MaximumLength(100);

            RuleFor(x => x.Content)
                .NotNull().NotEmpty();


            RuleFor(x => x.PosterFile)
                .NotNull().NotEmpty();
        }
    }
}
