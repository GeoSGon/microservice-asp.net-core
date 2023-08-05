using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using userService.Api.DTOs;
using userService.Api.Services.Interfaces;
using userService.Api.Services.Caching.Interfaces;
using userService.Utils.Validation;

namespace userService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICachingService _cache;
    private const string CACHE_COLLECTION_KEY = "AllUsers";
    public UsersController(ICachingService cache, IUserService userService)
    {
        _cache = cache;
        _userService = userService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IResult> Get()
    {
        var userCache = await _cache.Get(CACHE_COLLECTION_KEY);
        UserDTO? usersDTOCache;

        if (!string.IsNullOrWhiteSpace(userCache)) {
            usersDTOCache = JsonConvert.DeserializeObject<UserDTO>(userCache);

            return TypedResults.Ok(usersDTOCache);
        }

        var usersDTO = await _userService.GetAll();

        await _cache.Set(CACHE_COLLECTION_KEY, JsonConvert.SerializeObject(usersDTO));

        return usersDTO == null ? 
            TypedResults.NotFound("Users not found") : 
            TypedResults.Ok(usersDTO);
    }

    [HttpGet("v1/{id:int}")]
    [AllowAnonymous]
    public async Task<IResult> GetById(int id)
    {
        var userCache = await _cache.Get(id.ToString());
        UserDTO? userDTO;

        if (!string.IsNullOrWhiteSpace(userCache)) {
            userDTO = JsonConvert.DeserializeObject<UserDTO>(userCache);

            return TypedResults.Ok(userDTO);
        }

        if (Validation.IsInvalidId(id))
            return TypedResults.BadRequest("Id invalid");

        userDTO = await _userService.GetById(id);

        await _cache.Set(id.ToString(), JsonConvert.SerializeObject(userDTO));

        return userDTO == null ? 
            TypedResults.NotFound("Users not found") : 
            TypedResults.Ok(userDTO);
    }

    [HttpPost("create")]
    [AllowAnonymous]
    public async Task<IResult> Post([FromBody] UserDTO userDTO)
    {
        if (Validation.IsInvalidId(userDTO.Id))
            return TypedResults.BadRequest("Id invalid");

        await _userService.Add(userDTO);

        return userDTO == null ? 
            TypedResults.BadRequest("Data Invalid") :
            TypedResults.Created($"create/{userDTO.Id}", userDTO);
    }

    [HttpPut("update/v1/{id:int}")]
    [Authorize(Policy = "ManagerOrEmployee")]
    public async Task<IResult> Put(int id, [FromBody] UserDTO userDTO)
    {
        if (id != userDTO.Id)
            return TypedResults.NotFound("User not found");

        await _userService.Update(userDTO);

        return userDTO == null ? 
            TypedResults.BadRequest("Data Invalid") : 
            TypedResults.Ok(userDTO);
    }

    [HttpDelete("delete/v1/{id:int}")]
    [Authorize(Policy = "ManagerOrEmployee")]
    public async Task<IResult> Delete(int id)
    {
        var userDTO = await _userService.GetById(id);

        await _userService.Remove(id);

        return userDTO == null ? 
            TypedResults.NotFound("User not found") : 
            TypedResults.Ok(userDTO);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IResult> Authenticate([FromBody] UserLoginDTO userLoginDTO)
    {
        var token = await _userService.Authenticate(userLoginDTO);

        return userLoginDTO == null ? 
            TypedResults.NotFound("User not found") : 
            TypedResults.Ok(new { Token = token });
    }
}