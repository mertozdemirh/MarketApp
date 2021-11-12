using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarketApp.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MarketApp.Controllers
{
    public class UrunlerController : Controller
    {
        private readonly UygulamaDbContext _context;
        private readonly IWebHostEnvironment env;

        public UrunlerController(UygulamaDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Urunler
        public async Task<IActionResult> Index()
        {
            return View(await _context.Urunler.ToListAsync());
        }

        // GET: Urunler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urun = await _context.Urunler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urun == null)
            {
                return NotFound();
            }

            return View(urun);
        }

        // GET: Urunler/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Urunler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UrunAd,BirimFiyat,Resim")] Urun urun, IFormFile dosya)
        {
            ResimGecerliliginiKontrolEt(dosya);
            if (ModelState.IsValid)
            {
                urun.Resim = ResimKaydet(dosya);
                _context.Add(urun);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(urun);
        }

        private void ResimGecerliliginiKontrolEt(IFormFile dosya)
        {
            if (dosya != null)
            {
                if (dosya.Length > 10 * 1000 * 1000)
                {
                    ModelState.AddModelError("Resim", "Görselin boyutu 10MB'dan büyük olamaz");
                }
                else if (!dosya.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("Resim", "Bir resim dosyası seçmediniz");
                }

            }
        }

        private string ResimKaydet(IFormFile dosya)
        {
            if (dosya == null || dosya.Length == 0) return null;

            string uzanti = Path.GetExtension(dosya.FileName);
            string yeniAd = Guid.NewGuid() + uzanti;
            string kayitYolu = Path.Combine(env.WebRootPath, "uploads", yeniAd);

            using (var fs = System.IO.File.Create(kayitYolu))
            {
                dosya.CopyTo(fs);
            }
            return yeniAd;
    
        }

        // GET: Urunler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var urun = await _context.Urunler.FindAsync(id);
            if (urun == null)
            {
                return NotFound();
            }
            return View(urun);
        }

        // POST: Urunler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UrunAd,BirimFiyat,Resim")] Urun urun, IFormFile dosya)
        {
            if (id != urun.Id)
            {
                return NotFound();
            }
            ResimGecerliliginiKontrolEt(dosya);

            if (ModelState.IsValid)
            {
                try
                {
                    if (dosya != null && dosya.Length>0)
                    {
                        ResimSil(urun.Resim);
                        urun.Resim = ResimKaydet(dosya);
                    }
                    _context.Update(urun);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrunExists(urun.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(urun);
        }

        // GET: Urunler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urun = await _context.Urunler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urun == null)
            {
                return NotFound();
            }

            return View(urun);
        }

        // POST: Urunler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var urun = await _context.Urunler.FindAsync(id);
            ResimSil(urun.Resim);
            _context.Urunler.Remove(urun);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void ResimSil(string resim)
        {
            if (string.IsNullOrEmpty(resim)) return;
            string dosyaYolu = Path.Combine(env.WebRootPath, "uploads", resim);
            System.IO.File.Delete(dosyaYolu);

        }

        private bool UrunExists(int id)
        {
            return _context.Urunler.Any(e => e.Id == id);
        }
    }
}
