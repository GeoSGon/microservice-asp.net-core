using productService.Api.DTOs;

namespace productService.Api.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetAll();
    Task<IEnumerable<CategoriesWithProductsDTO>> GetWithProducts();
    Task<CategoryDTO> GetById(int id);
    Task Add(CategoryDTO categoryDTO);
    Task Update(CategoryDTO categoryDTO);
    Task Remove(int id);
}