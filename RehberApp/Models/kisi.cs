using System.ComponentModel.DataAnnotations;

namespace RehberApp.Models;

public class Kisi
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad zorunludur.")]
    [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
    public string Ad { get; set; } = string.Empty;

    [Required(ErrorMessage = "Soyad zorunludur.")]
    [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
    public string Soyad { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Eposta { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon zorunludur.")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
    public string Telefon { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ünvan zorunludur.")]
    public string Unvan { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birim seçmelisiniz.")]
    public int? BirimId { get; set; }
    public Birim? Birim { get; set; }
}
