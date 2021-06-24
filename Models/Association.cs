using System.ComponentModel.DataAnnotations;

namespace Products.Models
{
    public class Association
    {
        [Key]
        public int AssociationId { get; set; }
        public int ProductId { get; set; }
        public int CatagoryId { get; set; }
        public Product Product { get; set; }
        public Category Catagory { get; set; }
    }
}