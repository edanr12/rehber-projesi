using System.ComponentModel.DataAnnotations;

namespace RehberApp.Models;

public class Birim
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Birim adı zorunludur.")]
    [StringLength(100, ErrorMessage = "Birim adı en fazla 100 karakter olabilir.")]
    public string Ad { get; set; } = string.Empty;

    public List<Kisi> Kisiler { get; set; } = new();
}
