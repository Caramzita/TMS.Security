using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TMS.Security.Core;
using TMS.Security.DataAccess.Dto;
using TMS.Security.UseCases.Abstractions;

namespace TMS.Security.DataAccess.Repositories;

/// <summary>
/// Реализация <see cref="IRefreshTokenRepository"/>.
/// </summary>
public class RefreshTokenRepository : IRefreshTokenRepository
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
    /// Инициализирует новый экземпляр класса <see cref="RefreshTokenRepository"/>.
    /// </summary>
    /// <param name="context"> Контекст базы данных. </param>
    /// <param name="mapper"> Сервис для маппинга объектов. </param>
    public RefreshTokenRepository(DataBaseContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        var entity = await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Token == token)
            .ConfigureAwait(false);

        return _mapper.Map<RefreshToken>(entity); 
    }

    /// <inheritdoc/>
    public async Task CreateAsync(RefreshToken token)
    {
        var entity = _mapper.Map<RefreshTokenDto>(token);

        await _context.RefreshTokens.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task DeactivateAsync(RefreshToken token)
    {
        token.Deactivate();
        var entity = _mapper.Map<RefreshTokenDto>(token);

        _context.RefreshTokens.Update(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task DeactivateAllTokensAsync(Guid userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(x => x.UserId == userId)
            .ToListAsync()
            .ConfigureAwait(false);

        tokens.ForEach(x => x.IsUsed = true);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
