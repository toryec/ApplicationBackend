using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace DTOs.Dtos.UserEndPoint.Response;

[AutoMap(typeof(UserRole))]
public class GetUserRoleResponse
{
    public Guid UserId { get; set; }
    public int RoleId { get; set; }
    public string RoleCode { get; set; } = default!;
}
