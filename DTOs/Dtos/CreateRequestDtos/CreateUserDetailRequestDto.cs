using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace DTOs.Dtos.CreateRequestDtos;
[AutoMap(typeof(UserDetail), ReverseMap = true)]
public class CreateUserDetailRequestDto
{
    [Required]
    public string FirstName { get; set; } = default!;

    [Required]
    public string LastName { get; set; } = default!;

    [Required]
    public int Age { get; set; }
}
