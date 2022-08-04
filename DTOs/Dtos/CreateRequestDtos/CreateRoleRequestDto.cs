using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace DTOs.Dtos.CreateRequestDtos;
[AutoMap(typeof(Role), ReverseMap = true)]
public class CreateRoleRequestDto
{
    [Required]
    public string RoleCode { get; set; } = default!;
}
