using Ambev.DeveloperEvaluation.Domain.Interface.IRepositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly DefaultContext _context;

    public CustomerRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(int page, int size)
    {
        return await _context.Customers
            .Include(c => c.Branches)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task<int> GetTotalCustomersCountAsync()
    {
        return await _context.Customers.CountAsync();
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers
            .Include(c => c.Branches)
            .FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public async Task<Branch?> GetBranchByIdAsync(Guid branchId)
    {
        return await _context.Branches
            .FirstOrDefaultAsync(b => b.BranchId == branchId);
    }
}

