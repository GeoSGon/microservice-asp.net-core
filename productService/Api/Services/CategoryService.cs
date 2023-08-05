using AutoMapper;
using productService.Api.DTOs;
using productService.Domain.Entities;
using productService.Domain.Repositories.Interfaces;
using productService.Api.Services.Interfaces;

namespace productService.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    
    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDTO>> GetAll()
    {
        var categoriesEntity = await _categoryRepository.GetAll();
        return _mapper.Map<IEnumerable<CategoryDTO>>(categoriesEntity);
    }

    public async Task<IEnumerable<CategoriesWithProductsDTO>> GetWithProducts()
    {
        var categoriesEntity = await _categoryRepository.GetWithProducts();
        return _mapper.Map<IEnumerable<CategoriesWithProductsDTO>>(categoriesEntity);
    }

    public async Task<CategoryDTO> GetById(int id)
    {
        var categoryEntity = await _categoryRepository.GetById(id);
        return _mapper.Map<CategoryDTO>(categoryEntity);
    }

    public async Task Add(CategoryDTO categoryDTO)
    {
        var categoryEntity = _mapper.Map<Category>(categoryDTO);
        await _categoryRepository.Create(categoryEntity);
    }

    public async Task Update(CategoryDTO categoryDTO)
    {
        var categoryEntity = _mapper.Map<Category>(categoryDTO);
        await _categoryRepository.Update(categoryEntity);
    }
    
    public async Task Remove(int id)
    {
        var categoryEntity = await GetById(id);
        await _categoryRepository.Delete(categoryEntity.Id);
    }      
}
