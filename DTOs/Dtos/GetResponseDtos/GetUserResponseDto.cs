using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace DTOs.Dtos.GetResponseDtos;
[AutoMap(typeof(User))]
public class GetUserResponseDto
{
    [Key]
    public Guid Id { get; set; } 
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public IEnumerable<GetRoleResponseDto> Roles { get; set; } = default!;

    //public byte[] Salt { get; set; } = default!;
    public GetUserDetailResponseDto UserDetail { get; set; } = default!;
    public UserType UserType { get; set; }
}
