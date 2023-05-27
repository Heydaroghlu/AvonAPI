using Avon.Application.Abstractions.Email;
using Avon.Application.Abstractions.File;
using Avon.Application.Abstractions.Token;
using Avon.Application.Enums;
using Avon.Application.Storages;
using Avon.Infrastructure.Email;
using Avon.Infrastructure.File;
using Avon.Infrastructure.Services;
using Avon.Infrastructure.Storages.CloudinaryStorages;
using Avon.Infrastructure.Storages.LocalStorages;
using Avon.Infrastructure.Token;
using Microsoft.Extensions.DependencyInjection;

namespace Avon.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileService, FileService>();
        }
        public static void AddInfrastructureServices(this IServiceCollection services, StorageEnum storageEnum)
        {
            services.AddScoped<ITokenHandler, TokenHandler>();

            switch (storageEnum)
            {
                case StorageEnum.LocalStorage:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageEnum.CloudinaryStorage:
                    services.AddScoped<IStorage, CloudinaryStorage>();
                    break;
                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
