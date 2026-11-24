using System.ComponentModel.DataAnnotations;

namespace RehberApp.Models;

public class Kullanici
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string KullaniciAdi { get; set; } = string.Empty;

    [Required]
    public string SifreHash { get; set; } = string.Empty;

    // "Admin" veya "Kullanici"
    [Required]
    public string Rol { get; set; } = "Kullanici";
}