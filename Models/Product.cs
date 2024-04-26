using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProntoChef.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Massimo 255 caratteri")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Massimo 1000 caratteri")]
        public string ProductImage { get; set; }

        [Required]
        [Range(1, 99, ErrorMessage = "Inerisci un prezzo")]
        public decimal ProductPrice { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Massimo 50 caratteri")]
        public string PreparationTime { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Massimo 1000 caratteri")]
        public string Ingredients {  get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Scegli un etnia")]
        public string Category { get; set; }


        public ICollection<OrderItem> OrderItems { get; set; }
    }
}