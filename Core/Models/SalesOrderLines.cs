using Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    [Table("SalesOrderLines")]
    public class SalesOrderLine 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
       
        [Column("SalesOrderLineId")]
        public int Id{ get; set; }
        [Column("SalesOrderId")]
        [ForeignKey("SalesOrder")]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("Product")]
        [Column("ProductId")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

      
        public SalesOrder Order { get; set; }
        public Product Product { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal LineTotal { get; private set; }
    }
}