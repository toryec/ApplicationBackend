using Application.Core.Interfaces;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Models;
using DTOs.Dtos.UserEndPoint.Request;
using DTOs.Dtos.UserEndPoint.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperApi.Controllers;
[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IUserService userService;
    private readonly IMapper mapper;
    private readonly IAuthenticationManager authenticationManager;

    public IdentityController(IUserService userService, IMapper mapper, IAuthenticationManager authenticationManager)
    {
        this.userService = userService;
        this.mapper = mapper;
        this.authenticationManager = authenticationManager;
    }

    [HttpPost("Authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] CreateUserRequest user, CancellationToken cancellationToken = default)
    {
        var userModel = mapper.Map<User>(user);

        var result = await authenticationManager.Authenticate(userModel, cancellationToken);
        if(result == null)
        {
            return NotFound();
        }
        if(result == "Bad Request")
        {
            return BadRequest();
        }
        return Accepted(result);

    }

    [HttpPost("Register")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest newUser, CancellationToken cancellationToken = default)
    {
        // AutoMapper Already makes the mapping
        var userModel = mapper.Map<User>(newUser);

        var result = await userService.CreateUserAsync(userModel, cancellationToken);
        if(result)
        {
            var userResponse = mapper.Map<GetUserResponse>(userModel);
            userResponse.UserDetail = mapper.Map<GetUserDetailResponse>(userModel.UserDetail);
            userResponse.UserRoles = mapper.Map<IEnumerable<GetUserRoleResponse>>(userModel.UserRoles);
            return CreatedAtAction(nameof(UserController.GetUserAsync),"User", new { Id = userResponse.Id }, userResponse);
        }
        return BadRequest();   
        
    }

}
