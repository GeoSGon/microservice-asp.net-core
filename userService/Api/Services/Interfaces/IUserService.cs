using userService.Api.DTOs;

namespace userService.Api.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAll();
    Task<UserDTO> GetById(int id);
    Task Add(UserDTO userDTO);
    Task Update(UserDTO userDTO);
    Task Remove(int id);
    Task<string> Authenticate(UserLoginDTO userLoginDTO);
}