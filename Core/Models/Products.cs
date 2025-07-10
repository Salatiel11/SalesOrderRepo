using Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using static Core.Models.Customer;

namespace Core.Models;

[Table("Products")]
public class Product : EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ProductId")]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Column("Name")] // Mapea al nombre de columna en tu DB
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    [NotMapped]
    public string PriceString
    {
        get => Price.ToString("F2");
        set
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                Price = parsedValue;
            }
        }
    }
    // Navigation property
    public ICollection<SalesOrderLine> OrderLines { get; set; } = new List<SalesOrderLine>();
}