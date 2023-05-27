using Avon.Application;
using Avon.Application.Enums;
using Avon.Application.Middlewares;
using Avon.Infrastructure;
using Avon.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
                );
builder.Services.AddControllers();
builder.Services.AddCors(x => x.AddDefaultPolicy(policy => policy.WithOrigins("http://localhost:8000", "https://localhost:8000").AllowAnyMethod().AllowAnyHeader()));
Logger log = new LoggerConfiguration().WriteTo.File(
    Path.Combine("C:\\AvonLogs","Application","diagnostics.txt"),
    rollingInterval:RollingInterval.Day,
    fileSizeLimitBytes:10*1024*1024,
    outputTemplate: "-----{NewLine}{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}",
    shared:true,
    flushToDiskInterval:TimeSpan.FromSeconds(2)
    ).WriteTo.Seq(builder.Configuration["Seq:ServerURL"]).MinimumLevel.Information().CreateLogger();

builder.Host.UseSerilog(log); //build-in deki log mexanizmasi legv eledik serilog configini asign oldu.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationService();
builder.Services.AddPersistenceService();
builder.Services.AddInfrastructureServices();
builder.Services.AddInfrastructureServices(StorageEnum.LocalStorage);




builder.Services.AddAuthentication(c =>
{
    c.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
                .AddJwtBearer(c =>
                {
                    c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidIssuer = builder.Configuration.GetSection("Token:Issuer").Value,
                        ValidAudience = builder.Configuration.GetSection("Token:Audience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Token:SecurityKey").Value))
                    };
                });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
