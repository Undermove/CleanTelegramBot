using System.Diagnostics.CodeAnalysis;
using Application.Common;
using Domain.Entities;
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
}
