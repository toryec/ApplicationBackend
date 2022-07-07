using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Domain.Core.Interfaces;
using System.Data.Common;

namespace Domain.DALs;

public abstract class BaseDal : IBaseDal
{
    public BaseDal(DbConnection dbConnection)
    {
        DbConnection = dbConnection;
    }

    protected DbConnection DbConnection { get; }
}

