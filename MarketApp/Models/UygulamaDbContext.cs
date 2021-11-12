using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketApp.Models
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options)
        {

        }
        public DbSet<Urun> Urunler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Urun>().HasData(
                new Urun() { Id = 1, UrunAd = "Domates", BirimFiyat = 7.90m, Resim = "domates.jpg" },
                new Urun() { Id = 2, UrunAd = "Patlıcan", BirimFiyat = 12.90m, Resim = "patlican.jpg" }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
