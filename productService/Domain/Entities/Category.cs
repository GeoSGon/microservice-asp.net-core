using System.ComponentModel.DataAnnotations.Schema;

namespace productService.Domain.Entities;

[Table("Categories")]
public class Category
{
    [Column("Id")] 
    public int Id { get; private set; }

    [Column("Title")]
    public string? Name { get; private set; }
    public IList<Product>? Products { get; private set; }
}