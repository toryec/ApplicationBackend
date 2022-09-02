using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;

namespace DTOs.Core.Profiles;
public class AppProfile : Profile
{
    public AppProfile()
    {
        // Replaced App Profiles with [AutoMap(typeof(User), ReverseMap = true)] in each DTO for less complex mapping.

        //Source --> Target
        //CreateMap<User, GetUserResponse>();
        //CreateMap<CreateUserRequest, User>();
        //CreateMap<UserDetail, GetUserDetailResponse>();
        //CreateMap<CreateUserDetailRequest, UserDetail>();
    }
}
