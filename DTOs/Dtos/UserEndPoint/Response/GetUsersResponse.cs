using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace DTOs.Dtos.UserEndPoint.Response;

[AutoMap(typeof(User))]
public class GetUsersResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;

}
