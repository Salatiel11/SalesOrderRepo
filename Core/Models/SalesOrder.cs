using Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Core.Models.Customer;


namespace Core.Models;

[Table("SalesOrders")]
public class SalesOrder : EntityBase
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("SalesOrderId")]
    public int Id { get; set; }

    [Required]
    [Column("OrderDate")]
    public new DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public decimal TotalAmount { get; set; }


    public Customer Customer { get; set; } = null!;
    public ICollection<SalesOrderLine> OrderLines { get; set; } = new List<SalesOrderLine>();

    
    
}