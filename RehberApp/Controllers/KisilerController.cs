using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RehberApp.Data;
using RehberApp.Models;

namespace RehberApp.Controllers;

public class KisilerController : Controller
{
    private readonly RehberContext _context;

    public KisilerController(RehberContext context)
    {
        _context = context;
    }

    // Formlardaki birim açılır listesini doldurur
    private async Task BirimleriYukle(int? seciliBirimId = null)
    {
        var birimler = await _context.Birimler.OrderBy(b => b.Ad).ToListAsync();
        ViewBag.BirimId = new SelectList(birimler, "Id", "Ad", seciliBirimId);
    }

    // Liste
  // Liste, arama ve filtre
public async Task<IActionResult> Index(string? arama, int? birimId)
{
    var sorgu = _context.Kisiler
        .Include(k => k.Birim)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(arama))
    {
        var kelime = arama.Trim().ToLower();
        sorgu = sorgu.Where(k =>
            k.Ad.ToLower().Contains(kelime) ||
            k.Soyad.ToLower().Contains(kelime) ||
            k.Eposta.ToLower().Contains(kelime) ||
            k.Telefon.Contains(kelime) ||
            k.Unvan.ToLower().Contains(kelime) ||
            (k.Birim != null && k.Birim.Ad.ToLower().Contains(kelime)));
    }

    if (birimId.HasValue)
    {
        sorgu = sorgu.Where(k => k.BirimId == birimId.Value);
    }

    await BirimleriYukle(birimId);
    ViewBag.Arama = arama;

    var kisiler = await sorgu
        .OrderBy(k => k.Ad).ThenBy(k => k.Soyad)
        .ToListAsync();

    return View(kisiler);
}
// CSV indirme
public async Task<IActionResult> CsvIndir(string? arama, int? birimId)
{
    var sorgu = _context.Kisiler
        .Include(k => k.Birim)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(arama))
    {
        var kelime = arama.Trim().ToLower();
        sorgu = sorgu.Where(k =>
            k.Ad.ToLower().Contains(kelime) ||
            k.Soyad.ToLower().Contains(kelime) ||
            k.Eposta.ToLower().Contains(kelime) ||
            k.Telefon.Contains(kelime) ||
            k.Unvan.ToLower().Contains(kelime) ||
            (k.Birim != null && k.Birim.Ad.ToLower().Contains(kelime)));
    }

    if (birimId.HasValue)
    {
        sorgu = sorgu.Where(k => k.BirimId == birimId.Value);
    }

    var kisiler = await sorgu
        .OrderBy(k => k.Ad).ThenBy(k => k.Soyad)
        .ToListAsync();

    static string Temizle(string? deger)
    {
        var d = (deger ?? "").Replace("\"", "\"\"");
        return $"\"{d}\"";
    }

    var sb = new System.Text.StringBuilder();
    sb.AppendLine("Ad;Soyad;E-posta;Telefon;Unvan;Birim");
    foreach (var k in kisiler)
    {
        sb.AppendLine(string.Join(";",
            Temizle(k.Ad), Temizle(k.Soyad), Temizle(k.Eposta),
            Temizle("=\"" + k.Telefon + "\""), Temizle(k.Unvan), Temizle(k.Birim?.Ad)));
    }

    // Başa BOM ekliyoruz ki Excel Türkçe karakterleri doğru göstersin
    var bytes = new System.Text.UTF8Encoding(true).GetPreamble()
        .Concat(System.Text.Encoding.UTF8.GetBytes(sb.ToString()))
        .ToArray();

    return File(bytes, "text/csv", "rehber.csv");
}
    // Ekleme formu
    public async Task<IActionResult> Create()
    {
        await BirimleriYukle();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Ad,Soyad,Eposta,Telefon,Unvan,BirimId")] Kisi kisi)
    {
        if (!ModelState.IsValid)
        {
            await BirimleriYukle(kisi.BirimId);
            return View(kisi);
        }

        _context.Kisiler.Add(kisi);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Düzenleme formu
    public async Task<IActionResult> Edit(int id)
    {
        var kisi = await _context.Kisiler.FindAsync(id);
        if (kisi == null) return NotFound();

        await BirimleriYukle(kisi.BirimId);
        return View(kisi);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Ad,Soyad,Eposta,Telefon,Unvan,BirimId")] Kisi kisi)
    {
        if (id != kisi.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            await BirimleriYukle(kisi.BirimId);
            return View(kisi);
        }

        _context.Update(kisi);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Silme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var kisi = await _context.Kisiler.FindAsync(id);
        if (kisi != null)
        {
            _context.Kisiler.Remove(kisi);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}