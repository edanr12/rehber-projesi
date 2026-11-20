using Microsoft.EntityFrameworkCore;
using RehberApp.Models;

namespace RehberApp.Data;

public class RehberContext : DbContext
{
    public RehberContext(DbContextOptions<RehberContext> options) : base(options)
    {
    }

    public DbSet<Kisi> Kisiler { get; set; }
    public DbSet<Birim> Birimler { get; set; }
}
