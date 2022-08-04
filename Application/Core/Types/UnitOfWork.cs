using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Domain.Core.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Application.Core.Types;
public class UnitOfWork: IUnitOfWork
{
    private bool disposedValue;
    private readonly IConfiguration configuration;
    private readonly IDALFactory dalFactory;
    private readonly DbConnection dbConnection;

    private DbTransaction? dbtransaction;
    public UnitOfWork(IConfiguration configuration, IDALFactory dalFactory)
    {
        this.configuration = configuration;
        this.dalFactory = dalFactory;
        //Don't forget to add connection string
        dbConnection = new SqliteConnection(configuration.GetConnectionString("sqliteconnetion")); 
    }

    public void BeginTransaction()
    {
        dbtransaction = dbConnection.BeginTransaction();
    }

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
    {
        dbtransaction = await dbConnection.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public void Commit()
    {
        if(dbtransaction is null)
        {
            return;
        }
        dbtransaction.Commit();
        dbtransaction.Dispose();
        dbtransaction = null;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if(dbtransaction is null)
        {
            return;
        }
        await dbtransaction.CommitAsync(cancellationToken);
        await dbtransaction.DisposeAsync();
        dbtransaction = null;
    }

    public T GetDAL<T>() where T : IBaseDal
        => dalFactory.GetDAL<T>(dbConnection);
    public void RoleBack()
        => dbtransaction?.Rollback();
    
    public async Task RoleBackAsync(CancellationToken cancellationToken = default)
    {
        if (dbtransaction is null)
        {
            return;
        }

        await dbtransaction.RollbackAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if(dbtransaction is not null)
        {
            await dbtransaction.DisposeAsync();
        }

        await dbConnection.DisposeAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposedValue)
        {
            return;
        }

        if (disposing)
        {
            dbtransaction?.Dispose();
            dbConnection.Dispose();
        }

        disposedValue = true;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
