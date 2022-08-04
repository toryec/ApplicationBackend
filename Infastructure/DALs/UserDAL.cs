using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.DALs;
using Domain.DALs.Interface;
using Domain.Models;

namespace Infastructure.DALs;
public class UserDAL : BaseDal, IUserDAL
{
    public UserDAL(DbConnection dbConnection) : base(dbConnection)
    {
    }

    public async Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql = "INSERT INTO Users (Id, UserName, Password) VALUES (UPPER(@Id), @UserName, @Password)";
        var parameters = new DynamicParameters();
        parameters.Add("Id", user.Id, DbType.String);
        parameters.Add("UserName", user.UserName, DbType.String);
        parameters.Add("Password", user.Password, DbType.String);
        
        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        //var command = new CommandDefinition(sql, user, cancellationToken: cancellationToken);
        var result = await DbConnection.ExecuteAsync(command);
        return result > 0;
        
    }

    public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM Users WHERE Id = UPPER(@Id)";
        var command = new CommandDefinition(sql, new { Id = id.ToString() }, cancellationToken: cancellationToken);

        var result = await DbConnection.ExecuteAsync(command);
        return result > 0;
    }

    public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT * FROM Users";

        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
        
        return await DbConnection.QueryAsync<User>(command);
    }

    public async Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT * FROM Users WHERE Id = UPPER(@Id)";

        var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
 
        return await DbConnection.QuerySingleOrDefaultAsync<User>(command);

    }

    public async Task<bool> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql = "UPDATE Users SET UserName = @UserName, Password = @Password  WHERE Id = UPPER(@Id)";
        //var parameters = new DynamicParameters();
        //parameters.Add("UserName", user.UserName, DbType.String);
        //parameters.Add("Password", user.Password, DbType.String);
        //parameters.Add("Role", user.Role, DbType.String);
        //parameters.Add("Id", user.Id, DbType.String);
        //var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        var command = new CommandDefinition(sql, user, cancellationToken: cancellationToken);
        var result = await DbConnection.ExecuteAsync(command); 
        return result > 0;
    }

    public async Task<bool> CheckIfExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT COUNT (*) FROM Users WHERE Id = UPPER(@Id)";
        var command = new CommandDefinition(sql, new { Id = id.ToString() }, cancellationToken: cancellationToken);

        var result = await DbConnection.ExecuteScalarAsync<int>(command);
        return result > 0;
    }

    public async Task<bool> InsertUserDetailAsync(UserDetail userDetail, CancellationToken cancellationToken = default)
    {
        const string sql = "INSERT INTO UserDetails (UserId, FirstName, LastName, Age) VALUES (UPPER(@UserId), @FirstName, @LastName, @Age)";
        //var parameter = new DynamicParameters();
        //parameter.Add("UserId", userDetail.UserId, DbType.String);
        //parameter.Add("FirstName", userDetail.FirstName, DbType.String);
        //parameter.Add("LastName", userDetail.LastName, DbType.String);
        //parameter.Add("Age", userDetail.Age, DbType.Int32);

        //var command = new CommandDefinition(sql, parameter, cancellationToken: cancellationToken);
        var command = new CommandDefinition(sql, userDetail, cancellationToken: cancellationToken);
        var result = await DbConnection.ExecuteAsync(command);
        return result > 0;
    }

    public async Task<bool> UpdateUserDetailAsync(UserDetail userDetail, CancellationToken cancellationToken = default)
    {
        const string sql = "UPDATE UserDetails SET FirstName = @FirstName, LastName = @LastName, Age = @Age WHERE UserId = UPPER(@UserId)";
        //var parameter = new DynamicParameters();
        //parameter.Add("FirstName", userDetail.FirstName, DbType.String);
        //parameter.Add("LastName", userDetail.LastName, DbType.String);
        //parameter.Add("Age", userDetail.Age, DbType.Int32);
        //parameter.Add("UserId", userDetail.UserId, DbType.String);

        //var command = new CommandDefinition(sql, parameter, cancellationToken: cancellationToken);
        var command = new CommandDefinition(sql, userDetail, cancellationToken: cancellationToken);
        var result = await DbConnection.ExecuteAsync(command);
        return result > 0;
    }

    public async Task<bool> InsertUserTypeAsync(UserType userType, CancellationToken cancellationToken = default)
    {
        const string sql = "INSERT INTO UserTypes (Id,Type) VALUES (@Id, @Type)";
        
        var parameter = new DynamicParameters();
        parameter.Add("Id", (int)userType);
        parameter.Add("Type", userType);
        var command = new CommandDefinition(sql, parameter, cancellationToken: cancellationToken);
        var result = await DbConnection.ExecuteAsync(command);
        return result > 0;
    }

    public async Task<bool> InsertRoleAsync(IEnumerable<Role> roles, CancellationToken cancellationToken = default)
    {
        const string sql = "INSERT INTO Roles (Id, RoleCode) VALUES (@Id, @RoleCode)";
        var command = new CommandDefinition(sql, roles, cancellationToken: cancellationToken);
        var result = await DbConnection.ExecuteAsync(command);
        return result > 0;
    }

    public Task<UserDetail> GetUserDetailAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserType> GetUserTypeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Role>> GetRoleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

   
}
