using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using productService.Api.DTOs;
using productService.Api.Services.Interfaces;
using productService.Api.Services.Caching.Interfaces;
using productService.Utils.Validation;
using Microsoft.Extensions.Logging;

namespace productService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ICachingService _cache;
    private readonly ILogger<CategoriesController> _logger;
    private const string CATEGORIES_CACHE_COLLECTION_KEY = "AllCategories";
    private const string CATEGORIES_WITH_PRODUCTS_CACHE_COLLECTION_KEY = "AllCategoriesWithProducts";

    public CategoriesController(ICachingService cache, ICategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _cache = cache;
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IResult> Get()
    {
        var categoriesCache = await _cache.Get(CATEGORIES_CACHE_COLLECTION_KEY);
        CategoryDTO? productsDTOCache;

        if (!string.IsNullOrWhiteSpace(categoriesCache)) {
            productsDTOCache = JsonConvert.DeserializeObject<CategoryDTO>(categoriesCache);

            return TypedResults.Ok(productsDTOCache);
        }
        
        var categoriesDTO = await _categoryService.GetAll();

        await _cache.Set(CATEGORIES_CACHE_COLLECTION_KEY, JsonConvert.SerializeObject(categoriesDTO));

        return categoriesDTO == null ? 
            TypedResults.NotFound("Categories not found") : 
            TypedResults.Ok(categoriesDTO);
    }

    [HttpGet("products")]
    [AllowAnonymous]
    public async Task<IResult> GetWithProducts()
    {
        var categoriesWithProductsCache = await _cache.Get(CATEGORIES_WITH_PRODUCTS_CACHE_COLLECTION_KEY);
        CategoriesWithProductsDTO? categoriesWithProductsDTOCache;

        if (!string.IsNullOrWhiteSpace(categoriesWithProductsCache)) {
            categoriesWithProductsDTOCache = JsonConvert.DeserializeObject<CategoriesWithProductsDTO>(categoriesWithProductsCache);

            return TypedResults.Ok(categoriesWithProductsDTOCache);
        }

        var categoriesWithProductsDTO = await _categoryService.GetWithProducts();

        await _cache.Set(CATEGORIES_WITH_PRODUCTS_CACHE_COLLECTION_KEY, JsonConvert.SerializeObject(categoriesWithProductsDTO));


        return categoriesWithProductsDTO == null ? 
            TypedResults.NotFound("Categories with products not found") : 
            TypedResults.Ok(categoriesWithProductsDTO);
    }

    [HttpGet("v1/{id:int}")]
    [AllowAnonymous]
    public async Task<IResult> GetById(int id)
    {
        var categoryCache = await _cache.Get(id.ToString());
        CategoryDTO? categoryDTO;

        if (!string.IsNullOrWhiteSpace(categoryCache)) {
            categoryDTO = JsonConvert.DeserializeObject<CategoryDTO>(categoryCache);

            return TypedResults.Ok(categoryDTO);
        }

        if (Validation.IsInvalidId(id))
            return TypedResults.BadRequest("Id invalid");

        categoryDTO = await _categoryService.GetById(id);

         await _cache.Set(id.ToString(), JsonConvert.SerializeObject(categoryDTO));

        return categoryDTO == null ? 
            TypedResults.NotFound("Category not found") : 
            TypedResults.Ok(categoryDTO);
    }

    [HttpPost("create")]
    [Authorize(Policy = "ManagerOrEmployee")]
    public async Task<IResult> Post([FromBody] CategoryDTO categoryDTO)
    {
        if (Validation.IsInvalidId(categoryDTO.Id))
            return TypedResults.BadRequest("Id invalid");

        await _categoryService.Add(categoryDTO);

        return categoryDTO == null ? 
            TypedResults.BadRequest("Data Invalid") :
            TypedResults.Created($"create/{categoryDTO.Id}", categoryDTO);
    }

    [HttpPut("update/v1/{id:int}")]
    [Authorize(Policy = "ManagerOrEmployee")]
    public async Task<IResult> Put(int id, [FromBody] CategoryDTO categoryDTO)
    {
        if (id != categoryDTO.Id)
            return TypedResults.NotFound("Category not found");

        await _categoryService.Update(categoryDTO);

        return categoryDTO == null ? 
            TypedResults.BadRequest("Data Invalid") : 
            TypedResults.Ok(categoryDTO);
    }

    [HttpDelete("delete/v1{id:int}")]
    [Authorize(Policy = "ManagerOrEmployee")]
    public async Task<IResult> Delete(int id)
    {
        var categoryDTO = await _categoryService.GetById(id);

        await _categoryService.Remove(id);

        return categoryDTO == null ? 
            TypedResults.NotFound("Category not found") : 
            TypedResults.Ok(categoryDTO);
    }
}