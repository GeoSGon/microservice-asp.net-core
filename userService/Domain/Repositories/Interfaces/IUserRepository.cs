using userService.Domain.Entities;

namespace userService.Domain.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(int id);
    Task<User> Create(User user);
    Task<User> Update(User user);
    Task<User> Delete(int id);
    Task<User> Authenticate(User user);
}