using productService.Domain.Entities;

namespace productService.Domain.Repositories.Interfaces;

public interface ICategoryRepository 
{
    Task<IEnumerable<Category>> GetAll();
    Task<IEnumerable<Category>> GetWithProducts();
    Task<Category> GetById(int id);
    Task<Category> Create(Category category);
    Task<Category> Update(Category category);
    Task<Category> Delete(int id);
}