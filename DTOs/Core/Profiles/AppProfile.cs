using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using DTOs.Dtos.CreateRequestDtos;
using DTOs.Dtos.GetResponseDtos;

namespace DTOs.Core.Profiles;
public class AppProfile : Profile
{
    public AppProfile()
    {
        // Replaced App Profiles with [AutoMap(typeof(User), ReverseMap = true)] in each DTO for less complex mapping.

        //Source --> Target
        //CreateMap<User, GetUserResponseDto>();
        //CreateMap<CreateUserRequestDto, User>();
        //CreateMap<UserDetail, GetUserDetailResponseDto>();
        //CreateMap<CreateUserDetailRequestDto, UserDetail>();
    }
}
