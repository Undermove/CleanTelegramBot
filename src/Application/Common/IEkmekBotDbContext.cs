using Domain.New.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.New.Common;

public interface IEkmekBotDbContext
{
    DbSet<User> Users { get; }
    DbSet<Domain.New.Entities.Settings> Settings { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}