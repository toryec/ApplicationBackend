using System.Reflection;
using Application.Core.Interfaces;
using Application.Core.Types;
using Application.Services.Interfaces;
using Application.Services.Types;
using Dapper;
using Infastructure.Core.Types;
using Infastructure.Types;
using DTOs.Core.Interface;

namespace DapperApi.Service;

public static class Configurations
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped((sp) => GetDALFactory(sp)) //AddSinglton(GetDALFactory);
            .AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>()
            .AddTransient<UnitOfWork>()
            .AddScoped<IUserService, UserService>();
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
    //public static IServiceCollection RegisterHandler<TType, THandler>(this IServiceCollection services)
    //    where THandler : SqlMapper.TypeHandler<TType>, new()
    //{
    //    SqlMapper.AddTypeHandler(new THandler());

    //    return services;
    //}
}
