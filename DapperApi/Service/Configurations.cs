using System.Reflection;
using Application.Core.Interfaces;
using Application.Core.Types;
using Application.Services.Interfaces;
using Application.Services.Types;
using Dapper;
using Infastructure.Core.Types;
using Infastructure.Types;
using DTOs.Core.Interface;
using Microsoft.AspNetCore.Identity;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DapperApi.Service;

public static class Configurations
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped((sp) => GetDALFactory(sp)) //AddSinglton(GetDALFactory);
            .AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>()
            .AddTransient<UnitOfWork>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IAuthenticationManager, AuthenticationManager>()
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
           // 
        AddTypes();
        //services
        //    .RegisterHandler<Guid, GuidTypeHandler>()
        //    .RegisterHandler<Guid, GuidTypeHandler>()
        //    .RegisterHandler<Guid, GuidTypeHandler>()
        //    .RegisterHandler<Guid, GuidTypeHandler>();

        return services;

        
    }

    public static IDALFactory GetDALFactory(IServiceProvider serviceProvider)
    {
        var dalFactory = new DALFactory(serviceProvider);
        //Assembly Registration

        var assemblies = new[]
        {
            typeof(IDALAssembly).Assembly
        };

        dalFactory.RegisterDALs(assemblies);

        return dalFactory;
    }

    private static void AddTypes()
    {
        SqlMapper.AddTypeHandler(new GuidTypeHandler());
        
    }

    public static IServiceCollection AddAutoMapperHandler(this IServiceCollection services)
    {
        //var assembly = Assembly.Load("DTOs"); // Brings in the  System.Reflection Name Space
        var assembly = new[]
        {
            typeof(IDTOAssembly).Assembly
        };
        services.AddAutoMapper(assembly);

        return services;
    }
   
    // IMPORTANT Add Audience and Issuer
    public static IServiceCollection AddTokenValidator(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenKey = configuration["AppSettings:TokenKey"]; //configuration.GetValue<string>("TokenKey"); //configuration["TokenKey"];
        var key = Convert.FromBase64String(tokenKey);

        services
            .AddScoped<ApiJwtBearerEvents>()
            .AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                //x.Audience = "vetting-app";
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.EventsType = typeof(ApiJwtBearerEvents); // For Debugging the token; finding out what happen when we validate with a token
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


        return services;
    }
}
