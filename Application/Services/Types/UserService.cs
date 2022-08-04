using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Services.Interfaces;
using Domain.DALs.Interface;
using Domain.Models;

namespace Application.Services.Types;
public class UserService : IUserService
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;

    public UserService(IUnitOfWorkFactory unitOfWorkFactory)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
    }

    public async Task<bool> CheckIfExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        var userDAL = unitOfWork.GetDAL<IUserDAL>();
        return await userDAL.CheckIfExistsAsync(id, cancellationToken);
    }

    //Optional Insert for UserDetails
    public async Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        using var unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        var userDAL = unitOfWork.GetDAL<IUserDAL>();

        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.Id = Guid.NewGuid();
        var result = await userDAL.CreateUserAsync(user, cancellationToken);
        if(!result)
        {
            return false;
        }
         
        if(user.UserDetail is not null)
        {
            user.UserDetail.UserId = user.Id;
            result = await userDAL.InsertUserDetailAsync(user.UserDetail, cancellationToken);
        }
        
        if(!result)
        {
            return false;
        }

        if(user.Roles?.Any() == true)
        {
            foreach (var item in user.Roles)
            {
                item.Id = user.Id;
            }
            result = await userDAL.InsertRoleAsync(user.Roles, cancellationToken);
        }

        if (!result)
        {
            return false;
        }

        if (user.UserType != 0)
        {
            result = await userDAL.InsertUserTypeAsync(user.UserType, cancellationToken);
        }

        if (!result)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        var userDAL = unitOfWork.GetDAL<IUserDAL>();
        return await userDAL.DeleteUserAsync(id, cancellationToken);
    }

    public async Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        var userDAL = unitOfWork.GetDAL<IUserDAL>();
        return await userDAL.GetUserByIdAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        using var unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        var userDAL = unitOfWork.GetDAL<IUserDAL>();
        return await userDAL.GetUsersAsync(cancellationToken);
    }

    //Optional Update for UserDetails
    public async Task<bool> UpdateUserAsync(Guid id,User user, CancellationToken cancellationToken = default)
    {
        using var unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        var userDAL = unitOfWork.GetDAL<IUserDAL>();

        ArgumentNullException.ThrowIfNull(user, nameof(user));
        user.Id = id;

        var result  = await userDAL.UpdateUserAsync(user, cancellationToken);
        if(!result)
        {
            return false;
        }

        if(user.UserDetail is not null)
        {
            user.UserDetail.UserId = user.Id;
            result = await userDAL.UpdateUserDetailAsync(user.UserDetail, cancellationToken);
        }
        if(!result)
        {
            return false;
        }

        return true;
    }

}

