using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Application.Services.Interfaces;
using Domain.Core.Interfaces;
using Domain.DALs.Interface;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Types;
public class UserService : IUserService
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;
    private readonly IPasswordHasher<User> passwordHasher;

    public UserService(IUnitOfWorkFactory unitOfWorkFactory, IPasswordHasher<User> passwordHasher)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
        this.passwordHasher = passwordHasher;
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
        await unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);
        var userDAL = unitOfWork.GetDAL<IUserDAL>();

        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var exists = await userDAL.CheckUserNameExistsAsync(user.UserName, cancellationToken);
        if(exists)
        {
            return false;
        }

        user.Id = Guid.NewGuid();
        var hashedPassword = passwordHasher.HashPassword(user, user.Password);
        user.Password = hashedPassword;

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

        // Set the userRole Manually
        user.UserRoles = new List<UserRole>
        {
            new UserRole{UserId = user.Id, RoleId = 3}
        };

        result = await userDAL.InsertRoleAsync(user.UserRoles, cancellationToken);

        //if(user.UserRoles?.Any() == true)
        //{
        //    foreach (var item in user.UserRoles)
        //    {
        //        item.UserId = user.Id;
        //        item.RoleId = 3;
        //    }
        //    result = await userDAL.InsertRoleAsync(user.UserRoles, cancellationToken);
        //}

        if (!result)
        {
            return false;
        }

        await unitOfWork.CommitAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        var userDAL = unitOfWork.GetDAL<IUserDAL>();
        return await userDAL.DeleteUserAsync(id, cancellationToken);
    }

    public async Task<User> GetUserAsync(Guid id, bool includeRoles = false, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        using var unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        var userDAL = unitOfWork.GetDAL<IUserDAL>();

        var user =  await userDAL.GetUserAsync(id, cancellationToken);

        if (includeRoles)
        {
            user.UserRoles = await userDAL.GetRoleAsync(id, cancellationToken);
        }

        if (includeDetails)
        {
            user.UserDetail = await userDAL.GetUserDetailAsync(id, cancellationToken);
        }

        return user;
    }

    public async Task<IEnumerable<User>> GetUsersAsync(string? userName = null, CancellationToken cancellationToken = default)
    {
        using var unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        var userDAL = unitOfWork.GetDAL<IUserDAL>();
        return await userDAL.GetUsersAsync(userName, cancellationToken: cancellationToken);
    }

    //Optional Update for UserDetails
    public async Task<bool> UpdateUserAsync(Guid id, User user, CancellationToken cancellationToken = default)
    {
        using var unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        await unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);
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

        await unitOfWork.CommitAsync(cancellationToken);

        return true;
    }

}

