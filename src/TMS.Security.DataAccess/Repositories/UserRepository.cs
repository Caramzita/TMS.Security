using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TMS.Security.Core;
using TMS.Security.DataAccess.Dto;
using TMS.Security.UseCases.Abstractions;

namespace TMS.Security.DataAccess.Repositories;

/// <summary>
/// Реализация <see cref="IUserRepository"/>.
/// </summary>
public class UserRepository : IUserRepository
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly DataBaseContext _context;

    /// <summary>
    /// Маппер.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="UserRepository"/>.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    /// <param name="mapper">Сервис для маппинга объектов.</param>
    public UserRepository(DataBaseContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<User> GetByIdAsync(Guid id)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false);

        return _mapper.Map<User>(entity);
    }

    /// <inheritdoc/>
    public async Task<User> GetByUsernameAsync(string username)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username)
            .ConfigureAwait(false);

        return _mapper.Map<User>(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower())
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<User> CreateAsync(User user)
    {
        var entity = _mapper.Map<UserDto>(user);

        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();

        return user;
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(User user)
    {
        var entity = _mapper.Map<UserDto>(user);

        _context.Users.Update(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(User user)
    {
        var entity = _mapper.Map<UserDto>(user);

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
