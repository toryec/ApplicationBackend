using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace DTOs.Dtos.GetResponseDtos;
[AutoMap(typeof(UserDetail))]
public class GetUserDetailResponseDto
{
    [Key]
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = default!;    
    public string LastName { get; set; } = default!;
    public int Age { get; set; }
}
