using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RehberApp.Data;

namespace RehberApp.Controllers;

public class HesapController : Controller
{
    private readonly RehberContext _context;

    public HesapController(RehberContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public IActionResult Giris()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Giris(string kullaniciAdi, string sifre)
    {
        var kullanici = await _context.Kullanicilar
            .FirstOrDefaultAsync(k => k.KullaniciAdi == kullaniciAdi);

        if (kullanici == null || !SifreYardimci.Dogrula(sifre ?? "", kullanici.SifreHash))
        {
            ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }

        var kimlik = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, kullanici.KullaniciAdi),
            new Claim(ClaimTypes.Role, kullanici.Rol)
        }, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(kimlik));

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cikis()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Giris));
    }
}