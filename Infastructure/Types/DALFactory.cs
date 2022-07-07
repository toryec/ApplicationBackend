using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Domain.Core.Interfaces;
using Domain.DALs;
using Microsoft.Extensions.DependencyInjection;

namespace Infastructure.Types;
public class DALFactory : IDALFactory
{
    private ConcurrentDictionary<Type, Type> dalRegistry = new();
    private readonly IServiceProvider serviceProvider;

    public DALFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public T GetDAL<T>(DbConnection dbConnection) where T : IBaseDal
    {
        var baseType = typeof(T);

        if (!dalRegistry.TryGetValue(baseType, out var concreteType))
        {
            throw new ArgumentException($"DAL with type {baseType.FullName} is not registered");
        }

        var instance = ActivatorUtilities.CreateInstance(serviceProvider, concreteType, dbConnection);
        return (T)instance;
    }

    public void RegisterDALs(params Assembly[] assemblies)
    {
        var baseDALType = typeof(BaseDal);
        var iBaseDALType = typeof(IBaseDal);

        foreach (var type in assemblies.Distinct().SelectMany(a => a.GetTypes().Where(t => t.IsSubclassOf(baseDALType))))
        {
            if (type is null)
            {
                continue;
            }

            var interfaceType = type.GetInterfaces().SingleOrDefault(t => t != iBaseDALType && t.IsAssignableTo(iBaseDALType));

            if (interfaceType is null)
            {
                continue;
            }

            dalRegistry.TryAdd(interfaceType, type);
        }
    }

}
      

