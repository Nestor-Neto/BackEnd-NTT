using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interface.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    public SaleRepository(DefaultContext context){
        _context = context;
    }
    public async Task<Sale> AddAsync(Sale sale)
        {
           var result = await _context.Set<Sale>().AddAsync(sale);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Sale?> GetByIdAsync(Guid saleId)
        {
            return await _context.Set<Sale>()
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == saleId);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync(int page, int size)
        {
            return await _context.Set<Sale>()
                .Include(s => s.Items)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task UpdateAsync(Sale sale)
        {
            _context.Set<Sale>().Update(sale);
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Set<Sale>().CountAsync();
        }

        public async Task<Sale> CancelSaleAsync(Guid id)
        {
            var sale = await _context.Set<Sale>().FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null)
                throw new Exception("Sale not found!");
            sale.Cancelled = true; 

            var result = _context.Set<Sale>().Update(sale);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
}