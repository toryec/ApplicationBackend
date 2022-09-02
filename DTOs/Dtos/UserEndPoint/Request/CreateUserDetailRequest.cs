using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace DTOs.Dtos.UserEndPoint.Request;

[AutoMap(typeof(UserDetail), ReverseMap = true)]
public class CreateUserDetailRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Age { get; set; }
}
