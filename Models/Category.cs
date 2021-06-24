using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Products.Models
{
    public class Category
    {
        [Key]
        public int CatagoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public List<Association> Associations { get; set; }
    }
}