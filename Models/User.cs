using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ProntoChef.Models
{
    [Authorize(Roles = "admin")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Massimo 255 caratteri")]
        public string Email { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Massimo 16 caratteri")]
        public string Password { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Massimo 30 caratteri")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Massimo 100 caratteri")]
        public string Surname { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Role { get; set; } = "user";

        public ICollection<OrderSummary> OrderSummaries { get; set; }
    }
}