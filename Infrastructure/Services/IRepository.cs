using Microsoft.EntityFrameworkCore;
using SalesOrderApp.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesOrderApp.Infrastructure.Services;

public interface IRepository<T> where T : class
{
    Task<T?> GetAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
public class EfRepository<T>(AppDbContext context) : IRepository<T> where T : class
{
    protected readonly AppDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<T?> GetAsync(int id) => await _dbSet.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
    public async Task UpdateAsync(T entity) => _dbSet.Update(entity);
    public async Task DeleteAsync(int id)
    {
        var entity = await GetAsync(id);
        if (entity != null) _dbSet.Remove(entity);
    }
    public async Task<bool> ExistsAsync(int id) => await _dbSet.AnyAsync(e => EF.Property<int>(e, "Id") == id);
    public async Task SaveAsync() => await _context.SaveChangesAsync();
}