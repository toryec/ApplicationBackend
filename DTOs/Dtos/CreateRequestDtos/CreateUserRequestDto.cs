using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace DTOs.Dtos.CreateRequestDtos;

[AutoMap(typeof(User), ReverseMap = true)]
public class CreateUserRequestDto
{
    [Required]
    public string UserName { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;

    public IEnumerable<CreateRoleRequestDto>? Roles { get; set; }

    //[Required]
    //public byte[] Salt { get; set; } = default!;
    public CreateUserDetailRequestDto? UserDetail { get; set; }
    public UserType UserType { get; set; }
}
