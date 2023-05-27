using AutoMapper;
using Avon.Application.Profiles;
using Avon.Application.Validators.ContactUsValidators;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationService(this IServiceCollection services)
        {

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ContactUsValidator>());
            
        }
    }
}
