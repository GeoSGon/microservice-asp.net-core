using Microsoft.EntityFrameworkCore;
using productService.Domain.Entities;
using productService.Infra.Context;
using productService.Domain.Repositories.Interfaces;

namespace productService.Infra.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
        return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
    }

    public async Task<Product> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Update(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Delete(int id)
    {
        var product = await GetById(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return product;
    }
}