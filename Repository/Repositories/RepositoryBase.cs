using Microsoft.EntityFrameworkCore;
using Persistence.Abstracts;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class RepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity> where TContext : DbContext where TEntity : class
{
    private YapidromDbContext _context { get; }

    public RepositoryBase(YapidromDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate == null)
            return await _context.Set<TEntity>().ToListAsync();
        return await _context.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }
}