using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RehberApp.Data;
using RehberApp.Models;

namespace RehberApp.Controllers;

public class BirimlerController : Controller
{
    private readonly RehberContext _context;

    public BirimlerController(RehberContext context)
    {
        _context = context;
    }

    // Liste
    public async Task<IActionResult> Index()
    {
        var birimler = await _context.Birimler.OrderBy(b => b.Ad).ToListAsync();
        return View(birimler);
    }

    // Ekleme formu
    public IActionResult Create()
    {
        return View();
    }

    // Ekleme formu gönderilince
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Ad")] Birim birim)
    {
        if (!ModelState.IsValid)
            return View(birim);

        _context.Birimler.Add(birim);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Düzenleme formu
    public async Task<IActionResult> Edit(int id)
    {
        var birim = await _context.Birimler.FindAsync(id);
        if (birim == null) return NotFound();
        return View(birim);
    }

    // Düzenleme formu gönderilince
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Ad")] Birim birim)
    {
        if (id != birim.Id) return NotFound();
        if (!ModelState.IsValid) return View(birim);

        _context.Update(birim);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Silme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var birim = await _context.Birimler
            .Include(b => b.Kisiler)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (birim != null)
        {
            _context.Birimler.Remove(birim);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}