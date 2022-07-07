using Application.Core.Interfaces;
using Application.Core.Types;
using Infastructure.Types;

namespace DapperApi.Service;

public static class Configurations
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped((sp) => GetDALFactory(sp)) //AddSinglton(GetDALFactory);
            .AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>()
            .AddTransient<UnitOfWork>();
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
}
