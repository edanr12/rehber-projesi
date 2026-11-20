namespace RehberApp.Models;

public class Birim
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;

    public List<Kisi> Kisiler { get; set; } = new();
}
