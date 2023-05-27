using Avon.Application.Abstractions.User;
using Avon.Application.UnitOfWorks;
using Avon.Domain.Entities;
using Avon.Persistence.Concrets.User;
using Avon.Persistence.Configurations;
using Avon.Persistence.Contexts;
using Avon.Persistence.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using MySql;

namespace Avon.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceService(this IServiceCollection services)
        {

            services.AddDbContext<DataContext>(options =>
            {
                options.UseMySQL(ServiceConfiguration.MySQL);
                //options.UseSqlServer(ServiceConfiguration.MSSQL);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();

            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequiredUniqueChars = 0;
                opt.Password.RequireUppercase = false;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<DataContext>();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.SignIn.RequireConfirmedEmail = true;
            });

        }
    }
}
