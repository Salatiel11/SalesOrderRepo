using Core.Models;
using Microsoft.EntityFrameworkCore;
using SalesOrderApp.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface ISalesOrderService
    {
        Task<IEnumerable<SalesOrder>> GetAllAsync();
        Task<SalesOrder> GetAsync(int id);
        Task<int> AddAsync(SalesOrder order);
        Task<bool> UpdateAsync(SalesOrder order);
        Task<bool> DeleteAsync(int id);
        Task<SalesOrder> GetWithItemsAsync(int id);
        Task AddOrderLinesAsync(IEnumerable<SalesOrderLine> lines);
        Task UpdateOrderLinesAsync(IEnumerable<SalesOrderLine> lines);
    }

    public class SalesOrderService : ISalesOrderService
    {
        private readonly AppDbContext _context;

        public SalesOrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalesOrder>> GetAllAsync()
        {
            return await _context.SalesOrders
                .Include(o => o.Customer)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SalesOrder> GetAsync(int id)
        {
            return await _context.SalesOrders
                .Include(o => o.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<SalesOrder> GetWithItemsAsync(int id)
        {
            return await _context.SalesOrders
                .Include(o => o.Customer)
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<int> AddAsync(SalesOrder order)
        {
            SalesOrder salesOrder = new SalesOrder
            {
                CustomerId = order.CustomerId,
                TotalAmount = order.TotalAmount
            };
            await _context.SalesOrders.AddAsync(salesOrder);
            await _context.SaveChangesAsync();
            return salesOrder.Id;
        }

        public async Task<bool> UpdateAsync(SalesOrder order)
        {
            var trackedEntity = await _context.SalesOrders.FindAsync(order.Id);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).CurrentValues.SetValues(order);
            }
            else
            {
                _context.SalesOrders.Attach(order);
                _context.Entry(order).State = EntityState.Modified;
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.SalesOrders.FindAsync(id);
            var orderLine = _context.SalesOrderLines.Where(x => x.OrderId == order.Id);
            foreach (var line in orderLine)
            {
                _context.SalesOrderLines.Remove(line);
            }
            _context.SalesOrders.Remove(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task AddOrderLinesAsync(IEnumerable<SalesOrderLine> lines)
        {
            foreach (var line in lines)
            {
                SalesOrderLine newSalesOrderLine = new()
                {
                    OrderId = line.OrderId,
                    ProductId = line.ProductId,
                    Quantity = line.Quantity,
                    UnitPrice = line.UnitPrice
                };
                _context.SalesOrderLines.Add(newSalesOrderLine);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateOrderLinesAsync(IEnumerable<SalesOrderLine> lines)
        {
            foreach (var line in lines)
            {
                var existingEntity = await _context.SalesOrderLines.AsNoTracking().FirstOrDefaultAsync(l => l.Id == line.Id);

                if (existingEntity != null)
                {
                    
                    var trackedEntity = _context.ChangeTracker.Entries<SalesOrderLine>()
                        .FirstOrDefault(e => e.Entity.Id == line.Id);

                    if (trackedEntity != null)
                    {
                        trackedEntity.State = EntityState.Detached;
                    }

                    _context.Entry(line).State = EntityState.Modified;
                }
                else
                {
                    SalesOrderLine newOrderLine = new SalesOrderLine
                    {
                        OrderId = line.OrderId,
                        ProductId = line.ProductId,
                        Quantity = line.Quantity,
                        UnitPrice = line.UnitPrice
                    };
                    _context.SalesOrderLines.Attach(newOrderLine);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}