using System.Security.Cryptography;

namespace RehberApp.Data;

public static class SifreYardimci
{
    public static string Hashle(string sifre)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(sifre, salt, 100000, HashAlgorithmName.SHA256, 32);
        return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
    }

    public static bool Dogrula(string sifre, string kayit)
    {
        var parcalar = kayit.Split('.');
        if (parcalar.Length != 2) return false;

        var salt = Convert.FromBase64String(parcalar[0]);
        var beklenen = Convert.FromBase64String(parcalar[1]);
        var hash = Rfc2898DeriveBytes.Pbkdf2(sifre, salt, 100000, HashAlgorithmName.SHA256, 32);

        return CryptographicOperations.FixedTimeEquals(hash, beklenen);
    }
}