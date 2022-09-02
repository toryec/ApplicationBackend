using System.ComponentModel.DataAnnotations;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Models;
using DTOs.Dtos.UserEndPoint.Request;
using DTOs.Dtos.UserEndPoint.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperApi.Controllers;
//[Authorize(Roles = "User")]
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

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUsersAsync([FromQuery] GetUsersRequestFilter filter, CancellationToken cancellationToken = default)
    {
       var model = await userService.GetUsersAsync(filter.UserName, cancellationToken);
       if(model is null)
       {
           return NotFound();
       }
       return Ok(mapper.Map<IEnumerable<GetUsersResponse>>(model));
    }

    [ActionName(nameof(GetUserAsync))]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserAsync(Guid id, [FromQuery] GetUserRequestFilter filter, CancellationToken cancellationToken = default)
    {
        var model = await userService.GetUserAsync(id, filter.IncludeRoles, filter.IncludeDetails, cancellationToken);

        if(model is null)
        {
            return NotFound();
        }
        return Ok(mapper.Map<GetUserResponse>(model));
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
        [FromBody, Required] CreateUserRequest updatedUser, 
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
