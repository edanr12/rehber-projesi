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
    public async Task<IActionResult> Index()
    {
        var kisiler = await _context.Kisiler
            .Include(k => k.Birim)
            .OrderBy(k => k.Ad).ThenBy(k => k.Soyad)
            .ToListAsync();
        return View(kisiler);
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