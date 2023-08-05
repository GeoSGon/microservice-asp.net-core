using AutoMapper;
using userService.Api.DTOs;
using userService.Domain.Entities;
using userService.Domain.Repositories.Interfaces;
using userService.Api.Services.Interfaces;

namespace userService.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;
    
    public UserService(IUserRepository userRepository, IMapper mapper, TokenService tokenService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<IEnumerable<UserDTO>> GetAll()
    {
        var usersEntity = await _userRepository.GetAll();
        return _mapper.Map<IEnumerable<UserDTO>>(usersEntity);
    }

    public async Task<UserDTO> GetById(int id)
    {
        var userEntity = await _userRepository.GetById(id);
        return _mapper.Map<UserDTO>(userEntity);
    }

    public async Task Add(UserDTO userDTO)
    {
        var userEntity = _mapper.Map<User>(userDTO);
        await _userRepository.Create(userEntity);
    }

    public async Task Update(UserDTO userDTO)
    {
        var userEntity = _mapper.Map<User>(userDTO);
        await _userRepository.Update(userEntity);
    }
    
    public async Task Remove(int id)
    {
        var userEntity = await GetById(id);
        await _userRepository.Delete(userEntity.Id);
    }      

    public async Task<string> Authenticate(UserLoginDTO userLoginDTO)
    {
        var userEntity = _mapper.Map<User>(userLoginDTO);
        await _userRepository.Authenticate(userEntity);
        var token = _tokenService.GenerateToken(userLoginDTO);
        return token;
    }
}
