using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Interfaces;
using Domain.Core.Types;
using Domain.Models;

namespace Domain.DALs.Interface;
public interface IUserDAL : IBaseDal
{
    Task<IEnumerable<User>> GetUsersAsync(string? userName = null, CancellationToken cancellationToken = default);
    Task<PaginatedList<User>> GetPaginatedUsersAsync( int pageIndex , int pageSize, CancellationToken cancellationToken = default);
    Task<User> GetUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> UpdateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> CheckIfExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserDetail> GetUserDetailAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> InsertUserDetailAsync(UserDetail userDetail, CancellationToken cancellationToken = default);
    Task<bool> UpdateUserDetailAsync(UserDetail userDetail, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRole>> GetRoleAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> InsertRoleAsync(IEnumerable<UserRole> userRoles, CancellationToken cancellationToken = default);
    Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default);
    Task<bool> CheckUserNameExistsAsync(string userName, CancellationToken cancellationToken = default);
}
