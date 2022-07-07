using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Interfaces;

namespace Application.Core.Interfaces;
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    T GetDAL<T>() where T : IBaseDal;
    Task CommitAsync(CancellationToken cancellationToken = default);
    void Commit();
    Task RoleBackAsync(CancellationToken cancellationToken = default);
    void RoleBack();
    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
    void BeginTransaction();

}
