using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Services.Interfaces;
public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default);
    Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> UpdateUserAsync(Guid id,User user, CancellationToken cancellationToken = default);
    Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> CheckIfExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

