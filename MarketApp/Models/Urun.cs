using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MarketApp.Models
{
    [Table("Urunler")]
    public class Urun
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string UrunAd { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BirimFiyat { get; set; }
        [MaxLength(255)]
        public string Resim { get; set; }
    }
}
