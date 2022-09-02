using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace DTOs.Dtos.UserEndPoint.Response;

[AutoMap(typeof(User))]
public class GetUserResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public IEnumerable<GetUserRoleResponse> UserRoles { get; set; } = default!;
    public GetUserDetailResponse UserDetail { get; set; } = default!;
}
