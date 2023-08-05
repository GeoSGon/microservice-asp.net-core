using Microsoft.EntityFrameworkCore;
using userService.Domain.Entities;
using userService.Infra.Context;
using userService.Domain.Repositories.Interfaces;

namespace userService.Infra.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _context.Users
                .AsNoTracking()
                .ToListAsync();
    }

    public async Task<User> GetById(int id)
    {
        return await _context.Users
                .Where(u => u.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
    }

    public async Task<User> Create(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> Update(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> Delete(int id)
    {
        var user = await GetById(id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> Authenticate(User user)
    {
        return await _context.Users.AsNoTracking()
            .Where(u => u.Username == user.Username && u.Password == user.Password)
            .FirstOrDefaultAsync();
    }
}