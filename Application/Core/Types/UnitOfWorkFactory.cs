using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Core.Types;
public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IServiceProvider serviceProvider;

    public UnitOfWorkFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    public IUnitOfWork GetUnitOfWork()
    {
        return serviceProvider.GetRequiredService<UnitOfWork>();
    }
}
