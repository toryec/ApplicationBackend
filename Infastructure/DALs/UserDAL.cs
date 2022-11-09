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

    // CREATE USER
    public async Task<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql = "INSERT INTO User (Id, UserName, Password) VALUES (UPPER(@Id), @UserName, @Password)";
        var parameters = new DynamicParameters();
        parameters.Add("Id", user.Id, DbType.String);
        parameters.Add("UserName", user.UserName, DbType.String);
        parameters.Add("Password", user.Password, DbType.String);
        
        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        var result = await DbConnection.ExecuteAsync(command);
        return result > 0;
        
    }

    // DELETE USER
    public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "DELETE FROM User WHERE Id = UPPER(@Id)";
        var command = new CommandDefinition(sql, new { Id = id.ToString() }, cancellationToken: cancellationToken);

        var result = await DbConnection.ExecuteAsync(command);
        return result > 0;
    }

    //GET ALL USERS
    public async Task<IEnumerable<User>> GetUsersAsync(string? userName = null, CancellationToken cancellationToken = default)
    {
        //const string sql = "SELECT * FROM Users";

        //var command = new CommandDefinition(sql, cancellationToken: cancellationToken);

        //return await DbConnection.QueryAsync<User>(command);

        //var ss = "SELECT Id, UserName FROM User Where UserName LIKE %test% COLLATE NOCASE";

        const string sql = "SELECT Id, UserName FROM User /**where**/";
        var builder = new SqlBuilder();

        if (!string.IsNullOrWhiteSpace(userName))
        {
            builder.Where($"UserName LIKE @{nameof(userName)} COLLATE NOCASE", new { userName = $"%{userName}%" });
        }

        var template = builder.AddTemplate(sql);
        var command = new CommandDefinition(template.RawSql, template.Parameters, cancellationToken: cancellationToken);
        var result = await DbConnection.QueryAsync<User>(command);
        return result;
    }

    // GET USER
    public async Task<User> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT Id, UserName FROM User WHERE Id = UPPER(@Id)";

        var command = new CommandDefinition(sql, new { Id = id.ToString() }, cancellationToken: cancellationToken);
 
        return await DbConnection.QuerySingleOrDefaultAsync<User>(command);

    }

    // UPDATE USER
    public async Task<bool> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql = "UPDATE User SET UserName = @UserName, Password = @Password  WHERE Id = UPPER(@Id)";

        var command = new CommandDefinition(sql, user, cancellationToken: cancellationToken);
        var result = await DbConnection.ExecuteAsync(command); 
        return result > 0;
    }

    // USER DETAIL
    public async Task<bool> InsertUserDetailAsync(UserDetail userDetail, CancellationToken cancellationToken = default)
    {
        const string sql = "INSERT INTO UserDetail (UserId, FirstName, LastName, Age) VALUES (UPPER(@UserId), @FirstName, @LastName, @Age)";

        var command = new CommandDefinition(sql, userDetail, cancellationToken: cancellationToken);
        var result = await DbConnection.ExecuteAsync(command);
        return result > 0;
    }

    public async Task<bool> UpdateUserDetailAsync(UserDetail userDetail, CancellationToken cancellationToken = default)
    {
        const string sql = "UPDATE UserDetail SET FirstName = @FirstName, LastName = @LastName, Age = @Age WHERE UserId = UPPER(@UserId)";
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
    public async Task<UserDetail> GetUserDetailAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT * FROM UserDetail WHERE UserId = @UserId COLLATE NOCASE";
        var command = new CommandDefinition(sql, new { UserId = id }, cancellationToken: cancellationToken);
        return await DbConnection.QuerySingleOrDefaultAsync<UserDetail>(command);

    }
   

    //ROLE
    public async Task<bool> InsertRoleAsync(IEnumerable<UserRole> userRoles, CancellationToken cancellationToken = default)
    {
        const string sql = "INSERT INTO UserRole (RoleId, UserId) VALUES (@RoleId, UPPER(@UserId))";
        var command = new CommandDefinition(sql, userRoles, cancellationToken: cancellationToken);
        var result = await DbConnection.ExecuteAsync(command);
        return result > 0;
    }

    public async Task<IEnumerable<UserRole>> GetRoleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT x.*, (SELECT RoleCode FROM Role WHERE Id = x.RoleId) RoleCode 
            FROM UserRole x 
            WHERE UserId = UPPER(@Id)";

        var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
        return await DbConnection.QueryAsync<UserRole>(command);
    }

    // FILTER
    public async Task<bool> CheckIfExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT COUNT (*) FROM User WHERE Id = UPPER(@Id)";
        var command = new CommandDefinition(sql, new { Id = id.ToString() }, cancellationToken: cancellationToken);

        var result = await DbConnection.ExecuteScalarAsync<int>(command);
        return result > 0;
    }
    public async Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT Id, UserName FROM User WHERE UserName = @UserName";
        var command = new CommandDefinition(sql, new { UserName = userName }, cancellationToken: cancellationToken);
        return await DbConnection.QuerySingleOrDefaultAsync<User>(command);
    }

    public async Task<bool> CheckUserNameExistsAsync(string userName, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT COUNT (*) FROM User WHERE UserName = @UserName";
        var command = new CommandDefinition(sql, new { UserName = userName }, cancellationToken: cancellationToken);

        var result = await DbConnection.ExecuteScalarAsync<int>(command);
        return result > 0;
    }
}
