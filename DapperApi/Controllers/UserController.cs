using System.ComponentModel.DataAnnotations;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Models;
using DTOs.Dtos.CreateRequestDtos;
using DTOs.Dtos.GetResponseDtos;
using Microsoft.AspNetCore.Mvc;

namespace DapperApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    private readonly IMapper mapper;

   
    public UserController(IUserService userService, IMapper mapper)
    {
        this.userService = userService;
        this.mapper = mapper;
    }

   [HttpGet]
   public async Task<IActionResult> GetUsersAsync(CancellationToken cancellationToken = default)
   {
       var model = await userService.GetUsersAsync(cancellationToken);
       if(model is null)
       {
           return NotFound();
       }
       return Ok(mapper.Map<IEnumerable<GetUserResponseDto>>(model));
   }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var model = await userService.GetUserByIdAsync(id, cancellationToken);

        if(model is null)
        {
            return NotFound();
        }
        return Ok(mapper.Map<GetUserResponseDto>(model));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequestDto newUser, CancellationToken cancellationToken = default)
    {
        var userModel = mapper.Map<User>(newUser);

        var result = await userService.CreateUserAsync(userModel, cancellationToken);
        if(result)
        {
            var userResponse = mapper.Map<GetUserResponseDto>(userModel);
            userResponse.Roles = mapper.Map<IEnumerable<GetRoleResponseDto>>(userModel.Roles);
            //userResponse.UserDetail = mapper.Map<GetUserDetailResponseDto>(userModel.UserDetail);
           // userResponse.UserType = userModel.UserType;
            return Ok(userResponse);
        }
        return BadRequest();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exist = await userService.CheckIfExistsAsync(id, cancellationToken);
        if(exist)
        {
          var result = await userService.DeleteUserAsync(id, cancellationToken);
          return Ok(result);
        }
        return NotFound();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserAsync(
        [Required] Guid id,
        [FromBody, Required] CreateUserRequestDto updatedUser, 
        CancellationToken cancellationToken = default)
    {
        var exists = await userService.CheckIfExistsAsync(id, cancellationToken);
        if(exists)
        {
            var userModel = mapper.Map<User>(updatedUser);
            var result = await userService.UpdateUserAsync(id, userModel, cancellationToken);
            return Ok(result);
        }
        return BadRequest();
    }

}
