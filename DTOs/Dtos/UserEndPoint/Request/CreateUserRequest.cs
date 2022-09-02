using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace DTOs.Dtos.UserEndPoint.Request;

[AutoMap(typeof(User), ReverseMap = true)]
public class CreateUserRequest
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
   // public IEnumerable<CreateUserRoleRequest> UserRoles { get; set; } = default!;
    public CreateUserDetailRequest UserDetail { get; set; } = default!;
}
