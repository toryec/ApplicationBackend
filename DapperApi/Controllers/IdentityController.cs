using Application.Services.Interfaces;
using AutoMapper;
using Domain.Models;
using DTOs.Dtos.CreateRequestDtos;
using DTOs.Dtos.GetResponseDtos;
using Microsoft.AspNetCore.Mvc;

namespace DapperApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IUserService userService;
    private readonly IMapper mapper;

    public IdentityController(IUserService userService, IMapper mapper)
    {
        this.userService = userService;
        this.mapper = mapper;
    }

    /*[HttpPost("Authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] GetUserRequestDto User, CancellationToken cancellationToken = default)
    {
        
        return await BadRequest();
    }*/

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequestDto newUser, CancellationToken cancellationToken = default)
    {
        var userModel = mapper.Map<User>(newUser);

        //var userDetailsModel = mapper.Map<UserDetail>(newUser.UserDetail);
        //userModel.UserDetail = userDetailsModel;

        var result = await userService.CreateUserAsync(userModel, cancellationToken);
        if(result)
        {
            var userResponse = mapper.Map<GetUserResponseDto>(userModel);
            return Ok(userResponse);
        }
        return BadRequest();   
    }

}
