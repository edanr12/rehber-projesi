using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RehberApp.Data;
using RehberApp.Models;

namespace RehberApp.Controllers;

public class HomeController : Controller
{
    private readonly RehberContext _context;

    public HomeController(RehberContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.KisiSayisi = await _context.Kisiler.CountAsync();
        ViewBag.BirimSayisi = await _context.Birimler.CountAsync();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
