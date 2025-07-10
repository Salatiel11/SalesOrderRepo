using Core.Models;
using Microsoft.EntityFrameworkCore;
using SalesOrderApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }


        public class ProductService : IProductService
        {
            private readonly AppDbContext _context;

            public ProductService(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Product>> GetAllAsync()
            {
                return await _context.Products
                    .AsNoTracking()
                    .ToListAsync();
            }

            public async Task<Product> GetAsync(int id)
            {
                return await _context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);
            }

            public async Task<bool> ExistsAsync(int id)
            {
                return await _context.Products
                    .AnyAsync(p => p.Id == id);
            }

            public async Task<int> AddAsync(Product product)
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return product.Id;
            }

            public async Task<bool> UpdateAsync(Product product)
            {
            var trackedEntity = await _context.Products.FindAsync(product.Id);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).CurrentValues.SetValues(product);
            }
            else
            {
                _context.Products.Attach(product);
                _context.Entry(product).State = EntityState.Modified;
            }
            
                return await _context.SaveChangesAsync() > 0;
            }

            public async Task<bool> DeleteAsync(int id)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null) return false;

                _context.Products.Remove(product);
                return await _context.SaveChangesAsync() > 0;
            }
        }
    
}
