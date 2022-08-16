using System.Diagnostics.CodeAnalysis;
using Application.New.Common;
using Domain.New.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class EkmekBotDbContext : DbContext, IEkmekBotDbContext
{
    public EkmekBotDbContext(DbContextOptions<EkmekBotDbContext> options)
        : base(options)
    {
        
    }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Settings> Settings { get; set; } = null!;
}
