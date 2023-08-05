using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using productService.Api.DTOs;
using productService.Api.Services.Interfaces;
using productService.Api.Services.Caching.Interfaces;
using productService.Utils.Validation;

namespace productService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ICachingService _cache;
    private const string CACHE_COLLECTION_KEY = "AllProducts";
    public ProductsController(ICachingService cache, IProductService productService)
    {
        _cache = cache;
        _productService = productService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IResult> Get()
    {
        var productsCache = await _cache.Get(CACHE_COLLECTION_KEY);
        ProductDTO? productsDTOCache;

        if (!string.IsNullOrWhiteSpace(productsCache)) {
            productsDTOCache = JsonConvert.DeserializeObject<ProductDTO>(productsCache);

            return TypedResults.Ok(productsDTOCache);
        }

        var productsDTO = await _productService.GetAll();

        await _cache.Set(CACHE_COLLECTION_KEY, JsonConvert.SerializeObject(productsDTO));
        
        return productsDTO == null ? 
            TypedResults.NotFound("Products not found") : 
            TypedResults.Ok(productsDTO);
    }

    [HttpGet("v1/{id:int}")]
    [AllowAnonymous]
     public async Task<IResult> GetById(int id)
    {
        var productCache = await _cache.Get(id.ToString());
        ProductDTO? productDTO;

        if (!string.IsNullOrWhiteSpace(productCache)) {
            productDTO = JsonConvert.DeserializeObject<ProductDTO>(productCache);

            return TypedResults.Ok(productDTO);
        }

        if (Validation.IsInvalidId(id))
            return TypedResults.BadRequest("Id invalid");

        productDTO = await _productService.GetById(id);

        await _cache.Set(id.ToString(), JsonConvert.SerializeObject(productDTO));

        return productDTO == null ? 
            TypedResults.NotFound("Product not found") : 
            TypedResults.Ok(productDTO);
    }

    [HttpPost("create")]
    [Authorize(Policy = "ManagerOrEmployee")]
    public async Task<IResult> Post([FromBody] ProductDTO productDTO)
    {
        if (Validation.IsInvalidId(productDTO.Id))
            return TypedResults.BadRequest("Id invalid");

        await _productService.Add(productDTO);

        return productDTO == null ? 
            TypedResults.BadRequest("Data Invalid") :
            TypedResults.Created($"create/{productDTO.Id}", productDTO);
    }

    [HttpPut("update/v1/{id:int}")]
    [Authorize(Policy = "ManagerOrEmployee")]
    public async Task<IResult> Put(int id, [FromBody] ProductDTO productDTO)
    {
        if (id != productDTO.Id)
            return TypedResults.NotFound("Product not found");

        await _productService.Update(productDTO);

        return productDTO == null ? 
            TypedResults.BadRequest("Data Invalid") : 
            TypedResults.Ok(productDTO);
    }


    [HttpDelete("delete/v1/{id:int}")]
    [Authorize(Policy = "ManagerOrEmployee")]
    public async Task<IResult> Delete(int id)
    {
        var productDTO = await _productService.GetById(id);

        await _productService.Remove(id);

        return productDTO == null ? 
            TypedResults.NotFound("Product not found") : 
            TypedResults.Ok(productDTO);
    }
}