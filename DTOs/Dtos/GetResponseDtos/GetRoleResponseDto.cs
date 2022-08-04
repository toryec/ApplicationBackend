using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace DTOs.Dtos.GetResponseDtos;
[AutoMap(typeof(Role))]
public class GetRoleResponseDto
{
    
    public Guid Id { get; set; }
    public string RoleCode { get; set; } = default!;
}
