using System.ComponentModel.DataAnnotations.Schema;

namespace productService.Domain.Entities;

[Table("Products")] 
public class Product
{
    [Column("Id")]  
    public int Id { get; private set; }

    [Column("Title")]  
    public string? Name { get; private set; }

    [Column("Price")] 
    public decimal Price { get; private set; }

    [Column("Description")] 
    public string? Description { get; private set; }

    [Column("CategoryId")]
    public int CategoryId { get; private set; }
    public Category? Category { get; private set; }
}