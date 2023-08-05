using System.ComponentModel.DataAnnotations;

namespace productService.Api.DTOs;

public class CategoriesWithProductsDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [MaxLength(60, ErrorMessage = "This field must be between 3 and 60 characters.")]
    [MinLength(3, ErrorMessage = "This field must be between 3 and 60 characters.")]
    public string? Name { get; set; }
    public ICollection<ProductDTO>? Products { get; set; }
}
