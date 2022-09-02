using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Core.Interfaces;
using Domain.DALs.Interface;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Core.Types;
public class AuthenticationManager : IAuthenticationManager
{
    private readonly IUnitOfWorkFactory unitOfWorkFactory;
    private readonly IPasswordHasher<User> passwordHasher;
    private readonly IConfiguration configuration;

    public AuthenticationManager(IUnitOfWorkFactory unitOfWorkFactory, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
    {
        this.unitOfWorkFactory = unitOfWorkFactory;
        this.passwordHasher = passwordHasher;
        this.configuration = configuration;
    }

    public async Task<string?> Authenticate(User user, CancellationToken cancellationToken)
    {
        using var unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        var userDAL = unitOfWork.GetDAL<IUserDAL>();


        // Error Handling for Input String  
        if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
        {
            //throw new ArgumentNullException($"{nameof(email)} is not supposed to be null", $"{nameof(password)} is not supposed to be null");
            return "Bad Request";
        }

        var userModel =  await userDAL.GetByUserName(user.UserName,cancellationToken);
        if (userModel == null)
        {
            return null;
        }

        var verify = passwordHasher.VerifyHashedPassword(user, userModel.Password, user.Password);
        if (verify == PasswordVerificationResult.Failed)
        {
            return "Bad Request";
        }

        // Get Role for the User
        userModel.UserRoles = await userDAL.GetRoleAsync(userModel.Id, cancellationToken);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = configuration["AppSettings:TokenKey"];
        var key = Convert.FromBase64String(tokenKey);

        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, userModel.UserName));
        if(userModel.UserRoles?.Any() == true)
        {
            foreach (var role in userModel.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleCode));
            }
        }
       
        // 1
        //Add claims directly to identity
        //var tokenDescriptor = new SecurityTokenDescriptor
        //{
        //    Subject = new ClaimsIdentity(claims),
        //};

        // 2
       // var identity = new ClaimsIdentity();

        // 2.1
       // identity.AddClaims(claims);

        //OR

        //2.2
       // identity.AddClaims(userModel.Roles.Select(x => new Claim(ClaimTypes.Role, x.RoleCode)));


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), //identity,

            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);

    }
}
