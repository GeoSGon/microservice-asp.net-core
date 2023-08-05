using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace productService.Api.DTOs;

public class ProductDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [MaxLength(60, ErrorMessage = "This field must be between 3 and 60 characters.")]
    [MinLength(3, ErrorMessage = "This field must be between 3 and 60 characters.")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "The price must be greater than zero.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [MaxLength(1024, ErrorMessage = "This field must contain a maximum of 1024 characters.")]
    public string? Description { get; set; }

    public string? CategoryName { get; set; }

    [JsonIgnore]
    public int CategoryId { get; set; }

    [JsonIgnore]
    public CategoryDTO? Category { get; set; }
}
