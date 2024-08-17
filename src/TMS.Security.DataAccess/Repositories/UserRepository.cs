using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TMS.Security.Core;
using TMS.Security.DataAccess.Dto;
using TMS.Security.UseCases.Abstractions;

namespace TMS.Security.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataBaseContext _context;

    private readonly IMapper _mapper;

    public UserRepository(DataBaseContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false);

        return _mapper.Map<User>(entity);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username)
            .ConfigureAwait(false);

        return _mapper.Map<User>(entity);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }

        return await _context.Users
            .AnyAsync(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
            .ConfigureAwait(false);
    }

    public async Task<User> CreateAsync(User user)
    {
        var entity = _mapper.Map<UserDto>(user);

        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var entity = _mapper.Map<UserDto>(user);

        var existingEntity = await _context.Users.FindAsync(entity.Id);
        if (existingEntity == null)
        {
            throw new KeyNotFoundException($"User with ID {entity.Id} not found.");
        }

        _context.Users.Update(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task DeleteAsync(User user)
    {
        var entity = _mapper.Map<UserDto>(user);

        var existingEntity = await _context.Users.FindAsync(entity.Id);
        if (existingEntity == null)
        {
            throw new KeyNotFoundException($"User with ID {entity.Id} not found.");
        }

        _context.Users.Remove(existingEntity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
