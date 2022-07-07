using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Interfaces;

namespace Application.Core.Interfaces;
public interface IDALFactory
{
    // T is the returning type Dal. <T> is the Dal being passed through. They are both Interfaces
    T GetDAL<T>(DbConnection dbConnection) where T : IBaseDal;

    void RegisterDALs(params Assembly[] assemblies);

}
