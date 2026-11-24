using Microsoft.EntityFrameworkCore;
using RehberApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Giriş yapmayan hiç kimse hiçbir sayfaya giremesin
    var politika = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(politika));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(secenekler =>
    {
        secenekler.LoginPath = "/Hesap/Giris";
        secenekler.AccessDeniedPath = "/Hesap/Giris";
        secenekler.ExpireTimeSpan = TimeSpan.FromHours(8);
    });
builder.Services.AddDbContext<RehberContext>(options =>
    options.UseSqlite("Data Source=rehber.db"));
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RehberContext>();
    db.Database.Migrate();

    if (!db.Kullanicilar.Any())
    {
        db.Kullanicilar.Add(new RehberApp.Models.Kullanici
        {
            KullaniciAdi = "admin",
            SifreHash = SifreYardimci.Hashle("Admin123!"),
            Rol = "Admin"
        });
        db.Kullanicilar.Add(new RehberApp.Models.Kullanici
        {
            KullaniciAdi = "kullanici",
            SifreHash = SifreYardimci.Hashle("Kullanici123!"),
            Rol = "Kullanici"
        });
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
