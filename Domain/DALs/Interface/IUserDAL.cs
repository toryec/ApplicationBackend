using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Interfaces;
using Domain.Models;

namespace Domain.DALs.Interface;
public interface IUserDAL : IBaseDal
{
    Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> UpdateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> CheckIfExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> InsertUserDetailAsync(UserDetail userDetail, CancellationToken cancellationToken = default);
    Task<bool> UpdateUserDetailAsync(UserDetail userDetail, CancellationToken cancellationToken = default);
    Task<bool> InsertUserTypeAsync(UserType userType, CancellationToken cancellationToken = default);
    Task<UserType> GetUserTypeAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> InsertRoleAsync(IEnumerable<Role> roles, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetRoleAsync(Guid id, CancellationToken cancellationToken = default);

}
