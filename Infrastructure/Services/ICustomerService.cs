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
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetAsync(int id);
        Task<int> AddAsync(Customer customer);
        Task<bool> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Customer>> SearchAsync(string searchTerm);
    }

  
        public class CustomerService : ICustomerService
        {
            private readonly AppDbContext _context;

            public CustomerService(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Customer>> GetAllAsync()
            {
                return await _context.Customers
                    .AsNoTracking()
                    .ToListAsync();
            }

            public async Task<Customer> GetAsync(int id)
            {
                return await _context.Customers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);
            }

            public async Task<int> AddAsync(Customer customer)
            {
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                return customer.Id;
            }

            public async Task<bool> UpdateAsync(Customer customer)
            {
            var trackedEntity = await _context.Customers.FindAsync(customer.Id);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).CurrentValues.SetValues(customer);
            }
            else
            {
                _context.Customers.Attach(customer);
                _context.Entry(customer).State = EntityState.Modified;
            }
            return await _context.SaveChangesAsync() > 0;
        }

            public async Task<bool> DeleteAsync(int id)
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) return false;

                _context.Customers.Remove(customer);
                return await _context.SaveChangesAsync() > 0;
            }

            public async Task<IEnumerable<Customer>> SearchAsync(string searchTerm)
            {
                return await _context.Customers
                    .Where(c => c.Name.Contains(searchTerm) ||
                          c.Email.Contains(searchTerm))
                    .AsNoTracking()
                    .ToListAsync();
            }
        }
    
}
