using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace productService.Api.DTOs;

public class CategoryDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [MaxLength(60, ErrorMessage = "This field must be between 3 and 60 characters.")]
    [MinLength(3, ErrorMessage = "This field must be between 3 and 60 characters.")]
    public string? Name { get; set; }
    
    [JsonIgnore]
    public ICollection<ProductDTO>? Products { get; set; }
}